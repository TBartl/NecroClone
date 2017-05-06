using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHitDigOrMove : Action {
    public float moveRecoverTime;
    public float digRecoverTime;
    float recoverTime;

    WeaponHolder weaponHolder;
    ShovelHolder shovelHolder;

    protected override void Awake() {
        base.Awake();
        weaponHolder = this.GetComponent<WeaponHolder>();
        shovelHolder = this.GetComponent<ShovelHolder>();
    }

    public override float GetRecoverTime() {
        return recoverTime;
    }

    public override void Execute(IntVector2 direction) {
        IntVector2 targetPos = intTransform.GetPos() + direction;

        //Try Weapon
        if (weaponHolder != null && weaponHolder.weapon != null) {
            if (weaponHolder.weapon.CanHit(this.gameObject, direction)) {
                weaponHolder.weapon.Hit(this.gameObject, direction);

                intTransform.Bump(targetPos);
                SoundManager.S.Play(SoundManager.S.hit);

                recoverTime = weaponHolder.weapon.recoverTime;
                return;
            }
        }

        // Try Dig
        if (shovelHolder != null) {
            GameObject targetWall = intTransform.GetLevel().GetOccupantAt(targetPos);
            if (targetWall != null) {
                Diggable diggable = targetWall.GetComponent<Diggable>();
                if (diggable != null) {
                    diggable.Dig();

                    intTransform.Bump(targetPos);
                    SoundManager.S.Play(SoundManager.S.hit);

                    recoverTime = digRecoverTime;
                    return;
                }
            }
        }

        // Try move
        intTransform.TryMove(intTransform.GetPos() + direction);
        recoverTime = moveRecoverTime;
    }
}
