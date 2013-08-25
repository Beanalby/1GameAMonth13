using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LevelLink : MonoBehaviour {
    public Texture2D LinkOn, LinkOff;
    public Level levelFrom, levelTarget;

    private LineRenderer line;

    public void Start() {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.SetPosition(0, levelFrom.transform.position);
        line.SetPosition(1, levelTarget.transform.position);
        GameDriver8 driver = GameDriver8.instance;
        if(driver.IsLinkActive(levelFrom, levelTarget)) {
            line.material.mainTexture = LinkOn;
        } else {
            line.material.mainTexture = LinkOff;
        }
    }
}
