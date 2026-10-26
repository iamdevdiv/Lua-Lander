using System;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    public event EventHandler OnMenuButtonPressed;

    private void Awake() {
        Instance = this;

#if !UNITY_ANDROID && !UNITY_IOS
        GameObject touchUI = GameObject.Find("TouchUI");
        if (touchUI != null) {
            touchUI.SetActive(false);
        }
#endif

        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Menu.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);   
    }

    private void OnDestroy() {
        inputActions.Disable();
    }

    public bool IsUpActionPressed() {
        return inputActions.Player.LanderUp.IsPressed();
    }
    
    public bool IsRightActionPressed() {
        return inputActions.Player.LanderRight.IsPressed();
    }
    
    public bool IsLeftActionPressed() {
        return inputActions.Player.LanderLeft.IsPressed();
    }

    public Vector2 GetMovementInputVector() {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
}
