using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMoveSpeedLerpFactor;
    [SerializeField] private float maxJumpVelocity;

    private Rigidbody2D rb2D;
    private float horizontalInput = 0;
    private bool wasJumpButtonPressed = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        HandleHorizontalMovement();
        HandleJump();
    }

    private void HandleHorizontalMovement()
    {
        rb2D.velocity = Vector2.Lerp(rb2D.velocity, new Vector2 (horizontalInput * moveSpeed, rb2D.velocity.y), Time.deltaTime * horizontalMoveSpeedLerpFactor);
    }

    private void HandleJump()
    {
        if (wasJumpButtonPressed)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, maxJumpVelocity);
        }
    }

    private void GetInput()
    {
        if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Right)) horizontalInput = 1f;
        else if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Left)) horizontalInput = -1f;
        else horizontalInput = 0f;

        wasJumpButtonPressed = GameInput.Instance.WasButtonPressedThisFrame(GameInput.GameButton.Jump);
    }
}
