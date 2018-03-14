using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Item))]
[CanEditMultipleObjects]
public class ItemEditor : Editor {
	public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
		Item i = (Item)target;
		Sprite image = i.image;

		Texture2D preview = AssetPreview.GetAssetPreview(image);
		Texture2D final = new Texture2D(width, height);
		EditorUtility.CopySerialized(preview, final);

		return final;
	}
}
#endif

public class Item : ScriptableObject {
	public Sprite image;

	public virtual void OnEquip(GameObject owner) {
	}

	public virtual void OnUnequip(GameObject owner) {
	}
}
