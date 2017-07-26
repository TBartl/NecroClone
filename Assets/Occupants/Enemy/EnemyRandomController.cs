using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomController : Controller {

    IntVector2[] randomDirections = { IntVector2.up, IntVector2.right, IntVector2.down, IntVector2.left };
    
    ActionAutoHitDigOrMove hitDigOrMove;

    protected override void Awake() {
        base.Awake();
        hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
    }

    protected override void OnRecoverFinished() {
        IntVector2 direction = randomDirections[Random.Range(0, randomDirections.Length)];
        DoAction(hitDigOrMove.GetAction(direction), direction);
    }
}
