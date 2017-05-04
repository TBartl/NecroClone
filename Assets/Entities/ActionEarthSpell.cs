using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEarthSpell : Action {

    public override void Execute(IntVector2 direction) {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                IntVector2 target = pos.GetPos() + new IntVector2(x, y);
                if (LevelManager.S.level.InBounds(target) && !LevelManager.S.level.Occuppied(target))
                    LevelManager.S.level.AddOccupant(OccupantId.wall, target);
            }
        }
    }
}
