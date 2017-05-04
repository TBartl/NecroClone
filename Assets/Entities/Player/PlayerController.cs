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
            DoAction(1, IntVector2.zero);
        else
            StartCoroutine(WaitForInput());

    }

    public void OnInput(PlayerInputKey key) {
        buffer.Add(key);
    }
}
