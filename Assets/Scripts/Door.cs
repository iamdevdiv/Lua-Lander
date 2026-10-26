using UnityEngine;

public class Door : MonoBehaviour {
    private const string IS_OPEN = "IsOpen";

    private Animator animator;

    [SerializeField] private CargoArea dropOffCargoArea;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        dropOffCargoArea.OnDroppedOff += DropOffCargoArea_OnDroppedOff;
    }

    private void DropOffCargoArea_OnDroppedOff(object sender, System.EventArgs e) {
        OpenDoor();
    }

    public void OpenDoor() {
        animator.SetBool(IS_OPEN, true);
    }

    public void CloseDoor() {
        animator.SetBool(IS_OPEN, false);
    }
}
