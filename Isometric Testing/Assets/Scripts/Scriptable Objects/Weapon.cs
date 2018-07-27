using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Equipment/Weapon")]
public class Weapon : Equipment {
	void Reset () {
		itemName = "New Weapon";
		equipmentType = EquipmentType.Weapon;
	}

	[Header("Weapon Settings")]
	public WeaponSlot weaponSlot;
}
