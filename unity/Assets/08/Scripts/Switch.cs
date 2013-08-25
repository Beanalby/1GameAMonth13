using UnityEngine;
using System.Collections;

public delegate void SwitchHandler(bool isActive);

public class Switch : MonoBehaviour {
    public Color color;

    public event SwitchHandler switchListeners;

    public void AddSwitchListener(SwitchHandler handler) {
        switchListeners += handler;
    }

    public void SetColor(Color colorParam) {
        color = colorParam;
    }
    private void SetSwitch(bool isActiveParam) {
        if(switchListeners != null) {
            switchListeners(isActiveParam);
        }
    }
}
