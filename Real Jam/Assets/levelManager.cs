using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public string gameStatus = "progressing";
    public Animator UI;

    public void nextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels to load! You're at the last stage.");
        }
    }

    public void restartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void gameOver()
    {
        HandleGameState("GameOver", "Fail", "lose", 5f);
    }

    public void success()
    {
        HandleGameState("Win", "Success", "win", 2f);
    }

    private void HandleGameState(string animationTrigger, string soundName, string status, float soundVolume)
    {
        
        gameStatus = status;
        UI.SetTrigger(animationTrigger);
        StartCoroutine(PlayAudioAfterAnimation(animationTrigger, soundName, soundVolume));
    }

    private IEnumerator PlayAudioAfterAnimation(string animationName, string soundName, float soundVolume)
    {
        Debug.Log("Playing");
        AnimatorStateInfo stateInfo = UI.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName(animationName) == false || stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = UI.GetCurrentAnimatorStateInfo(0);
        }

        // Use the SoundEffectManager to play the sound
        SoundEffectManager.Instance.PlaySound(soundName, soundVolume, 1f);
    }
}
