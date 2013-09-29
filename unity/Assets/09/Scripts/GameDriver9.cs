using UnityEngine;
using System.Collections;

public class GameDriver9 : MonoBehaviour {

    private static GameDriver9 _instance;
    public static GameDriver9 instance {
        get { return _instance; }
    }
    private SpotValue currentTurn;

    public void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void Start() {
        currentTurn = SpotValue.X;
    }
    public void Update() {
        HandleClick();
    }

    private void HandleClick() {
        if(!Input.GetMouseButtonDown(0)) {
            return;
        }

        // find out if they clicked on anything clickable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            hit.collider.SendMessage("Chosen", currentTurn);
        }
    }

    public void MoveMade() {
        if(currentTurn == SpotValue.X) {
            currentTurn = SpotValue.O;
        } else {
            currentTurn = SpotValue.X;
        }
    }
}
