using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeController : Controller {

	public List<IntVector2> directions;
	int index = 0;

	ActionAutoHitDigOrMove hitDigOrMove;

	protected override void Awake() {
		base.Awake();
		hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
	}

	protected override void OnRecoverFinished() {
		IntVector2 direction = directions[index];
		Action action = hitDigOrMove.GetAction(direction);
		if (action.GetType() == typeof(ActionTryMove) && !hitDigOrMove.WillBump(direction)) {
			index = (index + 1) % directions.Count;
		}
		DoAction(action, direction);
	}
}
