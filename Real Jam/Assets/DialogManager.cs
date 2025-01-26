using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    [TextArea(3, 5)]
    public string[] dialogLines;

    public TMP_Text dialogText;
    public GameObject dialogUI;
    public GameObject Gecko;

    private int currentDialogIndex = 0;
    private Animator geckoAnimator;
    private bool isAnimationFinished = false;

    private static Dictionary<int, bool> dialogPlayed = new Dictionary<int, bool>();
    private CanvasGroup dialogCanvasGroup;

    public float fadeDuration = 0.5f; // Duration for fade in/out

    void Start()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        // Check if dialog has already been played in this level
        if (dialogPlayed.ContainsKey(currentSceneIndex) && dialogPlayed[currentSceneIndex])
        {
            dialogUI.SetActive(false);
            this.enabled = false; // Disable the script if the dialog has already been played
            return;
        }

        geckoAnimator = Gecko.GetComponent<Animator>();
        geckoAnimator.SetTrigger("FlyIn");

        dialogCanvasGroup = dialogUI.GetComponent<CanvasGroup>();
        if (dialogCanvasGroup == null)
        {
            Debug.LogError("Dialog UI is missing a CanvasGroup component!");
            return;
        }

        dialogCanvasGroup.alpha = 0; // Start with UI invisible
        dialogUI.SetActive(true); // Ensure it's active
    }

    void Update()
    {
        if (!isAnimationFinished && IsGeckoAnimationFinished("FlyInGecko"))
        {
            isAnimationFinished = true;
            StartCoroutine(FadeInDialogUI());

            if (dialogLines.Length > 0)
            {
                dialogText.text = dialogLines[currentDialogIndex];
            }
            else
            {
                Debug.LogWarning("No dialog lines assigned!");
            }
        }

        if (isAnimationFinished && Input.GetMouseButtonDown(0))
        {
            ShowNextDialog();
        }
    }

    public void ShowNextDialog()
    {
        currentDialogIndex++;

        if (currentDialogIndex < dialogLines.Length)
        {
            dialogText.text = dialogLines[currentDialogIndex];
        }
        else
        {
            dialogText.text = "";
            Debug.Log("Dialog finished!");
            StartCoroutine(FadeOutDialogUI());

            if (dialogUI != null)
            {
                geckoAnimator.SetTrigger("FlyOut");
            }

            // Mark the dialog as played for this level
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (!dialogPlayed.ContainsKey(currentSceneIndex))
            {
                dialogPlayed.Add(currentSceneIndex, true);
            }
        }
    }

    private bool IsGeckoAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = geckoAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    private IEnumerator FadeInDialogUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            dialogCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        dialogCanvasGroup.alpha = 1; // Ensure it's fully visible
    }

    private IEnumerator FadeOutDialogUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            dialogCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }
        dialogCanvasGroup.alpha = 0; // Ensure it's fully invisible
        dialogUI.SetActive(false); // Optionally deactivate the UI after fading out
    }
}
