using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public enum GameButton
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Powerup,
    }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        DontDestroyOnLoad(gameObject);
    }
    
    public bool IsButtonPressed(GameButton button)
    {
        InputAction buttonAction = GetButtonActionFromEnum(button);

        return buttonAction.IsPressed();
    }

    public bool WasButtonPressedThisFrame(GameButton button)
    {
        InputAction buttonAction = GetButtonActionFromEnum(button);

        return buttonAction.WasPressedThisFrame();
    }

    public bool WasButtonReleasedThisFrame(GameButton button)
    {
        InputAction buttonAction = GetButtonActionFromEnum(button);

        return buttonAction.WasReleasedThisFrame();
    }




    private InputAction GetButtonActionFromEnum(GameButton button)
    {
        switch (button)
        {
            case GameButton.Up:
                return inputActions.Player.Up;
            case GameButton.Down:
                return inputActions.Player.Down;
            case GameButton.Left:
                return inputActions.Player.Left;
            case GameButton.Right:
                return inputActions.Player.Right;
            case GameButton.Jump:
                return inputActions.Player.Jump;
            case GameButton.Powerup:
                return inputActions.Player.Powerup;
            default:
                return null;
        }
    }
}
