using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDatabase : MonoBehaviour {
    public static EffectDatabase S;

    public GameObject recover;
    public GameObject heart;
	public GameObject hitEffect;

    void Awake() {
		if (S != null)
			return;
		S = this;
    }

    public void CreateRecovery(Transform t, float recoverTime) {
		GameObject g = GameObject.Instantiate(recover, t);
		g.transform.localPosition = Vector3.zero;
		g.GetComponent<RecoverEffect>().length = recoverTime;
	}

	public void CreateHitEffect(Sprite sprite, IntVector2 from, IntVector2 direction) {
		GameObject g = GameObject.Instantiate(hitEffect);
		g.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
		g.transform.position = (Vector3)from;
		g.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg + 90, 0);
	}

}
