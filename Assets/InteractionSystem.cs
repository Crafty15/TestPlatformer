using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    public Transform detectionPoint;                //detection point
    private const float detectionRadius = 0.2f;     //detection radius
    public LayerMask detectionLayer;                //detection layer
    public GameObject detectedObject;               //Cached trigger object
    [Header("Examine Fields")]
    public GameObject examineWindow;                //Examine window object
    public Image examineImage;
    public Text examineText;
    public bool isExamining = false;
    [Header("Others")]
    public List<GameObject> pickedItems = new List<GameObject>(); //Picked up item list

    void Start() {
        //examineWindow.SetActive(false);
    }
    void Update(){
        if (DetectObject()) {
            if (InteractInput()) {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
        else if (examineWindow.activeSelf) {
            //close examine window
            examineWindow.SetActive(false);
            //disable boolean
            isExamining = false;
        }
    }

    bool InteractInput() {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject() {
        
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if (obj == null) {
            detectedObject = null;
            return false;
        }
        else {
            detectedObject = obj.gameObject;
            Debug.Log("Detecting object");
            return true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);

    }

    public void PickUpItem(GameObject item) {
        pickedItems.Add(item);
    }

    public void ExamineItem(Item item) {
        if (isExamining || !DetectObject()) {
            //close examine window
            examineWindow.SetActive(false);
            //disable boolean
            isExamining = false;
        }
        else {
            //Show the item's image in the middle of the window
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            //Write a description 
            examineText.text = item.descriptionText;
            //Display the examine window. Note: best to render this last when things are set up
            examineWindow.SetActive(true);
            //enable boolean
            isExamining = true;
        }
    }

}
