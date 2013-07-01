using UnityEngine;
using System.Collections;

public class LetterHeal : Letter {
    public HealthGlobe globePrefab;

    protected override void HandleDeath(Damage damage) {
        // also spawn our healthGlobe
        GameObject obj = Instantiate(globePrefab.gameObject) as GameObject;
        obj.transform.position = transform.position;
        base.HandleDeath(damage);
    }
}
