using UnityEngine;
using System.Collections;

public delegate void SpawnEventHandler(GameObject spawned);

public abstract class HomeBase : MonoBehaviour {


    protected Quaternion spawnFacing;
    protected Transform spawnArea;

    public event SpawnEventHandler spawnListeners;

	// Use this for initialization
	public void Start () {
        spawnArea = transform.Find("SpawnArea");
        string target;
        if (name == "HomePlayer")
            target = "HomeEnemy";
        else
            target = "HomePlayer";
        spawnFacing = Quaternion.LookRotation(GameObject.Find(target).transform.position - transform.position);
	}
    void OnDrawGizmosSelected() {
        spawnArea = transform.Find("SpawnArea");
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnArea.transform.position, spawnArea.transform.localScale);
    }

    protected void Spawn(GameObject template) {
        float x = spawnArea.position.x, dx = spawnArea.localScale.x / 2,
            y = spawnArea.position.y, dy = spawnArea.localScale.y / 2,
            z = spawnArea.position.z, dz = spawnArea.localScale.z / 2;
        Vector3 pos = new Vector3( Random.Range(x-dx,x+dx), 
            Random.Range(y-dy,y+dy), Random.Range(z-dz,z+dz));
        GameObject spawned = Instantiate(template, pos, spawnFacing) as GameObject;

        if(spawnListeners != null) {
            spawnListeners(spawned);
        }
    }
}
