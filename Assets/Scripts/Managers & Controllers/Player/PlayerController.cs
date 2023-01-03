using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class PlayerController : MonoBehaviour
{
    // Serialized fields:
    [BoxGroup("Physics")] [SerializeField] Rigidbody2D RB;
    [BoxGroup("Physics")] [SerializeField] Transform groundCheck;
    [BoxGroup("Physics")] [SerializeField] Transform ceilingCheck;
    [BoxGroup("Physics")] [SerializeField] Animator standingSpriteAnimator;
    [BoxGroup("Physics")] [SerializeField] Animator ballSpriteAnimator;

    [BoxGroup("Modes")] [SerializeField] GameObject standing;
    [BoxGroup("Modes")] [SerializeField] GameObject crouch;
    [BoxGroup("Modes")] [SerializeField] GameObject ball;

    [BoxGroup("After-image")] [SerializeField] SpriteRenderer playerSR;
    [BoxGroup("After-image")] [SerializeField] SpriteRenderer afterImageSR;
    [BoxGroup("After-image")] [SerializeField] Color afterImageColor;

    [BoxGroup("Floats")] [SerializeField] float moveSpeed;
    [BoxGroup("Floats")] [SerializeField] float jumpForce;
    [BoxGroup("Floats")] [SerializeField] float dashSpeed;
    [BoxGroup("Floats")] [SerializeField] float dashTime;
    [BoxGroup("Floats")] [SerializeField] float afterImageLifeTime;
    [BoxGroup("Floats")] [SerializeField] float timeBetweenAfterImages;
    [BoxGroup("Floats")] [SerializeField] float dashWait;
    [BoxGroup("Floats")] [SerializeField] float waitToBall;

    [Space]
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform standingFirepoint;
    [SerializeField] Transform crouchingFirepoint;
    [SerializeField] Transform bombPoint;

    [Title("LayerMasks")]
    [SerializeField] LayerMask whatIsGround;

    [Title("Scriptable Events")]
    [SerializeField] SFXDataScriptableEvent sfxEvent;

    // Private fields:
    PlayerAbilityTracker playerAbilityTracker;

    float dashCounter;
    float dashRechargeCounter;
    float afterImageCounter;
    float ballCounter;
    bool isOnGround;
    bool canDoubleJump;
    bool canMove;

    // Public properties:
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public Animator StandingSpriteAnimator
    {
        get { return standingSpriteAnimator; }
    }

    public Rigidbody2D RigidBody
    {
        get { return RB; }
    }

    private void OnEnable()
    {
        canMove = true;
        playerAbilityTracker = GetComponent<PlayerAbilityTracker>();
    }

    void Update()
    {
        if (!UIController.instance.isGamePaused && !UIController.instance.FullscreenMap.activeInHierarchy)
        {
            #region Movement
            if (canMove)
            {
                // movement only allowed if the player isn't crouched.
                // will update later with crouch animation:
                if (!crouch.activeSelf)
                {
                    // ===================================== MOVING (LEFT/RIGHT) =====================================:
                    //--> Dashing:
                    if (dashRechargeCounter > 0)
                        dashRechargeCounter -= Time.deltaTime;

                    else
                    {
                        // if RMB is pressed, player is standing & dash ability has been unlocked:
                        if (Input.GetButtonDown("Fire2") && standing.activeSelf && playerAbilityTracker.abilityTrackerSO.DashUnlocked)
                        {
                            dashCounter = dashTime;
                            ShowAfterImage();

                            // dash SFX:
                            sfxEvent.Raise(new SFXData(_sfxIndex: 7, _adj: true));
                        }
                    }

                    if (dashCounter > 0)
                    {
                        dashCounter -= Time.deltaTime;
                        RB.velocity = new Vector2(dashSpeed * transform.localScale.x, RB.velocity.y);

                        // --> Showing After-images:
                        afterImageCounter -= Time.deltaTime;
                        if (afterImageCounter <= 0)
                            ShowAfterImage();

                        dashRechargeCounter = dashWait; // when the player has dashed once, don't let them dash again immediately.
                                                         // instead, have a recharge timer in place
                    }

                    // --> Move normally if the player isn't dashing already:
                    else
                    {
                        // Input.GetAxisRaw used to get immediate snappy movement (without gradual smoothing)
                        float xMovement = Input.GetAxisRaw("Horizontal");
                        RB.velocity = new Vector2(x: xMovement * moveSpeed, y: RB.velocity.y);

                        // Flipping character when dir. is changed by adjusting its localScale:
                        if (RB.velocity.x < 0)
                            transform.localScale = new Vector3(-1, 1, 1);

                        else if (RB.velocity.x > 0)
                            transform.localScale = Vector3.one;
                    }

                    // the value saved in isOnGround will be determined by drawing a invisible circle which, when it'll overlap with
                    // the ground, will return a true/false value:
                    isOnGround = Physics2D.OverlapCircle(point: groundCheck.position, radius: .2f, layerMask: whatIsGround);

                    // ===================================== JUMPING =====================================
                    // mapped to the space button                     // if the player has already jumped and double jump ability has been unlocked:
                    if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && playerAbilityTracker.abilityTrackerSO.DoubleJumpUnlocked)))
                    {
                        // if the player is on the ground, they can double jump:
                        if (isOnGround)
                        {
                            canDoubleJump = true;

                            // jump SFX:
                            sfxEvent.Raise(new SFXData(_sfxIndex: 12, _adj: true));
                        }

                        // but not in the air or they'll keep jumping infinitely:
                        else
                        {
                            canDoubleJump = false;
                            standingSpriteAnimator.SetTrigger("doubleJump");

                            // DJ SFX:
                            sfxEvent.Raise(new SFXData(_sfxIndex: 9, _adj: true));
                        }

                        RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                    } 
                }

                // ===================================== BALL/STANDING/CROUCH MODE =====================================:
                if (!ball.activeSelf)
                {
                    float yMovement = Input.GetAxisRaw("Vertical");

                    if (yMovement < -.9f && playerAbilityTracker.abilityTrackerSO.BallModeUnlocked)
                    {
                        ballCounter -= Time.deltaTime;

                        if (ballCounter <= 0)
                        {
                            // turn to ball...
                            ball.SetActive(true);
                            standing.SetActive(false);

                            // ball SFX:
                            sfxEvent.Raise(new SFXData(_sfxIndex: 6));
                        }
                    }

                    // --> CROUCH:
                    else if (Input.GetKeyDown(KeyCode.C) && !crouch.activeSelf && isOnGround)
                    {
                        ball.SetActive(false);
                        standing.SetActive(false);
                        crouch.SetActive(true);
                    }

                    else if (crouch.activeSelf)
                    {
                        float crouchedHorizontalMovement = Input.GetAxisRaw("Horizontal");

                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            ball.SetActive(false);
                            standing.SetActive(true);
                            crouch.SetActive(false); 
                        }

                        // pressing left or right while crouched:
                        if (crouchedHorizontalMovement == 1)
                            transform.localScale = Vector3.one;

                        else if (crouchedHorizontalMovement == -1)
                            transform.localScale = new Vector3(-1, 1, 1);
                    }

                    else
                        ballCounter = waitToBall;
                }

                // if the player's already in ball mode:
                else
                {
                    float yMovement = Input.GetAxisRaw("Vertical");

                    if (yMovement > .9f)
                    {
                        ballCounter -= Time.deltaTime;

                        if (ballCounter <= 0)
                        {
                            // stand back up...
                            ball.SetActive(false);
                            standing.SetActive(true);

                            // stand SFX:
                            sfxEvent.Raise(new SFXData(_sfxIndex: 10));
                        }
                    }

                    else
                        ballCounter = waitToBall;
                }
            }

            else
                RB.velocity = Vector2.zero;

            // ===================================== UPDATING ANIMATIONS =====================================:
            if (standing.activeSelf)
            {
                // if the isOnGround bool in the script is true, it'll also set the animator's paramater with the same name to true,
                // and the jump anim will play:
                standingSpriteAnimator.SetBool("isOnGround", isOnGround);

                // if the abs. value of the velocity on the x-axis is > 0.1, the run anim. is played:
                standingSpriteAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x));
            }

            if (ball.activeSelf)
                ballSpriteAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x));
            #endregion

            #region Shooting

            #region Commented Shoot Up Code
            //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mousePosition.z = 0;

            //if (mousePosition.y > ceilingCheck.position.y + 2)
            //{
            //    standing.SetActive(false);
            //    //ball.SetActive(false);
            //    shootUp.SetActive(true);
            //}

            //else
            //{
            //    standing.SetActive(true);
            //    ball.SetActive(false);
            //    shootUp.SetActive(false);
            //} 
            #endregion

            if (Input.GetButtonDown("Fire1"))
            {
                if (standing.activeSelf || crouch.activeSelf)
                {
                    if (standing.activeSelf)
                    {
                        // the bullet is going to be flipped WRT to the player's dir., as well:
                        Instantiate(bulletPrefab, standingFirepoint.position, standingFirepoint.rotation).moveDir = new Vector2(transform.localScale.x, 0);
                        standingSpriteAnimator.SetTrigger("shotFired"); 
                    }

                    else if (crouch.activeSelf)
                        Instantiate(bulletPrefab, crouchingFirepoint.position, crouchingFirepoint.rotation).moveDir = new Vector2(transform.localScale.x, 0);

                    // bullet SFX:
                    sfxEvent.Raise(new SFXData(_sfxIndex: 14, _adj: true));
                }

                // if the player's in ball mode, the LMB will drop bombs:
                else if (ball.activeSelf && playerAbilityTracker.abilityTrackerSO.DropBombsUnlocked)
                {
                    Instantiate(bombPrefab, bombPoint.position, bombPoint.rotation);

                    // mine drop SFX:
                    sfxEvent.Raise(new SFXData(_sfxIndex: 13, _adj: true));
                }
            }
            #endregion 
        }
    }

    public void JumpOffPad(float jumpForce) => RB.velocity = new Vector2(RB.velocity.x, jumpForce);

    void ShowAfterImage()
    {
        SpriteRenderer afterImage = Instantiate(original: afterImageSR, position: transform.position, rotation: transform.rotation);
        afterImage.sprite = playerSR.sprite;
        afterImage.transform.localScale = transform.localScale;
        afterImage.color = afterImageColor;

        Destroy(afterImage.gameObject, afterImageLifeTime);
        afterImageCounter = timeBetweenAfterImages;
    }
}
