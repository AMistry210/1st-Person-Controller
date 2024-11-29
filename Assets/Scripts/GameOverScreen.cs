using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

