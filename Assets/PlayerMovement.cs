using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D playerRB;
    public float runSpeed = 40f;
    public float walkSpeed = 10f;
    float hMove = 0f;       //horizontal movement
    bool jump = false;
    bool crouch = false;
    bool grounded = true;
    bool wallGrab = false;
    //Sound stuff
    [SerializeField]public AudioSource[] sounds;
    //rope
    public bool isSwinging = false;
    public Vector2 ropeHook;
    public float swingForce = 5f;


    // Start is called before the first frame update
    void Start(){
        sounds = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (CanMove()) {
            //use update to get input from player
            hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            //give the animator the run speed
            animator.SetFloat("playerSpeed", Mathf.Abs(hMove));
            if (Input.GetButtonDown("Jump") && !isSwinging) {
                jump = true;
                animator.SetBool("isJumping", true);
            }
            if (Input.GetButton("Crouch")) {
                crouch = true;
            }
            else {
                crouch = !canStand();
            }
            CheckGrounded();
            checkWallGrab();

            //do the swinging stuff
            if (isSwinging) {
                animator.SetBool("isSwinging", true);
                // 1 - Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (hMove < 0) {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                playerRB.AddForce(force, ForceMode2D.Force);
            }
            else {
                animator.SetBool("isSwinging", false);
/*                if (grounded) {
                    var groundForce = speed * 2f;
                    rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                }*/
            }
        }

    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
    }

    public void CheckGrounded() {
        grounded = controller.isGrounded();
        animator.SetBool("isGrounded", grounded);
       // OnLanding();
    }

    public void checkWallGrab() {
        wallGrab = controller.isWallGrab();
        animator.SetBool("isWallGrab", wallGrab);
    }
    public bool canStand() {
        return controller.checkCanStand();
    }

    public void Hide() {
        gameObject.layer = 0; 
    }
    public void Unhide() {
        gameObject.layer = 10;
    }

    //NOTE: Check which controls can be moved into CharacterController
    public void OnCrouching(bool isCrouching) {
        animator.SetBool("isCrouching", isCrouching);
    }

    private void FixedUpdate() {
        //use fixed update to apply input to character
        if (CanMove()) {
            controller.Move(hMove * Time.deltaTime, crouch, jump);
            jump = false;
        }
    }
    //stops movement if the menu is open or similar
    bool CanMove() {
        bool canMove = true;
        if (FindObjectOfType<InteractionSystem>().isExamining) {
            canMove = false;
        }
        return canMove;
    }

    //Sounds related
    private void FootStep() {
        sounds[0].Play();
    }
    private void Jump() {
        sounds[1].Play();
    }
    private void CrouchWalk() {
        sounds[2].Play();
    }





}
