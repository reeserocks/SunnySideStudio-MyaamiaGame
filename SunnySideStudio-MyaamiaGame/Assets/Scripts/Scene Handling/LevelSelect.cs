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

        if (unlockedLevel < 1)
        { 
            unlockedLevel = 1; 
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int levelIndex = i + 1;
            bool isUnlocked = levelIndex <= unlockedLevel;

            buttons[i].interactable = isUnlocked;

            int levelToLoad = levelIndex;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => LoadLevel(levelToLoad));
        }
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
        
        Cursor.visible = false;
    }
}
