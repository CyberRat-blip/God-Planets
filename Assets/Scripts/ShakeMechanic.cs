using UnityEngine;

public class ShakeMechanic : MonoBehaviour
{
    public GameObject swipeTutorialUI; // UI элемент для обучения свайпу
    private bool isShakeActive = false;
    private Vector2 startTouchPosition;
    private float startTouchTime;
    public float maxForceHorizontal = 100f;
    public float maxForceVertical = 50f;
    public float delayBeforeShake = 0.5f; // Задержка перед началом свайпа
    public float delayAfterShake = 0.5f; // Задержка после свайпа
    public float minSwipeDistance = 50f; // Минимальная длина свайпа


    private void Start()
    {
        /*if (TutorialManager.TutorialsEnabled && swipeTutorialUI != null)
        {
            swipeTutorialUI.SetActive(true);
        }*/
    }

    public void PrepareForShake()
    {
        Debug.Log("Delay before shake starts");
        Invoke(nameof(EnableShakeMechanic), delayBeforeShake);
    }

    public void EnableShakeMechanic()
    {
        Debug.Log("Shake mechanic enabled");
        isShakeActive = true;
    }

    void Update()
    {
        if (isShakeActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchTime = Time.time;
                startTouchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 endTouchPosition = Input.mousePosition;
                Vector2 swipeDelta = endTouchPosition - startTouchPosition;
                float swipeDistance = swipeDelta.magnitude;
                float touchDuration = Time.time - startTouchTime;

                if (swipeDistance < minSwipeDistance) return; // Проверка минимальной длины свайпа

                Vector2 swipeDirection = swipeDelta.normalized;
                float swipeForce = swipeDistance / touchDuration;

                // Определение направления свайпа
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    swipeForce = Mathf.Min(swipeForce, maxForceHorizontal);
                    if  (swipeDirection.x > 0)
                    { 
                        CharacterAnimationController.Instance.SetSwipeRightAnimation();
                        swipeDirection = Vector2.right; 
                    }
                    else
                    { 
                        CharacterAnimationController.Instance.SetSwipeLeftAnimation();
                        swipeDirection = Vector2.left;
                    //swipeDirection = swipeDirection.x > 0 ? Vector2.right : Vector2.left;
                    }
                }
                else
                {
                    swipeForce = Mathf.Min(swipeForce, maxForceVertical);
                    if (swipeDirection.y > 0)
                    {
                        CharacterAnimationController.Instance.SetSwipeUpAnimation();
                        swipeDirection = Vector2.up;
                    }
                }

                ApplyForceToObjects(swipeDirection, swipeForce);
                isShakeActive = false;
                swipeTutorialUI.SetActive(false); // Закрываем UI для обучения свайпу
                Debug.Log("Delay after shake starts");
                Invoke(nameof(PostShakePause), delayAfterShake);
            }
        }
    }

    private void PostShakePause()
    {
        Debug.Log("Shake mechanic finished and throwing enabled");
        GameController.Instance.EnableThrowing();
    }

    private void ApplyForceToObjects(Vector2 direction, float force)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PrefabTag"))
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }
}
