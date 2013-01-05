using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public GameObject squareTemplate;

    private int boardSize = 5;
    private float squareX, squareZ;

    public SquareController[,] squares;

	// Use this for initialization
	void Start () {
        squareX = squareTemplate.GetComponent<BoxCollider>().size.x;
        squareZ = squareTemplate.GetComponent<BoxCollider>().size.z;

        squares = new SquareController[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                squares[i,j] = ((GameObject)GameObject.Instantiate(squareTemplate)).GetComponent<SquareController>();
                squares[i,j].gameObject.name = "sq" + i + j;
                squares[i, j].boardX = i; squares[i, j].boardY = j;
                squares[i,j].transform.parent = transform;
                squares[i,j].transform.position = new Vector3(
                    squares[i,j].transform.position.x + (i+.5f) * squareX,
                    squares[i,j].transform.position.y,
                    squares[i,j].transform.position.z + (j+.5f) * squareZ);
            }
        }
	}

    public Rect GetBounds()
    {
        return new Rect(0, 0, squareX * boardSize, squareZ * boardSize);
    }

    public void SquareHit(SquareController sc)
    {
        sc.Toggle();
        // toggle any additional squares around them
        if (sc.boardX > 0)
            squares[sc.boardX - 1, sc.boardY].Toggle();
        if(sc.boardX < boardSize - 1)
            squares[sc.boardX + 1, sc.boardY].Toggle();
        if(sc.boardY > 0)
            squares[sc.boardX, sc.boardY - 1].Toggle();
        if(sc.boardY < boardSize - 1)
            squares[sc.boardX, sc.boardY + 1].Toggle();
    }

    public void OnDrawGizmos()
    {
        Vector3 size = squareTemplate.GetComponent<BoxCollider>().size;
        Gizmos.color = Color.green / 2;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Vector3 pos = new Vector3((i + .5f) * size.x, 0, (j + .5f) * size.z);
                Gizmos.DrawWireCube(pos, size);
            }
        }
    }
}
