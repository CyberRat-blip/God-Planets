using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public GameController gameController; // Ссылка на GameController

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) >= minDistanceForSwipe)
        {
            Vector2 direction = fingerDownPosition - fingerUpPosition;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    Debug.Log("Right Swipe");
                }
                else
                {
                    Debug.Log("Left Swipe");
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    Debug.Log("Up Swipe");
                    // Вызываем метод SwipePerformed в GameController после обнаружения свайпа вверх
                    //gameController.SwipePerformed();
                }
                else
                {
                    Debug.Log("Down Swipe");
                }
            }

            fingerUpPosition = fingerDownPosition;
        }
    }
}
