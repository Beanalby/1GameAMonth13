using UnityEngine;
using System.Collections;

public class CuteAttackable : MonoBehaviour {
    public GameObject DeathEffect;
    public bool MustDestroy = true;

    public void Attacked() {
        Debug.Log(name + " got attacked!");
        if(DeathEffect != null) {
            GameObject obj = Instantiate(DeathEffect) as GameObject;
            obj.transform.position = transform.position;
        } 
        Destroy(gameObject);
    }
}
