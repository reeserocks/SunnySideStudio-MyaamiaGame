//GAMEMANAGER.CS

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance 
    {
        get
        { 
            if (!Application.isPlaying)
            {
                return null;
            }

            if (instance == null)
            {
                Instantiate(Resources.Load<GameManager>("GameManager"));
            }

            return instance;
        }
    }

    public Player Player { get; set; }
    public static PlayerSaveData playerSaveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SaveSystem.Save();
        }

        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SaveSystem.Load();
        }
    }

    public static void UnlockLevel(int currentLevel)
    {
        if(playerSaveData.levelUnlocked < currentLevel)
        {
            playerSaveData.levelUnlocked = currentLevel;
            SaveSystem.Save();
        }
    }
}
