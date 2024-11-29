using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public string deathTag = "Death";
    public string nextTag = "Next";
    public string completionTag = "End";
    public string GameOver = "GameOverScene";
    public string endSceneName = "EndScene";
    public string nextLevel = "Level2";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(deathTag))
        {
            SceneManager.LoadScene(GameOver);
        }
        else if (collision.gameObject.CompareTag(completionTag))
        {
            SceneManager.LoadScene(endSceneName);
        }
        else if (collision.gameObject.CompareTag(nextTag)) 
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
