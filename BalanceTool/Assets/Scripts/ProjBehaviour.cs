using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjBehaviour : MonoBehaviour
{
    public int damages = 0;

    public Character charaLinked = null;

    public bool haveBehaviour = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ProjDeath"))
		{
            Destroy(gameObject);
		}

        if (other.CompareTag("Character") && haveBehaviour)
		{
            Character charaTouched = other.gameObject.GetComponent<Character>();

            if (charaTouched)
			{
                float a = charaTouched.LoseHP(damages);

                if (charaLinked)
                    charaLinked.currentSpellCharge += a * charaLinked.data.spellAtkPoints;

                Destroy(gameObject);
			}
		}
	}
}
