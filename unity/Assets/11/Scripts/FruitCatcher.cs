using UnityEngine;
using System.Collections;

public class FruitCatcher : MonoBehaviour {

    public FruitHolder holder;

    public void OnTriggerEnter(Collider other) {
        Fruit fruit = other.GetComponent<Fruit>();
        if(fruit.IsCaught) {
            return;
        }
        holder.AddFruit(fruit);
    }

}
