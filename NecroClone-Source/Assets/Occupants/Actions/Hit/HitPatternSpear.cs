using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitPatterns/Spear")]
public class HitPatternSpear : HitPattern {

	public Sprite effectShort;
	Sprite effectLong;

	public override List<GameObject> GetValidTargets(GameObject source, IntVector2 direction) {
		if (effectLong == null)
			effectLong = effect;

		List<GameObject> validTargets = new List<GameObject>();
		IntVector2 targetPos = direction + source.GetComponent<IntTransform>().GetPos();
		if (GetPotentialTarget(source, targetPos) == null) {
			AddIfValidTarget(ref validTargets, source, targetPos + direction);
			effect = effectLong;
		}
		else {
			AddIfValidTarget(ref validTargets, source, targetPos);
			effect = effectShort;
		}
		return validTargets;
	}
}
