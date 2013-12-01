using UnityEngine;
using System.Collections;

public class FruitBin : MonoBehaviour {
    public FruitType Type;
    public GameObject effectGood, effectBad;
    FruitGameDriver driver;

    public void Start() {
        // adjust this bin's color
        GetComponentInChildren<Light>().color = SplatEffect.type2color[Type];
        driver = GameObject.Find("FruitGameDriver").GetComponent<FruitGameDriver>();
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
            driver.EnteredBinGood();
        } else {
            effect = (Instantiate(effectBad) as GameObject).GetComponent<SplatEffect>();
            driver.EnteredBinBad();
        }
        effect.transform.position = transform.position;
        effect.Type = Type;
        Destroy(fruit.gameObject);
    }
}
