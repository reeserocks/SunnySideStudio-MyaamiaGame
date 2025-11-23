//GAMEMANAGER.CS

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public static bool isPlacing = false;
    public static bool canType = false;
    public static int worldSelected;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            HandleCanvas(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleCanvas(scene.name);
        instance.GetComponent<ControlSwitcher>().SetLevelStart();
    }

    private void HandleCanvas(string sceneName)
    {
        bool isLevelScene = sceneName.StartsWith("Level");

        if (isLevelScene)
        {
            if (canvasInstance == null)
            {
                SpawnCanvas();
            }

            var hud = canvasInstance.GetComponentInChildren<UpdateHUD>(true);
            if (hud != null)
            {
                hud.UpdateLevelDisplay();
            }

            canvasInstance.SetActive(true);
        }
        else if (canvasInstance != null)
        {
            canvasInstance.SetActive(false);
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SaveSystem.Save();

            Debug.Log(SaveSystem.SaveFileName() + " saved.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SaveSystem.Load();

            Debug.Log(SaveSystem.SaveFileName() + " loaded.");
        }
        */

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

    public int CurrentWorld
    {
        get
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.StartsWith("Level"))
            {
                int.TryParse(sceneName.Replace("Level", ""), out int levelNum);
                return (levelNum - 1) / 10 + 1;
            }
            return playerSaveData.worldUnlocked;
        }
    }

    public static void UnlockLevel(int currentLevel)
    {
        int currentWorld = (currentLevel - 1) / 10 + 1;

        if (playerSaveData.levelUnlocked < currentLevel)
        {
            playerSaveData.levelUnlocked = currentLevel;
        }

        if (playerSaveData.worldUnlocked < currentWorld)
        {
            playerSaveData.worldUnlocked = currentWorld;
        }

        SaveSystem.Save();
    }
}
