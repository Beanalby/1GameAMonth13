using UnityEngine;
using System.Collections;

public class TinyBoard : MonoBehaviour {

    public Spot spotPrefab;

    private Spot[] spots;

    public void Start() {
        spots = new Spot[9];
        GameObject tmp;
        for(int i = 0; i < 9; i++) {
            tmp = Instantiate(spotPrefab.gameObject) as GameObject;
            tmp.name = name + "-spot" + i;
            tmp.transform.parent = transform;
            tmp.transform.localPosition = new Vector3(
                (i % 3) - 1,
                (i / 3) - 1,
                0);
        }
    }
}
