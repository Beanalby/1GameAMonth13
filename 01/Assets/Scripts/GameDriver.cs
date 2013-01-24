using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDriver : MonoBehaviour {

    private static GameDriver instance = null;

    Levels levels;
    LevelInfo _currentLevel;

    public LevelInfo currentLevel {
        get { return _currentLevel; }
    }

    public void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        } else {
            instance = this; 
            DontDestroyOnLoad(gameObject);
            levels = new Levels();
            _currentLevel = levels.GetFirstLevel();
        }
    }
    //void OnLevelWasLoaded(int level) {
    //}

    public void LevelFinished() {
        // do some fancy level finish thing
        _currentLevel = levels.GetNextLevel(_currentLevel);
        Application.LoadLevel(_currentLevel.scene);
    }
}
