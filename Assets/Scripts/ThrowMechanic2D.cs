using UnityEngine;

public class ThrowMechanic2D : MonoBehaviour
{
    public GameObject[] planetPrefabs; // ������ �������� ������
    public LineRenderer lineRenderer; // LineRenderer ��� ����������� ��������
    public Transform throwPoint; // �����, ������ ����� ������������� ������
    public int resolution = 30; // ���������� ����� (���������� ����� ����������)
    public float maxDragDistance = 5f; // ������������ ���������� ���������
    public float gravityScale = 1f; // ���������� ��� ����������� ����������

    private GameObject currentPlanet = null;
    private Vector3 dragStartPoint; // ����� ������ ��������������
    private Vector3 dragEndPoint; // ����� ��������� ��������������
    private bool isDragging = false;
    private int throwCount = 0; // ������� �������
    public bool canThrow = true; // ����� �� ����� ������� �������

    public ShakeMechanic shakeMechanic; // ������ �� ������ ShakeMechanic

    public int minThrowsBeforeShake = 10; // ����������� ���������� ������� �� ������
    public int maxThrowsBeforeShake = 20; // ������������ ���������� ������� �� ������
    private int throwsNeededForShake; // ��������� ����� ������� ��� ��������� ������

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
        // ���������, ����� �� �������������� ����
        if (angle < 0f)
        {
            angle += 360f; // ����������� ����
        }

        // ������������ ���� �������������� � ��������� �� 225 �� 315 ��������
        if (angle < 225f) angle = 225f; // ����������� ������ �� 225 �������� (����� -135 ��������)
        if (angle > 315f) angle = 315f; // ����������� ����� �� 315 �������� (����� -45 ��������)

        float magnitude = dragVector.magnitude;
        angle *= Mathf.Deg2Rad; // ����������� ������� � ������� ��� ����������

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
            shakeMechanic.PrepareForShake(); // �������������� �������� ������
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
