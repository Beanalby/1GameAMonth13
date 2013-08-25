using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDriver8 : MonoBehaviour {
    private static string FIRST_LEVEL = "levelTest1";

    private static GameDriver8 _instance;
    public static GameDriver8 instance {
        get {
            if(_instance == null) {
                GameObject obj;
                obj = new GameObject("GameDriver");
                _instance = obj.AddComponent<GameDriver8>();
                GameObject.DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    private List<string> finishedLevels;

    private string lastLevel;
    public string LastLevel {
        get {
            if(lastLevel != null) {
                return lastLevel;
            } else {
                return FIRST_LEVEL;
            }
        }
        set { lastLevel = value; }
    }

    public GameDriver8() {
        finishedLevels = new List<string>();
        //finishedLevels.Add("levelTest1");
        //finishedLevels.Add("levelTest2");
    }

    public void LevelFinished() {
        finishedLevels.Add(Application.loadedLevelName);
        Application.LoadLevel("selectLevel");
    }

    public void LoadLevel(Level level) {
        lastLevel = level.scene;
        Application.LoadLevel(level.scene);
    }

    public bool IsLinkActive(Level level1, Level level2) {
        return IsLevelFinished(level1) || IsLevelFinished(level2);
    }
    public bool IsLevelFinished(Level level) {
        return finishedLevels.Contains(level.scene);
    }
}
