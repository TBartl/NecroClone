using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitPatterns/Longsword")]
public class HitPatternLongsword : HitPattern {

	public override List<GameObject> GetValidTargets(GameObject source, IntVector2 direction) {
		List<GameObject> validTargets = new List<GameObject>();
		IntVector2 targetPos = direction + source.GetComponent<IntTransform>().GetPos();
		AddIfValidTarget(ref validTargets, source, targetPos);
		if (validTargets.Count > 0 || GetPotentialTarget(source, targetPos) == null)
			AddIfValidTarget(ref validTargets, source, targetPos + direction);
		return validTargets;
	}
}
