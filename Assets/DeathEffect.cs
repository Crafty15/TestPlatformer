using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEffect : MonoBehaviour
{

    public GameObject panel;
    private float fadeCounter = 0;
    public float fadeDuration;
    public float fadeBarrier;   //The point at which the transparency will stop and reverse
    //bool fromWhite = true;      //want to start from white and then to black by changing the transparency

    void Update(){
        //incrementally reduce the color values to fade the background
        if (fadeCounter < fadeDuration) {
            //panel.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, fadeCounter / fadeDuration);
            float lerpVal = Mathf.Lerp(1f, 0f, fadeCounter / fadeDuration);
            float transparencyLerpVal = 1f;
            if (lerpVal < fadeBarrier) {
                //decrease transparency
                //transparencyLerpVal = Mathf.Lerp(1f, fadeBarrier, fadeCounter / fadeDuration);
                //increase transparency
                transparencyLerpVal = Mathf.Lerp(fadeBarrier, 1f, fadeCounter / fadeDuration);
            }
            else {
                //increase transparency
                //transparencyLerpVal = Mathf.Lerp(fadeBarrier, 1f, fadeCounter / fadeDuration);
                //decrease transparency
                transparencyLerpVal = Mathf.Lerp(1f, fadeBarrier, fadeCounter / fadeDuration);
            }
            panel.GetComponent<Image>().color = new Color(lerpVal, lerpVal, lerpVal, transparencyLerpVal);

            fadeCounter += Time.deltaTime;
        }
    }
}
