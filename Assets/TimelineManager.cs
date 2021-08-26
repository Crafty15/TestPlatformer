using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    private bool fix = false;
    public Animator playerAnimator;
    public RuntimeAnimatorController pAnimController;
    public PlayableDirector director;
    // Start is called before the first frame update
    /*    public void Start() {
            director.Play();
        }*/
    public void Awake() {
        StarTimeline();
    }
    public void StarTimeline() {
        Debug.Log("Start timeline");
        director.Play();
    }
    
    void OnEnable()
    {
        pAnimController = playerAnimator.runtimeAnimatorController;
        playerAnimator.runtimeAnimatorController = null;
        /*StarTimeline();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix) {
            fix = true;
            playerAnimator.runtimeAnimatorController = pAnimController;
        } 
    }
}
