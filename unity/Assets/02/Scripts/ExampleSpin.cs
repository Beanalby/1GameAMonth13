using UnityEngine;
using System.Collections;

public class ExampleSpin : MonoBehaviour {
    public float spinSpeed = 10;
	void Update () {
        Vector3 angles = transform.rotation.eulerAngles;
        angles.y += spinSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(angles);
	}
}
