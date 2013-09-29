using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class WinEffect : MonoBehaviour {
    private static float SCALE = 1.4f;

    public SpotValue winner;
    public Texture texX, texO;
    public Vector3 pos1, pos2;
    public void Start() {
        LineRenderer line = GetComponent<LineRenderer>();
        if(winner == SpotValue.X) {
            line.material.mainTexture = texX;
        } else {
            line.material.mainTexture = texO;
        }
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
