using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
	}

    public void Die() {
        Debug.Log(name + ": Blarg, I are dead!");
        Destroy(gameObject);
    }
}
