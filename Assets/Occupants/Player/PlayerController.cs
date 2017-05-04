using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    List<PlayerInputKey> buffer = new List<PlayerInputKey>();

    protected override void OnRecoverFinished() {
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput() {
        while (buffer.Count == 0)
            yield return null;

        PlayerInputKey key = buffer[0];
        buffer.RemoveAt(0);

        if (key == PlayerInputKey.up)
            DoAction(0, IntVector2.up);
        else if (key == PlayerInputKey.right)
            DoAction(0, IntVector2.right);
        else if (key == PlayerInputKey.down)
            DoAction(0, IntVector2.down);
        else if (key == PlayerInputKey.left)
            DoAction(0, IntVector2.left);
        else if (key == PlayerInputKey.space)
            DoActionIfExists(1, IntVector2.zero);
        else if (key == PlayerInputKey.num1)
            DoActionIfExists(2, IntVector2.up);
        else if (key == PlayerInputKey.num2)
            DoActionIfExists(3, IntVector2.up);
        else if (key == PlayerInputKey.num3)
            DoActionIfExists(4, IntVector2.up);
        else if (key == PlayerInputKey.num4)
            DoActionIfExists(5, IntVector2.up);
        else if (key == PlayerInputKey.num5)
            DoActionIfExists(6, IntVector2.up);
        else
            StartCoroutine(WaitForInput());

    }

    void DoActionIfExists(int i, IntVector2 direction) {
        if (i < GetActionCount())
            DoAction(i, direction);
        else
            StartCoroutine(WaitForInput());
    }

    public void OnInput(PlayerInputKey key) {
        buffer.Add(key);
    }
}
