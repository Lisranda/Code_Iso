using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped : MonoBehaviour {
	Inventory inventory;

	[Header("Armor Slots")]
	[SerializeField] Armor[] armorSlots = new Armor[10];
	int headRef = 0;
	int chestRef = 1;
	int legsRef = 2;
	int handsRef = 3;
	int feetRef = 4;
	int wristsRef = 5;
	int earsRef = 6;
	int neckRef = 7;
	int leftFingerRef = 8;
	int rightFingerRef = 9;

	[Header("Weapon Slots")]
	[SerializeField] Weapon[] weaponSlots = new Weapon[3];
	int mainHandRef = 0;
	int offHandRef = 1;
	int rangedRef = 2;

	void Awake () {
		inventory = GetComponentInParent<Inventory> ();
	}

	public void Equip (Armor armor) {
		int slotRef = GetSlotFromEnum (armor);
		if (slotRef != -1) {
			if (slotRef == leftFingerRef) {
				if (armorSlots [leftFingerRef] == null) {
					armorSlots [leftFingerRef] = armor;
				} else if (armorSlots [rightFingerRef] == null) {
					armorSlots [rightFingerRef] = armor;
				} else {
					armorSlots [leftFingerRef] = armor;
				}
			}
			armorSlots [slotRef] = armor;
		}
	}

	public void Equip (Weapon weapon) {
		int slotRef = GetSlotFromEnum (weapon);
		if (slotRef != -1) {
			weaponSlots [slotRef] = weapon;
		}
	}

	public void UnequipArmor (int slot) {
		if (armorSlots [slot] != null) {
			if (inventory.Add ((Item)armorSlots [slot])) {
				armorSlots [slot] = null;
			}
		}
	}

	public void UnequipWeapon (int slot) {
		if (weaponSlots [slot] != null) {
			if (inventory.Add ((Item)weaponSlots [slot])) {
				weaponSlots [slot] = null;
			}
		}
	}

	int GetSlotFromEnum (Armor armor) {
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
		return -1;
	}

	int GetSlotFromEnum (Weapon weapon) {
		if (weapon.weaponSlot == WeaponSlot.MainHand)
			return mainHandRef;
		if (weapon.weaponSlot == WeaponSlot.OffHand)
			return offHandRef;
		if (weapon.weaponSlot == WeaponSlot.Ranged)
			return rangedRef;
		return -1;
	}
}
