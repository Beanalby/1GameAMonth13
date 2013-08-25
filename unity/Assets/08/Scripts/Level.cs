using UnityEngine;
using System.Collections;


public class Level : MonoBehaviour {
    public LevelLink linkPrefab;

    public string level;
    public Level levelTop, levelRight;
    [HideInInspector]
    public Level levelBottom, levelLeft;

    private float spinSpeed = .5f;

    private Transform mesh;

    public void Awake() {
        name = "level " + level;
        mesh = transform.Find("levelMesh");
    }

    public void Start() {
        // make links to the top & right
        if(levelTop != null) {
            levelTop.levelBottom = this;
            CreateLink(levelTop);
        }
        if(levelRight != null) {
            levelRight.levelLeft = this;
            CreateLink(levelRight);
        }
    }

    public void CreateLink(Level otherLevel) {
        LevelLink link = ((GameObject)Instantiate(linkPrefab.gameObject)).GetComponent<LevelLink>();
        link.levelFrom = this;
        link.levelTarget = otherLevel;
    }

    public void Selected() {
        StartSpin();
    }
    public void Unselected() {
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
