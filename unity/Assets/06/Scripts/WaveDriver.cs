using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WaveDriver : MonoBehaviour {

    public const float WAVE_DURATION = 3.428f;

    public TextCreator tc;
    public Wave waveNormalPrefab;
    public Wave waveKillerWordPrefab;

    public OmegaNormal omegaNormal;
    public OmegaMean omegaMean;
    public OmegaHappy omegaHappy;

    private AudioSource song;
    private string[] lines;
    private float initalDelayScale = .3f;
    private int sampleRate = 44100;
    private int samplesPerSection;
    private int nextSection;
    private int lyricsIndex = -1;
    private bool isRunning = true;
    private OmegaDriver omegaCurrent;

    void Start () {
        InitLines();
        samplesPerSection = (int)(sampleRate * WAVE_DURATION);
        song = GetComponent<AudioSource>();
        JumpToSection(0);
        song.Play();
        omegaNormal.gameObject.SetActive(false);
        omegaMean.gameObject.SetActive(false);
        omegaHappy.gameObject.SetActive(false);

        SetActiveOmega(omegaNormal);
        lyricsIndex = 3; // +++ omega-only
    }

    void Update () {
        if(isRunning) {
            CheckNextSection();
        }
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
                song.Stop();
                isRunning = false;
                Application.LoadLevel("06-Finish");
                return;
            }
            nextSection += samplesPerSection;
        }
    }

    private bool AdvanceSection() {
        // lyricsIndex++;
        lyricsIndex += 4; // +++ omega-only
        if(lyricsIndex == lines.Length) {
            return false;
        }

        Debug.Log(Time.time.ToString(".000")
            + " (" + song.timeSamples + " > " + nextSection + ") #"
            + lyricsIndex + "-" + lines[lyricsIndex]);
        if(lines[lyricsIndex] == "Omega") {
            omegaCurrent.ShowOmega((lyricsIndex - 3) / 4);
        } else {
            CreateWave(lyricsIndex);
        }
        return true;
    }

    private GameObject CreateWave(int waveIndex) {
        GameObject obj = Instantiate(waveNormalPrefab.gameObject) as GameObject;
        obj.transform.position = transform.position;
        Wave wave = obj.GetComponent<Wave>();
        wave.text = lines[waveIndex];
        wave.tc = tc;
        return wave.gameObject;
    }

    public void OmegaDead() {
        if(omegaCurrent == omegaNormal) {
            Debug.Log("+++ OmegaDead, going to mean!");
            SetActiveOmega(omegaMean);
        } else {
            Debug.Log("+++ OmegaDead, going to happy!");
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
    }
}
