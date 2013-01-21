using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

    public Material darkMaterial;
    public Material lightMaterial;

    public int boardIndex;

    private bool _isCorrupted = false;
    private MeshRenderer mr;
    
	// Use this for initialization
	void Start() {
        InitIndex();
        mr = GetComponentInChildren<MeshRenderer>();
        // we might've already been toggled when created
        UpdateMaterial();
	}

    public int boardX {
        get { return (int)(Mathf.Floor(boardIndex / BoardController.BoardSize)); }
    }
    public int boardY {
        get { return boardIndex % 5; }
    }
    public bool isCorrupted {
        get { return _isCorrupted; }
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
        _isCorrupted = !_isCorrupted;
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
}
