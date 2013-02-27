using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HomePlayer : HomeBase {

    [System.Serializable]
    public class Spawnable {
        public string name;
        public GameObject template;
        public int cost;
        public float spawnTime;
    }

    public GUISkin skin;
    public Texture coinImage;
    public Texture spawnBarImage;

    private int widgetWidth = 350;
    private int widgetSpace = 5;
    private int buttonHeight = 40;
    private int labelHeight = 20;
    private int spawnBarHeight = 12;

    private int coinCurrent = 100;
    private float coinRegenCooldown = .2f;
    private int coinRegenAmount = 1;
    private float coinRegenLast = -100f;

    private int pickupMask;

    public Spawnable[] spawnables;
    private Spawnable spawning=null;
    private float spawnStart=-1f;
    private AudioSource warpCharge, warpSuccess;
    new public void Start() {
        base.Start();
        pickupMask = 1 << LayerMask.NameToLayer("Pickup");
        AudioSource[] sources = GetComponents<AudioSource>();
        if(sources.Length != 2) {
            Debug.LogError("Expected 2 sources for warp start & end!");
        }
        warpCharge = sources[0];
        warpSuccess = sources[1];
    }
    public void Update() {
        HandleCoinRegen();
        HandlePickups();
        HandleSpawning();
    }
    public void OnGUI() {
        GUI.skin = skin;
        float currentPos = 5;
        GUI.DrawTexture(new Rect(6, currentPos, coinImage.width, coinImage.height), coinImage);
        GUI.Label(new Rect(24, currentPos, widgetWidth, labelHeight), coinCurrent + " Bitcoins in bank", skin.customStyles[0]);
        currentPos += labelHeight + widgetSpace;
        GUI.enabled = false;
        string buttonText;
        bool someButtonHeld = false;
        foreach (Spawnable spawn in spawnables) {
            GUI.skin = skin;
            if (spawning == spawn) {
                GUI.enabled = true;
                if (spawnStart != -1) {
                    buttonText = "Spawning " + spawn.name + "...";
                } else {
                    buttonText = spawn.name + " Spawned!";
                }
            } else {
                GUI.enabled = coinCurrent >= spawn.cost;
                buttonText = spawn.cost + ": Spawn " + spawn.name;
            }
            if (GUI.RepeatButton(new Rect(0, currentPos, widgetWidth, buttonHeight), buttonText)) {
                someButtonHeld = true;
                if (spawning == null && spawnStart == -1) {
                    this.spawning = spawn;
                    spawnStart = Time.time;
                    warpCharge.Play();
                }
            }
            if (spawning == spawn) {
                DrawSpawnBar(widgetWidth - 12, currentPos);
            } else {
                GUI.DrawTexture(new Rect(6, currentPos + 6, coinImage.width, coinImage.height), coinImage);
            }
            currentPos += buttonHeight + widgetSpace;
        }
        if (!someButtonHeld && Event.current.type == EventType.Repaint) {
            // no buttons are being held, reset spawnStart so we
            // can spawn again.
            spawning = null;
            spawnStart = -1f;
            if(warpCharge.isPlaying) {
                warpCharge.Stop();
            }
        }
        GUI.enabled = true;
    }

    private void DrawSpawnBar(float width, float offset) {
        float percent;
        if (spawnStart != -1) {
            percent = (Time.time - spawnStart) / spawning.spawnTime;
        } else {
            percent = 1;
        }
        width *= percent;
        Rect pos = new Rect(6, offset + 22, width, spawnBarHeight);
        Rect texPos = new Rect(0, 0, ((float)width) / spawnBarImage.width, ((float)spawnBarHeight) / spawnBarImage.height);
        GUI.DrawTextureWithTexCoords(pos, spawnBarImage, texPos);
    }
    private void HandleCoinRegen() {
        if (coinRegenLast + coinRegenCooldown > Time.time)
            return;
        coinCurrent += coinRegenAmount;
        coinRegenLast = Time.time;
    }
    private void HandlePickups() {
        // see if the cursor intersects any pickups
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, pickupMask);
        foreach (RaycastHit hit in hits) {
            Fragment frag = hit.transform.GetComponent<Fragment>();
            if (!frag || !frag.IsPickupable)
                continue;
            coinCurrent += frag.coinValue;
            frag.PickedUp();
        }
    }
    private void HandleSpawning() {
        if (spawning != null && spawnStart != -1f && spawnStart + spawning.spawnTime < Time.time) {
            Spawn(spawning.template);
            coinCurrent -= spawning.cost;
            warpCharge.Stop();
            warpSuccess.PlayOneShot(warpSuccess.clip);
            // clear spawnStart, but not spawning so we won't immediately spawn
            // another via holding down the button
            spawnStart = -1f;
        }
    }

}
