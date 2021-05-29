using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FruitPlayer : MonoBehaviour {

    public bool CanControl = true;

    public FruitHolder holder;
    public FruitLauncher launcher;

    private float maxDistance = 6;
    private float slideSpeed = 10;

    public void Update() {
        if(CanControl) {
            HandleInput();
        }
    }
    public void FixedUpdate() {
        if(CanControl) {
            HandleMovement();
        }
    }

    public void HandleInput() {
        if(Input.GetButton("Fire1")) {
            holder.DropFruit(GetComponent<Rigidbody>().velocity);
        }
    }
    public void HandleMovement() {
        Vector3 pos = GetComponent<Rigidbody>().position;
        pos.x += Input.GetAxis("Horizontal") * Time.deltaTime * slideSpeed;
        pos.x = Mathf.Max(-maxDistance, Mathf.Min(maxDistance, pos.x));
        GetComponent<Rigidbody>().MovePosition(pos);
    }
}
