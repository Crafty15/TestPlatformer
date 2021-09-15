using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour{
    private float startY, startX;
    public GameObject follow_object;        //usually the camera
    public float parallaxEffect;
    public bool lockY = false;
    // Start is called before the first frame update
    void Start(){
        startX = transform.position.x;
        startY = transform.position.y;
        //length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate(){
        //how far we moved in world space
        float dist = (follow_object.transform.position.x * parallaxEffect);
        //move camera
        if (lockY) {
            transform.position = new Vector3(startX + dist, startY, transform.position.z);
        }
        else {
            transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);
        }
        
    }
}
