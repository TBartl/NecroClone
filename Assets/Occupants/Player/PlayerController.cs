using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    List<PlayerInputKey> buffer = new List<PlayerInputKey>();

    ActionHitDigOrMove actionHitDigOrMove;
    ActionEarthSpell actionEarthSpell;
    ActionCycleWeapon actionCycleWeapon;

    protected override void OnRecoverFinished() {
        StartCoroutine(WaitForInput());
    }

    protected override void Awake()
    {
        base.Awake();
        actionHitDigOrMove = this.GetComponent<ActionHitDigOrMove>();
    }

    IEnumerator WaitForInput() {
        while (buffer.Count == 0)
            yield return null;

        PlayerInputKey key = buffer[0];
        buffer.RemoveAt(0);

        if (key == PlayerInputKey.up)
            DoAction(actionHitDigOrMove, IntVector2.up);
        else if (key == PlayerInputKey.right)
            DoAction(actionHitDigOrMove, IntVector2.right);
        else if (key == PlayerInputKey.down)
            DoAction(actionHitDigOrMove, IntVector2.down);
        else if (key == PlayerInputKey.left)
            DoAction(actionHitDigOrMove, IntVector2.left);
        //else if (key == PlayerInputKey.space)
        //    DoAction(actionEarthSpell, IntVector2.zero);
        //else if (key == PlayerInputKey.num1)
        //    DoAction(actionCycleWeapon, IntVector2.up);
        else
            StartCoroutine(WaitForInput());

    }

    //void DoActionIfExists(int i, IntVector2 direction) {
    //    if (i < GetActionCount())
    //        DoAction(i, direction);
    //    else
    //        StartCoroutine(WaitForInput());
    //}

    public void OnInput(PlayerInputKey key) {
        buffer.Add(key);
    }
}
