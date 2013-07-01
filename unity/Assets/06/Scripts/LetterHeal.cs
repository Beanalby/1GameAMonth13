using UnityEngine;
using System.Collections;

public class LetterHeal : Letter {
    public HealthGlobe globePrefab;

    public override void Start() {
        base.Start();
        scoreValue = 2;
    }

    protected override void HandleDeath(Damage damage) {
        // also spawn our healthGlobe
        GameObject obj = Instantiate(globePrefab.gameObject) as GameObject;
        obj.transform.position = transform.position;
        base.HandleDeath(damage);
    }
}
