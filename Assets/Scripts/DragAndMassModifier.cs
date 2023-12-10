using UnityEngine;

public class DragAndMassModifier : MonoBehaviour
{
    public float newLinearDrag = 10f; // ����� �������� Linear Drag
    public float newMass = 1f; // ����� �������� �����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.drag = newLinearDrag;
            rb.mass = newMass;
        }
    }
}
