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
	public float damageModifier = 0;
	[Header("")]
	public int healthFlat = 0;
	public float healthModifier = 0f;
	[Header("")]
	public int manaFlat = 0;
	public float manaModifier = 0;
	[Header("")]
	public int flatSTR = 0;
	public int flatCON = 0;
	public int flatINT = 0;
	public int flatWIS = 0;
	public int flatDEX = 0;
	[Header("")]
	public float magicalResist = 0f;
	[Header("")]
	public int bagSize = 0;
	[Header("")]
	public int weaponDamage = 0;

	public override bool UseItem (GameObject playerUsing, int inventoryIndex = -1) {
//		Inventory inventory = playerUsing.GetComponent<Inventory> ();
		Equipped equipped = playerUsing.GetComponent<Equipped> ();
		if (equipped.Equip ((Item)this, inventoryIndex)) {
			return true;
		}
		return false;
	}

}
