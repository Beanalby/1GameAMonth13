using UnityEngine;
using System.Collections;

public class tmpSpinner : MonoBehaviour {

    public float spinSpeed=10f;

    // Update is called once per frame
    void Update () {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.y += spinSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(euler);
    }
}
