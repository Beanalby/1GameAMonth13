using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IntroMessage))]
public class FinishedDriver : MonoBehaviour {

    private string endingBad = "The attackers have been repelled.  In less than 10 seconds, our last-ditch rescue program successsfully secured the network.  Unfortunately, it will only be a matter of time until they return...";
    private string retryBad = "Retry";
    private string endingGood = "The attackers have been defeated.  In less than 10 seconds, our rescue program worked beyond our wildest dreams, acquiring new tools as it ran and capturing attackers.  They won't be back anytime soon.";
    private string retryGood = "The End";

    IntroMessage intro;
    public void Awake() {
        intro = GetComponent<IntroMessage>();
        // if they've acquired all the powerups, give the good ending
        if(GameDriver8.instance.GotAllPickups()) {
            intro.message = endingGood;
            intro.retryText = retryGood;
        } else {
            intro.message = endingBad;
            intro.retryText = retryBad;
        }
    }
}
