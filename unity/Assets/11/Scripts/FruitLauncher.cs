using UnityEngine;
using System.Collections;

public class FruitLauncher : MonoBehaviour {

    public GameObject[] fruitPrefabs;
    public Transform launchPoint;
    public AudioClip launchSound;
    public FruitGameDriver driver;

    private float launchDelay = 2f;
    private float launchRandomize = .5f;
    private float launchNext;
    private float launchPower = 10;
    private float angleMin = 5, angleMax = 45;
    private float spinPower = 5;

    private float lastLaunch;

    public void Start() {
        lastLaunch = Mathf.NegativeInfinity;
        launchNext = launchDelay + Random.Range(-launchRandomize, launchRandomize);
    }
    public void Update() {
        HandleLaunch();
        //if(Input.GetButtonDown("Fire2")) {
        //    LaunchFruit();
        //}
    }

    private void HandleLaunch() {
        if(Time.time < lastLaunch + launchNext) {
            return;
        }
        if(!driver.IsGameRunning()) {
            return;
        }
        LaunchFruit();
        launchNext = launchDelay + Random.Range(-launchRandomize, launchRandomize);
    }

    public void LaunchFruit() {
        lastLaunch = Time.time;
        Vector3 velocity = new Vector3(0, launchPower, 0);
        // angle the launch trajectory
        float angle = Random.Range(angleMin, angleMax);
        Quaternion velocityTip = Quaternion.Euler(
            new Vector3(0, 0, angle));
        velocity = velocityTip * velocity;
        Vector3 angularVelocity = new Vector3(
            Random.Range(-spinPower, spinPower),
            Random.Range(-spinPower, spinPower),
            Random.Range(-spinPower, spinPower));

        int index = Random.Range(0, fruitPrefabs.Length);
        Fruit fruit = (Instantiate(fruitPrefabs[index]) as GameObject).GetComponent<Fruit>();
        fruit.Init(launchPoint.position, velocity, angularVelocity);
        AudioSource.PlayClipAtPoint(launchSound, Camera.main.transform.position);
    }

    private static Vector3 RotateAroundCenter(Vector3 point, Vector3 angle) {
        Quaternion q = Quaternion.Euler(angle);
        return q * point;
    }
}
