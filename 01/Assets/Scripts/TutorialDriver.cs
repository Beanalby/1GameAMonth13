using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TutorialState { Attack, Toggle1, Toggle2 };

public class TutorialDriver : MonoBehaviour {

    public GameObject ArrowTemplate;

    private GameObject arrow;
    private ShowControls scCancel, scAttack, scToggle1, scToggle2;
    private BoardController board;
    private PlayerController player;
    private TutorialState state = TutorialState.Attack;
    private TinyEnemyController[] enemies;

    void Start() {
        board = GameObject.Find("Board").GetComponent<BoardController>();
        board.driver = this.gameObject;
        InitShowControls();
        scAttack.Show();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.canCharge = false;
        player.driver = gameObject;
        enemies = GameObject.FindSceneObjectsOfType(typeof(TinyEnemyController)) as TinyEnemyController[];
    }
    void Update() {
        if(state == TutorialState.Attack && !AnyEnemiesExist()) {
            SwitchToToggle1();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevel("generalLevel");
        }
    }

    bool AnyEnemiesExist() {
        foreach(TinyEnemyController enemy in enemies) {
            if(enemy != null)
                return true;
        }
        return false;
    }
    void BoardClear() {
        switch(state) {
            case TutorialState.Toggle1:
                Debug.Log("Going to state2!");
                SwitchToToggle2();
                break;
            case TutorialState.Toggle2:
                Application.LoadLevel("generalLevel");
                break;
        }

        Debug.Log("Whee, board's clear");
    }
    void InitShowControls() {
        scCancel = ShowControls.CreateDocked(new ControlItem("Skip Tutorial", KeyCode.Escape));
        scCancel.position = ShowControlPosition.Bottom;
        scCancel.showDuration = -1;
        scCancel.size = ShowControlSize.Small;
        scCancel.Show();

        scAttack = ShowControls.CreateDocked(new ControlItem[] {
            new ControlItem("Move Penthos around", CustomDisplay.arrows),
            new ControlItem("Attack the draglins!", KeyCode.LeftControl)
        });
        scAttack.showDuration = -1;

        scToggle1 = ShowControls.CreateDocked(new ControlItem("Hold to open a rift, cleansing the corruption from this location and all nearby locations.", KeyCode.Space));
        scToggle1.showDuration = -1;

        scToggle2 = ShowControls.CreateDocked(new ControlItem("Beware, any clear location will also become corrupted.  Fully cleanse each level to continue.", KeyCode.Space));
        scToggle2.showDuration = -1;
    }
    void SwitchToToggle1() {
        state = TutorialState.Toggle1;
        board.SetBoard(BoardState.FromToggles(new List<int>() { 7 }));
        scAttack.Hide();
        scToggle1.Show();
        player.canCharge = true;
        player.restrictedCharge = board.getSquare(7);
        arrow = Instantiate(ArrowTemplate) as GameObject;
        arrow.transform.position = player.restrictedCharge.transform.position;
    }
    void SwitchToToggle2() {
        state = TutorialState.Toggle2;
        board.SetBoard(BoardState.FromToggles(new List<int>() { 17, 22 }));
        player.restrictedCharge = board.getSquare(17);
        arrow.transform.position = player.restrictedCharge.transform.position;
        scToggle1.Hide();
        scToggle2.Show();
    }
    void ToggledSquare(SquareController target) {
        switch(target.boardIndex) {
            case 17:
                Debug.Log("Now do 22!");
                player.restrictedCharge = board.getSquare(22);
                arrow.transform.position = player.restrictedCharge.transform.position;
                break;
            //case 22:
            //    Debug.Log("Done!");
            //    Application.LoadLevel("generalLevel");
            //    break;
        }
    }
}
