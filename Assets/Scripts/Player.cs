using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Season season;

    [Space]
    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMoveSpeedLerpFactor;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float springGlideMaxFallSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalDashForce;
    [SerializeField] private float verticalDashForce;
    [SerializeField] private int maxHorizontalDashes;
    [SerializeField] private int maxVerticalDashes;

    [Space]
    [Header("Colors")]
    [SerializeField] private Color springColor;
    [SerializeField] private Color summerColor;
    [SerializeField] private Color winterColor;

    public enum Season
    {
        Spring,
        Summer,
        Winter
    }

    private Rigidbody2D rb2D;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private bool wasJumpButtonPressed = false;
    private int horizontalDashesLeft = 0;
    private int verticalDashesLeft = 0;
    private bool canJump = true;
    private float fallSpeed;
    private bool isGliding = false;

    private SpriteRenderer sprite;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        horizontalDashesLeft = maxHorizontalDashes;
        verticalDashesLeft = maxVerticalDashes;
        fallSpeed = maxFallSpeed;
        canJump = true;
    }

    private void Update()
    {
        switch (season)
        {
            case Season.Spring:
                sprite.color = springColor;
                break;
            case Season.Summer:
                sprite.color = summerColor;
                break;
            case Season.Winter:
                sprite.color = winterColor;
                break;
            default:
                break;
        }

        GetInput();
        HandleHorizontalMovement();
        HandleJump();

        switch (season)
        {
            case Season.Spring:
                Spring_Ability();
                break;
            case Season.Summer:
                isGliding = false;
                Summer_Ability();
                break;
            case Season.Winter:
                isGliding = false;
                Winter_Ability();
                break;
            default:
                isGliding = false;
                break;
        }

        Debug.Log(canJump);
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
        if(horizontalDashesLeft > 0 && GameInput.Instance.WasButtonPressedThisFrame(GameInput.GameButton.Powerup))
        {
            HorizontalDash();
        }
    }
    private void Winter_Ability()
    {
        if (verticalDashesLeft > 0 && GameInput.Instance.WasButtonPressedThisFrame(GameInput.GameButton.Powerup))
        {
            VerticalDash();
        }
    }

    private void HorizontalDash()
    {
        if(horizontalDashesLeft > 0)
        {
            rb2D.AddForce(new Vector2(horizontalDashForce * transform.localScale.x, 0), ForceMode2D.Impulse);
            horizontalDashesLeft--;
        }
    }
    private void VerticalDash()
    {
        if (verticalDashesLeft > 0 && verticalInput != 0)
        {
            rb2D.AddForce(new Vector2(0, verticalDashForce * verticalInput), ForceMode2D.Impulse);
            verticalDashesLeft--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < transform.position.y) 
        {
            horizontalDashesLeft = maxHorizontalDashes;
            verticalDashesLeft = maxVerticalDashes;

            if (collision.gameObject.TryGetComponent(out Platform platform))
            {
                if (platform.GetPlatformSeason() != Platform.Season.Spring) canJump = true;
            }
            else canJump = true;
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            StartCoroutine(GameManager.Instance.PlayerDeath());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.position.y < transform.position.y)
        {
            canJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            StartCoroutine(GameManager.Instance.PlayerWin());
        }
    }

    private void GetInput()
    {
        if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Right)) horizontalInput = 1f;
        else if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Left)) horizontalInput = -1f;
        else horizontalInput = 0f;

        if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Up)) verticalInput = 1f;
        else if (GameInput.Instance.IsButtonPressed(GameInput.GameButton.Down)) verticalInput = -1f;
        else verticalInput = 0f;

        wasJumpButtonPressed = GameInput.Instance.WasButtonPressedThisFrame(GameInput.GameButton.Jump);
    }

    public void SetSeason(Season season)
    {
        this.season = season;
    }
}
