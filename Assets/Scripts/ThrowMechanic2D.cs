using UnityEngine;

public class ThrowMechanic2D : MonoBehaviour
{
    public GameObject[] planetPrefabs; // Массив префабов планет
    public LineRenderer lineRenderer; // LineRenderer для отображения параболы
    public Transform throwPoint; // Точка, откуда будет производиться бросок
    public int resolution = 30; // Разрешение линии (количество точек траектории)
    public float maxDragDistance = 5f; // Максимальное расстояние натяжения
    public float gravityScale = 1f; // Переменная для регулировки гравитации

    private GameObject currentPlanet = null;
    private Vector3 dragStartPoint; // Точка начала перетаскивания
    private Vector3 dragEndPoint; // Точка окончания перетаскивания
    private bool isDragging = false;
    private int throwCount = 0; // Счетчик бросков
    public bool canThrow = true; // Может ли игрок бросать объекты

    public ShakeMechanic shakeMechanic; // Ссылка на скрипт ShakeMechanic

    public int minThrowsBeforeShake = 10; // Минимальное количество бросков до тряски
    public int maxThrowsBeforeShake = 20; // Максимальное количество бросков до тряски
    private int throwsNeededForShake; // Случайное число бросков для активации тряски

    void Start()
    {
        ResetThrowMechanic();
    }

    void Update()
    {
        if (canThrow)
        {
            HandleThrowInput();
        }
    }

    private void HandleThrowInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            ContinueDrag();
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }
    }

    private void StartDrag()
    {
        dragStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartPoint.z = 0;

        GameObject selectedPrefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
        currentPlanet = Instantiate(selectedPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        isDragging = true;
    }

    private void ContinueDrag()
    {
        dragEndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragEndPoint.z = 0;

        Vector3 dragVector = dragEndPoint - dragStartPoint;
        if (dragVector.magnitude > maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        dragVector = ConstrainDragVector(dragVector);

        ShowTrajectory(throwPoint.position - dragVector);
    }

    private Vector3 ConstrainDragVector(Vector3 dragVector)
    {
        float angle = Mathf.Atan2(dragVector.y, -dragVector.x) * Mathf.Rad2Deg;

        if (angle > -90f) angle = -90f;
        // Проверяем, нужно ли корректировать угол
        if (angle < 0f)
        {
            angle += 360f; // Нормализуем угол
        }

        // Ограничиваем угол перетаскивания в диапазоне от 225 до 315 градусов
        if (angle < 225f) angle = 225f; // Ограничение сверху на 225 градусов (равно -135 градусам)
        if (angle > 315f) angle = 315f; // Ограничение снизу на 315 градусов (равно -45 градусам)

        float magnitude = dragVector.magnitude;
        angle *= Mathf.Deg2Rad; // Преобразуем обратно в радианы для вычислений

        return new Vector3(
            -magnitude * Mathf.Cos(angle),
            magnitude * Mathf.Sin(angle),
            0
        );
    }

    private void EndDrag()
    {
        isDragging = false;
        lineRenderer.positionCount = 0;

        Vector3 forceDirection = dragEndPoint - dragStartPoint;
        if (forceDirection.magnitude > maxDragDistance)
        {
            forceDirection = forceDirection.normalized * maxDragDistance;
        }

        currentPlanet.GetComponent<Rigidbody2D>().isKinematic = false;
        currentPlanet.GetComponent<Rigidbody2D>().AddForce(-forceDirection * 10f, ForceMode2D.Impulse);

        currentPlanet = null;

        throwCount++;
        if (throwCount >= throwsNeededForShake)
        {
            canThrow = false;
            shakeMechanic.PrepareForShake(); // Подготавливаем механику тряски
        }
    }

    private void ShowTrajectory(Vector3 startPoint)
    {
        Vector3[] points = new Vector3[resolution];
        lineRenderer.positionCount = points.Length;

        Vector3 velocity = (startPoint - throwPoint.position) * 10f;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            Vector3 gravityEffect = Physics.gravity * gravityScale * time * time / 2f;
            points[i] = throwPoint.position + velocity * time + gravityEffect;
            points[i].z = 0;

            if (points[i].y < throwPoint.position.y)
            {
                lineRenderer.positionCount = i + 1;
                break;
            }
        }

        lineRenderer.SetPositions(points);
    }

    public void ResetThrowMechanic()
    {
        throwCount = 0;
        canThrow = true;
        throwsNeededForShake = Random.Range(minThrowsBeforeShake, maxThrowsBeforeShake + 1);
    }
}
