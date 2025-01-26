using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailManager : MonoBehaviour
{
    public CanvasGroup mailUICanvasGroup; // CanvasGroup for mailUI
    public CanvasGroup composeUICanvasGroup; // CanvasGroup for composeUI
    public TMP_InputField workerName;
    public TMP_InputField message;

    public float fadeDuration = 0.5f; // Duration of fade-in/out

    void Update()
    {
        if (mailUICanvasGroup.alpha > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sentMessage();
                StartCoroutine(FadeOut(mailUICanvasGroup, () => StartCoroutine(FadeIn(composeUICanvasGroup))));
            }
        }
    }

    public void openMailUI()
    {
        StartCoroutine(FadeIn(mailUICanvasGroup));
        StartCoroutine(FadeOut(composeUICanvasGroup));
    }

    public void closeMailUI()
    {
        StartCoroutine(FadeOut(mailUICanvasGroup, () => StartCoroutine(FadeIn(composeUICanvasGroup))));
    }

    public void sentMessage()
    {
        Worker[] workers = FindObjectsOfType<Worker>();

        foreach (Worker worker in workers)
        {
            if (worker.name.ToLower() == workerName.text.ToLower())
            {
                worker.decideAndAct(message.text);
            }
        }
        SoundEffectManager.Instance.PlaySound("EmailNotification", 1f, 1f);
        closeMailUI();
    }

    public void clearText()
    {
        workerName.text = "";
        message.text = "";
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, System.Action onComplete = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f;
        clearText();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Call onComplete callback after fade out
        onComplete?.Invoke();
    }
}
