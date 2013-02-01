using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StageState { Intro, Running, Complete };

public class GameDriver : MonoBehaviour {

    public GUISkin skin;
    public Texture letterboxTexture;

    private static GameDriver instance = null;
    private StageState stageState;
    private Levels levels;
    private LevelInfo _currentLevel;
    private PlayerController player;

    public LevelInfo currentLevel {
        get { return _currentLevel; }
    }

    public void Awake() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        } 
        instance = this; 
        DontDestroyOnLoad(gameObject);
        levels = new Levels();
        _currentLevel = levels.GetFirstLevel();
        StartCoroutine(InitLevel());
    }
    void DrawLetterboxBars(string message) {
        float boxHeight = Screen.height / 8;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, boxHeight), letterboxTexture);
        GUI.DrawTexture(new Rect(0, Screen.height - boxHeight, Screen.width, boxHeight), letterboxTexture);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 8), message);

    }
    void OnGUI() {
        if(skin)
            GUI.skin = skin;
        switch(stageState){
            case StageState.Intro:
                DrawLetterboxBars("Stage " + currentLevel.name);
                break;
            case StageState.Complete:
                DrawLetterboxBars("Clear!");
                break;
        }
    }
    void OnLevelWasLoaded(int level) {
        StartCoroutine(InitLevel());
    }

    // performs the level "intro", keeps player locked
    IEnumerator InitLevel() {
        if(Application.loadedLevelName == "generalLevel") {
            stageState = StageState.Intro;
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.sceneFrozen = true;
            yield return new WaitForSeconds(2f);
            stageState = StageState.Running;
            player.sceneFrozen = false;
        }
    }
    public void LevelFinished() {
        stageState = StageState.Complete;
        // Destroy any enemies that may be left
        foreach(TinyEnemyController enemy in GameObject.FindSceneObjectsOfType(typeof(TinyEnemyController))) {
            enemy.KillSelf();
            Debug.Log("Enemy: " + enemy.gameObject.name);
        }
        GameObject.Find("Player").GetComponent<PlayerController>().sceneFrozen = true;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(3f);
        _currentLevel = levels.GetNextLevel(_currentLevel);
        Application.LoadLevel(_currentLevel.scene);
    }
}
