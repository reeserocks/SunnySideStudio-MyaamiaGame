//GAMEMANAGER.CS

using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    private GameObject canvasInstance;

    private GameObject menus;
    public static List<bool> discoveredWords = Enumerable.Repeat<bool>(false, 15).ToList();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SpawnCanvas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SaveSystem.Save();
        }

        if (Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            SaveSystem.Load();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menus.SetActive(!menus.activeSelf);
            Cursor.visible = true;
        }
    }

    private void SpawnCanvas()
    {
        if (canvasInstance != null)
        {
            return;
        }

        GameObject canvasPrefab = Resources.Load<GameObject>("Canvas");

        if (canvasPrefab != null)
        {
            canvasInstance = Instantiate(canvasPrefab);
            DontDestroyOnLoad(canvasInstance);
        }
        menus = canvasInstance.transform.Find("Menus")?.gameObject;
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
