using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawn : ActionFixedRecoverTime {

    public OccupantId toSpawn;

    public override void Execute(IntVector2 offset) {
        IntVector2 spawnPos = pos.GetPos() + offset;
        if (LevelManager.S.level.InBounds(spawnPos) && !LevelManager.S.level.Occuppied(spawnPos)) {
            LevelManager.S.level.AddOccupant(toSpawn, spawnPos);
            SoundManager.S.Play(SoundManager.S.spawn);
        }
    }

}
