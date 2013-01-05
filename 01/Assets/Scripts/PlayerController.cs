using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    private float walkSpeed = 5f;
    private float fireSpeed = 1f;

    private CapsuleCollider cc;
    private BoardController board;

    public GameObject ProgressCircleTemplate;
    private ProgressCircle pc;
    private float pcStart;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CapsuleCollider>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
        pcStart = -1;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovement();
        HandleFiring();
        if (pcStart != -1)
        {
            if (Time.time > pcStart + fireSpeed)
            {
                Destroy(pc.gameObject);
                pcStart = -1;
            }
            else
            {
                float percent = Mathf.InverseLerp(pcStart + fireSpeed, pcStart, Time.time);
                Debug.Log("Setting to " + percent);
                pc.percent = percent;
            }
        }
    }

    void HandleFiring()
    {
        if (!Input.GetButtonDown("Jump"))
            return;
        // see what's beneath us
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += .1f;
        if (Physics.Raycast(pos, -Vector3.up, out hit, cc.height * 1.1f))
        {
            SquareController sc = hit.collider.gameObject.GetComponent<SquareController>();
            if (sc != null)
            {
                board.SquareHit(sc);
                pc = ((GameObject)GameObject.Instantiate(ProgressCircleTemplate)).GetComponent<ProgressCircle>();
                pos = sc.transform.position;
                pos.y += .1f;
                pc.transform.position = pos;
                pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
                pcStart = Time.time - .01f;
            }
        }
    }
    void UpdateMovement()
    {
        float h = Input.GetAxis("Horizontal"), v = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += Time.deltaTime * walkSpeed * h;
        pos.z += Time.deltaTime * walkSpeed * v; 

        // Don't go outside the board game
        Rect bounds = board.GetBounds();
        pos.x = Mathf.Max(bounds.x+cc.radius, (Mathf.Min(bounds.width-cc.radius, pos.x)));
        pos.z = Mathf.Max(bounds.y+cc.radius, (Mathf.Min(bounds.height-cc.radius, pos.z)));

        if (h != 0 || v != 0)
        {
            Vector3 lookTarget = new Vector3(h, transform.position.y, v);
            transform.rotation = Quaternion.LookRotation(lookTarget);
            transform.position = pos;
        }
	}
}
