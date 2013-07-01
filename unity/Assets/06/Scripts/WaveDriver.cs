using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WaveDriver : MonoBehaviour {

    public const float WAVE_DURATION = 3.428f;

    public TextCreator tc;
    public Wave waveNormalPrefab;
    public Wave waveKillerWordPrefab;
    public GUISkin skin;

    public OmegaNormal omegaNormal;
    public OmegaMean omegaMean;
    public OmegaHappy omegaHappy;

    private AudioSource song;
    private string[] lines;
    private string[] waveLabels;

    private float initalDelayScale = .3f;
    private int sampleRate = 44100;
    private int samplesPerSection;
    private int nextSection;
    private int lyricsIndex = -1;
    private bool isRunning = true;
    private OmegaDriver omegaCurrent;
    private float stopRunningBegin = -1f;
    private float musicFadeDuration = 5f;

    void Start () {
        InitLines();
        samplesPerSection = (int)(sampleRate * WAVE_DURATION);
        song = GetComponent<AudioSource>();
        JumpToSection(110);
        song.Play();
        omegaNormal.gameObject.SetActive(false);
        omegaMean.gameObject.SetActive(false);
        omegaHappy.gameObject.SetActive(false);

        SetActiveOmega(omegaMean);
        //lyricsIndex = 3; // +++ omega-only
        //song.volume = 0; // FOR SANITY 
    }

    public void Update () {
        if(isRunning) {
            CheckNextSection();
        } else {
            if(stopRunningBegin != -1) {
                // fade out the music
                float percent = 1 - ((Time.time - stopRunningBegin) / musicFadeDuration);
                if(percent >= 1) {
                    song.Stop();
                    stopRunningBegin = -1;
                } else {
                    song.volume = percent;
                }
            }
        }
    }

    public void OnGUI() {
        GUI.skin = skin;
        DrawWaveLabel();
        if(!isRunning) {
            DrawRetryGUI();
        }
    }

    public void DrawWaveLabel() {
        if(lyricsIndex >= 0 && lyricsIndex < waveLabels.Length) {
            GUI.Label(new Rect(5, Screen.height - 30, Screen.width, 25),
                "Wave: " + waveLabels[lyricsIndex], skin.customStyles[0]);
        }
    }

    public void DrawRetryGUI() {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginVertical();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("You're dead!");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Not Crescent Fresh.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayoutOption[] opts = new GUILayoutOption[] {
            GUILayout.Width(200), GUILayout.Height(50)};
        if(GUILayout.Button("Retry", opts)) {
            Application.LoadLevel("06-game");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void JumpToSection(int section) {
        if(section == 0) {
            song.timeSamples = 0;
            nextSection = (int)(samplesPerSection * initalDelayScale);
        } else {
            int offset = (int)(samplesPerSection * initalDelayScale)
                + samplesPerSection * section;
            // pull audio start back by a second, helps flesh out the start
            song.timeSamples = offset - sampleRate;
            nextSection = offset;
            lyricsIndex = section-1;
        }
    }

    private void CheckNextSection() {
        if(song.timeSamples >= nextSection) {
            if(!AdvanceSection()) {
                StartCoroutine(LoadFinish());
                return;
            }
            nextSection += samplesPerSection;
        }
    }

    private IEnumerator LoadFinish() {
        yield return new WaitForSeconds(2f);
        Application.LoadLevel("06-Finish");
    }

    private bool AdvanceSection() {
        lyricsIndex++;
        //lyricsIndex += 4; // +++ omega-only
        if(lyricsIndex >= lines.Length) {
            return false;
        }

        //Debug.Log(Time.time.ToString(".000")
        //    + " (" + song.timeSamples + " > " + nextSection + ") #"
        //    + lyricsIndex + "-" + lines[lyricsIndex]);
        if(lines[lyricsIndex] == "Omega") {
            omegaCurrent.ShowOmega((lyricsIndex - 3) / 4);
        } else {
            CreateWave(lyricsIndex);
        }
        return true;
    }

    private GameObject CreateWave(int waveIndex) {
        GameObject obj;

        // later stages have higher chance of killerWord waves,
        // and also higher chance of killerWord waves being heal instead
        float stagePercent = ((float)lyricsIndex / 120);
        float killerChance = Mathf.Lerp(.4f, .9f, stagePercent);
        float tmp = Random.Range(0f, 1f);
        Debug.Log("killer: " + tmp + " < " + killerChance + "?");
        if( tmp < killerChance) {
            Debug.Log("Making killer!");
            obj = Instantiate(waveKillerWordPrefab.gameObject) as GameObject;
            // also check for flipping to heal instead
            float healChance = .25f;
            tmp = Random.Range(0f, 1f);
            Debug.Log("Heal: " + tmp + " < " + killerChance + "?");
            if(tmp < healChance) {
                obj.GetComponent<WaveKillerWord>().isKiller = false;
            }
        } else {
            obj = Instantiate(waveNormalPrefab.gameObject)
                as GameObject;
        }
        obj.transform.position = transform.position;
        Wave wave = obj.GetComponent<Wave>();
        wave.text = lines[waveIndex];
        wave.tc = tc;
        return wave.gameObject;
    }

    public void OmegaDead() {
        if(omegaCurrent == omegaNormal) {
            SetActiveOmega(omegaMean);
        } else {
            SetActiveOmega(omegaHappy);
        }
    }

    private void SetActiveOmega(OmegaDriver newDriver) {
        omegaCurrent = newDriver;
        omegaCurrent.gameObject.SetActive(true);
        //omegaNormal.gameObject.SetActive(omegaCurrent == omegaNormal);
        //omegaMean.gameObject.SetActive(omegaCurrent == omegaMean);
        //omegaHappy.gameObject.SetActive(omegaCurrent == omegaHappy);
    }

    public void StopRunning() {
        isRunning = false;
        stopRunningBegin = Time.time;
    }

    private void InitLines() {
        lines = new string[] {
            "Because you're not afraid to take a chance",
            "When dangers near you always hold your stance",
            "As you race through the great expanse",
            "Omega",
            "While others flee you still advance",
            "Thrust forth like an ancient lance",
            "Fighting like a modern dance",
            "Omega",
            "The universe knows you can't be beat",
            "Passing every test and you never cheat",
            "Star milk dripping from your metal teats",
            "Omega",
            "Your death makes love as it kills a fleet",
            "Of starships thinking that they could compete",
            "with the power of our ancient meat",
            "Omega",
            "You haven't got an ejection seat",
            "Only cowards run from the heat",
            "You make the universe complete",
            "Omega",
            "And so we frolic with a jaunty prance",
            "We get erotic with a snakey dance",
            "Help us out of our pants",
            "Omega",
            "We're talkin' tickets and a cash advance",
            "For a trip inside your sweet romance",
            "Oh baby, do a little belly dance",
            "Omega",
            "You never play by the books",
            "Omega, even all the lives you took",
            "Heartbreaker, sexy as a cozy nook",
            "Omega",
            "And when your shine like a silver beats",
            "Serving up your sexy catastrophic feast",
            "Your ball bearings need their grease",
            "Omega",
            "You know space pirates sure like to brawl",
            "Shooting laser holes right through the wall",
            "But none shall breach your hull",
            "Omega",
            "You're the best ship we've ever seen",
            "Posing on the cover of a magazine",
            "Gasoline and Vaseline",
            "Omega",
            "Your thrusters are burning bright",
            "Your force fields are mighty tight",
            "Your hard drives have mega-bites",
            "Omega",
            "You leave space pirates in your cosmic dust",
            "As your warp drives writhe and thrust",
            "Light speed towards our lust",
            "Omega",
            "You're old but you'll never rust",
            "You've got sweet jugs but you'll never bust",
            "You were our first, so we'll always trust",
            "Omega",
            "In war you never tire",
            "And when it comes down to the wire",
            "You were built for death and fire",
            "Omega",
            "You take worlds and you take control",
            "With your guns and your heart and soul",
            "We're so glad you're out on parole",
            "Omega",
            "We're shy but we had to ask",
            "For your autograph on our cast",
            "And could we give your engines a blast",
            "Omega",
            "Because you're like a big horny yacht",
            "That's flying through space a lot",
            "Tell us about the wars you fought",
            "Omega",
            "You know we were distraught",
            "When we heard about your dry rot",
            "Infesting your expansion slot",
            "Omega",
            "You're more than just a ship",
            "Got the universe in your grip",
            "When you bite your pouty lip",
            "Omega",
            "We're sure every docking bay",
            "Prays that you'll come their way",
            "They want to party in your gamma rays",
            "Omega",
            "Your metals stained in blood",
            "There's a pentagram on your H.U.D.",
            "Turning cities to mud",
            "Omega",
            "Party hearty like you just don't care",
            "Throw your rockets up in the air",
            "We'd kill to have your hair",
            "Omega",
            "There's one thing that you should know",
            "As far as star fighters go",
            "You're quite the vicious ho",
            "Omega",
            "You got moxy and you go the skills",
            "For a hundred million billion kills",
            "We'll leave you in our wills",
            "Omega",
            "You're a spaceship and a whore",
            "Built to kill stuff in war",
            "You get what you pay for",
            "Omega",
            "Hear her engines roar",
            "Like a burning liquor store",
            "With beautiful french doors",
            "Omega",
            "Credits",
            "Sifl: Matt Crocco",
            "Olly: Liam Lynch",
            "Omega",
            "Not Suing: hopefully Matt & Liam",
            "Sloppy Code: Jason Viers",
            "Cutie Pie: Tina Viers",
            "Omega",
            "Belly Rubs: Willow the Cat",
            "Spilling Water: Morgan the Cat",
            "Annoying Meow: Kirby the Cat",
            "Omega"
        };
        waveLabels = new string[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "Omega",
            "28",
            "29",
            "30",
            "Omega!",
            "32",
            "33",
            "34",
            "OMEGA!",
            "36",
            "37",
            "38",
            "Omeeeegaaaaa",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "Knock knock.",
            "Who's there?",
            "OOOMMEEEEGGAAA",
            "52",
            "53",
            "54",
            "55",
            "56",
            "Two rocks walk into a bar.",
            "The bartender says,",
            "OMEGA!",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "Is anyone gonna read this?",
            "Probably not",
            "68",
            "So much wasted effort",
            "All for naught!",
            "*sob*",
            "72",
            "73",
            "74",
            "75",
            "But you'll read it, won't you?",
            "I know you will.",
            "78",
            "79",
            "80",
            "81",
            "82",
            "83",
            "Keep it goin!",
            "85",
            "86",
            "Stayin alive!",
            "88",
            "89",
            "90",
            "91",
            "Is the song stuck in your head yet?",
            "93",
            "Think about how it is FOR ME.",
            "95",
            "I've lost count of how many times I've heard it.",
            "97",
            "I started humming it randomly throughout the day.",
            "99",
            "I fear I'll never be free of its lyrical grasp.",
            "101",
            "102",
            "Almost there...",
            "104",
            "105",
            "106",
            "107",
            "108",
            "109",
            "110",
            "111",
            "112",
            "113",
            "114",
            "115",
            "116",
            "117",
            "118",
            "Thanks for playing!"
        };
    }
}
