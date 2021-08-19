using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour{
    public GameObject followObject;         //the object that the camera follows
    public Vector2 followOffset;            //the area of screen that the object can move before cam follows
    public Vector2 threshold;               //followoffset - screen size creates a "bounding box" 
    public float speed = 3f;
    private Rigidbody2D playerRef;
    // Start is called before the first frame update
    void Start(){
        threshold = calcThreshold();
        playerRef = followObject.GetComponent<Rigidbody2D>();
    }

    // Update
    void FixedUpdate(){
        //Define the follow objects position
        Vector2 follow = followObject.transform.position;
        //Tracks the distance character is from center of the x axis
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        //check if distance exceeds threshold boundary, to decide if character should move
        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x) {
            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDifference) >= threshold.y) {
            newPosition.y = follow.y;
        }
        //default cam speed
        float moveSpeed = playerRef.velocity.magnitude > speed ? playerRef.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    //calcs the threshold of.....
    private Vector3 calcThreshold() {
        //define the aspect ratio of the camera
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    //Built in function that shows where the boundary box is
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector2 border = calcThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
