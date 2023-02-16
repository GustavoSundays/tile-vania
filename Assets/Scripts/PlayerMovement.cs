using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;

    bool playerHasHorizontalSpeed;
    bool playerHasVerticalSpeed;
    bool isJumping;
    float initialGravity;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        initialGravity = playerRigidbody.gravityScale;
    }

    void Update() {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {        
        if (value.isPressed && playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isJumping = true;
            playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
            playerAnimator.SetBool("isJumping", isJumping);
        }
    }

    void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

        if (isJumping || !playerFeetCollider.IsTouchingLayers()) {
            bool playerIsFalling = playerRigidbody.velocity.y < Mathf.Epsilon;
            playerAnimator.SetBool("isFalling", playerIsFalling);
        }
    }

    void FlipSprite() {
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder() {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            playerRigidbody.gravityScale = initialGravity;
            playerAnimator.SetBool("isClimbing", false);
            return;
        };

        playerRigidbody.gravityScale = 0f;

        Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
        playerRigidbody.velocity = climbVelocity;

        playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            isJumping = false;
            playerAnimator.SetBool("isJumping", isJumping);
            playerAnimator.SetBool("isFalling", isJumping);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ladder")) {
            isJumping = false;
            playerAnimator.SetBool("isJumping", isJumping);
            playerAnimator.SetBool("isFalling", isJumping);
        }
    }
    
}
