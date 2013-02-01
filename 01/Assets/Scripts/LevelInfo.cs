using System.Collections;
using System.Collections.Generic;

public class Levels {
    public List<LevelInfo> levels;

    public Levels() {
        levels = new List<LevelInfo>() {
            new LevelInfo("1-1", BoardState.FromToggles(new List<int>() {0})),
            new LevelInfo("1-2", BoardState.FromToggles(new List<int>() {0, 2, 4})),
            new LevelInfo("1-3", BoardState.FromToggles(new List<int>() {6, 7, 8, 12})),
            new LevelInfo("1-3", BoardState.FromToggles(new List<int>() {8, 9, 11, 13, 14})),
            new LevelInfo("1-4", BoardState.FromToggles(new List<int>() {1, 3, 5, 7, 9, 11, 13, 15})),
            new LevelInfo("1-5", BoardState.FromToggles(new List<int>() {22, 4, 6, 18, 15, 4})),
            new LevelInfo("end")
        };
    }

    public LevelInfo GetLevel(string name) {
        foreach(LevelInfo li in levels) {
            if(li.name == name) {
                return li;
            }
        }
        throw new KeyNotFoundException(name);
    }
    public LevelInfo GetFirstLevel() {
        return levels[0];
    }
    public LevelInfo GetNextLevel(LevelInfo level) {
        return levels[levels.IndexOf(level) + 1];
    }
}

public class LevelInfo {

    public int enemiesAtStart = 0;
    public float enemySpawnRate = 0;
    public float chanceInside = 0;
    public string scene = null;
    public string name = null;
    public BoardState state = null;

    public LevelInfo(string customScene) {
        this.scene = customScene;
    }
    public LevelInfo(string name, BoardState state): this(name, state, 0) { }
    public LevelInfo(string name, BoardState state, int enemiesAtStart) {
        this.scene = "generalLevel";
        this.name = name;
        this.state = state;
        this.enemiesAtStart = enemiesAtStart;
    }
}
