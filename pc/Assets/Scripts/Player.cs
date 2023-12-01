using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamId;

    [SerializeField] private int _id = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask moduleLayer;
    public Module usedModule { get; private set; } = null;
    private float moveInput = 0f;

    public float maxSpeed = 2f;
    public float moveForce = 10f;

    private Transform groundCheck;
    public static float timeToApex = 4f;
    public static float timeOfFalling = 2f;
    public static float jumpHeight = 20f;
    private bool quickFall = false;

    private Rigidbody2D rb;

    private static float gravityScaleUp => (2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
    private static float gravityScaleDown => (2 * jumpHeight) / Mathf.Pow(timeOfFalling, 2);
    private static float jumpVelocity => Mathf.Abs(gravityScaleUp * timeToApex);

    private float inputTranslation(float input)
    {
        return input; // todo
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.74f, 0.1f), 0f, groundLayer);
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
                    ModuleEventManager.instance.onModuleEntered.Invoke(_id, usedModule.type);

                    if (usedModule.type == Module.Type.MOVEMENT_MODULE) {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("steering_pos", "1"); // TODO: Add steering pos
                        dict.Add("speed_pos", "1"); // TODO: Add speed pos
                        ModuleEventManager.instance.onMinigameInitialized.Invoke(_id, dict);
                    } else if (usedModule.type == Module.Type.CANNON_MODULE) {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("fire_cooldown", "3.0"); // TODO: Add fire_cooldown
                        dict.Add("ammo", "5"); // TODO: Add ammo
                        dict.Add("chamber_open", "False"); // TODO: Add chamber_open
                        dict.Add("chamber_loaded", "True"); // TODO: Add chamber_loaded
                        ModuleEventManager.instance.onMinigameInitialized.Invoke(_id, dict);
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
        if (usedModule != null) return;
        quickFall = false;
        if (IsGrounded())
        {
            rb.gravityScale = gravityScaleUp;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpVelocity * rb.mass, ForceMode2D.Impulse);
        }
    }

    public void JumpEnd()
    {
        quickFall = true;
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
