using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	PawnController controller;
	public List<Item> items = new List<Item>();

	public int inventorySize = 10;

	void Awake () {
		controller = GetComponentInParent<PawnController> ();
	}

	void Update () {
		DetectInput ();
	}

	public void Add (Item item) {
		items.Add (item);
	}

	public void Remove (Item item) {
		items.Remove (item);
	}

	void DetectInput () {
		if (Input.GetKeyDown ("e"))
			Pickup ();
	}

	void Pickup () {
		RaycastHit hit;
		Vector3 mod = new Vector3 (0f, 0.1f, 0f);
		if (Physics.Raycast (transform.position + mod, controller.GetForwardVector3 (), out hit, 1f)) {			
			if (hit.transform.gameObject.GetComponentInParent<ItemInteraction> () != null) {
				if (items.Count < inventorySize) {
					GameObject go = hit.transform.gameObject;
					Item item = go.GetComponentInParent<ItemInteraction> ().item;
					Add (item);
					Destroy (go);
					Debug.Log ("Added " + item.itemName);

				} else
					Debug.Log ("Inventory Full");
			}
		} else
			Debug.Log ("Tried to pick up, but nothing found.");
	}

}
