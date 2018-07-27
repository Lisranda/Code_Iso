using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
[RequireComponent(typeof(Inventory))]
public class Equipped : MonoBehaviour {
	CreatureStats stats;
	Inventory inventory;
	PawnInitializer pawnInitializer;

	public int equipmentArraySize = 15;
	public Item [] equippedItems;

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

	int bagRef = 14;

	void Awake () {		
		inventory = GetComponentInParent<Inventory> ();
		stats = GetComponentInParent<CreatureStats> ();
		pawnInitializer = GetComponentInParent<PawnInitializer> ();
		equippedItems = new Item [equipmentArraySize];
	}

	void Start () {
	}		

	public bool Equip (Item item, int inventoryIndex = -1) {
		int slotRef = GetSlotFromEnum (item);

		if (slotRef == -1)
			return false;

		if (inventoryIndex == -1)
			return false;
		
		if (slotRef == leftFingerRef) {
			if (equippedItems [leftFingerRef] == null) {
				equippedItems [leftFingerRef] = item;
				inventory.Remove (inventoryIndex);
				pawnInitializer.onEquipmentChangeCallback ();
				return true;
			}
			
			if (equippedItems [rightFingerRef] == null) {
				equippedItems [rightFingerRef] = item;
				inventory.Remove (inventoryIndex);
				pawnInitializer.onEquipmentChangeCallback ();
				return true;
			}
			
			if (Swap (leftFingerRef, item, inventoryIndex)) {
				return true;
			}

			return false;
		}

		if (slotRef == bagRef) {
			if (equippedItems [bagRef] == null) {
				equippedItems [bagRef] = item;
				inventory.Remove (inventoryIndex);
				pawnInitializer.onEquipmentChangeCallback ();
				pawnInitializer.onInventorySizeChangeCallback ();
				return true;
			}

			if (Swap (bagRef, item, inventoryIndex)) {
				pawnInitializer.onInventorySizeChangeCallback ();
				return true;
			}

			return false;
		}
		
		if (equippedItems [slotRef] == null) {
			equippedItems [slotRef] = item;
			inventory.Remove (inventoryIndex);
			pawnInitializer.onEquipmentChangeCallback ();
			return true;
		}

		if (Swap (slotRef, item, inventoryIndex)) {
			return true;
		}

		return false;
	}

	public bool Swap (int slot, Item newItem, int inventoryIndex = -1) {
		if (equippedItems [slot] == null)
			return false;

		if (newItem.itemType != ItemType.Equipment)
			return false;

		if (inventoryIndex == -1)
			return false;

		Item swapItem = equippedItems [slot];
		equippedItems [slot] = newItem;
		inventory.Remove (inventoryIndex);
		inventory.Add (swapItem);
		pawnInitializer.onEquipmentChangeCallback ();
		return true;
	}

	public bool Unequip (int slot) {
		if (equippedItems [slot] == null)
			return false;

		if (inventory.Add (equippedItems [slot])) {
			equippedItems [slot] = null;
			pawnInitializer.onEquipmentChangeCallback ();

			if (slot == bagRef)
				pawnInitializer.onInventorySizeChangeCallback ();

			return true;
		}

		return false;
	}

	public void CalculateEquipmentBonuses () {
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
		int bagSize = 0;
		int weaponDamage = 0;

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
			bagSize += equipment.bagSize;
			weaponDamage += equipment.weaponDamage;
		}

		stats.UpdateStats (armorValue, damageModifier, healthFlat, healthModifier, manaFlat, manaModifier, flatSTR, flatCON, flatINT, flatWIS, flatDEX, magicalResist, bagSize, weaponDamage);
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

		if (equipment.equipmentType == EquipmentType.Bag)
			return bagRef;

		return -1;
	}
}
