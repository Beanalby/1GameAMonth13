using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserSwitch : MonoBehaviour {
    public GameObject laserTarget;
    public GameObject switchTarget;
    public bool invert = false;
    public Material laserOn, laserOff;


    private bool lastState = true;
    private LineRenderer line;
    bool sendInitial = true;

    void Start() {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        ApplyState();
        GameObject.Find("Player").GetComponent<Player>().resetListeners += OnReset;
    }
    void Update() {
        UpdateLaser();
    }

    private void ApplyState() {
        if(lastState) {
            line.material = laserOn;
        } else {
            line.material = laserOff;
        }
    }
    private void OnReset() {
        sendInitial = true;
    }
    private bool UpdateLaser() {
        line.SetPosition(0, transform.position);
        Ray ray = new Ray(transform.position,
            transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        bool newState = false;
        if(Physics.Raycast(ray, out hit)) {
            line.SetPosition(1, hit.point);
            newState = hit.collider.gameObject == laserTarget;
        } else {
            // didn't hit anything, just have the line shoot into distance
            line.SetPosition(1, ray.GetPoint(1000));
            newState = false;
        }
        if(sendInitial || lastState != newState) {
            lastState = newState;
            if(switchTarget) {
                switchTarget.SendMessage("SwitchToggled", newState ^ invert,
                    SendMessageOptions.RequireReceiver);
            }
            ApplyState();
            sendInitial = false;
        }
        return newState;
    }
}
