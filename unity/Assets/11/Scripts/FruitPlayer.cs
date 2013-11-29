using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FruitPlayer : MonoBehaviour {

    public FruitHolder holder;

    private float maxDistance = 6;
    private float slideSpeed = 10;

    public void Update() {
        HandleInput();
    }
    public void FixedUpdate() {
        HandleMovement();
    }

    public void HandleInput() {
        if(Input.GetButtonDown("Fire1")) {
            holder.DropFruit(rigidbody.velocity);
        }
    }
    public void HandleMovement() {
        Vector3 pos = rigidbody.position;
        pos.x += Input.GetAxis("Horizontal") * Time.deltaTime * slideSpeed;
        pos.x = Mathf.Max(-maxDistance, Mathf.Min(maxDistance, pos.x));
        rigidbody.MovePosition(pos);
    }
}
