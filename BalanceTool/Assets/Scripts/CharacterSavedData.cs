using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CharacterSavedData
{
	/*------------------------------- Stats values (saved) -------------------------------*/

	// General
	[Header("General")]
	public string characterName;    // Name of the character
	public int totalHP;             // Total health points of the character
	public int armorHP;             // Health points which negates a percentage of the damage received
	public float moveSpeed;         // Max move speed of the character


	// Weapon
	[Header("Weapon")]

	public int damage;            // Damage dealt at each attack
	public float fireRate;          // Shoot rate in shot per seconds
	public int maxAmmo;             // Max weapon ammo
	public float reloadTime;        // Time needed to reload your weapon

	public bool isProj;             // True if the weapon shoot projectiles like rockets
	public float projSpeed;         // ONLY USED IF isProj == true ! // Determine the projectile speed
	public GameObject projPrefab;   // ONLY USED IF isProj == true ! // Determine the projectile to shoot
	public float spreadRadius;      // Radius representative of the shot circle used for spread
	public FireMode fireMode;       // Weapon FireMode


	// Spell
	[Header("Spell")]
	public float spellAtkPoints;   // Spell points gained when dealing damage
	public float spellTnkPoints;   // Spell points gained when receiving damage
	public float spellPointsTrigger;	// Spell points needed to trigger spell
}
