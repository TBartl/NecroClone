using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour {

    public MoveAnimationData animationData;

    void Awake() {
        IntVectorPos pos = this.GetComponentInParent<IntVectorPos>();
        pos.onRealMove += OnRealMove;
        pos.onBumpMove += OnBump;
    }

    void OnRealMove(IntVector2 from, IntVector2 to) {
        StartCoroutine(JuicyMove(from, to));
    }
    void OnBump(IntVector2 from, IntVector2 to) {
        StartCoroutine(JuicyFailMove(from, to));
    }

    IEnumerator JuicyMove(IntVector2 from, IntVector2 to) {
        for (float t = 0; t < animationData.animLength; t += Time.deltaTime) {
            float p = t / animationData.animLength;
            Vector3 intermediatePos = Vector3.Lerp((Vector3)from, (Vector3)to, animationData.horizontal.Evaluate(p));
            intermediatePos.y = animationData.vertical.Evaluate(p);
            this.transform.position = intermediatePos;
            yield return null;
        }
        this.transform.position = (Vector3)to;
    }

    IEnumerator JuicyFailMove(IntVector2 from, IntVector2 to) {
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
