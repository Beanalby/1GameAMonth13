using UnityEngine;
using System.Collections;

public class FruitControls : MonoBehaviour {
    public void Start() {
        ShowControls sc = ShowControls.CreateDocked(new ControlItem[] {
            new ControlItem("Move the robot", new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }),
            new ControlItem("Drop fruit in the appropriate bin", KeyCode.Space)
        });
        sc.slideSpeed = -1;
        sc.showDuration = -1;
        sc.Show();
    }

}
