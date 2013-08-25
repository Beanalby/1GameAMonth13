using UnityEngine;
using System.Collections;

public class PowerTile : MonoBehaviour {
    private bool isActive = false;

    public AudioClip soundOn, soundOff;

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
        AudioSource.PlayClipAtPoint(soundOn, transform.position);
        levelDriver.SendMessage("UpdateTiles");
    }
    public void DeactivateTile() {
        isActive = false;
        renderer.material = matOff;
        AudioSource.PlayClipAtPoint(soundOff, transform.position);
        levelDriver.SendMessage("UpdateTiles");
    }
}
