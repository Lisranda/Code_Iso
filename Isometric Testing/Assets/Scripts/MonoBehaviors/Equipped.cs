using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
[RequireComponent(typeof(Inventory))]
public class Equipped : MonoBehaviour {
	CreatureStats stats;
	Inventory inventory;

	[Header("Armor Slots")]
	[SerializeField] Armor[] armorSlots = new Armor[11];
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
	int waistRef = 10;

	[Header("Weapon Slots")]
	[SerializeField] Weapon[] weaponSlots = new Weapon[3];
	int mainHandRef = 0;
	int offHandRef = 1;
	int rangedRef = 2;

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

	public void Equip (Armor armor) {
		int slotRef = GetSlotFromEnum (armor);
		if (slotRef != -1) {
			if (slotRef == leftFingerRef) {
				if (armorSlots [leftFingerRef] == null) {
					armorSlots [leftFingerRef] = armor;
					onEquipmentChangeCallback ();
				} else if (armorSlots [rightFingerRef] == null) {
					armorSlots [rightFingerRef] = armor;
					onEquipmentChangeCallback ();
				} else {
					if (UnequipArmor (leftFingerRef)) {
						armorSlots [leftFingerRef] = armor;
						onEquipmentChangeCallback ();
					}
				}
			} else {
				if (armorSlots [slotRef] == null) {
					armorSlots [slotRef] = armor;
					onEquipmentChangeCallback ();
				} else {
					if (UnequipArmor (slotRef)) {
						armorSlots [slotRef] = armor;
						onEquipmentChangeCallback ();
					}
				}
			}
		}
	}

	public void Equip (Weapon weapon) {
		int slotRef = GetSlotFromEnum (weapon);
		if (slotRef != -1) {
			if (weaponSlots [slotRef] == null) {
				weaponSlots [slotRef] = weapon;
				onEquipmentChangeCallback ();
			} else {
				if (UnequipWeapon (slotRef)) {
					weaponSlots [slotRef] = weapon;
					onEquipmentChangeCallback ();
				}
			}
		}
	}

	public bool UnequipArmor (int slot) {
		if (armorSlots [slot] != null) {
			if (inventory.Add ((Item)armorSlots [slot])) {
				armorSlots [slot] = null;
				onEquipmentChangeCallback ();
				return true;
			}
		}
		return false;
	}

	public bool UnequipWeapon (int slot) {
		if (weaponSlots [slot] != null) {
			if (inventory.Add ((Item)weaponSlots [slot])) {
				weaponSlots [slot] = null;
				onEquipmentChangeCallback ();
				return true;
			}
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

		foreach (Armor armor in armorSlots) {
			if (armor != null) {
				armorValue += armor.armorValue;
				damageModifier += armor.damageModifier;
				healthFlat += armor.healthFlat;
				healthModifier += armor.healthModifier;
				manaFlat += armor.manaFlat;
				manaModifier += armor.manaModifier;
				flatSTR += armor.flatSTR;
				flatCON += armor.flatCON;
				flatINT += armor.flatINT;
				flatWIS += armor.flatWIS;
				flatDEX += armor.flatDEX;
				magicalResist += armor.magicalResist;
			}
		}

		foreach (Weapon weapon in weaponSlots) {
			if (weapon != null) {
				armorValue += weapon.armorValue;
				damageModifier += weapon.damageModifier;
				healthFlat += weapon.healthFlat;
				healthModifier += weapon.healthModifier;
				manaFlat += weapon.manaFlat;
				manaModifier += weapon.manaModifier;
				flatSTR += weapon.flatSTR;
				flatCON += weapon.flatCON;
				flatINT += weapon.flatINT;
				flatWIS += weapon.flatWIS;
				flatDEX += weapon.flatDEX;
				magicalResist += weapon.magicalResist;
			}
		}

		stats.UpdateStats (armorValue, damageModifier, healthFlat, healthModifier, manaFlat, manaModifier, flatSTR, flatCON, flatINT, flatWIS, flatDEX, magicalResist);
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
		if (armor.armorSlot == ArmorSlot.Waist)
			return waistRef;
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
