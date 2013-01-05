using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

    public Material lightMaterial;
    public Material darkMaterial;

    public int boardX, boardY;

    private bool isLight = false;
    private MeshRenderer mr;
    
	// Use this for initialization
	void Start () {
        mr = GetComponentInChildren<MeshRenderer>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Toggle()
    {
        if (isLight)
        {
            isLight = false;
            mr.material = darkMaterial;
        }
        else
        {
            isLight = true;
            mr.material = lightMaterial;
        }
    }
}
