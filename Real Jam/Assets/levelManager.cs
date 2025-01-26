using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Game Over: The bubble touched something!");
        gameStatus = "lose";
        UI.SetTrigger("GameOver");
        gameObject.SetActive(false);
    }

    public void success()
    {
        Debug.Log("Win");
        gameStatus = "win";
        UI.SetTrigger("Win");
    }
}
