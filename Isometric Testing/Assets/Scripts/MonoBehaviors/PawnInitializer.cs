using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInitializer : MonoBehaviour {
	Inventory inventory;
	Equipped equipped;

	void Awake () {
		inventory = gameObject.GetComponentInParent<Inventory> ();
		equipped = gameObject.GetComponentInParent<Equipped> ();
	}

	void Start () {
		equipped.CalculateEquipmentBonuses ();
		inventory.InitializeInventory ();
	}
}
