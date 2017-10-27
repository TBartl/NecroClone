using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPattern : ScriptableObject {

	public virtual List<GameObject> GetValidTargets(GameObject source, IntVector2 direction) {
		List<GameObject> validTargets = new List<GameObject>();
		return validTargets;
	}

	protected bool AddIfValidTarget(ref List<GameObject> targets, GameObject source, IntVector2 targetPos) {
		GameObject target = source.GetComponent<IntTransform>().GetLevel().GetOccupantAt(targetPos);
		if (IsValidTarget(source, target)) {
			targets.Add(target);
			return true;
		}
		return false;
	}

	protected bool IsValidTarget(GameObject source, GameObject target) {
		if (target == null)
			return false;
		// Non Players Don't do damage to Non Players (enemies don't have friendly fire)
		if (!source.GetComponent<PlayerController>() && !target.GetComponent<PlayerController>())
			return false;
		if (target.GetComponent<Killable>() == null)
			return false;
		return true;
	}

}
