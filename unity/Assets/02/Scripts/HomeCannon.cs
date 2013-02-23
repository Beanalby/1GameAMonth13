using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class HomeCannon : WeaponRanged {

    public GameObject targetTemplate;

    private float targetSpeed = 20f;

    private LineRenderer line;
    private BoxCollider cannonArea;
    private Vector3 targetDir;
	new void Start () {
        base.Start();
        range = 1000f;
        cooldown = 0f;
        damage = 100;
        autoTarget = false;
        cannonArea = GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
	}
	new void Update () {
        base.Update();
        DrawLine();
        HandleFiring();
	}

    /// <summary>
    /// If the mouse cursor is over the canon's target area, draw a line from
    /// our base to the target.
    /// </summary>
    void DrawLine() {
        // don't change the line while we're targetting
        if (target != null)
            return;
        Vector3 cannonHit = GetMouseInCannonArea();
        if (cannonHit == Vector3.zero) {
            line.enabled = false;
            return;
        }
        line.enabled = true;
        Vector3 startPos = transform.position;
        startPos.y = .1f;
        Vector3 rayDir = cannonHit - transform.position;
        rayDir.y = 0;
        Ray ray = new Ray(startPos, rayDir);
        line.SetPosition(0, startPos);
        line.SetPosition(1, ray.GetPoint(1000) );
    }
    private void FireCannon() {
        Projectile proj = base.FireWeapon();
        proj.reticle = target;
        target = null;
    }
    private Vector3 GetMouseInCannonArea() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (cannonArea.Raycast(ray, out hit, Mathf.Infinity)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
    void HandleFiring() {
        if (target != null) {
            if (Input.GetButtonUp("Fire1")) {
                FireCannon();
            } else {
                MoveTarget();
            }
        } else {
            if (Input.GetButtonDown("Fire1")) {
                StartFiring();
            }
        }
    }
    void MoveTarget() {
        target.transform.position += targetDir * Time.deltaTime * targetSpeed;
    }
    void StartFiring() {
        // try to fire if we're pointed at the target area
        Vector3 mouseHit = GetMouseInCannonArea();
        if(mouseHit == Vector3.zero)
            return;
        target = Instantiate(targetTemplate) as GameObject;
        Vector3 targetPos = transform.position;
        targetPos.y = .1f;
        target.transform.position = targetPos;
        targetDir = (mouseHit - targetPos);
        targetDir.y = 0;
        targetDir.Normalize();
    }
}
