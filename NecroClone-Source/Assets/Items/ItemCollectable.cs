using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour {
	public Item item;

	void OnValidate() {
		this.GetComponentInChildren<SpriteRenderer>().sprite = item.image;
	}
}
