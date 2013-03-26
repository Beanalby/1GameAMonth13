using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class ballLauncher : MonoBehaviour {

    static float verticalWidth = .05f;

    private static Vector3 Simulate(Vector3 startPos, Vector3 velocity,
            float duration) {
        Vector3 endPos = startPos + velocity * duration;
        endPos.y += Mathf.Pow(duration, 2) * Physics.gravity.y / 2f;
        return endPos;
    }

    public GameObject aimCollisionTemplate;
    public GameObject ballTempalte;
    public Material verticalMat;

    private float powerScale = 5f;

    private float angleVertical = 45f;
    private float angleHorizontal = 0f;
    private Transform cannonMesh, launchPoint;
    private bool loaded = true;
    private Vector3 startVelocity;
    private int lineSegments = 32;
    private float segmentScale = 1/8f;
    private float lineStart = 0f;
    private float maxRimDistance = 4f;
    private float power = 7f;
    private float powerMin = 6f;
    private float powerMax = 8f;
    private float radius;

    private GameObject aimCollision;
    private Plane aimPlane, rimPlane;
    private GameObject[] rims;

    private LineRenderer line, vertical;
    private List<float> checks;

    void Start() {
        aimPlane = new Plane();
        rims = GameObject.FindGameObjectsWithTag("Rim");
        aimCollision = Instantiate(aimCollisionTemplate) as GameObject;
        aimCollision.SetActive(false);

        radius = ballTempalte.GetComponent<SphereCollider>().radius;
        cannonMesh = transform.Find("CannonMesh");
        launchPoint = cannonMesh.Find("LaunchPoint");
        line = GetComponent<LineRenderer>();
        checks = Enumerable.Range(0, lineSegments)
            .Select(x => x * segmentScale).ToList();
        line.SetVertexCount(lineSegments);

        // make a dummy gameobj to hold the vertical's linerenderer
        GameObject tmp = new GameObject("verticalLineHolder");
        tmp.transform.parent = transform;
        vertical = tmp.AddComponent<LineRenderer>();
        vertical.material = verticalMat;
        vertical.SetVertexCount(2);
        vertical.SetWidth(verticalWidth, verticalWidth);
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
        aimCollision.SetActive(false);
        // figure out which rim is the closest to our aiming plane
        aimPlane.SetNormalAndPosition(transform.right, transform.position);
        GameObject closestRim = null;
        float closestDistance = float.MaxValue, distance;
        foreach(GameObject rim in rims) {
            distance = aimPlane.GetDistanceToPoint(rim.transform.position);
            if(Mathf.Abs(distance) < Mathf.Abs(closestDistance)) {
                closestRim = rim;
                closestDistance = distance;
            }
        }
        Vector3 rimOnAimPlane = Vector3.zero;
        if(closestRim != null) {
            rimOnAimPlane = closestRim.transform.position - closestDistance * aimPlane.normal;
            rimPlane.SetNormalAndPosition(closestRim.transform.up,
                closestRim.transform.position);
            // draw a line between the aimplane and the rim
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
        Vector3 previous = launchPoint.position;
        line.SetVertexCount(checks.Count);
        int mask = ~(1 << LayerMask.NameToLayer("Player"));
        for(int i = 0; i < checks.Count; i++) {
            Vector3 next = Simulate(launchPoint.position, startVelocity, lineStart + checks[i]);
            // see if this goes through the rim's plane
            if(closestRim != null && rimPlane.GetSide(previous) && !rimPlane.GetSide(next)) {
                Ray aimRay = new Ray(previous, next - previous);
                float rimPlaneDist;
                rimPlane.Raycast(aimRay, out rimPlaneDist);
                Vector3 aimPoint = aimRay.GetPoint(rimPlaneDist);
                // don't bother unless it's within maxRimDistance away
                float aimDistance = (aimPoint - closestRim.transform.position).magnitude;
                if(aimDistance < maxRimDistance) {
                    Debug.DrawLine(rimOnAimPlane, closestRim.transform.position, Color.yellow);
                    Debug.DrawLine(aimPoint, rimOnAimPlane, Color.green);
                    line.SetPosition(i, aimPoint);
                    previous = aimPoint;
                    line.SetVertexCount(i + 1);
                    break;
                }
            }

            // see if this will collide with anything
            RaycastHit hit;
            Ray collideRay = new Ray(previous, next - previous);
            float dist = (next - previous).magnitude;
            if(Physics.SphereCast(collideRay, radius, out hit, dist, mask)) {
                Vector3 hitPoint = collideRay.GetPoint(hit.distance);
                Debug.DrawRay(hit.point, hit.normal * 5, Color.green);
                aimCollision.transform.position = hit.point;
                aimCollision.transform.rotation = Quaternion.LookRotation(-hit.normal);
                aimCollision.SetActive(true);
                line.SetPosition(i, hitPoint);
                previous = hitPoint;
                line.SetVertexCount(i + 1);
                break;
            }

            // nothing stopping this, contine on to the next segment
            line.SetPosition(i, next);
            previous = next;
        }
        vertical.SetPosition(0, previous);
        vertical.SetPosition(1, new Vector3(previous.x, 0, previous.z));
    }
}
