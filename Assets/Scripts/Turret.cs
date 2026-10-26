using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] private Transform headTransform;
    [SerializeField] private TurretBullet turretBulletPrefab;
    [SerializeField] private Transform shootPointTransform;

    private float shootTimer;

    private void Update() {
        if (!IsPlayerInRange()) {
            return;
        }

        Vector2 aimDirecton = (Lander.Instance.transform.position - headTransform.position).normalized;
        headTransform.right = aimDirecton;

        shootTimer -= Time.deltaTime;
        if (shootTimer < 0f) {
            float shootTimerMax = 1.5f;
            shootTimer = shootTimerMax;

            TurretBullet turretBullet = Instantiate(turretBulletPrefab, shootPointTransform.position, Quaternion.identity);
            turretBullet.Setup(aimDirecton);
        }
    }

    private bool IsPlayerInRange() {
        float range = 14f;
        return Vector2.Distance(transform.position, Lander.Instance.transform.position) < range;
    }
}
