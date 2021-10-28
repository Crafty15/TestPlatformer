using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame() {
        Debug.Log("Loading scene 1");
        GlobalController.SetLevel(1);
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame() {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
