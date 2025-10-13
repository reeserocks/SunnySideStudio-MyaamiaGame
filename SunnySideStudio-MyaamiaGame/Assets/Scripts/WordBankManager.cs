using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordBankManager : MonoBehaviour
{
    public GameObject wordBank;
    public int positionChange;
    private static List<bool> discoveredWords = Enumerable.Repeat<bool>(false, 15).ToList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wordBank.SetActive(false);
        updateDiscoveredWords();
        List<GameObject> childButtons = new List<GameObject>();
        foreach (Transform button in transform)
        {
            childButtons.Add(button.gameObject);
        }

        for (int i = 0; i < childButtons.Count; i++)
        {
            childButtons[i].SetActive(discoveredWords[i]);
        }
    }

    void updateDiscoveredWords()
    {
        if (positionChange != -1)
        {
            discoveredWords[positionChange] = true;
        }
    }

    // SAVE DATA 
    public static List<bool> GetDiscoveredWords()
    {
        return new List<bool>(discoveredWords);
    }

    public static void LoadDiscoveredWords(List<bool> loadedList)
    {
        if (loadedList != null || loadedList.Count == 0)
        {
            return;
        }

        discoveredWords = loadedList;
    }
}
