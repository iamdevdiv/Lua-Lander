using UnityEngine;
using UnityEngine.UI;

public class CargoAreaVisual : MonoBehaviour {
    [SerializeField] private Image interactBarImage;
    [SerializeField] private SpriteRenderer iconSpriteRenderer;

    private CargoArea cargoArea;

    private void Awake() {
        cargoArea = GetComponent<CargoArea>();
    }

    private void Start() {
        cargoArea.OnInteractTimerChange += CargoArea_OnInteractTimerChange;

        interactBarImage.fillAmount = 0f;

        iconSpriteRenderer.sprite = cargoArea.GetCargoSO().sprite;
    }

    private void CargoArea_OnInteractTimerChange(object sender, System.EventArgs e) {
        interactBarImage.fillAmount = cargoArea.GetInteractTimerNormalized();
    }
}
