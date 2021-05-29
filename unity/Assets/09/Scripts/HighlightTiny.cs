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
            GetComponent<Renderer>().enabled = false;
        } else {
            if(driver.IsValidMove(spot)) {
                GetComponent<Renderer>().enabled = true;
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
                GetComponent<Renderer>().enabled = false;
                currentSpot = null;
            }
        }
    }

    private void UpdateTexture() {
        currentTex = driver.CurrentTurn;
        if(currentTex == SpotValue.X) {
            GetComponent<Renderer>().material.mainTexture = texX;
        } else {
            GetComponent<Renderer>().material.mainTexture = texO;
        }
    }
}
