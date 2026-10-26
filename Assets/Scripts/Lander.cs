using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour {
    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance {
        get; private set;
    }

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
        CargoCrashed,
    }

    public enum State {
        WaitingToStart,
        Normal,
        GameOver,
    }

    [SerializeField] private Transform cargoRopePrefab;

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;
    private Transform cargoRopeTransform;
    private CargoSO cargoSO;

    private void Awake() {
        Instance = this;
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
        state = State.WaitingToStart;
    }

    private void FixedUpdate() {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state) {
            default:

            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed() || 
                    GameInput.Instance.GetMovementInputVector() != Vector2.zero) {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;

            case State.Normal:
                if (fuelAmount <= 0f) {
                    // No fuel
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() ||
                    GameInput.Instance.IsRightActionPressed() ||
                    GameInput.Instance.GetMovementInputVector() != Vector2.zero) {
                    ConsumeFuel();
                }

                float joystickDeadzone = 0.4f;

                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.GetMovementInputVector().y > joystickDeadzone) {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                if (GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.GetMovementInputVector().x < -joystickDeadzone) {
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }

                if (GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector().x > joystickDeadzone) {
                    float turnSpeed = -100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.GameOver:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.TryGetComponent(out LandingPad landingPad)) {
            // Crashed on the terrain
            OnLanded?.Invoke(this, new OnLandedEventArgs {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude) {
            OnLanded?.Invoke(this, new OnLandedEventArgs {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector) {
            // Landed on too steep angle
            OnLanded?.Invoke(this, new OnLandedEventArgs {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        Debug.Log("Score: " + score);

        OnLanded?.Invoke(this, new OnLandedEventArgs {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out FuelPickup fuelPickup)) {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;

            if (fuelAmount > fuelAmountMax) {
                fuelAmount = fuelAmountMax;
            }
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
            fuelPickup.DestroySelf();
        }

        if (collision.gameObject.TryGetComponent(out CoinPickup coinPickup)) {
            coinPickup.DestroySelf();
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetState(State state) {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state,
        });
    }

    private void ConsumeFuel() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetSpeedX() {
        return landerRigidbody2D.linearVelocity.x;
    }

    public float GetSpeedY() {
        return landerRigidbody2D.linearVelocity.y;
    }

    public float GetFuel() {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized() {
        return fuelAmount / fuelAmountMax;
    }

    public void PickUpCargo(CargoSO cargoSO) {
        this.cargoSO = cargoSO;
        cargoRopeTransform = Instantiate(cargoRopePrefab, transform);
    }

    public void CargoCrashed() {
        OnLanded?.Invoke(this, new OnLandedEventArgs {
            landingType = LandingType.CargoCrashed,
            dotVector = 0f,
            landingSpeed = 0f,
            scoreMultiplier = 0f,
            score = 0,
        });
        SetState(State.GameOver);
    }

    public void DropCargo() {
        if (cargoRopePrefab != null) {
            cargoSO = null;
            Destroy(cargoRopeTransform.gameObject);
        }
    }

    public CargoSO GetCargoSO() {
        return cargoSO;
    }

    public void AddForce(Vector2 force) {
        landerRigidbody2D.AddForce(force * Time.deltaTime);
    }
}
    