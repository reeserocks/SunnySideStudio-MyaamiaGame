using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UI;

public class WordReviewScreen : MonoBehaviour
{
    public CanvasGroup group;
    public Image wordImage;
    public TextMeshProUGUI myaamiaText;
    public TextMeshProUGUI englishText;
    public TextMeshProUGUI descriptionText;
    public AspectRatioFitter imageAspectFitter;

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

        int world = GameManager.playerSaveData.worldUnlocked;
        if (world <= 0)
        {
            world = 1;
        }

        Image background = GameObject.Find("Background").GetComponent<Image>();
        Sprite bg = Resources.Load<Sprite>($"world{world}_lvlSelectBG");

        if (bg != null)
        {
            background.sprite = bg;
        }

        imageAspectFitter = wordImage.GetComponent<AspectRatioFitter>();

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
        if (GameManager.learnedNewWord)
        {
            randomIndex = learnedIndices.Count - 1;
        }

        // make sure assets are named Word_x.asset
        WordData data = Resources.Load<WordData>($"WordData/Word_{randomIndex}");

        if (data != null)
        {
            myaamiaText.text = data.myaamiaWord;
            englishText.text = data.englishTranslation;
            descriptionText.text = data.description;
            wordImage.sprite = data.image;
            float width = data.image.rect.width;
            float height = data.image.rect.height;
            imageAspectFitter.aspectRatio = width / height;
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

        int nextLevel = GameManager.playerSaveData.levelUnlocked;
        SceneManager.LoadScene("Level" + nextLevel);
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
