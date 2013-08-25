using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PowerTileCheck : Switch {

    private List<PowerTile> tiles;

    public void Awake() {
        color = Color.yellow;
    }

    public void Start() {
        tiles = new List<PowerTile>();
        Object[] tmp = Resources.FindObjectsOfTypeAll(typeof(PowerTile));
        foreach(Object obj in tmp) {
            PowerTile tile = (PowerTile)obj;
            if(tile.transform.position != Vector3.zero) { // prefab
                tiles.Add(tile);
            }
        }
    }
    public void UpdateTiles() {
        bool anyInactive = false;
        foreach(PowerTile tile in tiles) {
            if(!tile.IsActive) {
                anyInactive = true;
                break;
            }
        }
        SendMessage("SetSwitch", !anyInactive);
    }
}
