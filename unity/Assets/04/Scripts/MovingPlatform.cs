using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour {
    public float speed = 2f;

    private Player player;
    private Vector3 originalPosition;
    private bool isMoving = false;

    void Start() {
        originalPosition = transform.position;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update() {
        if(isMoving) {
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            rigidbody.MovePosition(pos);
        }
    }
    public void LandedOn(Player player) {
        isMoving = true;
        player.resetListeners += OnReset;
    }
    public void Die() {
        isMoving = false;
        collider.enabled = false;
        renderer.enabled = false;
    }
    public void OnReset() {
        isMoving = false;
        transform.position = originalPosition;
        collider.enabled = true;
        renderer.enabled = true;
        player.resetListeners -= OnReset;
    }
}
