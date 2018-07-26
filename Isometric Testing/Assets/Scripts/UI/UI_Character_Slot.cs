using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character_Slot : MonoBehaviour {
	GameObject player;
//	Inventory inventory;
	Equipped equipped;

	public GameObject iconObject;
	public GameObject background;
	public Color defaultColor;
	public Color mouseOverColor;
	public int slotID;

	void Awake () {
		player = transform.root.gameObject;
//		inventory = player.GetComponent<Inventory> ();
		equipped = player.GetComponent<Equipped> ();
	}

	public void SetSlotID (int newID) {
		slotID = newID;
	}

	public void RightClick () {
		if (equipped.equippedItems [slotID] == null)
			return;
		
		if (equipped.Unequip (slotID)) {
			background.GetComponent<Image> ().color = defaultColor;
		}
	}

	public void OnMouseEnter () {
		if (equipped.equippedItems [slotID] == null)
			return;
		background.GetComponent<Image> ().color = mouseOverColor;
	}

	public void OnMouseExit () {
		background.GetComponent<Image> ().color = defaultColor;
	}
}
