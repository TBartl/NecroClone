using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ChangeableProperty {
    void Encode(ref BinaryWriter writer);
    void Decode(ref BinaryReader reader);
}
