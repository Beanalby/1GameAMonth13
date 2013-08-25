using UnityEngine;
using System.Collections;


public class Level : MonoBehaviour {
    public GUISkin skin;
    public LevelLink linkPrefab;

    public bool needSpecialExit = false;
    public string label;
    public string scene;
    public Level levelTop, levelRight;
    [HideInInspector]
    public Level levelBottom, levelLeft;
    [HideInInspector]
    public bool ShowLabel = true;

    private float spinSpeed = .5f;
    private bool isSelected = false;

    private GameObject mesh;

    public void Awake() {
        mesh = transform.Find("levelMesh").gameObject;
    }

    public void Start() {
        // make links to the top & right
        if(levelRight != null) {
            CreateLink(levelRight);
        }
        if(levelTop != null) {
            CreateLink(levelTop);
        }
        if(GameDriver8.instance.IsLevelFinished(this)) {
            Color color = mesh.renderer.material.color;
            color /= 5;
            mesh.renderer.material.color = color;
        }
    }

    public void CreateLink(Level otherLevel) {
        LevelLink link = ((GameObject)Instantiate(linkPrefab.gameObject)).GetComponent<LevelLink>();
        link.levelFrom = this;
        link.levelTarget = otherLevel;
    }

    public void OnGUI() {
        GUI.skin = skin;
        if(isSelected && ShowLabel) {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            pos.y = Screen.height - pos.y;
            GUI.Box(new Rect(pos.x - 200, pos.y + 50, 400, 40),
                (label!="" ? label : (":" + scene)) );
        }
    }

    public void Selected() {
        isSelected = true;
        StartSpin();
    }
    public void Unselected() {
        isSelected = false;
        StopSpin();
    }

    private void StartSpin() {
        mesh.rigidbody.angularVelocity = new Vector3(0, spinSpeed, 0);
    }
    private void StopSpin() {
        mesh.rigidbody.angularVelocity = Vector3.zero;
        mesh.transform.rotation = Quaternion.identity;
    }

}
