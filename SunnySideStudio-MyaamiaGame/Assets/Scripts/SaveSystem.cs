// SAVESYSTEM.CS

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
        public List<SpawnedObjectData> SpawnedObjects;
        public List<bool> DiscoveredWords;
    }

    [System.Serializable]
    public struct SpawnedObjectData
    {
        public string prefabName;
        public Vector3 Position;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/savefile" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    private static void HandleSaveData()
    {
        // player data
        GameManager.Instance.Player.Save(ref _saveData.PlayerData);

        // objects data
        _saveData.SpawnedObjects = new List<SpawnedObjectData>();
        foreach (GameObject obj in ItemSpawner.spawnedObjects)
        {
            if (obj != null)
            {
                _saveData.SpawnedObjects.Add(new SpawnedObjectData
                {
                    prefabName = obj.name.Replace("(Clone)", "").Trim(),
                    Position = obj.transform.position
                });
            }
        }

        // word bank data 
        _saveData.DiscoveredWords = WordBankManager.GetDiscoveredWords();
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    public static void HandleLoadData()
    {
        // player data
        GameManager.Instance.Player.Load(_saveData.PlayerData);

        // objects data
        foreach (GameObject obj in ItemSpawner.spawnedObjects)
        {
            if (obj != null)
            {
                Object.Destroy(obj);
            }
        }
        ItemSpawner.spawnedObjects.Clear();

        foreach (var data in _saveData.SpawnedObjects)
        {
            GameObject prefab = Resources.Load<GameObject>(data.prefabName);
            if (prefab != null)
            {
                GameObject obj = Object.Instantiate(prefab, data.Position, Quaternion.identity);
                ItemSpawner.spawnedObjects.Add(obj);
            }
            else
            {
                Debug.LogWarning("Prefab not found: " + data.prefabName);
            }
        }

        // word bank data
        WordBankManager.LoadDiscoveredWords(_saveData.DiscoveredWords);

        // level data
        GameManager.playerSaveData.levelUnlocked = _saveData.PlayerData.levelUnlocked;
    }
}
