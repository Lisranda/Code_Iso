using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	Equipped equipped;
	public List<Item> items = new List<Item>();

	public int inventorySize = 10;

	void Awake () {
		equipped = GetComponentInParent<Equipped> ();
		InitializeInventory ();
	}

	void Start () {
		
	}

	void Update () {
		DetectInput ();
		AutoEquipDebug ();
	}

	void InitializeInventory () {
		for (int i = 0; i < inventorySize; i++) {
			items.Add (null);
		}
	}

	void AutoEquipDebug () {
		if (items.Count > 0) {
			if (items [0] != null) {
				if (items [0].GetType ().ToString() == "Weapon") {
					equipped.Equip ((Weapon)items [0]);
					Remove (items [0]);
					return;
				}
				if (items [0].GetType ().ToString() == "Armor") {
					equipped.Equip ((Armor)items [0]);
					Remove (items [0]);
					return;
				}
			}
		}
	}

	public bool Add (Item item) {
		for (int i = 0; i < inventorySize; i++) {
			if (items [i] != null) {
				if (i == inventorySize - 1) {
					Debug.Log ("Inventory Full");
					return false;
				}
				continue;
			}

			items [i] = item;
			break;
		}
		return true;

//		if (items.Count < inventorySize) {
//			items.Add (item);
//			Debug.Log ("Added " + item.itemName);
//			return true;
//		} else {
//			Debug.Log ("Inventory Full");
//			return false;
//		}	

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
		if (Physics.Raycast (transform.position + mod, transform.forward, out hit, 1f)) {			
			if (hit.transform.gameObject.GetComponentInParent<ItemInteraction> () != null) {
				
					GameObject go = hit.transform.gameObject;
					Item item = go.GetComponentInParent<ItemInteraction> ().item;
				if (Add (item)) {
					Destroy (go);					
				}
			}
		} else
			Debug.Log ("Tried to pick up, but nothing found.");
	}

}
