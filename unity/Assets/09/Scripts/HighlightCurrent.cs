using UnityEngine;
using System.Collections;

public class HighlightCurrent : MonoBehaviour {

    private const float TINY_OFFSET = .5f;

    public Texture2D texX, texO;

    Vector3 startPos;
    Vector3 deltaPos;

    Vector3 startScale = new Vector3(1,1,1), deltaScale = new Vector3(2.5f, 2.5f, 1);
    private bool stay = false;

    float moveStart = -1, moveDelay=.1f, moveDuration=.75f;
    Interpolate.Function ease = Interpolate.Ease(Interpolate.EaseType.EaseInOutCubic);
    public void Start() {
        renderer.enabled = false;
    }

    public void Highlight(SpotValue currentTurn, TinySpot spot, TinyBoard targetBoard, bool stay) {
        startPos = spot.transform.position;
        deltaPos = targetBoard.transform.position - spot.transform.position;
        startPos.z = transform.position.z;
        if(currentTurn == SpotValue.X) {
            renderer.material.mainTexture = texX;
        } else {
            renderer.material.mainTexture = texO;
        }
        transform.position = startPos;
        transform.localScale = startScale;
        this.stay = stay;
        renderer.enabled = true;
        StartCoroutine(MoveAfterDelay());
    }

    private IEnumerator MoveAfterDelay() {
        yield return new WaitForSeconds(moveDelay);
        moveStart = Time.time;
    }
    public void Update() {
        HandleMove();
    }

    private void HandleMove() {
        if(moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if(percent >= 1) {
            if(stay) {
                transform.position = startPos + deltaPos;
                transform.localScale = startScale + deltaScale;
                if(renderer.material.mainTexture == texX) {
                    renderer.material.mainTexture = texO;
                } else {
                    renderer.material.mainTexture = texX;
                }
            } else {
                renderer.enabled = false;
            }
            moveStart = -1f;
        } else {
            transform.position = Interpolate.Ease(ease,
                startPos, deltaPos, percent, 1);
            transform.localScale = Interpolate.Ease(ease,
                startScale, deltaScale, percent, 1);
        }
    }
}
