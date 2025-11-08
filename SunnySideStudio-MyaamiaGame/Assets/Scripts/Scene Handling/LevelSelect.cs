//LEVELSELECT.CS
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button[] buttons;

    void Start()
    {
        int unlockedLevel = GameManager.playerSaveData.levelUnlocked;
        int currentWorld = GameManager.playerSaveData.worldUnlocked;

        //delete once we have tutorial level
        if (unlockedLevel < 1)
            unlockedLevel = 1;

        int startLevel = (currentWorld - 1) * 10 + 1;
        int endLevel = startLevel + 9;

        for (int i = 0; i < buttons.Length; i++)
        {
            int levelIndex = i + 1;
            bool isUnlocked = levelIndex <= unlockedLevel;

            buttons[i].interactable = isUnlocked;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int level)
    {
        string sceneName = "Level" + level;

        SceneManager.LoadScene(sceneName);
        Cursor.visible = false;
    }
}
