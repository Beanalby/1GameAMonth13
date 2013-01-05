using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public GameObject squareTemplate;

    private int boardSize = 4;
    private float squareSpacing = 1.05f;
    private float squareX, squareZ;

    public GameObject[,] squares;

	// Use this for initialization
	void Start () {
        squareX = squareTemplate.GetComponent<BoxCollider>().size.x * 2 * squareSpacing;
        squareZ = squareTemplate.GetComponent<BoxCollider>().size.z * 2 * squareSpacing;

        squares = new GameObject[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                squares[i,j] = (GameObject)GameObject.Instantiate(squareTemplate);
                squares[i,j].transform.parent = transform;
                squares[i,j].name = "sq" + i + j;
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
}
