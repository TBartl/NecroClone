using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveAnimationData : ScriptableObject {
    public float animLength;
    public AnimationCurve horizontal;
    public AnimationCurve vertical;
    public AnimationCurve horizontalFail;
}