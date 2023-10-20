using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody2D>();
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

    public void Interact()
    {
        if (usedModule != null) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, moduleLayer);
        if (hit.collider == null) return;

        Module module = hit.collider.GetComponent<Module>();
        if (module.isBeingUsed) return;

        UpdateMoveInput(0f);
        usedModule = module;
        usedModule.isBeingUsed = true;
        Debug.Log($"Interaction with {usedModule.type} module started!");
    }

    public void FinishInteraction()
    {
        usedModule.isBeingUsed = false;
        Debug.Log($"Interaction with {usedModule.type} module finished!");
        usedModule = null;
    }
}
