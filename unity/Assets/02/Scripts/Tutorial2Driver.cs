using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void StateSetup();
public delegate bool ToContinue();

class Tutorial2State {
    public string name;
    public StateSetup setup;
    public ShowControls control;
    public string next;
    public ToContinue toContinue;
    public StateSetup teardown;

    public Tutorial2State(string name, ShowControls control, string next, StateSetup setup, ToContinue toContinue, StateSetup teardown) {
        this.name = name;
        this.setup = setup;
        this.control = control;
        this.next = next;
        this.toContinue = toContinue;
        this.teardown = teardown;
    }
}

public class Tutorial2Driver : MonoBehaviour {

    public Transform battlefield;
    private Tutorial2State state;

    Dictionary<string, Tutorial2State> states;
    Tutorial2State current;

    public void Start() {
        GameObject exampleNoob = battlefield.FindChild("ExampleNoob").gameObject;
        GameObject exampleJack = battlefield.FindChild("ExampleJack").gameObject;
        GameObject exampleEjector = battlefield.FindChild("ExampleEjector").gameObject;

        HomePlayer homePlayer = GameObject.Find("HomePlayer").GetComponent<HomePlayer>();
        WeaponCannon cannon = homePlayer.transform.Find("HomeCannon").GetComponent<WeaponCannon>();
        GameObject target1 = battlefield.FindChild("Target1").gameObject;
        GameObject target2 = battlefield.FindChild("Target2").gameObject;

        states = new Dictionary<string, Tutorial2State>();

        string startMsg = "Internet Trolls are attacking!\nWell, that or internet trolls need to be attacked.  Is there a difference?\nPress space to continue.";
        string noobMsg = "This is the weakest troll you'll fight, the noob.  Not exactly the sharpest light bulb in the drawer, if you catch my drift.";
        string cannonMsg1 = "Shoot the noobs with your cannon!  Aim with your mouse to change the direction.";
        string cannonMsg2 = "Hold the mouse to charge the cannon, and release to fire!";
        string jackMsg = "You aren't alone in fighting the trolls.  This is Jack, he'll fight for you by launching network packets at the noobs.";
        string ejectorMsg = "Heavier artillery comes with the ejector.  It launches media at trolls, currently just SD cards.";
        string spawnMsg1 = "Left Click & hold on a menu item to spawn the selected component.  Spawning costs bitcoins, which regenerate over time and are dropped when things die.";
        string spawnMsg2 = "Spawn some stuff, and press Space to continue.";
        string completeMsg = "You've learned all you need to.  Go forth and assault!";

        AddState("start", new ControlItem(startMsg, KeyCode.Space), "introNoob",
            () => { },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { }
        );

        AddState("introNoob", new ControlItem(noobMsg, KeyCode.Space), "introCannon",
            () => { exampleNoob.SetActive(true); },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { exampleNoob.SetActive(false); }
        );

        AddState("introCannon",
            new ControlItem(cannonMsg1, MouseDirection.Both),
            new ControlItem(cannonMsg2, MouseButton.LeftClick),
            "introJack",
            () => { cannon.isActive = true; target1.SetActive(true); target2.SetActive(true); },
            () => { return target1 == null && target2 == null; },
            () => { }
        );

        AddState("introJack",
            new ControlItem(jackMsg, KeyCode.Space),
            "introEjector",
            () => { exampleJack.SetActive(true); exampleJack.GetComponent<Animator>().SetFloat("Speed", 1); },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { exampleJack.SetActive(false);  }
        );

        AddState("introEjector",
            new ControlItem(ejectorMsg, KeyCode.Space),
            "introSpawn",
            () => { exampleEjector.SetActive(true); },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { exampleEjector.SetActive(false); }
        );

        AddState("introSpawn",
            new ControlItem(spawnMsg1, MouseButton.LeftClick),
            new ControlItem(spawnMsg2, KeyCode.Space),
            "complete",
            () => { homePlayer.isActive = true; },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { homePlayer.isActive = false; }
        );
        states["introSpawn"].control.position = ShowControlPosition.Bottom;

        AddState("complete",
            new ControlItem(completeMsg, KeyCode.Space),
            "backToMenu",
            () => { },
            () => { return Input.GetKeyDown(KeyCode.Space); },
            () => { }
        );
        
        AddState("backToMenu",
            null,
            "",
            () => { Application.LoadLevel("02title"); },
            () => { return false; },
            () => { }
        );
        current = states["start"];
        current.setup();
        current.control.Show();
    }

    public void Update() {
        if(current.toContinue()) {
            if(current.control) {
                current.control.Hide();
            }
            current.teardown();
            current = states[current.next];
            current.setup();
            if(current.control) {
                current.control.Show();
            }
        }
    }

    private void AddState(string name, ControlItem control, string next, StateSetup setup, ToContinue continueFunc, StateSetup teardown) {
        ShowControls sc = null;
        if(control != null) {
            sc = ShowControls.CreateDocked(control);
            sc.showDuration = -1;
        }
        Tutorial2State state = new Tutorial2State(name, sc, next, setup, continueFunc, teardown);
        states[state.name] = state;
    }
    private void AddState(string name, ControlItem control1, ControlItem control2, string next, StateSetup setup, ToContinue continueFunc, StateSetup teardown) {
        ShowControls sc = null;
        if(control1 != null && control2 != null) {
            sc = ShowControls.CreateDocked(new ControlItem[] { control1, control2 });
            sc.showDuration = -1;
        }
        Tutorial2State state = new Tutorial2State(name, sc, next, setup, continueFunc, teardown);
        states[state.name] = state;
    }
}
