using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour {
	Equipped equipped;

	[SerializeField] GameObject characterWindow;
	[SerializeField] GameObject leftSlots;
	[SerializeField] GameObject rightSlots;
	[SerializeField] GameObject weaponSlots;
	[SerializeField] GameObject slotPrefab;

	GameObject [] equipmentSlots;

	void Awake () {		
		equipped = transform.root.gameObject.GetComponent<Equipped> ();
		equipmentSlots = new GameObject [equipped.equipmentArraySize];
	}

	void Start () {
		equipped.onEquipmentChangeCallback += UpdateSlots;
		InitializeCharacterUI ();
	}

	void Update () {
		DetectInput ();
	}

	void PopulateCharacterSlots () {
		for (int i = 0; i < equipmentSlots.Length; i++) {
			if (i < 6) {
				GameObject slot = Instantiate (slotPrefab, leftSlots.transform);
				slot.GetComponent<UI_Character_Slot> ().SetSlotID (i);
				equipmentSlots [i] = slot;
				continue;
			}

			if (i < 11) {
				GameObject slot = Instantiate (slotPrefab, rightSlots.transform);
				slot.GetComponent<UI_Character_Slot> ().SetSlotID (i);
				equipmentSlots [i] = slot;
				continue;
			}

			if (i > 10) {
				GameObject slot = Instantiate (slotPrefab, weaponSlots.transform);
				slot.GetComponent<UI_Character_Slot> ().SetSlotID (i);
				equipmentSlots [i] = slot;
				continue;
			}
		}
	}

	void InitializeCharacterUI () {
		PopulateCharacterSlots ();
		UpdateSlots ();

		if (characterWindow.activeInHierarchy)
			characterWindow.SetActive (false);
	}

	void DetectInput () {
		if (Input.GetKeyDown ("c"))
			ToggleCharacterUI ();	
	}

	void ToggleCharacterUI () {
		if (characterWindow.activeInHierarchy)
			characterWindow.SetActive (false);
		else
			characterWindow.SetActive (true);
	}

	void UpdateSlots () {
		for (int i = 0; i < equipmentSlots.Length; i++) {			
			if (equipped.equippedItems [i] == null) {
				equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.GetComponent<Image> ().sprite = null;
				equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.SetActive (false);
				continue;
			}

			if (equipped.equippedItems [i].itemIcon == null) {
				equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.GetComponent<Image> ().sprite = null;
				equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.SetActive (true);
				continue;
			}

			equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.GetComponent<Image> ().sprite = equipped.equippedItems [i].itemIcon;
			equipmentSlots [i].GetComponent<UI_Character_Slot> ().iconObject.SetActive (true);
		}
	}
}
