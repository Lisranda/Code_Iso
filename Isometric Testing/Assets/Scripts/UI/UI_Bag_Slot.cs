using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bag_Slot : MonoBehaviour {
	GameObject player;
	Inventory inventory;

	public GameObject iconObject;
	public GameObject background;
	public Color defaultColor;
	public Color mouseOverColor;
	int slotID;

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

		if (inventory.items [slotID].UseItem (player, slotID)) {
			background.GetComponent<Image> ().color = defaultColor;
		}
	}

	public void OnMouseEnter () {
		if (inventory.items [slotID] == null)
			return;
		background.GetComponent<Image> ().color = mouseOverColor;
	}

	public void OnMouseExit () {
		background.GetComponent<Image> ().color = defaultColor;
	}
}
