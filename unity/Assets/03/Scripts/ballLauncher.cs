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

    private float angleVertical = 45f;
    private float angleHorizontal = 0f;
    private float power = 10f;
    private bool loaded = true;
    private Vector3 startVelocity;
    private Vector3 startPos;

    private int lineSegments = 16;
    private float segmentScale = 1/8f;
    private float lineStart = .5f;

    private LineRenderer line;
    private List<float> checks;


    void Start() {
        line = GetComponent<LineRenderer>();
        checks = Enumerable.Range(0, lineSegments)
            .Select(x => x * segmentScale).ToList();
        line.SetVertexCount(lineSegments);
        UpdateTrajectory();
    }
    void Update() {
        UpdateRotation();
        HandleFiring();
    }

    public void FireBall() {
        GameObject ball = Instantiate(ballTempalte,
            transform.position, transform.rotation) as GameObject;
        ball.rigidbody.velocity = startVelocity;
    }
    public void HandleFiring() {
        if(Input.GetButtonDown("Fire1")) {
            FireBall();
        }
    }
    public void UpdateRotation() {
        /// scale the X & Y mouse position for rotation such that mostly
        /// left / mostly right map to full left rotation / full
        /// right rotation, and mostly top / mostly bottom map to 90deg up /
        /// straight straight forward, respectively.
        float xPercent = Input.mousePosition.x / Screen.width,
            yPercent = Input.mousePosition.y / Screen.height;

        // 20% => -90, 80% => +90, capped at those values
        angleHorizontal = Mathf.Min(90, Mathf.Max(-90,
            300 * xPercent - 150));
        
        // 20% => 90, 80% => 0, capped at those valuse
        angleVertical = Mathf.Min(90, Mathf.Max(-90,
            -150 * yPercent + 120));
        UpdateTrajectory();
        //if(loaded) {
        //    angleVertical += vert;
        //    angleHorizontal += horiz;
        //    if(vert!=0 || horiz!=0) {
        //        UpdateTrajectory();
        //    }
        //}
    }
    public void UpdateTrajectory() {
        if(!loaded) {
            return;
        }
        startVelocity = 
            Quaternion.Euler(0, angleHorizontal, 0)
            * Vector3.Slerp(Vector3.right, Vector3.up, angleVertical / 90)
            * power;
        Vector3 lookTarget = new Vector3(
            startVelocity.x, 0, startVelocity.z);
        if(lookTarget != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(lookTarget);
        }

        for(int i = 0; i < checks.Count; i++) {
            line.SetPosition(i,
                Simulate(transform.position, startVelocity, lineStart + checks[i]));
        }

    }
}
