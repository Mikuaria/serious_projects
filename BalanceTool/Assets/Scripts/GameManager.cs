using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ToolState
{
    EDIT,
    PLAY,
    NB_ToolState
};

public class GameManager : MonoBehaviour
{
    public ToolState currentToolState;

    public static GameManager instance;

	[SerializeField] Text currentStateText = null;

	public Text infoDisplay = null;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}

		currentStateText.text = "Edition Mode";
	}

	// Update is called once per frame
	void Update()
    {
		Color tempColor = infoDisplay.color;

		tempColor.a -= Time.deltaTime / 3.0f;

		infoDisplay.color = tempColor;
    }

    public void SwitchToolState()
	{
        if (currentToolState == ToolState.EDIT)
		{
			Cursor.lockState = CursorLockMode.Locked;
			currentToolState = ToolState.PLAY;

			currentStateText.text = "Play mode";
		}
		else if (currentToolState == ToolState.PLAY)
		{
			Cursor.lockState = CursorLockMode.None;
			currentToolState = ToolState.EDIT;

			currentStateText.text = "Edition mode";
		}
	}

	public void ChangeInfoText(string _stringOutput)
	{
		infoDisplay.text = _stringOutput;

		Color tempColor = GameManager.instance.infoDisplay.color;

		tempColor.a = 1.0f;

		GameManager.instance.infoDisplay.color = tempColor;
	}
}
