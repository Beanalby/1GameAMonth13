using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public static int BoardSize = 5;

    private List<int> silentPattern1, silentPattern2, silentPattern3;
    private List<SquareController> squares;
    public GameObject squareTemplate;
    private float squareX, squareZ;

	void Start() {
        squareX = squareTemplate.GetComponent<BoxCollider>().size.x;
        squareZ = squareTemplate.GetComponent<BoxCollider>().size.z;

        squares = new List<SquareController>(BoardSize * BoardSize);
        for(int i = 0; i < BoardSize*BoardSize; i++) {
            squares.Add(((GameObject)GameObject.Instantiate(squareTemplate)).GetComponent<SquareController>());
            squares[i].boardIndex = i;
            squares[i].transform.parent = transform;
        }
        InitSilentPatterns();
	}

    void Update() {
        // only for development / debugging
        if(Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log(DumpBoard());
        }
        if(Input.GetKeyDown(KeyCode.Mouse2)) { // toggle single square
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << LayerMask.NameToLayer("Square")))) {
                hit.collider.gameObject.GetComponent<SquareController>().Toggle();
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse3)) { // make solvable
            List<int> boardState = Board2list();
            if(!IsSolvable(boardState)) {
                MakeSolvable(ref boardState);
                SetBoard(boardState);
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse4)) {
            RandomizeBoard();
        }
    }
    public void OnDrawGizmos() {
        Vector3 size = squareTemplate.GetComponent<BoxCollider>().size;
        Gizmos.color = Color.green / 2;
        for(int i = 0; i < BoardSize; i++) {
            for(int j = 0; j < BoardSize; j++) {
                Vector3 pos = new Vector3((i + .5f) * size.x, -.5f * size.y, (j + .5f) * size.z);
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }

    private List<int> Board2list() {
        List<int> boardState = new List<int>();
        foreach(SquareController sc in squares) {
            if(sc.isCorrupted) {
                boardState.Add(sc.boardIndex);
            }
        }
        return boardState;
    }

    public string DumpBoard() {
        Converter<int, string> converter = delegate(int i) { return i.ToString(); };
        string[] boardState = Board2list().ConvertAll<string>(converter).ToArray();
        return "{" + string.Join(", ", boardState.ToArray()) + "}";
    }

    private void InitSilentPatterns() {
        /* XX.XX
         * .....
         * XX.XX
         * .....
         * XX.XX */
        silentPattern1 = new List<int>() {
            0,  1,  3,  4,
            10, 11, 13, 14,
            20, 21, 23, 24
        };
        /* X.X.X
         * X.X.X
         * .....
         * X.X.X
         * X.X.X */
        silentPattern2 = new List<int>() {
            0,  2,  4,
            5,  7,  9,
            15, 17, 19,
            20, 22, 24
        };
        /* 3 is combination of first two
         * .XXX.
         * X.X.X
         * XX.XX
         * X.X.X
         * .XXX. */
        silentPattern3 = new List<int>() {
            1, 2, 3,
            5, 7, 9,
            10, 11, 13, 14,
            15, 17, 19,
            21, 22, 23
        };
    }

    public bool IsClear() {
        foreach(SquareController sc in squares) {
            if(sc.isCorrupted) {
                return false;
            }
        }
        return true;
    }

    public bool IsSolvable(List<int> boardState) {
        /* a board is unsolvable if either of the silent patterns
         * have an odd number marked. */
        return PassPattern(boardState, silentPattern1)
            && PassPattern(boardState, silentPattern2);
    }

    public Rect GetBounds() {
        return new Rect(0, 0, squareX * BoardSize, squareZ * BoardSize);
    }

    public void MakeSolvable(ref List<int> boardState) {
        bool pass1 = PassPattern(boardState, silentPattern1);
        bool pass2 = PassPattern(boardState, silentPattern2);
        if(pass1 && pass2) {
            return;
        } else if(pass1 && !pass2) {
            if(boardState.Contains(5)) {
                boardState.Remove(5);
            } else {
                boardState.Add(5);
            }
        } else if(!pass1 && pass2) {
            if(boardState.Contains(1)) {
                boardState.Remove(1);
            } else {
                boardState.Add(1);
            }
        } else {
            if(boardState.Contains(0)) {
                boardState.Remove(0);
            } else {
                boardState.Add(0);
            }
        }
    }

    private bool PassPattern(List<int> boardState, List<int> pattern) {
        // board state passes the silent pattern if there's an
        // even number of its squares corrupted
        return (boardState.Intersect(pattern).Count() % 2) == 0;
    }
    public void RandomizeBoard() {
        List<int> boardState = new List<int>();
        for(int i = 0; i < BoardSize * BoardSize; i++) {
            if(UnityEngine.Random.value > .5f) {
                boardState.Add(i);
            }
        }
        MakeSolvable(ref boardState);
        SetBoard(boardState);
    }

    public void SetBoard(List<int> boardState) {
        foreach(SquareController sc in squares) {
            sc.isCorrupted = boardState.Contains(sc.boardIndex);
        }
    }

    public void SquareHit(SquareController sc) {
        sc.Toggle();
        // toggle any additional squares around them
        if(sc.boardY > 0)
            squares[sc.boardIndex-1].Toggle();
        if(sc.boardY < BoardSize - 1)
            squares[sc.boardIndex+1].Toggle();
        if(sc.boardX > 0)
            squares[sc.boardIndex-BoardSize].Toggle();
        if(sc.boardX < BoardSize - 1)
            squares[sc.boardIndex+BoardSize].Toggle();
    }
}
