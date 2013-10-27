﻿using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {

    int playerLayer;
    public GameObject crashEffect;

    public void Awake() {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void GotHit() {
        Instantiate(crashEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided with " + collision.gameObject);
        if(collision.gameObject.layer != playerLayer) {
            return;
        }
        collision.gameObject.SendMessage("Crashed");
        GotHit();
    }
}
