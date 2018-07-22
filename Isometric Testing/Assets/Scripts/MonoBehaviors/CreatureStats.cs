using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes { Physical, Fire, Water, Earth, Air, Light, Dark, True };
public enum EntityTypes { Player, NPC, Monster };

public class CreatureStats : MonoBehaviour {
	[Header("Entity Type")]
	[SerializeField] protected EntityTypes entityType;

	[Header("Speeds")]
	[SerializeField] protected int speedNormal;
	[SerializeField] protected int speedSprint;
	[SerializeField] protected float speedModifier;

	[Header("Combat")]
	[SerializeField] protected int armorValue;
	[SerializeField] protected float damageModifier;

	[Header("Health")]
	[SerializeField] protected int healthBase;
	[SerializeField] protected int healthBonusFlat;
	[SerializeField] protected float healthBonusModifier;
	[SerializeField] protected int healthMax;
	[SerializeField] protected int healthCurrent;
	[SerializeField] protected int healthRegenRate;

	[Header("Mana")]
	[SerializeField] protected int manaBase;
	[SerializeField] protected int manaBonusFlat;
	[SerializeField] protected float manaBonusModifier;
	[SerializeField] protected int manaMax;
	[SerializeField] protected int manaCurrent;
	[SerializeField] protected int manaRegenRate;

	[Header("Base Attributes")]
	[SerializeField] protected int baseStrength;
	[SerializeField] protected int baseConstitution;
	[SerializeField] protected int baseIntelligence;
	[SerializeField] protected int baseWisdom;
	[SerializeField] protected int baseDexterity;

	[Header("Attribute Modifiers")]
	[SerializeField] protected int modStrength;
	[SerializeField] protected int modConstitution;
	[SerializeField] protected int modIntelligence;
	[SerializeField] protected int modWisdom;
	[SerializeField] protected int modDexterity;

	[Header("Current Attributes")]
	[SerializeField] protected int currentStrength;
	[SerializeField] protected int currentConstitution;
	[SerializeField] protected int currentIntelligence;
	[SerializeField] protected int currentWisdom;
	[SerializeField] protected int currentDexterity;

	[Header("Resists")]
	[SerializeField] protected float globalResist;
	[SerializeField] protected float physicalResist;
	[SerializeField] protected float magicalResist;
	[SerializeField] protected float fireResist;
	[SerializeField] protected float waterResist;
	[SerializeField] protected float earthResist;
	[SerializeField] protected float airResist;
	[SerializeField] protected float lightResist;
	[SerializeField] protected float darkResist;

	void Start () {
		
	}

	public void UpdateStats (int armorVal, float damageMod, int hpFlat, float hpMod, int mpFlat, float mpMod, int flatSTR, int flatCON, int flatINT, int flatWIS, int flatDEX, float magicalRes) {
		armorValue = armorVal;
		damageModifier = damageMod;
		healthBonusFlat = hpFlat;
		healthBonusModifier = hpMod;
		manaBonusFlat = mpFlat;
		manaBonusModifier = mpMod;
		modStrength = flatSTR;
		modConstitution = flatCON;
		modIntelligence = flatINT;
		modWisdom = flatWIS;
		modDexterity = flatDEX;
		magicalResist = magicalRes;

		healthMax = Mathf.RoundToInt ((healthBase + healthBonusFlat) * (1f + healthBonusModifier));
		manaMax = Mathf.RoundToInt ((manaBase + manaBonusFlat) * (1f + manaBonusModifier));

		currentStrength = baseStrength + modStrength;
		currentConstitution = baseConstitution + modConstitution;
		currentIntelligence = baseIntelligence + modIntelligence;
		currentWisdom = baseWisdom + modWisdom;
		currentDexterity = baseDexterity + modDexterity;
	}

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
