using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Other };
public enum EquipmentType { Armor, Weapon, Bag };
public enum ArmorSlot { Head, Chest, Legs, Hands, Feet, Wrists, Ears, Neck, Fingers, Waist };
public enum WeaponSlot { TwoHand, MainHand, OneHand, OffHand, Ranged };

public class Item : ScriptableObject {
	[Header("Item Settings")]
	public string itemName = "New Item";
	public Sprite itemIcon = null;


	public int itemValue = 0;
	public int itemStackSize = 1;
	public ItemType itemType;

	public virtual bool UseItem (GameObject playerUsing) {
		Debug.Log ("Using Item: " + itemName);
		return true;
	}
}
