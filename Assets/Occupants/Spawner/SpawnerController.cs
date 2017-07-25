using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Controller {
    public int radius;
    public float attemptsBeforeFail = 10;

    ActionSpawn actionSpawn;

    protected override void Awake() {
        base.Awake();
        actionSpawn = this.GetComponent<ActionSpawn>();
    }

    protected override void OnRecoverFinished() {
        IntVector2 center = this.GetComponent<IntTransform>().GetPos();
        IntVector2 offset = IntVector2.zero;
        for (int attempt = 0; attempt < attemptsBeforeFail; attempt++) {
            IntVector2 tryOffset = new IntVector2(Random.Range(-radius, radius + 1), Random.Range(-radius, radius + 1));
            IntVector2 tryPos = center + tryOffset;
            if (!intTransform.GetLevel().Occuppied(tryPos)) {
                offset = tryOffset;
                break;
            }
        }

        DoAction(actionSpawn, offset);
    }

}
