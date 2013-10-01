using UnityEngine;
using System.Collections;

public class TinySpot : MonoBehaviour, Spot {

    private SpotValue value = SpotValue.None;
    public GameObject prefabX;
    public GameObject prefabO;
    public AudioClip throwSound, hitOSound, hitXSound, winOSound, winXSound;

    private GameObject mark = null;
    private TinyBoard board;

    private float markStart = -1, markDuration = .25f;
    private Vector3 markStartPos;
    private Vector3 markStartSpin = new Vector3(180, 0, 0);
    private Interpolate.Function
        moveEase = Interpolate.Ease(Interpolate.EaseType.Linear),
        spinEase = Interpolate.Ease(Interpolate.EaseType.Linear);
    public TinyBoard Board {
        get { return board; }
    }

    public void Start() {
        board = transform.parent.GetComponent<TinyBoard>();
    }

    public void Update() {
        MoveMark();
    }

    private void MoveMark() {
        if(markStart == -1) {
            return;
        }
        float percent = (Time.time - markStart) / markDuration;
        if(percent >= 1) {
            markStart = -1;
            mark.transform.localPosition = Vector3.zero;
            mark.transform.rotation = Quaternion.Euler(Vector3.zero);
            AudioClip clip;
            switch(board.Winner) {
                case SpotValue.O:
                    clip = winOSound; break;
                case SpotValue.X:
                    clip = winXSound; break;
                default:
                    if(value == SpotValue.O) {
                        clip = hitOSound;
                    } else {
                        clip = hitXSound;
                    }
                    break;
            }
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        } else {
            mark.transform.localPosition = Interpolate.Ease(moveEase,
                markStartPos, -markStartPos, percent, 1);
            mark.transform.rotation = Quaternion.Euler(
                Interpolate.Ease(spinEase, markStartSpin, -markStartSpin,
                                percent, 1));
        }
    }

    public SpotValue GetValue() {
        return value;
    }

    public void MakeMark(SpotValue newValue) {
        value = newValue;
        if(value == SpotValue.X) {
            mark = Instantiate(prefabX) as GameObject;
        } else {
            mark = Instantiate(prefabO) as GameObject;
        }
        AudioSource.PlayClipAtPoint(throwSound, Camera.main.transform.position);
        mark.transform.parent = transform;
        markStartPos = Camera.main.transform.position;
        markStartPos += TinyBoard.IndexToCoordinate(board.GetIndex(this));
        mark.transform.localPosition = markStartPos;
        mark.transform.rotation = Quaternion.Euler(markStartSpin);
        markStart = Time.time;
    }
}
