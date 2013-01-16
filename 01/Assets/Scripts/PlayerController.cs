using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    private float walkSpeed = 5f;
    private float chargeTime = 1f;

    private CapsuleCollider cc;
    private BoardController board;

    public GameObject ProgressCircleTemplate;
    private ProgressCircle pc;
    private float pcStart;
    private SquareController pcTarget;

	void Start () {
        cc = GetComponent<CapsuleCollider>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
        pcStart = -1;
	}
	
	void Update () {
        UpdateMovement();
        HandleCharging();
        HandleFiring();
    }

    void HandleFiring()
    {

    }
    void HandleCharging()
    {
        if (pcStart == -1 && Input.GetButtonDown("Jump"))  {
            // started charging, grab the target square
            RaycastHit hit;
            Vector3 pos = transform.position;
            pos.y += .1f;
            if (Physics.Raycast(pos, -Vector3.up, out hit, cc.height * 1.1f))  {
                pcTarget = hit.collider.gameObject.GetComponent<SquareController>();
                if (pcTarget != null)  {
                    pc = ((GameObject)GameObject.Instantiate(ProgressCircleTemplate)).GetComponent<ProgressCircle>();
                    pos = pcTarget.transform.position;
                    pos.y += .1f;
                    pc.transform.position = pos;
                    pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
                    pcStart = Time.time;
                    //Debug.Break();
                }
            }
            return;
        }
        else if (pcStart != -1)  {
            if (Input.GetButtonUp("Jump"))  {
                // they let go, cancel charging
                Destroy(pc.gameObject);
                pcStart = -1;
                return;
            }
            if (Time.time > pcStart + chargeTime)  {
                // finished charging, DOO EET
                Destroy(pc.gameObject);
                pcStart = -1;
                board.SquareHit(pcTarget);
                return;
            }
            // still charging, update the progress
            float percent = Mathf.InverseLerp(pcStart, pcStart + chargeTime, Time.time);
            pc.percent = percent;
            return;
        }
    }
    void UpdateMovement() {
        // don't allow any movement if they're charging
        if (pcStart != -1)
            return;
        float h = Input.GetAxis("Horizontal"), v = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += Time.deltaTime * walkSpeed * h;
        pos.z += Time.deltaTime * walkSpeed * v; 

        // Don't go outside the board game
        Rect bounds = board.GetBounds();
        pos.x = Mathf.Max(bounds.x+cc.radius, (Mathf.Min(bounds.width-cc.radius, pos.x)));
        pos.z = Mathf.Max(bounds.y+cc.radius, (Mathf.Min(bounds.height-cc.radius, pos.z)));

        Vector3 lookTarget = new Vector3(h, transform.position.y, v);
        if (lookTarget != Vector3.zero)  {
            transform.rotation = Quaternion.LookRotation(lookTarget);
            transform.position = pos;
        }
	}
}
