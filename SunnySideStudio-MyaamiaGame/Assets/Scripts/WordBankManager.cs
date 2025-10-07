using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordBankManager : MonoBehaviour
{
    public GameObject wordBank;
    private static List<bool> discoveredWords = Enumerable.Repeat<bool>(false, 15).ToList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wordBank.SetActive(false);
        List<GameObject> childButtons = new List<GameObject>();
        foreach (Transform button in transform)
        {
            childButtons.Add(button.gameObject);
        }

        for (int i = 0; i < discoveredWords.Count; i++)
        {
            childButtons[i].SetActive(discoveredWords[i]);
        }
    }

    void updateDiscoveredWords(int pos)
    {
        discoveredWords[pos] = true;
        Start();
    }
}
