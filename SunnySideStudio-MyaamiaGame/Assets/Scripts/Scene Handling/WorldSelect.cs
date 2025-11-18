using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelect : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        int unlockedWorld = GameManager.playerSaveData.worldUnlocked;

        if (unlockedWorld < 1)
        {
            unlockedWorld = 0;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int worldIndex = i;
            bool isUnlocked = worldIndex <= unlockedWorld;

            buttons[i].interactable = isUnlocked;

            int worldToLoad = worldIndex;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => LoadWorld(worldToLoad));
        }
    }

    private void LoadWorld(int world)
    {
        if (world != 0)
        {
            string sceneName = $"World" + world.ToString() + "LevelSelect";
            GameManager.worldSelected = world;

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene("Level0");
        }
    }
}
