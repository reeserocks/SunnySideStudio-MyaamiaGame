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
        findAllButtons(transform, childButtons);

        for (int i = 0; i < childButtons.Count; i++)
        {
            childButtons[i].SetActive(GameManager.discoveredWords[i]);
        }
    }

    void findAllButtons(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.CompareTag("DisWord"))
                list.Add(child.gameObject);
            findAllButtons(child, list);
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
