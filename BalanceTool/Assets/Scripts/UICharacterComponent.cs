using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterComponent : MonoBehaviour
{
	public
	InputField fileName_UIText;

	// General
	[Header("General")]
	public InputField characterName_UIText;
	public InputField totalHP_UIText;
	public InputField armorHP_UIText;
	public InputField moveSpeed_UIText;

	// Weapon
	[Header("Weapon")]
	public InputField damage_UIText;
	public InputField fireRate_UIText;
	public InputField maxAmmo_UIText;
	public InputField reloadTime_UIText;

	public Toggle isProj_UIText;
	public InputField projSpeed_UIText;
	public InputField projPrefab_UIText;
	public InputField spreadRadius_UIText;
	public Dropdown fireMode_UIDropDown;

	// Spell
	[Header("Spell")]
	public InputField spellAtkPoints_UIText;
	public InputField spellTnkPoints_UIText;
	public InputField spellPointsTrigger_UIText;

	// Private
	[Header("Private")]
	public InputField currentSpellCharge_UIText;
	public InputField currentHP_UIText;
	public Text currentAmmoCount;
}
