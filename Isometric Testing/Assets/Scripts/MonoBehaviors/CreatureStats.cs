using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour {
	[SerializeField] protected int healthMax;
	[SerializeField] protected int manaMax;
	[SerializeField] protected int healthCurrent;
	[SerializeField] protected int manaCurrent;
	[SerializeField] protected int healthRegenRate;
	[SerializeField] protected int manaRegenRate;

	[SerializeField] protected int strength;
	[SerializeField] protected int constitution;
	[SerializeField] protected int intelligence;
	[SerializeField] protected int wisdom;
	[SerializeField] protected int dexterity;
}
