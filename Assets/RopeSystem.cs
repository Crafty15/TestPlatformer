using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour
{
    //Variables
    public bool swingEnabled = true;
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public PlayerMovement playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;
    //firing
    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;             //layer the rope can interact with
    private float ropeMaxCastDistance = 20f;    //max firing distance
    private List<Vector2> ropePositions = new List<Vector2>();
    //
    private bool distanceSet;
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    //rapelling
    public float climbSpeed = 10f;
    private bool isColliding;


    //disable rope joint and get the player position
    void Awake() {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    void Update() {
        //get the mouse cursor pos
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        //determine shooting direction
        Vector3 facingDirection = mousePosition - transform.position;
        //calculate the aiming angle
        float aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f) {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }
        //rotation
        Vector3 aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;

        if (!ropeAttached) {
            SetCrosshairPosition(aimAngle);
            playerMovement.isSwinging = false;
        }
        else {
            playerMovement.isSwinging = true;
            playerMovement.ropeHook = ropePositions.Last();
            crosshairSprite.enabled = false;
            //If rope positions has any values 
            if (ropePositions.Count > 0) {
                // fire a raycast from the players pos in the direction of player
                var lastRopePoint = ropePositions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);
                // if the raycast hits something
                if (playerToCurrentNextHit) {
                    var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                    if (colliderWithVertices != null) {
                        var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);
                        // check position is not being wrapped again, if so cut rope
                        if (wrapPointsLookup.ContainsKey(closestPointToHit)) {
                            ResetRope();
                            return;
                        }
                        // 
                        ropePositions.Add(closestPointToHit);
                        wrapPointsLookup.Add(closestPointToHit, 0);
                        distanceSet = false;
                    }
                }
            }

        }
        HandleInput(aimDirection);
        UpdateRopePositions();
        HandleRopeLength();
    }

    //set the crosshair position
    private void SetCrosshairPosition(float aimAngle) {
        if (!crosshairSprite.enabled) {
            crosshairSprite.enabled = true;
        }
        float x = transform.position.x + 1.5f * Mathf.Cos(aimAngle);
        float y = transform.position.y + 1.5f * Mathf.Sin(aimAngle);
        Vector3 crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    //
    private void HandleInput(Vector2 aimDirection) {
        if (Input.GetMouseButton(0)) {
            if (ropeAttached) {
                return;
            }
            ropeRenderer.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
            if (hit.collider != null) {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point)) {
                    //jump slightly to get the player off the ground before swinging
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    ropePositions.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    ropeJoint.enabled = true;
                    ropeHingeAnchorSprite.enabled = true;
                }
            }
            else {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }
        }
        if (Input.GetMouseButton(1)) {
            ResetRope();
        }
    }

    //
    private void ResetRope() {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerMovement.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        wrapPointsLookup.Clear();
    }

    //set rope vertex positions on the line renderer
    private void UpdateRopePositions() {
        // 1
        if (!ropeAttached) {
            return;
        }
        // 2
        ropeRenderer.positionCount = ropePositions.Count + 1;
        // 3
        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--) {
            if (i != ropeRenderer.positionCount - 1) {
                // if not the Last point of line renderer
                ropeRenderer.SetPosition(i, ropePositions[i]);
                // 4
                if (i == ropePositions.Count - 1 || ropePositions.Count == 1) {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1) {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet) {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet) {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last())) {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet) {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else {
                // 6
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }


    //Handles wrapping of the rope (or will, when it works)
    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider) {
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }


    private void HandleRopeLength() {
        //look for input and change the rope length as needed
        if (Input.GetAxis("Vertical") >= 1f && ropeAttached && !isColliding) {
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetAxis("Vertical") < 0f && ropeAttached) {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
    }
    //If a collider is currently set colliding to true
    void OnTriggerStay2D(Collider2D colliderStay) {
        isColliding = true;
    }
    //if collider is not touching another
    private void OnTriggerExit2D(Collider2D colliderOnExit) {
        isColliding = false;
    }

}
