using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes { Physical, Fire, Water, Earth, Air, Light, Dark, True };
public enum EntityTypes { Player, NPC, Monster };

public class CreatureStats : MonoBehaviour {
	[SerializeField] protected EntityTypes entityType;
	[SerializeField] protected int speedNormal;
	[SerializeField] protected int speedSprint;

	[SerializeField] protected float attackDamage;

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

	[SerializeField] protected float globalResist;
	[SerializeField] protected float physicalResist;
	[SerializeField] protected float magicalResist;
	[SerializeField] protected float fireResist;
	[SerializeField] protected float waterResist;
	[SerializeField] protected float earthResist;
	[SerializeField] protected float airResist;
	[SerializeField] protected float lightResist;
	[SerializeField] protected float darkResist;

	public int GetSpeedNormal () {
		return speedNormal;
	}

	public int GetSpeedSprint () {
		return speedSprint;
	}

	public void ApplyDamage (float damageAmount, DamageTypes damageType) {
		float appliedResist = 0f;
		if (damageType == DamageTypes.Physical)
			appliedResist = globalResist + physicalResist;
		else if (damageType == DamageTypes.Fire)
			appliedResist = globalResist + magicalResist + fireResist;
		else if (damageType == DamageTypes.Water)
			appliedResist = globalResist + magicalResist + waterResist;
		else if (damageType == DamageTypes.Earth)
			appliedResist = globalResist + magicalResist + earthResist;
		else if (damageType == DamageTypes.Air)
			appliedResist = globalResist + magicalResist + airResist;
		else if (damageType == DamageTypes.Light)
			appliedResist = globalResist + magicalResist + lightResist;
		else if (damageType == DamageTypes.Dark)
			appliedResist = globalResist + magicalResist + darkResist;
		else if (damageType == DamageTypes.True)
			appliedResist = globalResist;

		float resistModifier = Mathf.Clamp (1f - appliedResist, 0f, float.MaxValue);

		int actualDamage = Mathf.Clamp (Mathf.RoundToInt (damageAmount * resistModifier), 0, int.MaxValue);

		healthCurrent -= actualDamage;
		Debug.Log ("Dealt " + actualDamage + " damage.");

		if (healthCurrent <= 0)
			Die ();
	}

	protected void Die () {
		if (entityType == EntityTypes.Player) {
			Debug.Log ("The player received enough damage to die.  HP Reset.");
			healthCurrent = healthMax;
			return;
		}

		if (entityType == EntityTypes.NPC) {
			Debug.Log ("The NPC received enough damage to die.  HP Reset.");
			healthCurrent = healthMax;
			return;
		}

		if (entityType == EntityTypes.Monster) {
			Debug.Log ("The MONSTER received enough damage to die.");
			Destroy (this.gameObject);
			return;
		}
	}
}
