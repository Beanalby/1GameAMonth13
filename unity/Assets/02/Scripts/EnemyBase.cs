using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
	}

    public void Die() {
        Debug.Log("KABOOOM!");
        Destroy(this.gameObject);
    }
}
