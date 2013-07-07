using UnityEngine;
using System.Collections;

public class LetterOmegaMean : LetterOmega {

    public GameObject rockPrefab;
    private float rockCooldown = 3f;

    private float nextRock = -1;

    public override void Start() {
        base.Start();
        // mean omega letters are always vunlerable
        invincible = false;
    }
    public override void Update() {
        base.Update();
        if(nextRock != -1 && Time.time > nextRock) {
            ThrowRock();
        }
    }

    public void ShowOmega() {
        SetNextRock(.1f, .5f);
        StartCoroutine(HideOmega());
    }

    private IEnumerator HideOmega() {
        yield return new WaitForSeconds(WaveDriver.WAVE_DURATION-.5f);
        nextRock = -1;
    }

    private void SetNextRock(float min, float scale) {
        nextRock = Time.time
            + (min * rockCooldown)
            + (scale * Random.Range(0, rockCooldown));
    }

    protected override void HandleDeath(Damage damage) {
        base.HandleDeath(damage);
        rockCooldown = rockCooldown / 2f;
    }

    private void ThrowRock() {
        if(Ship.ship != null) {
            GameObject obj = Instantiate(rockPrefab) as GameObject;
            obj.transform.position = transform.position;
            Rock rock = obj.GetComponent<Rock>();
            rock.target = Ship.ship.transform;
            if(!isAlive) {
                rock.speed *= 2;
            }
            SetNextRock(.5f, 1);
        }
    }
}
