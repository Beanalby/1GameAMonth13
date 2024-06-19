using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float effectDuration;
    public Sprite[] sprites;

    private float effectStart, spriteStart;
    private float spriteDuration;
    private int spriteIndex;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        spriteIndex = 0;
        sr.sprite = sprites[spriteIndex];
        sr.size = new Vector2(10, 10);

        effectStart = Time.time;
        spriteDuration = effectDuration / sprites.Length;
        spriteStart = Time.time;

        Vector3 facingCamera = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(facingCamera);
    }

    void Update()
    {
        // if we've run long enough, self-destruct
        if (Time.time > effectStart + effectDuration)
        {
            Destroy(gameObject);
            return;
        }

        Color c = sr.color;
        float alpha = 1 - ((Time.time - effectStart) / effectDuration);
        sr.color = new Color(c.r, c.g, c.b, alpha);
        // not time to end the whole effect, see if
        // it's time to cycle to the next sprite
        if (Time.time > spriteStart + spriteDuration)
        {
            spriteIndex = (spriteIndex + 1) % sprites.Length;
            sr.sprite = sprites[spriteIndex];
            spriteStart = Time.time;
        }
    }
}
