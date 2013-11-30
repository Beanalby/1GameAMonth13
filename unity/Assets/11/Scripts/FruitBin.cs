using UnityEngine;
using System.Collections;

public class FruitBin : MonoBehaviour {
    public FruitType Type;
    public GameObject effectPrefab;

    public void Start() {
        // adjust this bin's color
        GetComponentInChildren<Light>().color = SplatEffect.type2color[Type];
    }
    public void OnTriggerEnter(Collider col) {
        Fruit fruit = col.transform.GetComponent<Fruit>();
        if(fruit == null) {
            Debug.LogError("!!! No fruit in bin's collider?");
            return;
        }
        if(fruit.Type == Type) {
            SplatEffect effect = (Instantiate(effectPrefab) as GameObject).GetComponent<SplatEffect>();
            effect.transform.position = transform.position;
            effect.Type = Type;
        } else {
            Debug.Log("EWWW, " + Type + "!");
        }
        Destroy(fruit.gameObject);
    }
}
