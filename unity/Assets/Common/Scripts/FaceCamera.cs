using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
    }
}
