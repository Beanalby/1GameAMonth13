using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour {

    Transform mesh, cam;

    public void Start() {
        mesh = transform.Find("mesh");
        cam = Camera.main.transform;
    }
    public void Update() {
        mesh.rotation = cam.rotation;
    }
}
