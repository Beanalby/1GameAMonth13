using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    private float walkSpeed = 5f;

    private CapsuleCollider cc;
    private BoardController board;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CapsuleCollider>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x += Time.deltaTime * walkSpeed * Input.GetAxis("Horizontal");
        pos.z += Time.deltaTime * walkSpeed * Input.GetAxis("Vertical");

        // Don't go outside the board game
        Rect bounds = board.GetBounds();
        pos.x = Mathf.Max(bounds.x+cc.radius, (Mathf.Min(bounds.width-cc.radius, pos.x)));
        pos.z = Mathf.Max(bounds.y+cc.radius, (Mathf.Min(bounds.height-cc.radius, pos.z)));

        Vector3 lookTarget = new Vector3(Input.GetAxis("Horizontal"), transform.position.y, Input.GetAxis("Vertical"));
        transform.rotation = Quaternion.LookRotation(lookTarget);
        transform.position = pos;
	}
}
