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
        if (sceneName.Equals("Level1")) {
            SceneManager.LoadScene("EndLevel");
            
        }
/*        switch (sceneName) {
            case "Level1":
                SceneManager.LoadScene("EndLevel");
                break;
            case "EndLevel":
                SceneManager.LoadScene("MainMenu");
                break;
        }*/
        
    }

    public void Continue() {
        SceneManager.LoadScene("MainMenu");
    }

}
