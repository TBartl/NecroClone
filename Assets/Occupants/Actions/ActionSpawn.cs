using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawn : ActionFixedRecoverTime {

    public string toSpawn;

    public override void Execute(IntVector2 offset) {
        IntVector2 spawnPos = intTransform.GetPos() + offset;
        if (intTransform.GetLevel().InBounds(spawnPos) && !intTransform.GetLevel().Occuppied(spawnPos)) {
            intTransform.GetLevel().SpawnOccupant(toSpawn, spawnPos);
            SoundManager.S.Play(SoundManager.S.spawn);
        }
    }

}
