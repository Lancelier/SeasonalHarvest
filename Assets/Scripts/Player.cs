using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Season season;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMoveSpeedLerpFactor;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float springGlideMaxFallSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private int maxDashes;


    public enum Season
    {
        Spring,
        Summer,
        Winter
    }

    private Rigidbody2D rb2D;
    private float horizontalInput = 0;
    private bool wasJumpButtonPressed = false;
    private int dashesLeft = 0;
    private bool canJump = false;
    private float fallSpeed;
    private bool isGliding = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        dashesLeft = maxDashes;
        fallSpeed = maxFallSpeed;
    }

    private void Update()
    {
        GetInput();
        HandleHorizontalMovement();
        HandleJump();

        switch (season)
        {
            case Season.Spring:
                Spring_Ability();
                break;
            case Season.Summer:
                Summer_Ability();
                break;
            case Season.Winter:
                Winter_Ability();
                break;
            default:
                break;
        }

        Debug.Log(rb2D.velocity.y);
    }

    private void HandleHorizontalMovement()
    {
        if(horizontalInput != 0f) transform.localScale = new Vector3(horizontalInput, transform.localScale.y, transform.localScale.z);

        if (!isGliding) fallSpeed = -maxFallSpeed;
        else fallSpeed = -springGlideMaxFallSpeed;

        if (rb2D.velocity.y > fallSpeed) fallSpeed = rb2D.velocity.y;

        rb2D.velocity = Vector2.Lerp(rb2D.velocity, new Vector2(horizontalInput * moveSpeed, fallSpeed), Time.deltaTime * horizontalMoveSpeedLerpFactor);
    }

    private void HandleJump()
    {
        if (canJump && wasJumpButtonPressed)
        {
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Spring_Ability()
    {
        if(!canJump && GameInput.Instance.IsButtonPressed(GameInput.GameButton.Powerup))
        {
            isGliding = true;
        }
        else if(GameInput.Instance.WasButtonReleasedThisFrame(GameInput.GameButton.Powerup))
        {
            isGliding = false;
        }
    }
    private void Summer_Ability()
    {
        if(dashesLeft > 0 && GameInput.Instance.WasButtonPressedThisFrame(GameInput.GameButton.Powerup))
        {
            Dash();
        }
    }
    private void Winter_Ability()
    {

    }

    private void Dash()
    {
        if(dashesLeft > 0)
        {
            rb2D.AddForce(new Vector2(dashForce * transform.localScale.x, 0), ForceMode2D.Impulse);
            dashesLeft--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < transform.position.y) 
        {
            dashesLeft = maxDashes;

            if (collision.gameObject.TryGetComponent(out Platform platform))
            {
                if (platform.GetPlatformSeason() != Platform.Season.Spring) canJump = true;
            }
            else canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.position.y < transform.position.y)
        {
            canJump = false;
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
