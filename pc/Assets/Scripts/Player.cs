using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamId;

    [SerializeField] private int _id = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask moduleLayer;
    [SerializeField] private LayerMask crewmateLayer;
    public Module usedModule { get; private set; } = null;
    private float moveInput = 0f;

    public float maxSpeed = 2f;
    public float moveForce = 10f;

    private Transform groundCheck;
    public static float timeToApex = 4f;
    public static float timeOfFalling = 2f;
    public static float jumpHeight = 20f;
    private bool quickFall = false;
    private float timeOfLastJump = 0f;
    public static float jumpBuffering = 0.1f;

    private Rigidbody2D rb;
    private float timeOfLastGrounded = 0f;
    public static float coyoteTime = 0.1f;

    private static float gravityScaleUp => (2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
    private static float gravityScaleDown => (2 * jumpHeight) / Mathf.Pow(timeOfFalling, 2);
    private static float jumpVelocity => Mathf.Abs(gravityScaleUp * timeToApex);

    public static float deadZone = 0.1f;
    public static float maxZone = 0.4f;

    private float inputTranslation(float input)
    {
        float absInput = Mathf.Abs(input);
        if (absInput < deadZone) {
            return 0f;
        } else if (absInput < maxZone) {
            return (absInput - deadZone) / (maxZone - deadZone) * Mathf.Sign(input);
        } else {
            return Mathf.Sign(input);
        }
    }

    private bool IsGrounded()
    {
        bool standingOnGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.38f, 0.01f), 0f, groundLayer);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(0.38f, 0.01f), 0f, crewmateLayer);
        bool standingOnCrewmate = false;
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject != gameObject) {
                standingOnCrewmate = true;
                break;
            }
        }

        return standingOnGround || standingOnCrewmate;
    }

    void Start()
    {

        ModuleEventManager.instance.teamIds.Add(_id, teamId);
        groundCheck = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody2D>();

        CrewmateEventManager.instance.onCrewmateButtonAPushed.AddListener(
            (id) => {
                if (id != _id) return;
                JumpStart();
            }
        );

        CrewmateEventManager.instance.onCrewmateButtonAReleased.AddListener(
            (id) => {
                if (id != _id) return;
                JumpEnd();
            }
        );

        CrewmateEventManager.instance.onCrewmateButtonBPushed.AddListener(
            (id) => {
                if (id != _id) return;
                bool success = Interact();
                if (success) {
                    if (usedModule.type == Module.Type.MOVEMENT_MODULE) {
                        MovementModule movementModule = (MovementModule)usedModule;
                        Dictionary<string, string> dict = new Dictionary<string, string>
                        {
                            { "steering_pos", movementModule.GetRotationSliderPosition().ToString() },
                            { "speed_pos", movementModule.GetSpeedLeverPosition().ToString() }
                        };
                        ModuleEventManager.instance.onModuleEntered.Invoke(_id, usedModule.type, dict);
                    } else if (usedModule.type == Module.Type.CANNON_MODULE) {
                        ShootingModule cannonModule = (ShootingModule)usedModule;
                        Dictionary<string, string> dict = new Dictionary<string, string>
                        {
                            { "fire_cooldown", "3.0" },
                            { "ammo", "5" },
                            { "chamber_open", cannonModule.IsCannonClosed().ToString() },
                            { "chamber_loaded", cannonModule.IsCannonLoaded().ToString() }
                        };
                        ModuleEventManager.instance.onModuleEntered.Invoke(_id, usedModule.type, dict);
                    }
                    else if(usedModule.type == Module.Type.GYROSCOPE_MODULE)
                    {
                        GyroscopeModule gyroscopeModule = (GyroscopeModule)usedModule;
                        Dictionary<string, string> dict = new Dictionary<string, string>
                        {
                            {"gyroscope_calibration", gyroscopeModule.GetGyroscopeDecalibration().ToString()}
                        };
                        ModuleEventManager.instance.onModuleEntered.Invoke(_id, usedModule.type, dict);
                    }
                }
            }
        );
        
        CrewmateEventManager.instance.onCrewmateMoveInputUpdate.AddListener(
            (id, inputX, inputY) => {
                if (id != _id) return;
                UpdateMoveInput(inputX);
            }
        );

        ModuleEventManager.instance.onMinigameAborted.AddListener(
            (id) => {
                if (id != _id) return;
                FinishInteraction();
            }
        );

    }

    void FixedUpdate()
    {
        if (IsGrounded()) {
            timeOfLastGrounded = Time.time;
        }

        if (timeOfLastJump + jumpBuffering > Time.time && timeOfLastGrounded + coyoteTime > Time.time) {
            Jump();
        }
        
        float input = inputTranslation(moveInput);
        float desiredVelocity;
        if (input == 0f) {
            desiredVelocity = 0f;
        } else {
            desiredVelocity = input * maxSpeed;
        }

        float velocityChange = desiredVelocity - rb.velocity.x;
        float force = moveForce * rb.mass * velocityChange;
        rb.AddForce(Vector2.right * force, ForceMode2D.Force);

        if (rb.velocity.y > 0 && !quickFall) {
            rb.gravityScale = gravityScaleUp;
        } else {
            rb.gravityScale = gravityScaleDown;
        }
    }

    public void UpdateMoveInput(float input)
    {
        if (usedModule != null) return;
        moveInput = input;
    }

    public void JumpStart()
    {
        timeOfLastJump = Time.time;
    }

    public void Jump()
    {
        if (usedModule != null) return;
        quickFall = false;
        rb.gravityScale = gravityScaleUp;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpVelocity * rb.mass, ForceMode2D.Impulse);
    }

    public void JumpEnd()
    {
        quickFall = true;
        timeOfLastJump = 0f;
    }

    public bool Interact()
    {
        if (usedModule != null) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, moduleLayer);
        if (hit.collider == null) return false;

        Module module = hit.collider.GetComponent<Module>();
        if (module.isBeingUsed) return false;

        UpdateMoveInput(0f);
        usedModule = module;
        usedModule.isBeingUsed = true;
        Debug.Log($"Interaction with {usedModule.type} module started!");
        return true;
    }

    public void FinishInteraction()
    {
        usedModule.isBeingUsed = false;
        Debug.Log($"Interaction with {usedModule.type} module finished!");
        usedModule = null;
    }
}
