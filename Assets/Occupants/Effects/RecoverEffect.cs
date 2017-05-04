using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : MonoBehaviour {
    public SpriteRenderer scale;
    public SpriteRenderer alpha;
    public Color finalColor;

    public float length = 1f;
    static float fadeOutLength = .1f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Run());
	}

    IEnumerator Run() {
        scale.color = finalColor;
        for (float t = 0; t < length; t += Time.deltaTime) {
            float p = t / length;
            scale.transform.localScale = Vector3.one * Mathf.Pow(p, 1f);
            alpha.color = new Color(finalColor.r,finalColor.g,finalColor.b, finalColor.a * p);
            yield return null;
        }
        scale.transform.localScale = Vector3.one;
        alpha.color = finalColor;

        for (float t = 0; t < fadeOutLength; t += Time.deltaTime) {
            float p = 1 - t / fadeOutLength;
            Color c = new Color(finalColor.r, finalColor.g, finalColor.b, finalColor.a * p);
            scale.color = c;
            alpha.color = c;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
