using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour {
	Equipped equipped;

	public GameObject characterWindow;

	public GameObject [] equipmentSlots = new GameObject [14];

	void Awake () {
		equipped = transform.root.gameObject.GetComponent<Equipped> ();
	}

	void Start () {
		equipped.onEquipmentChangeCallback += UpdateSlots;
		InitializeCharacterUI ();
	}

	void Update () {
		DetectInput ();
	}

	void InitializeCharacterSlots () {
		for (int i = 0; i < equipmentSlots.Length; i++) {
			equipmentSlots [i].GetComponent<UI_Character_Slot> ().SetSlotID (i);
		}
	}

	void InitializeCharacterUI () {
		InitializeCharacterSlots ();
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
