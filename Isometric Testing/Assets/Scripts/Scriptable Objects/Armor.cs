using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Equipment/Armor")]
public class Armor : Equipment {
	void Reset () {
		itemName = "New Armor";
		equipmentType = EquipmentType.Armor;
	}

	[Header("Armor Settings")]
	public ArmorSlot armorSlot;
}
