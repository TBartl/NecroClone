using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombieController : Controller {

	public bool horizontal = true;
	bool to;

	ActionAutoHitDigOrMove hitDigOrMove;

	protected override void Awake() {
		base.Awake();
		hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
	}

	protected override void OnRecoverFinished() {
		IntVector2 direction = IntVector2.zero;
		if (horizontal)
			direction = to ? IntVector2.right : IntVector2.left;
		else
			direction = to ? IntVector2.up : IntVector2.down;
		IntVector2 start = intTransform.GetPos();
		DoAction(hitDigOrMove.GetAction(direction), direction);
		IntVector2 end = intTransform.GetPos();

		if (start == end)
			to = !to;
	}
}
