using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFlag : MonoBehaviour
{
    public int currentLevel;

    private TextMeshProUGUI levelCompleteText;
    private float fadeDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            // player anim
            Player player = other.GetComponent<Player>();
            player.SetWin(true);

            // level complete!
            Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            Transform textTransform = canvas.transform.Find("LevelCompleteTxt");
            levelCompleteText = textTransform.GetComponent<TextMeshProUGUI>();
            
            StartCoroutine(ShowAndFadeText());
            StartCoroutine(WaitForWinAnimation(player));

            SaveSystem.Save();
            GameManager.UnlockLevel(currentLevel);
        }
    }

    private IEnumerator ShowAndFadeText()
    {
        CanvasGroup group = levelCompleteText.GetComponent<CanvasGroup>();
        if (group == null)
        {
            group = levelCompleteText.gameObject.AddComponent<CanvasGroup>();
        }
        group.alpha = 0f;
        levelCompleteText.gameObject.SetActive(true);

        // fade in text
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        // fade out text
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        group.alpha = 0f;
        levelCompleteText.gameObject.SetActive(false);
    }

    private IEnumerator WaitForWinAnimation(Player player)
    {
        Animator anim = player.GetComponent<Animator>();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Win"));

        yield return new WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
            !anim.IsInTransition(0));

        player.SetWin(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
