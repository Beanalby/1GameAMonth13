using UnityEngine;
using System.Collections;

public enum SpotValue { O, X, None };

public abstract class Spot : MonoBehaviour {

    public abstract SpotValue GetValue();
}
