using UnityEngine;
using System.Collections;

public class FruitGameDriver : MonoBehaviour {

    public GUISkin skin;

    private float gameStart;
    private float gameDuration = 60;
    private float score=0;

    private FruitPlayer player;

    public void Start() {
        gameStart = Time.time;
        player = GameObject.Find("FruitPlayer").GetComponent<FruitPlayer>();
    }
    public void Update() {
        HandleGameEnd();
    }
    public void OnGUI() {
        GUI.skin = skin;
        float timeLeft = GetTimeLeft();
        if(timeLeft > 0) {
            GUI.Label(new Rect(10, 55, 300, 100), "Time: " + timeLeft.ToString(".0")
                + "\nScore: " + score);
        } else {
            GUI.Label(new Rect(0, 150, Screen.width, 200),
                "Game Over\nScore: " + score,
                skin.customStyles[0]);
            if(GUI.Button(new Rect(Screen.width / 2 - 100, 250, 200, 65),
                "Play Again")) {
                    Application.LoadLevel("intro");
            }

        }
    }

    public void EnteredBinGood() {
        if(GetTimeLeft() > 0)
            score += 5;
    }
    public void EnteredBinBad() {
        if(GetTimeLeft() > 0)
            score -= 2;
    }
    public void FruitSplat() {
        if(GetTimeLeft() > 0)
            score -= 2;
    }

    public float GetTimeLeft() {
        return Mathf.Max(0, gameDuration - (Time.time - gameStart));
    }
    private void HandleGameEnd() {
        if(GetTimeLeft() <= 0) {
            player.CanControl = false;
        }
    }
    public bool IsGameRunning() {
        return GetTimeLeft() > 0;
    }
}
