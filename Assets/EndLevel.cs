using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private string sceneName = SceneManager.GetActiveScene().name;
    //private GameObject player = GameObject.Find("Player");
    
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger");
        Debug.Log(sceneName + "Complete");
    }
}
