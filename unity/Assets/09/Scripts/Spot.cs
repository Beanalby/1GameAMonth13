using UnityEngine;
using System.Collections;

public enum SpotValue { O, X, Tie, None };

public interface Spot {
    SpotValue GetValue();
}
