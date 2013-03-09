using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class ballLauncher : MonoBehaviour {

    private static Vector3 Simulate(Vector3 startPos, Vector3 velocity,
            float duration) {
        Vector3 endPos = startPos + velocity * duration;
        endPos.y += Mathf.Pow(duration, 2) * Physics.gravity.y / 2f;
        return endPos;
    }

    public GameObject ballTempalte;
    private float powerScale = 5f;

    private float angleVertical = 45f;
    private float angleHorizontal = 0f;
    private Transform cannonMesh, launchPoint;
    private bool loaded = true;
    private Vector3 startVelocity;
    private int lineSegments = 16;
    private float segmentScale = 1/8f;
    private float lineStart = 0f;
    private float power = 10f;
    private float powerMin = 5f;
    private float powerMax = 20f;

    private LineRenderer line;
    private List<float> checks;


    void Start() {
        cannonMesh = transform.Find("CannonMesh");
        launchPoint = cannonMesh.Find("LaunchPoint");
        line = GetComponent<LineRenderer>();
        checks = Enumerable.Range(0, lineSegments)
            .Select(x => x * segmentScale).ToList();
        line.SetVertexCount(lineSegments);
        UpdateTrajectory();
    }
    void Update() {
        UpdatePower();
        UpdateRotation();
        HandleFiring();
    }

    public void FireBall() {
        GameObject ball = Instantiate(ballTempalte,
            launchPoint.position, transform.rotation) as GameObject;
        ball.rigidbody.velocity = startVelocity;
    }
    public void HandleFiring() {
        if(Input.GetButtonDown("Fire1")) {
            FireBall();
        }
    }
    public void UpdatePower() {
        power += Input.GetAxis("Vertical") * powerScale * Time.deltaTime;
        power = Mathf.Max(powerMin, Mathf.Min(powerMax, power));
    }
    public void UpdateRotation() {
        /// scale the X & Y mouse position for rotation such that mostly
        /// left / mostly right map to full left rotation / full
        /// right rotation, and mostly top / mostly bottom map to 90deg up /
        /// straight straight forward, respectively.
        float xPercent = Input.mousePosition.x / Screen.width,
            yPercent = Input.mousePosition.y / Screen.height;

        // don't update the rotation if the mouse is outside of the window
        if(xPercent < 0 || xPercent > 1 || yPercent < 0 || yPercent > 1) {
            return;
        }

        // 20% => -90, 80% => +90, capped at those values
        angleHorizontal = Mathf.Min(90, Mathf.Max(-90,
            300 * xPercent - 150));
        
        // use 89 as max vertical instead of 90; going straight vertical
        // causes annoying issues with determining look direction.
        // 20% => 89, 80% => 0, capped at those valuse
        angleVertical = Mathf.Min(89, Mathf.Max(0,
            -150 * yPercent + 120));
        UpdateTrajectory();
    }
    public void UpdateTrajectory() {
        if(!loaded) {
            return;
        }
        startVelocity = Quaternion.Euler(0, angleHorizontal, 0)
            * Vector3.forward;
        startVelocity = Vector3.Slerp(startVelocity, Vector3.up,
            angleVertical / 90);
        startVelocity *= power;
        Vector3 lookTarget = new Vector3(startVelocity.x, 0, startVelocity.z);
        transform.rotation = Quaternion.LookRotation(lookTarget);
        // the real launcher doesn't raise vertically for the angle, so
        // rotate the cannon mesh itself
        cannonMesh.localEulerAngles = new Vector3(-angleVertical, 0, 0);
        for(int i = 0; i < checks.Count; i++) {
            line.SetPosition(i,
                Simulate(launchPoint.position, startVelocity, lineStart + checks[i]));
        }

    }
}
