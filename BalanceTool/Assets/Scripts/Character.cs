using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FireMode
{
	FULL_AUTO,
	SEMI_AUTO,
	MANUAL,
	NB_FireMode
};

// Base character class, used for the player and the enemy

public class Character : MonoBehaviour
{
	[SerializeField] protected string fileName;                // Name of the characterFile

	public CharacterSavedData data;

	[Header("Weapon")]

	[SerializeField] GameObject gun = null;

	/*------------------------------- Members values (not saved) -------------------------------*/

	protected int currentHP;

	protected int currentArmorHP;

	public float currentSpellCharge;

	protected float currentShootCooldown;

	protected int currAmmo;

	protected bool isReloading = false;

	protected float currTimerReload = 0.0f;


	/*------------------------------- Other members -------------------------------*/

	protected UICharacterComponent UIcc;

	protected TextAsset JSONtxt;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		UIStart();

		//LoadData();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		UIUpdate();
		UpdateCharacterFromUI();

		currentSpellCharge += Time.deltaTime;

		currentShootCooldown += Time.deltaTime;
	}

	// Custom functions

	protected void UpdateCharacterFromUI()
	{
		fileName = UIcc.fileName_UIText.text;

		data.characterName = UIcc.characterName_UIText.text;

		int.TryParse(UIcc.totalHP_UIText.text, out data.totalHP);

		int.TryParse(UIcc.armorHP_UIText.text, out data.armorHP);

		float.TryParse(UIcc.moveSpeed_UIText.text, out data.moveSpeed);

		int.TryParse(UIcc.damage_UIText.text, out data.damage);

		float.TryParse(UIcc.fireRate_UIText.text, out data.fireRate);

		int.TryParse(UIcc.maxAmmo_UIText.text, out data.maxAmmo);

		float.TryParse(UIcc.reloadTime_UIText.text, out data.reloadTime);

		data.isProj = UIcc.isProj_UIText.isOn;

		float.TryParse(UIcc.projSpeed_UIText.text, out data.projSpeed);

		float.TryParse(UIcc.spreadRadius_UIText.text, out data.spreadRadius);

		float.TryParse(UIcc.spellAtkPoints_UIText.text, out data.spellAtkPoints);

		float.TryParse(UIcc.spellTnkPoints_UIText.text, out data.spellTnkPoints);

		float.TryParse(UIcc.spellPointsTrigger_UIText.text, out data.spellPointsTrigger);
	}

	protected void UIStart()
	{
		UIcc.fileName_UIText.text = fileName;

		UIcc.characterName_UIText.text = data.characterName;
		UIcc.totalHP_UIText.text = data.totalHP.ToString();
		UIcc.armorHP_UIText.text = data.armorHP.ToString();
		UIcc.moveSpeed_UIText.text = data.moveSpeed.ToString();

		UIcc.damage_UIText.text = data.damage.ToString();
		UIcc.fireRate_UIText.text = data.fireRate.ToString();
		UIcc.maxAmmo_UIText.text = data.maxAmmo.ToString();
		UIcc.reloadTime_UIText.text = data.reloadTime.ToString();

		if (data.isProj)
		{
			UIcc.isProj_UIText.isOn = true;
		}
		else
		{
			UIcc.isProj_UIText.isOn = false;
		}

		UIcc.projSpeed_UIText.text = data.projSpeed.ToString();
		if (data.projPrefab)
		{
			UIcc.projPrefab_UIText.text = data.projPrefab.name;
		}
		UIcc.spreadRadius_UIText.text = data.spreadRadius.ToString();

		UIcc.spellAtkPoints_UIText.text = data.spellAtkPoints.ToString();

		UIcc.spellTnkPoints_UIText.text = data.spellTnkPoints.ToString();

		UIcc.spellPointsTrigger_UIText.text = data.spellPointsTrigger.ToString();
	}

	protected void UIUpdate()
	{

		// Private values

		UIcc.currentHP_UIText.text = currentHP.ToString();
		UIcc.currentSpellCharge_UIText.text = currentSpellCharge.ToString();
	}

	protected void SaveData()
	{
		UpdateCharacterFromUI();

		string jsonData = JsonUtility.ToJson(data);

		FileAccessSingleton.WriteFile(jsonData, Application.streamingAssetsPath + "/charaData/" + fileName + ".charaData");

		GameManager.instance.ChangeInfoText("Character Saved");
	}

	protected void LoadData(bool _doUIStart = true)
	{
		string jsonData = FileAccessSingleton.LoadFile(Application.streamingAssetsPath + "/charaData/" + fileName + ".charaData");

		if (jsonData != "")
		{
			JSONtxt = new TextAsset(jsonData);
			JsonUtility.FromJsonOverwrite(jsonData, data);

			GameManager.instance.ChangeInfoText("Character Loaded");

			currentHP = data.totalHP;

			currentArmorHP = data.armorHP;

			currAmmo = data.maxAmmo;

			if (_doUIStart)
				UIStart();
		}
	}


	/////////////// Gameplay ///////////////

	protected void Shoot()
	{
		Vector2 temp = Random.insideUnitCircle;
		Vector3 direction = temp * data.spreadRadius * 10;
		direction.z = 500; // circle is at Z units 
		direction = transform.TransformDirection(direction.normalized);

		if (data.isProj)
		{
			if (data.projPrefab.GetComponent<Rigidbody>())
			{

				GameObject tempProj = Instantiate(data.projPrefab, gun.transform.GetChild(0).position, Quaternion.identity);

				tempProj.GetComponent<Rigidbody>().AddForce(direction * data.projSpeed * 10);
				tempProj.GetComponent<ProjBehaviour>().damages = data.damage;
			}
			else
			{
				//Debug.Log("NoRB");
			}
		}
		else
		{
			//Raycast and debug
			Ray r = new Ray(transform.position, direction);


			if (data.projPrefab.GetComponent<Rigidbody>())
			{

				GameObject tempProj = Instantiate(data.projPrefab, gun.transform.GetChild(0).position, Quaternion.identity);

				tempProj.GetComponent<Rigidbody>().AddForce(r.direction * 5000);

				tempProj.GetComponent<ProjBehaviour>().haveBehaviour = false;
			}
			else
			{
				//Debug.Log("NoRB");
			}

			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.CompareTag("Character"))
				{
					if (hit.collider.gameObject.GetComponent<Character>())
					{
						float a = hit.collider.gameObject.GetComponent<Character>().LoseHP(data.damage);

						currentSpellCharge += a * data.spellAtkPoints;
					}
				}
			}
			//Debug.DrawLine(transform.position, r.direction * 100, Color.red, 150);

			//RaycastHit hit;
			//if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward*50, out hit))
			//{
			//	if (hit.collider.CompareTag("Character"))
			//	{
			//		if (hit.collider.gameObject.GetComponent<Character>())
			//		{
			//			hit.collider.gameObject.GetComponent<Character>().LoseHP(data.damage);
			//		}
			//	}
			//}

			//Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 50, Color.red, 100);


		}
		currAmmo--;

		currentShootCooldown = 0.0f;
	}

	// Character loses HP, returns the amount of damage dealt
	public int LoseHP(int _amount)
	{
		int damagesDealt = _amount;

		if (currentArmorHP > 0)
		{
			damagesDealt = (int)(damagesDealt * 0.75f);
			//+Debug.Log(damagesDealt);
			currentArmorHP -= damagesDealt;
			if (currentHP - damagesDealt >= 0)
			{
				currentHP -= damagesDealt;
			}
			else
			{
				damagesDealt = currentHP;

				currentHP = 0;
			}
		}
		else
		{
			if (currentHP - _amount >= 0)
			{
				currentHP -= _amount;
			}
			else
			{
				damagesDealt = currentHP;

				currentHP = 0;
			}
		}

		currentSpellCharge += data.spellTnkPoints * damagesDealt;

		return damagesDealt;
	}

	public float HpPercent()
	{
		return (float)((float)(currentHP) / (float)(data.totalHP));
	}

	protected void ExecuteSpell()
	{
		if (currentSpellCharge >= data.spellPointsTrigger)
		{
			if (data.projPrefab.GetComponent<Rigidbody>())
			{
				for (int i = 0; i < 10; i++)
				{
					Vector2 temp = Random.insideUnitCircle;
					Vector3 direction = temp * 5 * 10;
					direction.z = 500; // circle is at Z units 
					direction = transform.TransformDirection(direction.normalized);
					GameObject tempProj = Instantiate(data.projPrefab, gun.transform.GetChild(0).position, Quaternion.identity);

					tempProj.GetComponent<Rigidbody>().AddForce(direction * 5000);
					tempProj.GetComponent<ProjBehaviour>().damages = data.damage;
				}
			}
			else
			{
				//Debug.Log("NoRB");
			}

			currentSpellCharge = 0;
		}
		else
		{
			GameManager.instance.ChangeInfoText("Spell not charged !");
		}
	}
}
