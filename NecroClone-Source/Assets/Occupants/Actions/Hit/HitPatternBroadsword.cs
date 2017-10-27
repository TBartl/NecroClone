using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitPatterns/Broadsword")]
public class HitPatternBroadsword : HitPattern {

	public override List<GameObject> GetValidTargets(GameObject source, IntVector2 direction) {
		List<GameObject> validTargets = new List<GameObject>();
		IntVector2 targetPos = direction + source.GetComponent<IntTransform>().GetPos();

		// TODO: Handle diagonal here?
		AddIfValidTarget(ref validTargets, source, targetPos);

		if (direction.x != 0) {
			AddIfValidTarget(ref validTargets, source, targetPos + IntVector2.up);
			AddIfValidTarget(ref validTargets, source, targetPos + IntVector2.down);
		} else {
			AddIfValidTarget(ref validTargets, source, targetPos + IntVector2.left);
			AddIfValidTarget(ref validTargets, source, targetPos + IntVector2.right);
		}

		return validTargets;
	}
}