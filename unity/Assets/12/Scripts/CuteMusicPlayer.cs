using UnityEngine;
using System.Collections;

public class CuteMusicPlayer : MonoBehaviour {

    static private CuteMusicPlayer instance = null;
    static public CuteMusicPlayer Instance {
        get { return instance; }
    }

    public AudioClip titleMusic;
    public AudioClip stageMusic;

    private string currentMusic;

    public void Awake() {
        if(instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    public void PlayMusic(string name) {
        if(name=="titleMusic") {
            audio.clip = titleMusic;
        } else if(name == "stageMusic") {
            audio.clip = stageMusic;
        } else {
            return;
        }
        if(!audio.isPlaying) {
            audio.Play();
        }
    }        
}
