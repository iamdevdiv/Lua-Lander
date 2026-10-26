using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CargoArea : MonoBehaviour {
    public event EventHandler OnInteractTimerChange;
    public event EventHandler OnDroppedOff;
    public event EventHandler OnPickedUp;

    public enum InteractType {
        PickUp,
        Drop,
    }

    [SerializeField] private InteractType interactType;
    [SerializeField] private float interactTimerMax = 2f;
    [SerializeField] private CargoSO cargoSO;

    private float interactTimer;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Lander lander)) {
            switch (interactType) {
                case InteractType.PickUp:
                    if (lander.GetCargoSO() != null) {
                        // Lander is already carrying something, cannot carry more
                        return;
                    }
                    break;
                case InteractType.Drop:
                    if (lander.GetCargoSO() == null) {
                        // Lander is not carrying anything so it cannot drop anything
                        return;
                    }

                    // Lander is carrying something, validate it here
                    if (lander.GetCargoSO() != cargoSO) {
                        // Lander is carrying something that is not valid to drop here
                        return;
                    }
                    break;
            }

            interactTimer += Time.deltaTime;
            OnInteractTimerChange?.Invoke(this, EventArgs.Empty);
            if (interactTimer > interactTimerMax) {
                switch (interactType) {
                    case InteractType.PickUp:
                        lander.PickUpCargo(cargoSO);
                        OnPickedUp?.Invoke(this, EventArgs.Empty);
                        break;
                    case InteractType.Drop:
                        lander.DropCargo();
                        OnDroppedOff?.Invoke(this, EventArgs.Empty);
                        break;
                }
                
                DestroySelf();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        interactTimer = 0f;
        OnInteractTimerChange?.Invoke(this, EventArgs.Empty);
    }

    public float GetInteractTimerNormalized() {
        return interactTimer / interactTimerMax;
    }

    public CargoSO GetCargoSO() {
        return cargoSO;
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
