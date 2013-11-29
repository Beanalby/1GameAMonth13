using UnityEngine;
using System.Collections;

public class FruitLauncher : MonoBehaviour {

    public GameObject fruitPrefab;
    public Transform launchPoint;

    private float launchDelay = .5f;
    private float launchPower = 10;
    private float angleMin = 5, angleMax = 45;

    private float lastLaunch;

    public void Start() {
        lastLaunch = Mathf.NegativeInfinity;
    }
    public void Update() {
        HandleLaunch();
        if(Input.GetButtonDown("Fire2")) {
            LaunchFruit();
        }
    }

    private void HandleLaunch() {
        if(Time.time < lastLaunch + launchDelay) {
            return;
        }
        //LaunchFruit();
    }

    private void LaunchFruit() {
        lastLaunch = Time.time;
        Vector3 velocity = new Vector3(0, launchPower, 0);
        // angle the launch trajectory
        float angle = Random.Range(angleMin, angleMax);
        Quaternion velocityTip = Quaternion.Euler(
            new Vector3(0, 0, angle));
        velocity = velocityTip * velocity;

        Fruit fruit = (Instantiate(fruitPrefab) as GameObject).GetComponent<Fruit>();
        fruit.Init(launchPoint.position, velocity, Vector3.zero);
    }

    private static Vector3 RotateAroundCenter(Vector3 point, Vector3 angle) {
        Quaternion q = Quaternion.Euler(angle);
        return q * point;
    }
}
