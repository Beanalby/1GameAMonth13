using UnityEngine;
using System.Collections;

public enum TriggerType { LandedOn, Switch };

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour {
    public float speed = 2f;
    public TriggerType triggerType;
    public bool MovingOnStart = false;

    private Player player;
    private Vector3 originalPosition;
    private bool isMoving = false;


    void Start() {
        originalPosition = transform.position;
        isMoving = MovingOnStart;
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
        if(triggerType == TriggerType.LandedOn && !isMoving) {
            isMoving = true;
            player.resetListeners += OnReset;
        }
    }
    public void Die() {
        isMoving = false;
        collider.enabled = false;
        renderer.enabled = false;
    }
    public void OnReset() {
        isMoving = MovingOnStart;
        transform.position = originalPosition;
        collider.enabled = true;
        renderer.enabled = true;
        player.resetListeners -= OnReset;
    }
    public void SwitchToggled(bool state) {
        if(triggerType == TriggerType.Switch) {
            isMoving = state;
        }
        player.resetListeners += OnReset;
    }
}
