using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreatureStats : NetworkBehaviour {
	[SerializeField, SyncVar] protected int healthMax;
	[SerializeField, SyncVar] protected int manaMax;
	[SerializeField, SyncVar] protected int healthCurrent;
	[SerializeField, SyncVar] protected int manaCurrent;
	[SerializeField, SyncVar] protected int healthRegenRate;
	[SerializeField, SyncVar] protected int manaRegenRate;

	[SerializeField, SyncVar] protected int strength;
	[SerializeField, SyncVar] protected int constitution;
	[SerializeField, SyncVar] protected int intelligence;
	[SerializeField, SyncVar] protected int wisdom;
	[SerializeField, SyncVar] protected int dexterity;
}
