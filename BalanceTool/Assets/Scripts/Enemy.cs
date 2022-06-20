using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    [SerializeField] InputField enFileName = null;

	[SerializeField] Slider HPSlider = null;

	[SerializeField] Text hpTxt = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        LoadData(false);
    }

    protected override void Update()
	{

		currentShootCooldown += Time.deltaTime;
		fileName = enFileName.text;

		HPSlider.value = HpPercent();

		if (Input.GetKeyDown(KeyCode.F4))
		{
			if (!fileName.Equals(""))
			{
				LoadData(false);
			}
			else
			{
				GameManager.instance.ChangeInfoText("EnemyFilename is empty !");
			}
		}

		if (currentShootCooldown >= 1.0f / data.fireRate)
		{
			Shoot();

			//Debug.Log("Enemy shoots");
		}

		hpTxt.text = currentHP.ToString();
	}
}
