using UnityEngine;
using System.Collections;

public class TinySpot : MonoBehaviour, Spot {

    private SpotValue value = SpotValue.None;
    public GameObject prefabX;
    public GameObject prefabO;

    private GameObject mark = null;
    private TinyBoard board;
    public TinyBoard Board {
        get { return board; }
    }

    public void Start() {
        board = transform.parent.GetComponent<TinyBoard>();
    }

    public SpotValue GetValue() {
        return value;
    }

    public void MakeMark(SpotValue newValue) {
        value = newValue;
        if(value == SpotValue.X) {
            mark = Instantiate(prefabX) as GameObject;
        } else {
            mark = Instantiate(prefabO) as GameObject;
        }
        mark.transform.parent = transform;
        mark.transform.localPosition = Vector3.zero;
    }
}
