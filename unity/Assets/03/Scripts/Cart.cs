using UnityEngine;
using System.Collections;

public enum CartState { Stopped, Moving, Turning };

[RequireComponent(typeof(AudioSource))]
public class Cart : MonoBehaviour {

    public AudioClip cartMoving;
    public AudioClip cartTurning;
    public TrackPoint startingTrack;

    private float moveSpeed = 2f;
    private float rotateSpeed = .5f;

    private AudioSource audioSource;
    private CartState cartState = CartState.Moving;
    private float rotationPercent = 0;
    private TrackPoint currentTrack;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        cartState = CartState.Stopped;
        StartMove(startingTrack);
    }
    void Update () {
        if(cartState == CartState.Stopped && Input.GetKeyDown(KeyCode.Space)) {
            StartMove(startingTrack);
        }
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
    }
    void StartTurn() {
        transform.position = currentTrack.next.transform.position;
        rotationPercent = 0;
        cartState = CartState.Turning;
        audioSource.clip = cartTurning;
        audioSource.Play();
    }
}
