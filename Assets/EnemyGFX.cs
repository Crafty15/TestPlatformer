using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyGFX : MonoBehaviour
{
    public AIPath bobbyAI;
    Vector3 lScale;             //use this to flip sprite

/*    private Rigidbody2D enemyRB;*/

    public void Awake() {
        lScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //is moving right
        if (bobbyAI.desiredVelocity.x >= 0.01f) {
            /*lScale.x *=*/
            transform.localScale.x;
        }
        //moving left
        else if (bobbyAI.desiredVelocity.x >= -0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


}
