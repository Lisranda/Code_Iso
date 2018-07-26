﻿using System.Collections;
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

	public override bool UseItem (GameObject playerUsing) {
		Inventory inventory = playerUsing.GetComponent<Inventory> ();
		Equipped equipped = playerUsing.GetComponent<Equipped> ();
		if (equipped.Equip ((Item)this)) {
			inventory.Remove ((Item)this);
			return true;
		}
		return false;
	}

}
