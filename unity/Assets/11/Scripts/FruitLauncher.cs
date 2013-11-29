using UnityEngine;
using System.Collections;

public class FruitLauncher : MonoBehaviour {

    public GameObject fruitPrefab;
    private float launchDelay = .5f;

    private float lastLaunch;

    public void Start() {
        lastLaunch = Mathf.NegativeInfinity;
    }
    public void Update() {
        HandleLaunch();
    }

    private void HandleLaunch() {
        if(Time.time < lastLaunch + launchDelay) {
            return;
        }
        LaunchFruit();
    }

    private void LaunchFruit() {
        GameObject fruit = Instantiate(fruitPrefab) as GameObject;
        fruit.transform.position = transform.position;
        lastLaunch = Time.time;
    }
}
