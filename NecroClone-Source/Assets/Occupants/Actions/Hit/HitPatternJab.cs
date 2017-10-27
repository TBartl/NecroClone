using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitPatterns/Jab")]
public class HitPatternJab : HitPattern {

	public override List<GameObject> GetValidTargets(GameObject source, IntVector2 direction) {
		List<GameObject> validTargets = new List<GameObject>();
		IntVector2 target = direction + source.GetComponent<IntTransform>().GetPos();
		AddIfValidTarget(ref validTargets, source, target);
		return validTargets;
	}
}
