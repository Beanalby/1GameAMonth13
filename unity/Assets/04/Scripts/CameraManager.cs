using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    public GameObject Current {
        get { return _current; }
        set {
            _current = value;
            foreach(GameObject cam in cameras) {
                if(cam != value) {
                    cam.gameObject.SetActive(false);
                }
            }
            value.gameObject.SetActive(true);
        }
    }

    private GameObject _current;
    private GameObject[] cameras;

    // Use this for initialization
    void Start () {
        if(CameraManager.instance != null) {
            Debug.LogError("Multipel CameraManager instances!");
            Destroy(gameObject);
            return;
        }
        CameraManager.instance = this;
        cameras = System.Array.ConvertAll(GetComponentsInChildren<Camera>(),
            t => t.gameObject);
        Current = Camera.main.gameObject;
    }
}
