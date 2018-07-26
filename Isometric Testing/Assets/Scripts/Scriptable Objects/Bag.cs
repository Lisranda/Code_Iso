using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag", menuName = "Items/Equipment/Bag")]
public class Bag : Equipment {
	void Reset () {
		itemName = "New Bag";
		equipmentType = EquipmentType.Bag;
	}
}
