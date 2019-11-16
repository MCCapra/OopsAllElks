using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startSceneManager : MonoBehaviour
{

    public void goToCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void goToGame()
    {
        SceneManager.LoadScene("Level 0");
    }

    public void Quit()
    {
        Application.Quit();
    }
}