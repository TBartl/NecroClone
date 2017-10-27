using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeController : Controller {

	public IntVector2 direction;

    ActionAutoHitDigOrMove hitDigOrMove;

    protected override void Awake() {
        base.Awake();
        hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
    }

    protected override void OnRecoverFinished() {
        DoAction(hitDigOrMove.GetAction(direction), direction);
    }
}
