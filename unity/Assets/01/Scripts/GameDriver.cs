using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StageState { Intro, Running, Complete };

public class GameDriver : MonoBehaviour {

    public GUISkin skin;
    public Texture letterboxTexture;
    public GameObject TinyEnemyTemplate;
    private static GameDriver instance = null;
    private StageState stageState;
    private Levels levels;
    private LevelInfo _currentLevel;
    private PlayerController player;
    private float lastSpawn = -1;
    private GameObject[] spawnPoints;

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
    void Update() {
        HandleSpawn();
    }

    void DrawLetterboxBars(string message) {
        float boxHeight = Screen.height / 8;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, boxHeight), letterboxTexture);
        GUI.DrawTexture(new Rect(0, Screen.height - boxHeight, Screen.width, boxHeight), letterboxTexture);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 8), message);
    }
    void HandleSpawn() {
        if(lastSpawn != -1 && currentLevel.spawnRate != -1 && lastSpawn + currentLevel.spawnRate < Time.time) {
            SpawnEnemy();
        }
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
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            SpawnEnemy();
            SpawnEnemy();
            lastSpawn = Time.time;
        }
    }
    public void BoardClear() {
        stageState = StageState.Complete;
        // Destroy any enemies that may be left
        foreach(TinyEnemyController enemy in GameObject.FindObjectsOfType(typeof(TinyEnemyController))) {
            enemy.KillSelf();
        }
        lastSpawn = -1;
        GameObject.Find("Player").GetComponent<PlayerController>().sceneFrozen = true;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(3f);
        _currentLevel = levels.GetNextLevel(_currentLevel);
        Application.LoadLevel(_currentLevel.scene);
    }
    void SpawnEnemy() {
        GameObject tmp = Instantiate(TinyEnemyTemplate) as GameObject;
        int index = (int)Random.Range(0, spawnPoints.Length);
        tmp.transform.position = spawnPoints[index].transform.position;
        Vector3 lookPos = player.transform.position - tmp.transform.position;
        lookPos.y = 0;
        tmp.transform.rotation = Quaternion.LookRotation(lookPos);
        lastSpawn = Time.time;
    }
}
