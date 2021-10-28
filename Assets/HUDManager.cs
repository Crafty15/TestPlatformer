using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI loot;
    public GameObject player;
    private int currentHealth = 3;
    private int currentLoot = 0;
    void Start(){

        //player.GetComponent<CharacterController2D>().
    }

    // Update is called once per frame
    void Update(){
        int checkHealth = player.GetComponent<CharacterController2D>().getHealth();
        if (checkHealth != currentHealth) {
            UpdateHealth(checkHealth);
            currentHealth = checkHealth;
        }
        int checkLoot = player.GetComponent<InteractionSystem>().GetItemCount();
        if (checkLoot != currentLoot) {
            UpdateLoot(checkLoot);
        }
        
    }

    public void UpdateHealth(int value) {
        health.text = "x " + value;
    }

    public void UpdateLoot(int value) {
        loot.text = "x " + value;
    }
}
