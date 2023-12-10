using UnityEngine;

public class DragAndMassModifier : MonoBehaviour
{
    public float newLinearDrag = 10f; // Новое значение Linear Drag
    public float newMass = 1f; // Новое значение массы

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
