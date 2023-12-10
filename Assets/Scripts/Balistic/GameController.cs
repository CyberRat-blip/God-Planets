using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Api;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Ballistics ballisticsScript;
    public TrajectoryRenderer trajectoryRendererScript;
    public Transform movableObject; // Объект, который будет перемещаться
    public LineRenderer lineRendererComponent;
    public ShakeMechanic shakeMechanic;
    public GameObject startTutorialUI; // UI элемент для начального обучения
    public GameObject swipeTutorialUI; // UI элемент для обучения свайпу
    private bool swipeTutorialShown = false; // Флаг для отслеживания показа обучения по свайпу

    public float maxAngleRight = 75f; // Максимальный угол при крайнем правом положении
    public float maxAngleLeft = 68f; // Максимальный угол при крайнем левом положении
    public float minAngle = 0f; // Минимальный угол
    public float angleChangeSpeed = 0.1f; // Скорость изменения угла
    public float minX, maxX; // Диапазон перемещения по оси X

    public int maxThrows = 5; // Максимальное количество бросков
    private int throwsCount = 0; // Счетчик бросков

    private Camera mainCamera; // Главная камера
    private bool isDragging = false; // Флаг для отслеживания, зажата ли ЛКМ
    private float lastMouseY; // Последняя позиция мыши по Y
    private bool canThrow = true; // Флаг, позволяющий бросать объект
    private float tutorialTimer = 15f; // Таймер для повторного показа обучения
    private float lastInteractionTime; // Время последнего взаимодействия пользователя

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
        lineRendererComponent.enabled = false;
    }

    void Start()
    {
        Time.timeScale = 1f;
        //AppodealAdsManager.ShowBanner();
        if (TutorialManager.TutorialsEnabled && startTutorialUI != null)
        {
            startTutorialUI.SetActive(true);
            lastInteractionTime = Time.time; // Инициализация времени последнего взаимодействия
        }
    }

    void Update()
    {
        if (canThrow)
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverNoTrackZone())
            {
                ResetTutorialTimer();
                CharacterAnimationController.Instance.SetHoldPlanetAnimation();
                StartDragging();
            }

            if (isDragging)
            {
                
                HandleDragging();
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                StopDragging();
                CharacterAnimationController.Instance.PlayAnimationOnce("Throw");
            }

            if (TutorialManager.TutorialsEnabled)
            {
                // Проверяем, прошло ли 15 секунд с последнего взаимодействия
                if (Time.time - lastInteractionTime > tutorialTimer)
                {
                    ShowTutorial();
                    ResetTutorialTimer();
                }
            }
        }
    }

    private bool IsPointerOverNoTrackZone()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("NoTrack"))
        {
            return true;
        }

        return false;
    }
    private void StartDragging()
    {
        isDragging = true;
        lineRendererComponent.enabled = true;
        lastMouseY = Input.mousePosition.y;

        // Закрываем начальный UI при первом нажатии
        startTutorialUI.SetActive(false);
    }

    private void HandleDragging()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.WorldToScreenPoint(movableObject.position).z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.x = Mathf.Clamp(worldPosition.x, minX, maxX);
        movableObject.position = new Vector3(worldPosition.x, movableObject.position.y, movableObject.position.z);

        float mouseYDelta = Input.mousePosition.y - lastMouseY;
        float lerpFactor = (movableObject.position.x - minX) / (maxX - minX);
        float maxAngle = Mathf.Lerp(maxAngleLeft, maxAngleRight, lerpFactor);
        float newAngle = Mathf.Clamp(ballisticsScript.AngleInDegrees + mouseYDelta * angleChangeSpeed, minAngle, maxAngle);
        ballisticsScript.AngleInDegrees = newAngle;
        trajectoryRendererScript.angleInDegrees = newAngle;
        lastMouseY = Input.mousePosition.y;
    }

    private void StopDragging()
    {
        isDragging = false;
        lineRendererComponent.enabled = false;

        if (throwsCount < maxThrows)
        {
            ballisticsScript.Shot();
            throwsCount++;

            // Показываем UI для обучения свайпу после первого броска с задержкой
            if (throwsCount == 1 && TutorialManager.TutorialsEnabled && !swipeTutorialShown && swipeTutorialUI != null)
            {
                StartCoroutine(ShowSwipeTutorialWithDelay(shakeMechanic.delayBeforeShake));
            }
        }

        if (throwsCount >= maxThrows)
        {
            DisableThrowing();
        }
    }

    private IEnumerator ShowSwipeTutorialWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        swipeTutorialUI.SetActive(true);
        swipeTutorialShown = true;
    }

    public void EnableThrowing()
    {
        canThrow = true;
        ballisticsScript.SetCanPrepareShot(true);
        throwsCount = 0;
    }

    public void DisableThrowing()
    {
        canThrow = false;
        ballisticsScript.SetCanPrepareShot(false);
        StartCoroutine(DelayBeforeShake());
    }

    private IEnumerator DelayBeforeShake()
    {
        yield return new WaitForSeconds(shakeMechanic.delayBeforeShake);
        shakeMechanic.EnableShakeMechanic();
    }

    public void ReloadScene()
    {
        // Сброс Time.timeScale обратно в нормальное состояние
        Time.timeScale = 1f;
        // Перезагрузка текущей сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void ResetTutorialTimer()
    {
        lastInteractionTime = Time.time; // Сброс таймера
        if (swipeTutorialUI.activeSelf)
        {
            swipeTutorialUI.SetActive(false); // Скрываем обучение, если оно уже показано
        }
    }

    private void ShowTutorial()
    {
        startTutorialUI.SetActive(true); // Показываем обучение
    }
}
