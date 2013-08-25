using UnityEngine;
using System.Collections;

public class TitleDriver8 : MonoBehaviour {
    public Texture tex;

    //private Vector3 posMin, posMax, sizeMin, sizeMax;g
    private Rect rectCurrent;
    //private float pulseRate =1;
    //private Interpolate.Function easeUp = Interpolate.Ease(Interpolate.EaseType.Linear),
    //    easeDown = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic);
    public void Start() {
        int width = tex.width, height = tex.height;
        //posMax = new Vector3(Screen.width / 2 - width / 2,
        //    Screen.height / 2 - height / 2, 0);
        //sizeMax = new Vector3(width, height, 0);
        //width = (int)(width * .8f);
        //height = (int)(height * .8f);
        //posMin = new Vector3(Screen.width / 2 - width / 2,
        //    Screen.height / 2 - height / 2, 0);
        //sizeMin = new Vector3(width, height, 0);
        rectCurrent = new Rect(Screen.width / 2 - width / 2,
            Screen.height / 2 - height / 2, width, height);
    }

    public void Update() {
        //UpdateRectCurrent();
        if(Input.GetButtonDown("Fire1")) {
            GameDriver8.instance.LoadSelectLevel();
        }
    }
    public void OnGUI() {
        GUI.DrawTexture(rectCurrent, tex);
        int width=200, height=50;
        Rect startRect = new Rect(Screen.width / 2 - width / 2,
            (Screen.height * .75f) - height / 2, width, height);
        if(GUI.Button(startRect, "Press Space to Begin")) {
            GameDriver8.instance.LoadSelectLevel();
        }
    }

    //private void UpdateRectCurrent() {
    //    float percent = (Time.time % pulseRate) / pulseRate;

    //    Vector3 posCur, sizeCur;
    //    if(percent <= .5f) {
    //        posCur = Interpolate.Ease(easeUp, posMin, posMax, percent, .5f);
    //        sizeCur = Interpolate.Ease(easeUp, sizeMin, sizeMax, percent, .5f);
    //    } else {
    //        percent -= .5f;
    //        posCur = Interpolate.Ease(easeUp, posMin, posMax, percent, .5f);
    //        sizeCur = Interpolate.Ease(easeUp, sizeMin, sizeMax, percent, .5f);
    //        //posCur = Interpolate.Ease(easeDown, posMax, posMin, percent, .5f);
    //        //sizeCur = Interpolate.Ease(easeDown, sizeMax, sizeMin, percent, .5f);
    //    }
    //    rectCurrent = new Rect(posCur.x, posCur.y, sizeCur.x, sizeCur.y);
    //    //rectCurrent.y = posCur.y;
    //    //rectCurrent.width = sizeCur.x;
    //    //rectCurrent.height = sizeCur.y;
    //}
}
