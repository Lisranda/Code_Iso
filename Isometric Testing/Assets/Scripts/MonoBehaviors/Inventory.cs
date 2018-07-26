using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	Equipped equipped;
	CreatureStats stats;
	public List<Item> items = new List<Item>();

	public int inventorySize;

	public delegate void OnInventoryChange ();
	public OnInventoryChange onInventoryChangeCallback;

	public delegate void OnInventorySizeChange ();
	public OnInventorySizeChange onInventorySizeChangeCallback;

	void Awake () {
		equipped = GetComponentInParent<Equipped> ();
		stats = GetComponentInParent<CreatureStats> ();
	}

	void Start () {
		onInventorySizeChangeCallback += ChangeInventorySize;
	}

	void Update () {
		DetectInput ();
//		AutoEquipDebug ();
	}

	public void InitializeInventory () {
		inventorySize = stats.GetCurrentStorageSize ();
		for (int i = 0; i < inventorySize; i++) {
			items.Add (null);
		}
	}

	void ChangeInventorySize () {
		int oldCount = items.Count;
		inventorySize = stats.GetCurrentStorageSize ();

		if (inventorySize > oldCount) {
			for (int i = 0; i < inventorySize; i++) {
				if (i < oldCount)
					continue;
				
				items.Add (null);
			}
		}

		if (inventorySize < oldCount) {
			for (int i = inventorySize; i < oldCount; i++) {
				if (items [i] == null)
					continue;

				for (int o = 0; o < inventorySize; o++) {
					if (items [o] != null) {
						if (o == inventorySize - 1) {
							Debug.Log ("Inventory Full, Dropping: " + items [i].itemName);
							Remove (items [i]);
						}
						continue;
					}

					items [o] = items [i];
					break;
				}
			}

			for (int i = inventorySize; i < oldCount; i++) {
				items.Remove (items [i]);
			}
		}

		onInventoryChangeCallback ();
	}

	void AutoEquipDebug () {
		if (items.Count < 1)
			return;
		if (items [0] == null)
			return;
		if (equipped.Equip (items [0]))
			Remove (items [0]);
	}

	public bool Add (Item item) {
		if (inventorySize == 0) {
			Debug.Log ("Inventory Full: No Inventory");
			return false;
		}

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
		onInventoryChangeCallback ();
		return true;
	}

	public void Remove (Item item) {
		int index = (items.IndexOf (item));
		items [index] = null;
		onInventoryChangeCallback ();
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
