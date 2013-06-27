using UnityEngine;
using System.Collections;

public abstract class Wave : MonoBehaviour {
    [HideInInspector]
    public GameObject[] letters;

    protected float speed = 2f;
    protected float duration = 4f;

    public abstract GameObject GetLetterPrefab(string text, int index);
}
