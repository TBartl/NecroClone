using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour {
	public Item item;

	public void SetItem(Item item) {
		this.item = item;
		OnValidate();
	}

	void OnValidate() {
		this.GetComponentInChildren<SpriteRenderer>().sprite = item.image;
	}
}
