using UnityEngine;
using System.Collections;

public class HomeEnemy : HomeBase {

    public GUISkin skin;

    public GameObject enemyTemplate;
    public float spawnCooldown;

    private float lastSpawn = -100;
    private bool isDead = false;

    new public void Start() {
        base.Start();
        GetComponent<ShootableThing>().defaultDieAction = false;
    }
	void Update () {
        SpawnEnemy();
	}
    void OnGUI() {
        if(isDead) {
            GUI.skin = skin;
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("You win!  The internet is cleansed!");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    public void Die() {
        Debug.Log("KABOOOM!");
        isDead = true;
        Time.timeScale = 0;
        transform.Find("Cube").gameObject.SetActive(false);
    }
    private void SpawnEnemy()  {
        if(spawnCooldown == 0)
            return;
        if (lastSpawn + spawnCooldown > Time.time)
            return;
        lastSpawn = Time.time;
        Spawn(enemyTemplate);
    }
}
