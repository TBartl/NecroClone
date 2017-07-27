using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour {

    public MoveAnimationData animationData;
    IntTransform intTransform;

    void Awake() {
        intTransform = this.GetComponentInParent<IntTransform>();
    }

    public void Move(IntVector2 from, IntVector2 to) {
        StartCoroutine(JuicyMove(from, to));
    }
    public void Bump(IntVector2 target) {
        IntVector2 pos = intTransform.GetPos();
        StartCoroutine(JuicyBump(pos, target));
    }

    IEnumerator JuicyMove(IntVector2 from, IntVector2 to) {
        IntVector2 direction = to - from;
        foreach (RotateToDirection rotate in this.GetComponentsInChildren<RotateToDirection>())
            rotate.Rotate(direction);
        for (float t = 0; t < animationData.animLength; t += Time.deltaTime) {
            float p = t / animationData.animLength;
            Vector3 intermediatePos = Vector3.Lerp((Vector3)from, (Vector3)to, animationData.horizontal.Evaluate(p));
            intermediatePos.y = animationData.vertical.Evaluate(p);
            this.transform.position = intermediatePos;
            yield return null;
        }
        this.transform.position = (Vector3)to;
    }

    IEnumerator JuicyBump(IntVector2 from, IntVector2 to) {
        IntVector2 direction = to - from;
        foreach (RotateToDirection rotate in this.GetComponentsInChildren<RotateToDirection>())
            rotate.Rotate(direction);
        for (float t = 0; t < animationData.animLength; t += Time.deltaTime) {
            float p = t / animationData.animLength;
            Vector3 intermediatePos = Vector3.Lerp((Vector3)from, (Vector3)to, animationData.horizontalFail.Evaluate(p));
            intermediatePos.y = animationData.vertical.Evaluate(p);
            this.transform.position = intermediatePos;
            yield return null;
        }
        this.transform.position = (Vector3)from;
    }

}
