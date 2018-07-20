﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Other };
public enum EquipmentType { Armor, Weapon, Bag };
public enum ArmorSlot { Head, Chest, Legs, Hands, Feet, Wrists, Ears, Neck, Fingers };
public enum WeaponSlot { TwoHand, MainHand, OneHand, OffHand, Ranged };

public class Item : ScriptableObject {
	[Header("Item Settings")]
	public string itemName = "New Item";
	public Sprite itemIcon = null;


	public int itemValue = 0;
	public int itemStackSize = 1;
	public ItemType itemType;
}