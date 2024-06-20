using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MeshFaceCamera : MonoBehaviour {
    public void Start() {
        transform.Find("mesh").rotation = Camera.main.transform.rotation;
    }
}
