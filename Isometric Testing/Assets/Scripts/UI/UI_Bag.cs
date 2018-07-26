using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bag : MonoBehaviour {
	Inventory inventory;

	[SerializeField] GameObject bagWindow;
	[SerializeField] GameObject bagSlotPrefab;

	[SerializeField] int slotsPerRow = 5;

	[SerializeField] List<GameObject> bagSlots = new List<GameObject> ();

	void Awake () {
		inventory = transform.root.gameObject.GetComponent<Inventory> ();
		inventory.onInventorySizeChangeCallback += SetBagDimensions;
		inventory.onInventorySizeChangeCallback += PopulateSlots;
		inventory.onInventoryChangeCallback += UpdateSlots;
	}

	void Start () {
		InitializeBagUI ();
	}

	void Update () {
		DetectInput ();
	}

	void DetectInput () {
		if (Input.GetKeyDown ("i"))
			ToggleInventory ();
	}

	void ToggleInventory () {
		if (bagWindow.activeInHierarchy)
			bagWindow.SetActive (false);
		else
			bagWindow.SetActive (true);
	}

	void PopulateSlots () {
		bagSlots.Clear ();
		int numberOfSlots = inventory.inventorySize;
		Debug.Log ("Making " + numberOfSlots + " slots.");

		for (int i = 0; i < numberOfSlots; i++) {
			GameObject slot = Instantiate (bagSlotPrefab, bagWindow.transform);
			slot.GetComponent<UI_Bag_Slot> ().SetSlotID (i);
			bagSlots.Add (slot);
		}
	}

	void InitializeBagUI () {
		SetBagDimensions ();
		PopulateSlots ();
		UpdateSlots ();

		if (bagWindow.activeInHierarchy)
			bagWindow.SetActive (false);
	}

	void SetBagDimensions () {
		GridLayoutGroup gridLayoutGroup = bagWindow.GetComponent<GridLayoutGroup> ();

		float bagSlotHeight = gridLayoutGroup.cellSize.y;
		float bagSlotWidth = gridLayoutGroup.cellSize.x;

		float bagSlotSpacingX = gridLayoutGroup.spacing.x;
		float bagSlotSpacingY = gridLayoutGroup.spacing.y;

		int bagPaddingLeft = gridLayoutGroup.padding.left;
		int bagPaddingRight = gridLayoutGroup.padding.right;
		int bagPaddingTop = gridLayoutGroup.padding.top;
		int bagPaddingBottom = gridLayoutGroup.padding.bottom;
		
		int numberOfSlots = inventory.inventorySize;
		int numberOfRows = Mathf.CeilToInt ((float)numberOfSlots / (float)slotsPerRow);

		float bagHeight = ((float)numberOfRows * bagSlotHeight) + (((float)numberOfRows - 1f) * bagSlotSpacingY) + ((float)bagPaddingLeft + (float)bagPaddingRight);
		float bagWidth = ((float)slotsPerRow * bagSlotWidth) + (((float)slotsPerRow - 1f) * bagSlotSpacingX) + ((float)bagPaddingBottom + (float)bagPaddingTop);

		Vector2 bagSizeVector2 = new Vector2 (bagWidth, bagHeight);

		RectTransform bagWindowRectTransform = bagWindow.GetComponent<RectTransform> ();
		bagWindowRectTransform.sizeDelta = bagSizeVector2;
	}

	void UpdateSlots () {
		for (int i = 0; i < inventory.items.Count; i++) {
			if (inventory.items [i] == null) {
				bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.GetComponent<Image> ().sprite = null;
				bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.SetActive (false);
				continue;
			}

			if (inventory.items [i].itemIcon == null) {
				bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.GetComponent<Image> ().sprite = null;
				bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.SetActive (true);
				continue;
			}

			bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.GetComponent<Image> ().sprite = inventory.items [i].itemIcon;
			bagSlots [i].GetComponent<UI_Bag_Slot> ().iconObject.SetActive (true);
		}
	}
}
