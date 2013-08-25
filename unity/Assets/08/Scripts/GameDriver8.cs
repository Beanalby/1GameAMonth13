using UnityEngine;
using System.Collections;

public class GameDriver8 : MonoBehaviour {
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

    public string lastLevel;

    public void Start() {
    }

    public void LevelFinished() {
        Debug.Log("+++ Back to levelSelect");
    }
    public bool IsLinkActive(Level level1, Level level2) {
        return IsLevelComplete(level1) && IsLevelComplete(level2);
    }
    public bool IsLevelComplete(Level level) {
        return level.level != "test4";
    }
}
