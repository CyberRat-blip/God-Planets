using UnityEngine;
using TMPro;

public class UIManagerGameScene : MonoBehaviour
{
    public TextMeshProUGUI lifeCountText;

    private void Update()
    {
        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        int lifeCount = LifeManager.Instance.GetLifeCount();
        lifeCountText.text = lifeCount.ToString();
    }
}
