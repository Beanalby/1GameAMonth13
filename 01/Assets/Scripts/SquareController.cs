using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

    public Material lightMaterial;
    public Material darkMaterial;

    public int boardIndex;

    private bool isCorrupted = false;
    private MeshRenderer mr;
    
	// Use this for initialization
	void Start() {
        InitIndex();
        mr = GetComponentInChildren<MeshRenderer>();
        // we might've already been toggled when created
        UpdateMaterial();
	}
    private void InitIndex() {
        float width = GetComponentInChildren<BoxCollider>().size.x;
        float length = GetComponentInChildren<BoxCollider>().size.z;
        gameObject.name = "sq" + boardX + boardY;
        transform.position = new Vector3(
            transform.position.x + (boardX + .5f) * width,
            transform.position.y,
            transform.position.z + (boardY + .5f) * length);
    }

    public void Toggle() {
        isCorrupted = !isCorrupted;
        UpdateMaterial();
    }
    private void UpdateMaterial() {
        if(mr == null) { // might not be Start()ed yet
            return;
        }
        if(isCorrupted) {
            mr.material = darkMaterial;
        } else {
            mr.material = lightMaterial;
        }
    }
    public bool IsCorrupted {
        get { return isCorrupted; }
    }
    public int boardX {
        get { return (int)(Mathf.Floor(boardIndex / BoardController.BoardSize)); }
    }
    public int boardY {
        get { return boardIndex % 5; }
    }
}
