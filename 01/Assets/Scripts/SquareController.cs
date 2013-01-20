using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

    public Material lightMaterial;
    public Material darkMaterial;

    public int boardX, boardY;

    private bool isLight = false;
    private MeshRenderer mr;
    
	// Use this for initialization
	void Start() {
        mr = GetComponentInChildren<MeshRenderer>();
        // we might've already been toggled when created
        UpdateMaterial();
	}
	
    public void Toggle() {
        isLight = !isLight;
        UpdateMaterial();
    }
    private void UpdateMaterial() {
        if(mr == null) { // might not be Start()ed yet
            return;
        }
        if(isLight) {
            mr.material = lightMaterial;
        } else {
            mr.material = darkMaterial;
        }
    }
}
