using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour {

    public GUISkin skin;
    public int coinValue=5;
    public Texture coinImage;

    private float bounceScale = .8f;
    private float maxLifetime = 5f;
    private float minLifetime = 1f;
    private float hRange = 2f;
    private float vRange = 20f;
    private float vMin = 2f;

    private float lifeStart;
    private float pickedUpStart = -1f;
    private Vector3 pickedUpPos;
    private float pickedUpDuration = 1f;
    private float pickedUpDist = 10f;

    public bool IsPickupable {
        get { return (Time.time > lifeStart + minLifetime); }
    }

	// Use this for initialization
	void Start () {
        lifeStart = Time.time;
        maxLifetime += Random.Range(-.2f * maxLifetime, .2f * maxLifetime);
        GetComponent<Rigidbody>().velocity = new Vector3(
            Random.Range(-hRange, hRange),
            Random.Range(vMin, vRange),
            Random.Range(-hRange, hRange));
	}
    void Update() {
        if (pickedUpStart != -1) {
            if (pickedUpStart + pickedUpDuration < Time.time) {
                Destroy(gameObject);
                return;
            }
        } else {
            if (Time.time > lifeStart + maxLifetime) {
                Destroy(gameObject);
                return;
            }
        }
    }
    void OnCollisionEnter(Collision info) {
        Vector3 newV = -info.relativeVelocity;
        newV.y *= -bounceScale;
        GetComponent<Rigidbody>().velocity = newV;
    }
    public void OnGUI() {
        if(pickedUpStart != -1) {
            Vector3 pos = pickedUpPos;
            pos.y -= ((Time.time - pickedUpStart) / pickedUpDuration) * pickedUpDist;
            GUI.Label(new Rect(pos.x, pos.y, 20, 20), coinValue.ToString(), skin.customStyles[1]);
            GUI.DrawTexture(new Rect(pos.x - 16, pos.y, coinImage.width, coinImage.height), coinImage);
        }
    }
    public void PickedUp() {
        GetComponent<Collider>().enabled = false;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        pickedUpStart = Time.time;
        pickedUpPos = Camera.main.WorldToScreenPoint(transform.position);
        // WorldtoScreenPoint is bottom=0, but GUI.Label is top=0
        pickedUpPos.y = Screen.height - pickedUpPos.y;

    }
}
