using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartButton : MonoBehaviour
{
    public void Gamestart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOut()
    {
        Application.Quit();
    }
}
