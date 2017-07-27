using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    List<PlayerInputKey> buffer = new List<PlayerInputKey>();

    ActionAutoHitDigOrMove hitDigOrMove;

    protected override void OnRecoverFinished() {
        StartCoroutine(WaitForInput());
    }

    protected override void Awake() {
        base.Awake();
        hitDigOrMove = new ActionAutoHitDigOrMove(this.gameObject);
    }

    IEnumerator WaitForInput() {
        while (buffer.Count == 0)
            yield return null;

        PlayerInputKey key = buffer[0];
        buffer.RemoveAt(0);

        if (key == PlayerInputKey.up)
            DoAction(hitDigOrMove.GetAction(IntVector2.up), IntVector2.up);
        else if (key == PlayerInputKey.right)
            DoAction(hitDigOrMove.GetAction(IntVector2.right), IntVector2.right);
        else if (key == PlayerInputKey.down)
            DoAction(hitDigOrMove.GetAction(IntVector2.down), IntVector2.down);
        else if (key == PlayerInputKey.left)
            DoAction(hitDigOrMove.GetAction(IntVector2.left), IntVector2.left);
        else {
            StartCoroutine(WaitForInput());
            yield break; //Break out to signify that valid input wasn't actually provided (don't call onPlayerAction;
        }

        intTransform.GetLevel().OnPlayerAction(this);

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
