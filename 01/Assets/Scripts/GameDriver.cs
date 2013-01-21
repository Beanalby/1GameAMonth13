using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDriver : MonoBehaviour {

    private BoardController bc;

    public void Start() {
        bc = GameObject.Find("Board").GetComponent<BoardController>();
    }

    public void Update() {
        List<int> boardState = null;
            //bc.SetBoard(new List<int>() { 2, 12, 22});
            //bc.SetBoard(new List<int>() {0, 1, 3, 5, 8, 15, 18, 20, 21, 23});
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            boardState = new List<int>() { 0, 1 };
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            boardState = new List<int>() { 24 };
        }
        if(boardState != null) {
            bc.SetBoard(boardState);
        }
    }
}
