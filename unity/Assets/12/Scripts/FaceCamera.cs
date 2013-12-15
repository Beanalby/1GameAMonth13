using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour {
    public void Start() {
        transform.Find("mesh").rotation = Camera.main.transform.rotation;
    }
}
