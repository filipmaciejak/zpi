using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _id = 0;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask moduleLayer;
    public Module usedModule { get; private set; } = null;
    private float moveInput = 0f;
    private Transform groundCheck;
    private Rigidbody2D rb;

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.74f, 0.1f), 0f, groundLayer);
    }

    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody2D>();

        CrewmateEventManager.instance.onCrewmateButtonAPushed.AddListener(
            (id) => {
                if (id != _id) return;
                Jump();
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
        if (moveInput != 0) {
            if (rb.velocity.x < 0 && moveInput > 0 || rb.velocity.x > 0 && moveInput < 0) {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            rb.AddForce(Vector2.right * moveInput * moveForce);
        } else if (IsGrounded()) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void UpdateMoveInput(float input)
    {
        if (usedModule != null) return;
        moveInput = input;
    }

    public void Jump()
    {
        if (usedModule != null) return;
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
