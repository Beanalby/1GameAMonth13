using UnityEngine;
using System.Collections;

public enum SpotValue { O, X, None };

public interface Spot {
    SpotValue GetValue();
}
