//UPDATEHUD.CS

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UpdateHUD : MonoBehaviour
{
    [SerializeField] Image worldIcon;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Sprite[] worldIcons;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return null;
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
            if (level == 0)
            {
                world = 0;
            }
            else
            {
                world = (level - 1) / 10 + 1;
                level = (level - 1) % 10 + 1;
            }
        }

        worldIcon.sprite = worldIcons[world];
        levelText.text = level.ToString();
    }
}
