using UnityEngine;

public class WindZone : MonoBehaviour {
    [SerializeField] private Vector2 windForce;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Lander lander)) {
            lander.AddForce(windForce);
        }
    }

    public Vector2 GetWindForce() { 
        return windForce; 
    }
}
