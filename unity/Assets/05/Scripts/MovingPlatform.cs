using UnityEngine;
using System.Collections;

public enum TriggerType { LandedOn, Switch };

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour {
    public Vector3 movement = new Vector3(2, 0, 0);
    public TriggerType triggerType;

    private Player player;
    private Vector3 originalPosition;
    private bool isMoving = false;
    private Renderer myRenderer;

    void Start() {
        originalPosition = transform.position;
        player = GameObject.Find("Player").GetComponent<Player>();
        myRenderer = GetComponentInChildren<Renderer>();
    }
    void FixedUpdate() {
        if(isMoving) {
            Vector3 pos = transform.position;
            pos += movement * Time.deltaTime;
            GetComponent<Rigidbody>().MovePosition(pos);
        }
    }
    public void LandedOn(Player player) {
        if(triggerType == TriggerType.LandedOn && !isMoving) {
            isMoving = true;
            player.resetListeners += OnReset;
        }
        if (isMoving) {
            Vector3 v = player.GetComponent<Rigidbody>().velocity;
            v.x = movement.x;
            // only push the player upwards.  If he's already moving upwards
            // faster than we'd push him, don't bother.
            v.y = Mathf.Max(v.y, movement.y);
            player.GetComponent<Rigidbody>().velocity = v;
            //if (player.rigidbody.position.y < transform.position.y) {
            //}
        }

    }
    public void Die() {
        isMoving = false;
        GetComponent<Collider>().enabled = false;
        myRenderer.enabled = false;
    }
    public void OnReset() {
        isMoving = false;
        transform.position = originalPosition;
        GetComponent<Collider>().enabled = true;
        myRenderer.enabled = true;
        player.resetListeners -= OnReset;
    }
    public void SwitchToggled(bool state) {
        if(triggerType == TriggerType.Switch) {
            isMoving = state;
        }
        player.resetListeners += OnReset;
    }
}
