using UnityEngine;
using System.Collections;

/// <summary>
/// Proxy class to consistently access information about the runner that
/// may or may not exist
/// </summary>
public class RunnerInfo : MonoBehaviour {

    public Runner runnerPrefab;

    private Runner runner;
    private float savedDistance = -1;

    public float DistanceTravelled {
        get {
            if(runner) {
                return runner.distanceTravelled;
            } else {
                return savedDistance;
            }
        }
    }
    public float Health {
        get {
            if(runner) {
                return runner.Health;
            } else {
                return 0;
            }
        }
    }
    public float Speed {
        get {
            if(runner) {
                return runner.Speed;
            } else {
                return 0;
            }
        }
    }

    public void SpawnRunner() {
        runner = (Instantiate(runnerPrefab.gameObject) as GameObject).GetComponent<Runner>();
        runner.info = this;
        savedDistance = -1;
    }

    public void RunnerDied() {
        savedDistance = runner.distanceTravelled;
    }
}
