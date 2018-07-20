using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item {
	void Reset () {
		itemType = ItemType.Equipment;
	}

	[Header("Equipment Settings")]
	public EquipmentType equipmentType;
	public int armorValue = 0;
	public int attackDamage = 0;
}
