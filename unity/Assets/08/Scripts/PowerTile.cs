using UnityEngine;
using System.Collections;

public class PowerTile : MonoBehaviour {
    private Color colorOn = Color.yellow, colorOff;
    private bool isActive = false;

    private GameObject levelDriver;

    public bool IsActive {
        get { return isActive; }
    }

    public void Start() {
        colorOff = colorOn / 5;
        levelDriver = GameObject.Find("LevelDriver");
    }

    public void ActivateTile() {
        isActive = true;
        renderer.material.color = colorOn;
        levelDriver.SendMessage("UpdateTiles");
    }
    public void DeactivateTile() {
        isActive = false;
        renderer.material.color = colorOff;
        levelDriver.SendMessage("UpdateTiles");
    }
}
