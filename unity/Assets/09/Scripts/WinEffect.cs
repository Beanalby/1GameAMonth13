using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class WinEffect : MonoBehaviour {
    private static float SCALE = 1.4f;

    public SpotValue winner;
    public Texture texX, texO;
    public GameObject prefabX, prefabO;
    [HideInInspector]
    public Vector3 pos1, pos2;
    [HideInInspector]
    public TinyBoard board;

    public void Start() {
        LineRenderer line = GetComponent<LineRenderer>();
        GameObject obj;
        switch(winner) {
            case SpotValue.X: 
                line.material.mainTexture = texX;
                obj = Instantiate(prefabX) as GameObject;
                obj.transform.position = board.transform.position;
                break;
            case SpotValue.O:
                line.material.mainTexture = texO;
                obj = Instantiate(prefabO) as GameObject;
                obj.transform.position = board.transform.position;
                break;
            case SpotValue.Tie:
                // create both O & X objects, but no line
                obj = Instantiate(prefabX) as GameObject;
                obj.transform.position = board.transform.position;
                obj = Instantiate(prefabO) as GameObject;
                obj.transform.position = board.transform.position;
                line.enabled = false;
                break;
        }

        if(line.enabled) {
            Vector3 tmp;

            tmp = pos1;
            if(pos1.x != pos2.x) {
                tmp.x *= SCALE;
            }
            if(pos1.y != pos2.y) {
                tmp.y *= SCALE;
            }
            tmp.z = -.2f;
            line.SetPosition(0, tmp);

            tmp = pos2;
            if(pos1.x != pos2.x) {
                tmp.x *= SCALE;
            }
            if(pos1.y != pos2.y) {
                tmp.y = SCALE;
            }
            tmp.z = -.2f;
            line.SetPosition(1, tmp);
        }
    }
}
