using UnityEngine;

public class GameLevelScripting_Level1 : MonoBehaviour {
    [SerializeField] private CargoArea dropOffCargoArea;

    private void Start() {
        dropOffCargoArea.OnDroppedOff += DropOffCargoArea_OnDroppedOff;
    }

    private void DropOffCargoArea_OnDroppedOff(object sender, System.EventArgs e) {
        GameManager.Instance.AddScore(100);
    }
}
