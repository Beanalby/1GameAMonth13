using UnityEngine;
using System.Collections;

public enum Speaking { None, Caller, Sifl, Ollie, CallerScared };

[RequireComponent(typeof(AudioSource))]
public class SpeakingDriver : MonoBehaviour {

    private const int INTRO_CUTOFF = 3980333; //90.257s * 44100;

    public Texture CallerClosed;
    public Texture CallerOpen;

    public Texture OllieClosed;
    public Texture OllieOpen;
    public Texture SiflClosed;
    public Texture SiflOpen;
    public Texture Microphone;

    private Speaking previousSpeaker = Speaking.None;
    private bool isMouthOpen = true;
    private float nextMouthToggle = -1f;
    private float mouthCooldown = .25f;

    private AudioSource sound;
    private Rect rectSifl, rectOllie, rectCaller;

    public void Start() {
        float size = OllieClosed.width;
        rectSifl = new Rect(0, Screen.height - size, size, size);
        rectOllie = new Rect(Screen.width - size, Screen.height - size,
            size, size);
        rectCaller = new Rect(Screen.width /2 - size / 2, Screen.height - size,
            size, size);

        sound = GetComponent<AudioSource>();
        //Debug.Log("Jumping to " + (sound.clip.frequency * 118));
        //sound.timeSamples = sound.clip.frequency * 85;
        sound.Play();
    }

    public void OnGUI() {
        Speaking current = UpdateSpeaking();
        //Debug.Log(sound.timeSamples + " (" + ((float)sound.timeSamples / sound.clip.frequency) + ")=" + current);
        switch(current) {
            case Speaking.None:
                GUI.DrawTexture(rectSifl, SiflClosed);
                GUI.DrawTexture(rectOllie, OllieClosed);
                break;
            case Speaking.Ollie:
                GUI.DrawTexture(rectSifl, SiflClosed);
                if(isMouthOpen) {
                    GUI.DrawTexture(rectOllie, OllieOpen);
                } else {
                    GUI.DrawTexture(rectOllie, OllieClosed);
                }
                break;
            case Speaking.Sifl:
                if(isMouthOpen) {
                    GUI.DrawTexture(rectSifl, SiflOpen);
                } else {
                    GUI.DrawTexture(rectSifl, SiflClosed);
                }
                GUI.DrawTexture(rectOllie, OllieClosed);
                break;
            case Speaking.Caller:
                if(isMouthOpen) {
                    GUI.DrawTexture(rectCaller, CallerOpen);
                } else {
                    GUI.DrawTexture(rectCaller, CallerClosed);
                }
                break;
            case Speaking.CallerScared:
                GUI.DrawTexture(rectCaller, CallerClosed);
                break;
        }
        if(sound.timeSamples < INTRO_CUTOFF) {
            if(GUI.Button(new Rect(0, 0, 300, 50), "Skip Intro")) {
                sound.Stop();
                sound.timeSamples = INTRO_CUTOFF;
                sound.Play();
            }
        }
    }
    
    private Speaking UpdateSpeaking() {
        Speaking current = GetSpeaking();
        // if this speaker is different from the last one, start with the
        // mouth open and reset the cooldown
        if(current != previousSpeaker) {
            isMouthOpen = true;
            nextMouthToggle = Time.time
                + Random.Range(.5f * mouthCooldown, 1.5f * mouthCooldown);
        } else {
            // same speaker, see if we need to toggle the mouth
            if(Time.time > nextMouthToggle) {
                isMouthOpen = !isMouthOpen;
                nextMouthToggle = Time.time
                    + Random.Range(.5f * mouthCooldown, 1.5f * mouthCooldown);
            } 
        }
        previousSpeaker = current;
        return current;
    }

    private Speaking GetSpeaking() {
        float pos = ((float)sound.timeSamples / sound.clip.frequency);
        if(pos < 1) {
            return Speaking.Sifl;
        } else if(pos < 18) {
            return Speaking.Caller;
        } else if(pos < 22.8) {
            return Speaking.Ollie;
        } else if(pos < 28.6) {
            return Speaking.Sifl;
        } else if(pos < 36) {
            return Speaking.Caller;
        } else if(pos < 42.9) {
            return Speaking.Sifl;
        } else if(pos < 53.6) {
            return Speaking.Ollie;
        } else if(pos < 57.6) {
            return Speaking.Sifl;
        } else if(pos < 78.7) {
            return Speaking.Ollie;
        } else if(pos < 79.4) {
            return Speaking.None;
        } else if(pos < 82.5) {  // yeah well he kinda deserves it
            return Speaking.Sifl;
        } else if(pos < 90.25) {
            return Speaking.Ollie;
        } else if(pos < 93) {
            return Speaking.Caller;
        } else if(pos < 95.4) {
            return Speaking.Ollie;
        } else if(pos < 96.5) {
            return Speaking.CallerScared;
        } else {
            Application.LoadLevel("06-game");
            return Speaking.CallerScared;
        }
    }
}