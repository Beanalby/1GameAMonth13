using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public GameObject squareTemplate;

    public GameObject driver;
    private List<SquareController> squares;
    private BoardState state;
    private float squareX, squareZ;

	void Start() {
        squareX = squareTemplate.GetComponent<BoxCollider>().size.x;
        squareZ = squareTemplate.GetComponent<BoxCollider>().size.z;

        GameObject tmp = GameObject.Find("GameDriver");
        if(tmp != null) {
            driver = tmp;
            state = tmp.GetComponent<GameDriver>().currentLevel.state;
        } else {
            state = new BoardState();
        }
        squares = new List<SquareController>(BoardState.BoardSize);
        for(int i = 0; i < BoardState.BoardSize; i++) {
            squares.Add(((GameObject)GameObject.Instantiate(squareTemplate)).GetComponent<SquareController>());
            squares[i].boardIndex = i;
            squares[i].transform.parent = transform;
        }
        UpdateSquares();
	}

    void Update() {
        // dump state (only for development / debugging)
        if(Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log(state);
        }
        if(Input.GetKeyDown(KeyCode.Mouse2)) { // toggle single square
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Square")))) {
                int index = hit.collider.gameObject.GetComponent<SquareController>().boardIndex;
                state.Toggle(index);
                UpdateSquares();
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse3)) {
            state.MakeSolvable();
            UpdateSquares();
        }
        if(Input.GetKeyDown(KeyCode.Mouse4)) {
            state.RandomizeBoard();
            UpdateSquares();
        }
    }
    public void OnDrawGizmos() {
        Vector3 size = squareTemplate.GetComponent<BoxCollider>().size;
        Gizmos.color = Color.green / 2;
        for(int i = 0; i < BoardState.BoardSide; i++) {
            for(int j = 0; j < BoardState.BoardSide; j++) {
                Vector3 pos = new Vector3((i + .5f) * size.x, -.5f * size.y, (j + .5f) * size.z);
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }

    public Rect GetBounds() {
        return new Rect(0, 0,
            squareX * BoardState.BoardSide, squareZ * BoardState.BoardSide);
    }
    public SquareController getSquare(int index) {
        return squares[index];
    }
    public void SetBoard(BoardState state) {
        this.state = state;
        Debug.Log("Board state set to " + this.state);
        UpdateSquares();
    }
    public void SetBoard(List<int> boardState) {
        state.Set(boardState);
        UpdateSquares();
    }

    public void SquareHit(SquareController sc) {
        state.ToggleArea(sc.boardIndex);
        UpdateSquares();
    }

    private void UpdateSquares() {
        foreach(SquareController sc in squares) {
            sc.isCorrupted = state.Get(sc.boardIndex);
        }
        if(driver != null && state.IsClear()) {
            driver.SendMessage("BoardClear");
        }
    }
}
