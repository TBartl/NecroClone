using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHit : ActionFixedRecoverTime {
    public int damage;
	public HitPattern pattern;

    List<GameObject> validTargets;

    public virtual bool CanHit(IntVector2 direction) {
        return pattern.GetValidTargets(this.gameObject, direction).Count > 0;
    }

    public override void Execute(IntVector2 direction) {
        List<GameObject> validTargets = pattern.GetValidTargets(this.gameObject, direction);
        foreach (GameObject target in validTargets) {
			HitInfo hitInfo;
			hitInfo.damage = damage;
			hitInfo.direction = direction;
            target.GetComponent<Killable>().Hit(hitInfo);
        }
        
        foreach (SmoothMove smoothMove in this.GetComponentsInChildren<SmoothMove>())
            smoothMove.Bump(intTransform.GetPos() + direction);
		EffectDatabase.S.CreateHitEffect(pattern.GetEffect(), intTransform.GetPos(), direction);
        SoundManager.S.Play(SoundManager.S.hit);
    }

    
}
