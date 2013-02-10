using UnityEngine;
using System.Collections;

public class WeaponJack : WeaponBase {

	new void Start () {
        base.Start();
        range = 3;
	}
	void Update () {
	}

    public override void FireWeapon() {
        Debug.Log("PEW!");
    }
}

