using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle tutorialToggle;
    public Toggle musicToggle;
    public TutorialManagerHelper tutorialManagerHelper;
    public MusicManagerHelper musicManagerHelper;

    private void Start()
    {
        tutorialToggle.isOn = TutorialManager.TutorialsEnabled;
        tutorialToggle.onValueChanged.AddListener(tutorialManagerHelper.ToggleTutorials);

        // ������������� ������������� ������ � ������������ � ����������� ����������
        musicToggle.isOn = MusicManager.IsMusicPlaying;
        musicToggle.onValueChanged.AddListener(musicManagerHelper.ToggleMusic);
    }
}
