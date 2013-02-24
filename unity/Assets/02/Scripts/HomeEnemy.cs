using UnityEngine;
using System.Collections;

public class HomeEnemy : HomeBase {

    public GameObject enemyTemplate;
    public float spawnCooldown;

    private float lastSpawn = 1;

	void Update () {
        SpawnEnemy();
	}

    public void Die() {
        Debug.Log("KABOOOM!");
        Destroy(this.gameObject);
    }
    private void SpawnEnemy()  {
        if (lastSpawn + spawnCooldown > Time.time)
            return;
        lastSpawn = Time.time;
        Spawn(enemyTemplate);
    }
}
