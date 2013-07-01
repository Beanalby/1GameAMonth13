using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TitleSiflOllie : MonoBehaviour {

    public Texture[] images;

    AudioSource sound;

    private bool running = false;

    public void Start() {
        sound = GetComponent<AudioSource>();
    }

    public void OnGUI() {
        Texture tex = GetImage();
        if(tex != null) {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
        }

        if(!running) {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayoutOption[] opts = new GUILayoutOption[] {
                GUILayout.Width(300), GUILayout.Height(50) };
            if(GUILayout.Button("Start", opts)) {
                running = true;
                sound.Play();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    private Texture GetImage() {
        float pos = ((float)sound.timeSamples) / sound.clip.frequency;
        if(pos < .17) {
            return images[0]; // black
        } else if(pos < .53) {
            return images[1];
        } else if(pos < .87) {
            return images[2];
        } else if(pos < 1.18) {
            return images[3];
        } else if(pos < 1.49) {
            return images[4];
        } else if(pos < 2.02) {
            return images[5]; // sifl
        } else if(pos < 2.71) {
            return images[6]; // &
        } else if(pos < 3.97) {
            return images[7]; // ollie
        } else if(pos < 7.0) {
            return images[8]; // title;
        } else {
            Application.LoadLevel("06-speaking");
            return images[8]; // title;
        }
    }
}
