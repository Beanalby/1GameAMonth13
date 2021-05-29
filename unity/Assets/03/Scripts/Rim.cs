using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Rim : MonoBehaviour {

    public float minSoundVelocity =5f;
    public int value = 2;

    private AudioSource audioSource;
    private GameObject driver;
    private Transform shadow;
    private float shadowDistance;
    private Vector3 shadowPosition;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        driver = GameObject.Find("GameDriver") as GameObject;
        shadow = transform.Find("RimShadow");
        shadowDistance = (transform.position - shadow.position).magnitude;
    }
    void Update () {
        shadowPosition = transform.position;
        shadowPosition.y += shadowDistance;
        shadow.position = shadowPosition;
        shadow.rotation = Quaternion.LookRotation(Vector3.down);
    }

    public void ShotSuccess() {
        driver.SendMessage("ShotSuccess", this);
        audioSource.Play();
    }
}
