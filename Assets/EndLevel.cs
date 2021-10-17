using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private string sceneName;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision) {
        player = GameObject.Find("Player");
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Trigger");
        Debug.Log(sceneName + "Complete");
        player.GetComponent<InteractionSystem>().SavePlayerData();
        //if coming from a level, increment the level counter and load the end level scene
        if (sceneName.Equals("Level1") || sceneName.Equals("Level2") || sceneName.Equals("Level3")) {
            GlobalController.IncrementLevel();
            Debug.Log(GlobalController.GetLevel());
            SceneManager.LoadScene("EndLevel");        
        }
        //if coming from endlevel scene, check level counter and load appropriate level
/*        else if (sceneName.Equals("EndLevel")) {
            if (GlobalController.GetLevel() == 1) {
                SceneManager.LoadScene("Level2");
            }
            else if (GlobalController.GetLevel() == 2) {
                SceneManager.LoadScene("Level3");
            }
        }*/
        else {
            SceneManager.LoadScene("MainMenu");
        }

    }

    public void Continue() {
        if (GlobalController.GetLevel() == 1) {
            SceneManager.LoadScene("Level2");
        }
        else if (GlobalController.GetLevel() == 2) {
            SceneManager.LoadScene("Level3");
        }
        else {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
