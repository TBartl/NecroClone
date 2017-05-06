using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEarthSpell : ActionFixedRecoverTime {

    public override void Execute(IntVector2 direction) {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                IntVector2 target = intTransform.GetPos() + new IntVector2(x, y);
                if (intTransform.GetLevel().InBounds(target) && !intTransform.GetLevel().Occuppied(target))
                    intTransform.GetLevel().AddOccupant(OccupantId.wall, target);
            }
        }
        SoundManager.S.Play(SoundManager.S.spawn);
    }
}
