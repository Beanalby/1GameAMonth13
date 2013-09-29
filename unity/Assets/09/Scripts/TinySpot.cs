using UnityEngine;
using System.Collections;

public class TinySpot : Spot {

    private SpotValue value = SpotValue.None;
    public GameObject prefabX;
    public GameObject prefabO;

    GameObject mark = null;

    public override SpotValue GetValue() {
        return value;
    }

    public void Chosen(SpotValue currentTurn) {
        if(value != SpotValue.None) {
            return;
        }
        value = currentTurn;
        MakeMark();
        GameDriver9.instance.SendMessage("MoveMade");
    }

    private void MakeMark() {
        if(value == SpotValue.X) {
            mark = Instantiate(prefabX) as GameObject;
        } else {
            mark = Instantiate(prefabO) as GameObject;
        }
        mark.transform.parent = transform;
        mark.transform.localPosition = Vector3.zero;
    }
}
