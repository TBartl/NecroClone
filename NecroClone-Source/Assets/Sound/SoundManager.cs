using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager S;

    public AudioSource hit;
    public AudioSource spawn;
	public AudioSource pickup;

    void Awake() {
		if (S != null)
			return;
		S = this;
    }

    public void Play(AudioSource source) {
        AudioSource clone = Instantiate(source.gameObject).GetComponent<AudioSource>();
        clone.Play();
        Destroy(clone.gameObject, 1);
    }
}
