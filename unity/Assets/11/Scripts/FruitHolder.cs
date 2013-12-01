using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FruitHolder : MonoBehaviour {

    public FruitCatcher catcher;
    public AudioClip catchSound;

    /// <summary>amount of space between fruits when being held</summary>
    private float stackSize = .5f;

    private Queue<Fruit> fruits;

    private float slideStart = -1;

    public void Start() {
        fruits = new Queue<Fruit>();
    }
    public void Update() {
        HandleMovement();
    }

    public void AddFruit(Fruit fruit) {
        Transform t = fruit.transform;
        t.parent = transform;
        t.localRotation = Quaternion.identity;
        t.localPosition = new Vector3(0, fruits.Count * stackSize, 0);

        fruit.Caught();

        fruits.Enqueue(fruit);
        catcher.transform.localPosition = new Vector3(0, fruits.Count * stackSize, 0);
        AudioSource.PlayClipAtPoint(catchSound, Camera.main.transform.position);
    }

    public void DropFruit(Vector3 velocity) {
        // skip if there's nothing to drop, or if it's still sliding down
        if(fruits.Count == 0 || slideStart!=-1) {
            return;
        }
        Fruit fruit = fruits.Dequeue();
        // drop it like it's hot!
        fruit.Released(velocity);

        // move the other fruits down
        for(int i = 0; i < fruits.Count; i++) {
            fruits.ElementAt(i).transform.localPosition =
                new Vector3(0, (i) * stackSize, 0);
        }
        catcher.transform.localPosition = new Vector3(0, fruits.Count * stackSize, 0);
        // move the catcher itself up, and slide it back down
        // to simulate the still-held fruit and catcher "dropping down 1"
        //Debug.Break();
        transform.localPosition = new Vector3(0, stackSize, 0);
        slideStart = Time.time;
    }

    private void HandleMovement() {
        if(slideStart != -1) {
            Vector3 pos = transform.localPosition;
            pos += (Physics.gravity * (Time.time - slideStart)) * Time.deltaTime;
            if(pos.y <= 0) {
                pos = Vector3.zero;
                slideStart = -1;
            }
            transform.localPosition = pos;
        }
    }
}
