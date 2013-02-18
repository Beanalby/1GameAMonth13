using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

    GameObject enemyTemplate;

    private Transform spawnArea;

	void Start () {
        spawnArea = transform.Find("SpawnArea");
	}
	void Update () {
	}
    void OnDrawGizmosSelected() {
        spawnArea = transform.Find("SpawnArea");
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnArea.transform.position, spawnArea.transform.localScale);
    }

    public void Die() {
        Debug.Log("KABOOOM!");
        Destroy(this.gameObject);
    }
}
