using UnityEngine;

public class LanderVisual : MonoBehaviour {
    [SerializeField] ParticleSystem leftThrustedParticleSystem;
    [SerializeField] ParticleSystem middleThrustedParticleSystem;
    [SerializeField] ParticleSystem rightThrustedParticleSystem;
    [SerializeField] GameObject landerExplosionVfx;

    private Lander lander;

    private void Awake() {
        lander = GetComponent<Lander>();
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnabledThrusterParticleSystem(leftThrustedParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrustedParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrustedParticleSystem, false);
    }

    private void Start() {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        switch (e.landingType) {
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.WrongLandingArea:
            case Lander.LandingType.CargoCrashed:
                // Crash
                Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrustedParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrustedParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrustedParticleSystem, false);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrustedParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(rightThrustedParticleSystem, true);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrustedParticleSystem, true);
        SetEnabledThrusterParticleSystem(middleThrustedParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightThrustedParticleSystem, true);
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled) {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
