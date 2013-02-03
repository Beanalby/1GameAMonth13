using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

    public GameObject squareEffect;

    public Material[] darkMaterial;
    public Material[] lightMaterial;

    public int boardIndex;

    private bool _isCorrupted = false;
    private MeshRenderer mr;

	// Use this for initialization
	void Start() {
        InitIndex();
        mr = GetComponentInChildren<MeshRenderer>();
        // we might've already been toggled when created
        UpdateMaterial(false);
	}

    public bool isCorrupted {
        get { return _isCorrupted; }
        set {
            if(value != _isCorrupted) {
                _isCorrupted = value;
                UpdateMaterial(true);
            }
        }
    }

    private void InitIndex() {
        float width = GetComponentInChildren<BoxCollider>().size.x;
        float length = GetComponentInChildren<BoxCollider>().size.z;
        int x = BoardState.IndexToX(boardIndex);
        int y = BoardState.IndexToY(boardIndex);
        gameObject.name = "sq" + x + y;
        transform.position = new Vector3(
            transform.position.x + (x + .5f) * width,
            transform.position.y,
            transform.position.z + (y + .5f) * length);
    }
    public void SetIsCorrupted(bool isCorrupted, bool doEffect) {
        if(isCorrupted != this.isCorrupted) {
            this.isCorrupted = isCorrupted;
            UpdateMaterial(doEffect);
        }
    }
    private void UpdateMaterial(bool doEffect) {
        if(mr == null) { // might not be Start()ed yet
            return;
        }
        if(isCorrupted) {
            mr.material = darkMaterial[(int)Random.Range(0, darkMaterial.Length-1)];
        } else {
            mr.material = lightMaterial[(int)Random.Range(0, lightMaterial.Length-1)];
        }
        if(doEffect && squareEffect != null) {
            GameObject tmp = Instantiate(squareEffect) as GameObject;
            tmp.transform.position = transform.position;
            tmp.GetComponent<SquareEffect>().isCorrupted = isCorrupted;
        }
    }

}
