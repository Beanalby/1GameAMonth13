using UnityEngine;
using System.Collections;

public class PowerTile : MonoBehaviour {
    private bool isActive = false;

    public Material matOn, matOff;

    private GameObject levelDriver;

    public bool IsActive {
        get { return isActive; }
    }

    public void Start() {
        levelDriver = GameObject.Find("LevelDriver");
    }

    public void ActivateTile() {
        isActive = true;
        renderer.material = matOn;
        levelDriver.SendMessage("UpdateTiles");
    }
    public void DeactivateTile() {
        isActive = false;
        renderer.material = matOff;
        levelDriver.SendMessage("UpdateTiles");
    }
}
