using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
[RequireComponent(typeof(Inventory))]
public class Equipped : MonoBehaviour {
	CreatureStats stats;
	Inventory inventory;

	public Item [] equippedItems = new Item [14];

	int headRef = 0;
	int chestRef = 1;
	int handsRef = 2;
	int legsRef = 3;
	int feetRef = 4;
	int waistRef = 5;
	int earsRef = 6;
	int neckRef = 7;
	int wristsRef = 8;
	int leftFingerRef = 9;
	int rightFingerRef = 10;

	int mainHandRef = 11;
	int offHandRef = 12;
	int rangedRef = 13;

	public delegate void OneEquipmentChange ();
	public OneEquipmentChange onEquipmentChangeCallback;

	void Awake () {
		inventory = GetComponentInParent<Inventory> ();
		stats = GetComponentInParent<CreatureStats> ();
		onEquipmentChangeCallback += CalculateEquipmentBonuses;
	}

	void Start () {
		CalculateEquipmentBonuses ();
	}		

	public bool Equip (Item item) {
		int slotRef = GetSlotFromEnum (item);

		if (slotRef == -1)
			return false;
		
		if (slotRef == leftFingerRef) {
			if (equippedItems [leftFingerRef] == null) {
				equippedItems [leftFingerRef] = item;
				onEquipmentChangeCallback ();
				return true;
			}
			
			if (equippedItems [rightFingerRef] == null) {
				equippedItems [rightFingerRef] = item;
				onEquipmentChangeCallback ();
				return true;
			}
			
			if (Unequip (leftFingerRef)) {
				equippedItems [leftFingerRef] = item;
				onEquipmentChangeCallback ();
				return true;
			}

			return false;
		}
		
		if (equippedItems [slotRef] == null) {
			equippedItems [slotRef] = item;
			onEquipmentChangeCallback ();
			return true;
		}

		if (Unequip (slotRef)) {
			equippedItems [slotRef] = item;
			onEquipmentChangeCallback ();
			return true;
		}

		return false;
	}

	public bool Unequip (int slot) {
		if (equippedItems [slot] == null)
			return false;

		if (inventory.Add (equippedItems [slot])) {
			equippedItems [slot] = null;
			onEquipmentChangeCallback ();
			return true;
		}

		return false;
	}

	void CalculateEquipmentBonuses () {
		int armorValue = 0;
		float damageModifier = 0f;
		int healthFlat = 0;
		float healthModifier = 0f;
		int manaFlat = 0;
		float manaModifier = 0;
		int flatSTR = 0;
		int flatCON = 0;
		int flatINT = 0;
		int flatWIS = 0;
		int flatDEX = 0;
		float magicalResist = 0f;

		foreach (Item item in equippedItems) {
			if (item == null)
				continue;
			
			Equipment equipment = (Equipment)item;

			armorValue += equipment.armorValue;
			damageModifier += equipment.damageModifier;
			healthFlat += equipment.healthFlat;
			healthModifier += equipment.healthModifier;
			manaFlat += equipment.manaFlat;
			manaModifier += equipment.manaModifier;
			flatSTR += equipment.flatSTR;
			flatCON += equipment.flatCON;
			flatINT += equipment.flatINT;
			flatWIS += equipment.flatWIS;
			flatDEX += equipment.flatDEX;
			magicalResist += equipment.magicalResist;
		}

		stats.UpdateStats (armorValue, damageModifier, healthFlat, healthModifier, manaFlat, manaModifier, flatSTR, flatCON, flatINT, flatWIS, flatDEX, magicalResist);
	}

	int GetSlotFromEnum (Item item) {
		Equipment equipment;
		Armor armor;
		Weapon weapon;

		if (item.itemType != ItemType.Equipment) {
			Debug.Log ("Trying to equip an item that is not equipment.");
			return -1;
		}

		equipment = (Equipment)item;

		if (equipment.equipmentType == EquipmentType.Armor) {
			armor = (Armor)equipment;
			if (armor.armorSlot == ArmorSlot.Head)
				return headRef;
			if (armor.armorSlot == ArmorSlot.Chest)
				return chestRef;
			if (armor.armorSlot == ArmorSlot.Legs)
				return legsRef;
			if (armor.armorSlot == ArmorSlot.Hands)
				return handsRef;
			if (armor.armorSlot == ArmorSlot.Feet)
				return feetRef;
			if (armor.armorSlot == ArmorSlot.Wrists)
				return wristsRef;
			if (armor.armorSlot == ArmorSlot.Ears)
				return earsRef;
			if (armor.armorSlot == ArmorSlot.Neck)
				return neckRef;
			if (armor.armorSlot == ArmorSlot.Fingers)
				return leftFingerRef;
			if (armor.armorSlot == ArmorSlot.Waist)
				return waistRef;
			return -1;
		}

		if (equipment.equipmentType == EquipmentType.Weapon) {
			weapon = (Weapon)equipment;
			if (weapon.weaponSlot == WeaponSlot.MainHand)
				return mainHandRef;
			if (weapon.weaponSlot == WeaponSlot.OffHand)
				return offHandRef;
			if (weapon.weaponSlot == WeaponSlot.Ranged)
				return rangedRef;
			return -1;
		}

		return -1;
	}
}
