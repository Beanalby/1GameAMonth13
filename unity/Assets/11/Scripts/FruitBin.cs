using UnityEngine;
using System.Collections;

public class FruitBin : MonoBehaviour {
    public FruitType Type;
    public GameObject effectGood, effectBad;

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
        SplatEffect effect;
        if(fruit.Type == Type) {
            effect = (Instantiate(effectGood) as GameObject).GetComponent<SplatEffect>();
        } else {
            effect = (Instantiate(effectBad) as GameObject).GetComponent<SplatEffect>();
        }
        effect.transform.position = transform.position;
        effect.Type = Type;
        Destroy(fruit.gameObject);
    }
}
