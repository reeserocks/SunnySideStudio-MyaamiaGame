using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WordReviewScreen : MonoBehaviour
{
    public CanvasGroup group;
    public Image wordImage;
    public TextMeshProUGUI myaamiaText;
    public TextMeshProUGUI englishText;
    public TextMeshProUGUI descriptionText;

    private List<bool> discoveredWords;
    private float fadeDuration = 0.6f;
    private bool canContinue = false;

    private void Start()
    {
        if (group == null)
        {
            group = GetComponent<CanvasGroup>();
        }

        group.alpha = 0f;
        discoveredWords = WordBankManager.GetDiscoveredWords();

        int world = PlayerPrefs.GetInt("CurrentWorld", 1);
        int nextLevelIndex = PlayerPrefs.GetInt("NextLevelToLoad", 2);
        Image background = GameObject.Find("Background").GetComponent<Image>();
        Sprite bg = Resources.Load<Sprite>("world" + world + "_lvlSelectBG");
        background.sprite = bg;

        ShowRandomWord();
        StartCoroutine(FadeIn());
    }

    void ShowRandomWord()
    {
        List<int> learnedIndices = new List<int>();
        for (int i = 0; i < discoveredWords.Count; i++)
        {
            if (discoveredWords[i]) learnedIndices.Add(i);
        }

        if (learnedIndices.Count == 0)
        {
            myaamiaText.text = "No words learned yet!";
            englishText.text = "";
            descriptionText.text = "";
            return;
        }

        int randomIndex = learnedIndices[Random.Range(0, learnedIndices.Count)];

        // make sure assets are named Word_x.asset
        WordData data = Resources.Load<WordData>($"WordData/Word_{randomIndex}");

        if (data != null)
        {
            myaamiaText.text = data.myaamiaWord;
            englishText.text = data.englishTranslation;
            descriptionText.text = data.description;
            wordImage.sprite = data.image;
        }
        else
        {
            myaamiaText.text = "Missing data!";
            Debug.LogWarning($"WordData not found for index {randomIndex}");
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        group.alpha = 1f;
        yield return new WaitForSeconds(1f);
        canContinue = true;
    }

    private IEnumerator FadeOutAndLoad()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        int nextLevelIndex = PlayerPrefs.GetInt("NextLevelToLoad", 0);
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void Update()
    {
        if (canContinue && Input.anyKeyDown)
        {
            canContinue = false;
            StartCoroutine(FadeOutAndLoad());
        }
    }
}
