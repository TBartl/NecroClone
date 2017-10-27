using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHead : MonoBehaviour, IOnHit {

	Killable killable;
	public MeshRenderer head;

	void Awake() {
		killable = this.GetComponent<Killable>();
	}
	public void OnHit(HitInfo info) {
		if (killable.GetHealth() <= 1) {
			Destroy(this.GetComponent<EnemyChaseController>());
			Destroy(head);
			EnemyFleeController newController = this.gameObject.AddComponent<EnemyFleeController>();
			newController.direction = info.direction;
		}
	}
}
