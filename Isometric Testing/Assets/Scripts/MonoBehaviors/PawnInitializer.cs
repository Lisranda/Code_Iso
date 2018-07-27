using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInitializer : MonoBehaviour {
	Inventory inventory;
	Equipped equipped;
	[SerializeField] UI_Bag uiBag;
	[SerializeField] UI_Character uiCharacter;

	public delegate void OneEquipmentChange ();
	public OneEquipmentChange onEquipmentChangeCallback;

	public delegate void OnInventoryChange ();
	public OnInventoryChange onInventoryChangeCallback;

	public delegate void OnInventorySizeChange ();
	public OnInventorySizeChange onInventorySizeChangeCallback;

	void Awake () {
		inventory = gameObject.GetComponentInParent<Inventory> ();
		equipped = gameObject.GetComponentInParent<Equipped> ();
	}

	void Start () {
		onEquipmentChangeCallback += equipped.CalculateEquipmentBonuses;
		onEquipmentChangeCallback += uiCharacter.UpdateSlots;

		onInventorySizeChangeCallback += inventory.ChangeInventorySize;
		onInventorySizeChangeCallback += uiBag.SetBagDimensions;
		onInventorySizeChangeCallback += uiBag.PopulateSlots;

		onInventoryChangeCallback += uiBag.UpdateSlots;

		equipped.CalculateEquipmentBonuses ();
		inventory.InitializeInventory ();
	}
}
