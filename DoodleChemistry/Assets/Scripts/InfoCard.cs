using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoCard : MonoBehaviour
{
    public void NextLevel()
    {
        int nextScene = (SceneManager.GetActiveScene().buildIndex+1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }
}
