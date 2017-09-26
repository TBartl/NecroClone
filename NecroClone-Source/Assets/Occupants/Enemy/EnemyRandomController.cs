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
        int randIndex = Random.Range(0, randomDirections.Length);
        for (int i = 0; i < randomDirections.Length; i++) {
            IntVector2 direction = randomDirections[(randIndex + i) % randomDirections.Length];
            if (!hitDigOrMove.WillBump(direction) || i == randomDirections.Length - 1) {
                DoAction(hitDigOrMove.GetAction(direction), direction);
                return;
            }
        }
    }
}
