using UnityEngine;
using System.Collections;

/* like a normal letter, but it goes after the ship if alive at wave end */
public class LetterKiller : Letter {
    private int kamikazeDamage = 10;
    private float kamikazeSpeed = 6f;
    private bool isPursuing = false;

    public override void Start() {
        base.Start();
        scoreValue = 5;
    }

    public void Update() {
        if(isAlive && isPursuing) {
            PursueShip();
        }
    }

    private void PursueShip() {
        if(Ship.ship == null) {
            isPursuing = false;
        } else {
            Vector3 shipDir = Ship.ship.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity = shipDir.normalized * kamikazeSpeed;
        }
    }

    public void WaveEnd() {
        transform.parent = null;
        isPursuing = true;
    }

    public void OnTriggerEnter(Collider other) {
        if(Ship.ship != null && other.gameObject == Ship.ship.gameObject) {
            
            Ship.ship.SendMessage("TakeDamage",
                new Damage(transform, kamikazeDamage));
            Destroy(gameObject);
        }
    }
}
