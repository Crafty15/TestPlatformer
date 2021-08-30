using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
	[SerializeField] private float m_JumpForce = 1000f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private bool canStand = true;
	//wall climb stuff
	[SerializeField] private LayerMask whatIsWall;
	[SerializeField] private float m_WallJumpForceY = 20f;
	[SerializeField] private float m_WallJumpForceX = 5f;
	[SerializeField] private float m_wallJumpTime = .3f;
	const float wallGrabRadius = .2f;
	public Transform wallGrabPoint;
	private bool m_canWallGrab;
	private bool m_isWallGrab;
	private float wallJumpCounter;
	private float gravStore;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake() {
		m_JumpForce = 1000f;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
    private void Start() {
		gravStore = m_Rigidbody2D.gravityScale;
    }
    private void FixedUpdate() {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;		
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

    public void Move(float move, bool crouch, bool jump) {
        //Wall jump related
        if (wallJumpCounter <= 0) {
			// If crouching, check to see if the character can stand up
			if (crouch) {
				// If the character has a ceiling preventing them from standing up, keep them crouching
				if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) {
					crouch = true;
					canStand = false;
				}
                else {
					canStand = true;
                }
			}
			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl) {

				// If crouching
				if (crouch) {
					if (!m_wasCrouching) {
						m_wasCrouching = true;
						OnCrouchEvent.Invoke(true);
					}

					// Reduce the speed by the crouchSpeed multiplier
					move *= m_CrouchSpeed;

					// Disable one of the colliders when crouching
					if (m_CrouchDisableCollider != null)
						m_CrouchDisableCollider.enabled = false;
				}
				else {
					// Enable the collider when not crouching
					if (m_CrouchDisableCollider != null)
						m_CrouchDisableCollider.enabled = true;

					if (m_wasCrouching) {
						m_wasCrouching = false;
						OnCrouchEvent.Invoke(false);
					}
				}

				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight) {
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight) {
					// ... flip the player.
					Flip();
				}
			}
            //****Regular jump ****
            if (m_Grounded && jump) {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            //****Wall Jump *****
            m_isWallGrab = false;
            m_canWallGrab = Physics2D.OverlapCircle(wallGrabPoint.position, wallGrabRadius, whatIsWall);
            //check if against wall - if facing right and pushing right OR facing left and pushing left
            if (m_canWallGrab && !m_Grounded) {
                //Debug.Log("canWallGrab && !grounded");
                if ((transform.localScale.x >= 1f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x <= -1f && Input.GetAxisRaw("Horizontal") < 0)) {
                    m_isWallGrab = true;
                    //Debug.Log("wallGrabbing");
                }
            }
            //what happens when we are wall grabbing?
            if (m_isWallGrab) {
                m_Rigidbody2D.gravityScale = 0f;
                m_Rigidbody2D.velocity = Vector2.zero;
                //Debug.Log("IsWallGrab");

                //wall jump
                if (jump) {
                    wallJumpCounter = m_wallJumpTime;
                    //Debug.Log("IsWallGrab AND JUMP");
                    m_Rigidbody2D.velocity = new Vector2(-move * m_WallJumpForceX, m_WallJumpForceY);
                    //m_Rigidbody2D.AddForce(new Vector2(-move, m_JumpForce));
                    m_Rigidbody2D.gravityScale = gravStore;
                    m_isWallGrab = false;
                }
            }
            else {
                m_Rigidbody2D.gravityScale = gravStore;
            }
        }
        else {
            wallJumpCounter -= Time.deltaTime;
        }
    }



    private void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool isGrounded() {
        return m_Grounded;
    }

    public bool isWallGrab() {
        return m_isWallGrab;
    }

	public bool checkCanStand() {
		return canStand;
    }


/*    public void WallJump(float hMoveSpeed) {
        //****Wall Jump stuff *****
        m_isWallGrab = false;
        m_canWallGrab = Physics2D.OverlapCircle(wallGrabPoint.position, wallGrabRadius, whatIsWall);
        //check if against wall - if facing right and pushing right OR facing left and pushing left
        if (m_canWallGrab && !m_Grounded) {
            Debug.Log("canWallGrab && !grounded");
            if ((transform.localScale.x >= 1f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x <= -1f && Input.GetAxisRaw("Horizontal") < 0)) {
                m_isWallGrab = true;
                *//*Debug.Log("wallGrabbing");
			}
		}
		//what happens when we are wall grabbing?
		if (m_isWallGrab) {
			m_Rigidbody2D.gravityScale = 0f;
			m_Rigidbody2D.velocity = Vector2.zero;
			Debug.Log("IsWallGrab");

			//wall jump
			if (Input.GetButtonDown("Jump")) {
				wallJumpCounter = m_wallJumpTime;
				Debug.Log("IsWallGrab AND JUMP");
				/*m_Rigidbody2D.velocity = new Vector2( -Input.GetAxisRaw("Horizontal") * move, m_JumpForce);*//*
                m_Rigidbody2D.AddForce(new Vector2(-Input.GetAxisRaw("Horizontal") * hMoveSpeed, m_JumpForce));
				m_Rigidbody2D.gravityScale = gravStore;
				m_isWallGrab = false;
			}
		}
		else {
			m_Rigidbody2D.gravityScale = gravStore;
		}
	}*/

}