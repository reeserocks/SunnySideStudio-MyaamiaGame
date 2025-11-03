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
        string sceneName = $"World" + world.ToString() +"LevelSelect";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' not found. Check your build settings.");
        }
    }
}
