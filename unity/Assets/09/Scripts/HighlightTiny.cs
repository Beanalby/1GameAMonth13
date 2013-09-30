using UnityEngine;
using System.Collections;

public class HighlightTiny : MonoBehaviour {
    public Texture texO, texX;

    GameDriver9 driver;
    private SpotValue currentTex;

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
            } else {
                renderer.enabled = false;
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
