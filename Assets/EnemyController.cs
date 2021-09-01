using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    public AIPath bobbyAI;
    Vector3 lScale;             //use this to flip sprite
    private bool isFacingRight = true;
    public Animator enemyAnimator;
    public float radius = 20f;
    private GameObject playerT;
    public float viewDist = 10f;
    //Point for start of enemies line of sight
    [SerializeField] private Transform castPoint;
/*    private Rigidbody2D enemyRB;*/

    public void Awake() {
        lScale = transform.localScale;
        playerT = GameObject.Find("Player");
        //try setting the AI destination to something arbitrary on start up 
        bobbyAI.destination = playerT.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //pick between chasing the player and wandering
        if (CanSeePlayer(viewDist)) {
            //Debug.Log("Can see player");
            //set player as target
            bobbyAI.destination = playerT.transform.position;
            //bobbyAI.maxSpeed = 4f;
            //Debug.Log("Can see player");
            enemyAnimator.SetBool("canSeePlayer", true);
        }
        else {
            //Debug.Log("Cant see player");
            //set to wander
            //If AI is not already calculating a path and has reached end of path or has no path at all
            //Debug.Log("Path pending: " + bobbyAI.pathPending);
            //Debug.Log("Reached end of path: " + bobbyAI.reachedEndOfPath);
            //Debug.Log("Has path: " + bobbyAI.hasPath);

            if (!bobbyAI.pathPending && (bobbyAI.reachedEndOfPath || !bobbyAI.hasPath)) {
                //Debug.Log("Cannot see player");
                bobbyAI.destination = PickRandomPoint();
                //bobbyAI.maxSpeed = 1.5f;
                bobbyAI.SearchPath();
                enemyAnimator.SetBool("canSeePlayer", false);
            }
        }
        //if we run into a wall, piack another destination
        if (CanSeeWall(viewDist)) {
            Debug.Log("Bobby saw a wall");
            bobbyAI.destination = PickRandomPoint();
            bobbyAI.SearchPath();
        }
        //Debug.Log("AIobject found? " + bobbyAI.name);
        //Debug.Log("Player object found? " + playerT.name);

        //give the animator the ai speed
        enemyAnimator.SetFloat("enemySpeed", Mathf.Abs(bobbyAI.desiredVelocity.x));
        //is moving right
        if (bobbyAI.desiredVelocity.x > 0f && !isFacingRight) {
            /*lScale.x *=*/
            Flip();
        }
        //moving left
        else if (bobbyAI.desiredVelocity.x < 0f && isFacingRight) {
            Flip();
        }
        //Debug.Log("Bobby is heading to: " + bobbyAI.destination);

    }

    bool CanSeePlayer(float viewDist) {
        //Debug.Log("Checking for player");
        //Debug.Log("Cast point val: " + castPoint.position);

        //Debug.DrawLine();
        if (!isFacingRight) {
            viewDist = -viewDist;
        }
        Vector2 endPos = castPoint.position + Vector3.right * viewDist;
        //Debug.Log("end point val: " + endPos);
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));
        //Debug.Log("Ray is hitting: "+hit.collider);
        //check if found player
        Debug.DrawLine(castPoint.position, endPos, Color.blue);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                //chase the player
                return true;
            }
        }
        return false;
    }

    bool CanSeeWall(float viewDist) {
        //Debug.Log("Checking for wall");
        if (!isFacingRight) {
            viewDist = -viewDist;
        }
        Vector2 endPos = castPoint.position + Vector3.right * (viewDist/2);
        //Debug.Log("end point val: " + endPos);
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));
        //Debug.Log("Ray is hitting: "+hit.collider);
        //check if found wall
        Debug.DrawLine(castPoint.position, endPos, Color.red);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Walls")) {
                //Can see a wall
                return true;
            }
        }
        return false;
    }


    Vector3 PickRandomPoint() {
        //Debug.Log("Picking random point");
        Vector3 point = Random.insideUnitSphere * radius;
        point.y = 0;
        point += bobbyAI.position;
        return point;
    }
    private void Flip() {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;
        // Multiply the player's x local scale by -1.
        lScale.x *= -1;
        transform.localScale = lScale;
    }


}
