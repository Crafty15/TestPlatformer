using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{ 
    public enum InteractionType { NONE, PickUp, Examine};       //Interaction types
    public InteractionType type;                                //Class interaction type
    [Header("Examine")]
    public string descriptionText;
    public Sprite image;

/*    private void Awake() {
        DontDestroyOnLoad(this);
    }*/
    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    //interaction method
    public void Interact() {
        switch (type) {
            case InteractionType.PickUp:
                //Add item to the PickedUpItem list
                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
                //Disable object on pickup
                gameObject.SetActive(false);
                Debug.Log("Pickup");
                break;
            case InteractionType.Examine:
                //Call the examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                Debug.Log("Examine");
                break;
            default:
                Debug.Log("Null item");
                break;
        }
    }
}
