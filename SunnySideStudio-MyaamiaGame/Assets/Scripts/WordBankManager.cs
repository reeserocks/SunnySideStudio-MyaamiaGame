using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordBankManager : MonoBehaviour
{
    public GameObject wordBank;
    public int positionChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        updateDiscoveredWords();
        List<GameObject> childButtons = new List<GameObject>();
        foreach (Transform button in transform)
        {
            childButtons.Add(button.gameObject);
        }

        for (int i = 1; i < childButtons.Count; i++)
        {
            childButtons[i].SetActive(GameManager.discoveredWords[i]);
        }
    }

    void updateDiscoveredWords()
    {
        if (positionChange != -1)
        {
            GameManager.discoveredWords[positionChange] = true;
        }
    }

    // SAVE DATA 
    public static List<bool> GetDiscoveredWords()
    {
        return new List<bool>(GameManager.discoveredWords);
    }

    public static void LoadDiscoveredWords(List<bool> loadedList)
    {
        if (loadedList != null || loadedList.Count == 0)
        {
            return;
        }

        GameManager.discoveredWords = loadedList;
    }
}
