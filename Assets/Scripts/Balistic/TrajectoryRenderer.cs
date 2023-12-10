using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public LineRenderer lineRendererComponent;
    public float velocityMultiplier = 1.0f; // ��������� ��������
    public Transform targetTransform; // ����, ���� ���������� ����������
    public float angleInDegrees = 45f; // ����, ������� ����� �������� �� ����� ����
    public float minHeight = 22f; // ����������� ������ ��� ��������� �����

    void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
    }

    void Update()
    {
        ShowTrajectory(transform.position, targetTransform.position, velocityMultiplier, angleInDegrees);
    }

    public void ShowTrajectory(Vector3 origin, Vector3 target, float velocityMultiplier, float angleInDegrees)
    {
        Vector3[] points = new Vector3[100];
        lineRendererComponent.positionCount = points.Length;

        float angleInRadians = angleInDegrees * Mathf.PI / 180;
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float distance = toTargetXZ.magnitude;
        Vector3 direction = toTargetXZ.normalized;

        float v2 = (Physics.gravity.y * distance * distance) / (2 * (toTarget.y - Mathf.Tan(angleInRadians) * distance) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2)) * velocityMultiplier;

        Vector3 velocity = ((direction * Mathf.Cos(angleInRadians) + Vector3.up * Mathf.Sin(angleInRadians)) * v);

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + velocity * time + Physics.gravity * time * time / 2f;

            // ���������, �� ���������� �� ����� ���� ����������� ������
            if (points[i].y < minHeight)
            {
                // ������� ���������� ���������� ����� ��� ���������� ����������
                lineRendererComponent.positionCount = Mathf.Max(i, 1);
                break;
            }
        }

        lineRendererComponent.SetPositions(points);
    }
}
