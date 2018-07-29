using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bag_Slot : MonoBehaviour {
	GameObject player;
	Inventory inventory;

	GameObject draggedItem;

	public GameObject iconObject;
	public GameObject background;
	public Color defaultColor;
	public Color mouseOverColor;
	int slotID;

	static bool isDragging = false;
	static UI_Bag_Slot currentMouseOverSlot;

	void Awake () {
		player = transform.root.gameObject;
		inventory = player.GetComponent<Inventory> ();
	}

	public void SetSlotID (int newID) {
		slotID = newID;
	}

	public void RightClick () {
		if (inventory.items [slotID] == null)
			return;

		if (Input.GetKey ("left ctrl")) {
			inventory.Drop (slotID);
			return;
		}

		if (inventory.items [slotID].UseItem (player, slotID)) {
			background.GetComponent<Image> ().color = defaultColor;
		}
	}

	public void OnMouseEnter () {
		currentMouseOverSlot = this;

		if (isDragging) {
			background.GetComponent<Image> ().color = mouseOverColor;
			return;
		}

		if (inventory.items [slotID] == null)
			return;
		
		background.GetComponent<Image> ().color = mouseOverColor;
	}

	public void OnMouseExit () {
		background.GetComponent<Image> ().color = defaultColor;
		currentMouseOverSlot = null;
	}

	public void OnBeginDrag () {
		if (draggedItem != null)
			return;

		if (inventory.items [slotID] == null)
			return;
		
		draggedItem = Instantiate (iconObject, Input.mousePosition, Quaternion.identity, transform.parent.parent.parent);
		isDragging = true;
	}

	public void OnDrag () {
		if (draggedItem == null)
			return;

		draggedItem.transform.position = Input.mousePosition;
	}

	public void OnEndDrag () {
		if (draggedItem == null)
			return;

		Destroy (draggedItem);
		isDragging = false;

		if (currentMouseOverSlot != null) {
			if (inventory.items [currentMouseOverSlot.slotID] == null) {
				if (inventory.Insert (inventory.items [slotID], currentMouseOverSlot.slotID))
					inventory.Remove (slotID);
				return;
			}
			currentMouseOverSlot.background.GetComponent<Image> ().color = defaultColor;
		}
	}

	void OnDisable () {
		background.GetComponent<Image> ().color = defaultColor;
		isDragging = false;
		currentMouseOverSlot = null;
		if (draggedItem != null)
			Destroy (draggedItem);
	}
}
