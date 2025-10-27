//UPDATEHUD.CS

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateHUD : MonoBehaviour
{
    [SerializeField] Image worldIcon;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Sprite[] worldIcons;

    private void Awake()
    {
        UpdateLevelDisplay();
    }

    public void UpdateLevelDisplay()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int level = 0;
        int world = 1;

        int index = SceneManager.GetActiveScene().buildIndex;
        if (sceneName.StartsWith("Level"))
        {
            int.TryParse(sceneName.Replace("Level", ""), out level);
            world = (level - 1) / 10 + 1;
            level = (level - 1) % 10 + 1;
        }

        if (worldIcons.Length >= world)
        {
            worldIcon.sprite = worldIcons[world - 1];
        }

        levelText.text = level.ToString();
    }
}
