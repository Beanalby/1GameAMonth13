using UnityEngine;
using System.Collections;

public class Cart : MonoBehaviour {

    public TrackPoint startingTrack;

    private TrackPoint currentTrack;

    private float speed = 2f;

    void Start () {
        UpdateTrack(startingTrack);
    }
    void Update () {
        MoveCart();
    }

    void MoveCart() {
        rigidbody.MovePosition(rigidbody.position + currentTrack.direction * Time.deltaTime * speed);

        float dist = (transform.position - currentTrack.transform.position).magnitude;
        if(dist > currentTrack.length) {
            UpdateTrack(currentTrack.next);
        }
    }
    private void UpdateTrack(TrackPoint newTrack) {
        transform.position = newTrack.transform.position;
        transform.rotation = Quaternion.LookRotation(newTrack.direction);
        currentTrack = newTrack;
    }
}
