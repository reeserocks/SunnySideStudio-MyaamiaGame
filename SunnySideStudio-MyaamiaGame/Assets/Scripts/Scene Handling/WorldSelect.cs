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
            unlockedWorld = 1;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int worldIndex = i + 1;
            bool isUnlocked = worldIndex <= unlockedWorld;

            buttons[i].interactable = isUnlocked;

            int worldToLoad = worldIndex;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => LoadWorld(worldToLoad));
        }
    }

    private void LoadWorld(int world)
    {
        if (world != 1)
        {
            string sceneName = $"World" + world.ToString() + "LevelSelect";

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            //change to Level0 after we have tutorial
            SceneManager.LoadScene("World1LevelSelect");
        }
    }
}
