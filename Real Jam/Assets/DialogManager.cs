using UnityEngine;
using TMPro;

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

    void Start()
    {
        geckoAnimator = Gecko.GetComponent<Animator>();
        geckoAnimator.SetTrigger("FlyIn");

        dialogUI.SetActive(false);
    }

    void Update()
    {
        if (!isAnimationFinished && IsGeckoAnimationFinished("FlyInGecko"))
        {
            isAnimationFinished = true;
            dialogUI.SetActive(true);

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
            if (dialogUI != null)
            {
                dialogUI.SetActive(false);
                geckoAnimator.SetTrigger("FlyOut");
            }
        }
    }

    private bool IsGeckoAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = geckoAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}
