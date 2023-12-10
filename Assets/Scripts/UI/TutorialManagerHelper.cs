using UnityEngine;

public class TutorialManagerHelper : MonoBehaviour
{
    public void ToggleTutorials(bool isEnabled)
    {
        if (isEnabled)

            TutorialManager.EnableTutorials();

        else

            TutorialManager.DisableTutorials();
    }
}
