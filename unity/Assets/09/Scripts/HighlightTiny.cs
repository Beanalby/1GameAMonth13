using UnityEngine;
using System.Collections;

public class HighlightTiny : MonoBehaviour {
    public Texture texO, texX;
    public AudioClip hoverSound;

    GameDriver9 driver;
    private SpotValue currentTex;
    private TinySpot currentSpot = null;

    public void Start() {
        driver = GameDriver9.instance;
        UpdateTexture();
    }

    public void Update() {
        // see if we're mouseOver a valid move
        TinySpot spot = driver.GetHovered();
        if(spot == null) {
            renderer.enabled = false;
        } else {
            if(driver.IsValidMove(spot)) {
                renderer.enabled = true;
                transform.position = spot.transform.position;
                if(currentTex != driver.CurrentTurn) {
                    UpdateTexture();
                }
                if(currentSpot != spot) {
                    currentSpot = spot;
                    AudioSource.PlayClipAtPoint(hoverSound,
                        Camera.main.transform.position);
                }
            } else {
                renderer.enabled = false;
                currentSpot = null;
            }
        }
    }

    private void UpdateTexture() {
        currentTex = driver.CurrentTurn;
        if(currentTex == SpotValue.X) {
            renderer.material.mainTexture = texX;
        } else {
            renderer.material.mainTexture = texO;
        }
    }
}
