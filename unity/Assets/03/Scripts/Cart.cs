using UnityEngine;
using System.Collections;

public enum CartState { Stopped, Moving, Turning };

[RequireComponent(typeof(AudioSource))]
public class Cart : MonoBehaviour {

    public AudioClip cartMoving;
    public AudioClip cartTurning;
    public TrackPoint startingTrack;

    private float moveSpeed = 1f;
    private float rotateSpeed = 1f;

    private GameObject gameDriver;
    private AudioSource audioSource;
    private CartState cartState = CartState.Moving;
    private float rotationPercent = 0;
    private TrackPoint currentTrack;

    void Start () {
        gameDriver = GameObject.Find("GameDriver") as GameObject;
        audioSource = GetComponent<AudioSource>();
        cartState = CartState.Stopped;
        StartMove(startingTrack);
    }
    void Update () {
        HandleMovement();
    }

    void ApplyMove() {
        rigidbody.MovePosition(rigidbody.position + currentTrack.direction * Time.deltaTime * moveSpeed);
        float dist = (transform.position - currentTrack.transform.position).magnitude;
        if(dist > currentTrack.length) {
            StartTurn();
        }
    }
    void ApplyTurn() {
        rotationPercent += rotateSpeed * Time.deltaTime;
        if(rotationPercent >= 1) {
            StartMove(currentTrack.next);
        } else {
            Vector3 dir = Vector3.Lerp(currentTrack.direction,
                currentTrack.next.direction, rotationPercent);
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void HandleMovement() {
        switch(cartState) {
            case CartState.Moving:
                ApplyMove(); break;
            case CartState.Turning:
                ApplyTurn(); break;
        }
    }

    void StartMove(TrackPoint newTrack) {
        transform.position = newTrack.transform.position;
        transform.rotation = Quaternion.LookRotation(newTrack.direction);
        currentTrack = newTrack;
        cartState = CartState.Moving;
        audioSource.clip = cartMoving;
        audioSource.Play();
        if(currentTrack.next.next == null) {
            gameDriver.SendMessage("TrackFinished");
        }
    }
    void StartTurn() {
        if(currentTrack.next.next == null) {
            cartState = CartState.Stopped;
            audioSource.Stop();
            return;
        }
        //if(currentTrack.transform.rotation == currentTrack.next.transform.rotation) {
        //    StartMove(currentTrack.next);
        //}
        transform.position = currentTrack.next.transform.position;
        rotationPercent = 0;
        cartState = CartState.Turning;
        audioSource.clip = cartTurning;
        audioSource.Play();
    }
}
