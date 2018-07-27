using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	Equipped equipped;
	CreatureStats stats;
	PawnInitializer pawnInitializer;

	public List<Item> items = new List<Item>();

	[SerializeField] GameObject itemPrefab;

	public int inventorySize;

	void Awake () {
		equipped = GetComponentInParent<Equipped> ();
		stats = GetComponentInParent<CreatureStats> ();
		pawnInitializer = GetComponentInParent<PawnInitializer> ();
	}

	void Start () {
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

	public void ChangeInventorySize () {
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
							Drop (i);
						}
						continue;
					}
					items [o] = items [i];
					Remove (i);
					break;
				}
			}

			for (int i = inventorySize; i < oldCount; oldCount--) {
				items.Remove (items [i]);
			}
		}
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
		pawnInitializer.onInventoryChangeCallback ();
		return true;
	}

	public void Remove (Item item) {
		int index = (items.IndexOf (item));
		items [index] = null;
		pawnInitializer.onInventoryChangeCallback ();
	}

	public void Remove (int inventoryIndex) {
		items [inventoryIndex] = null;
		pawnInitializer.onInventoryChangeCallback ();
	}

	void DetectInput () {
		if (Input.GetKeyDown ("e"))
			Pickup ();
	}

	public void Drop (Item item) {
	}

	public void Drop (int inventoryIndex) {
		Vector3 location = GetComponentInParent<PawnController> ().GetTileLocation ();
		GameObject newItem = Instantiate (itemPrefab, location, Quaternion.identity);
		newItem.GetComponent<ItemInteraction> ().item = items [inventoryIndex];
		newItem.name = items [inventoryIndex].itemName;
		Remove (inventoryIndex);
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
