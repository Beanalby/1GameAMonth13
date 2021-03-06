﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDriver8 : MonoBehaviour {
    private static string FIRST_LEVEL = "level1";

    private float totalTime;
    public float TotalTime {
        get { return totalTime; }
    }
    private float lastLevelTime;
    public float LastLevelTime {
        get { return lastLevelTime; }
    }
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
    private List<string> finishedLevels, specialExits;
    private List<string> pickups;

    public string displayPickup;

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
        InitData();
    }

    public void AddPickup(string name) {
        pickups.Add(name);
        displayPickup = name;
    }
    public void DecrementTime(float amount) {
        amount = Mathf.Min(amount, lastLevelTime);
        lastLevelTime -= amount;
        totalTime = Mathf.Max(0, totalTime - amount / 10);
    }
    public bool GotAllPickups() {
        return pickups.Count == 2;
    }
    private void InitData() {
        finishedLevels = new List<string>();
        pickups = new List<string>();
        specialExits = new List<string>();
        lastLevelTime = -1f;
        lastLevel = null;
        totalTime = 10f;
        displayPickup = null;
    }

    public void LevelFinished(float duration, string specialExit) {
        finishedLevels.Add(Application.loadedLevelName);
        if(specialExit != null && specialExit != "") {
            specialExits.Add(specialExit);
        }
        lastLevelTime = duration;
        displayPickup = null;
        Application.LoadLevel("reduceTime");
    }

    public void LoadSelectLevel() {
        Application.LoadLevel("selectLevel");
    }
    public void LoadLevel(Level level) {
        lastLevel = level.scene;
        Application.LoadLevel(level.scene);
    }
    public bool IsLinkActive(Level level1, Level level2) {
        if(level1.needSpecialExit && !specialExits.Contains(level1.scene)) {
            return false;
        }
        if(level2.needSpecialExit && !specialExits.Contains(level2.scene)) {
            return false;
        }
        return IsLevelFinished(level1) || IsLevelFinished(level2);
    }
    public bool IsLevelFinished(Level level) {
        return finishedLevels.Contains(level.scene);
    }
    public void ReduceFinished() {
        if(totalTime <= 0) {
            Application.LoadLevel("levelLose");
        } else {
            Application.LoadLevel("selectLevel");
        }
    }
    public void Restart() {
        InitData();
        Application.LoadLevel("title");
    }
}
