using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowControl : MonoBehaviour
{	

//Want to put all panels in an array but unsure if I can name items within an array
//	private GameObject[] chatPanels;

	public float blinkSpeed;
	public Color blinkOn, blinkOff;
	
	public GameObject chatContentKayla, chatContentAmy, chatPanelKayla, chatPanelAmy;
	public TextAsset[] kayla, amy;
	public Image kaylaButton, amyButton;
	
	private InkController inkControl;
	private int timesChattedKayla, timesChattedAmy;
	private bool kaylaOpen, amyOpen;
	private bool kaylaAvailable, amyAvailable;
	private float blinkTimer;
	
	
	// Use this for initialization
	void Start ()
	{
		inkControl = GetComponent<InkController>();
		kaylaAvailable = true;
		kaylaButton.color = blinkOff;
		amyButton.color = blinkOff;
	}

	private void Update()
	{
		if (kaylaAvailable)
		{
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= blinkSpeed)
			{
				if (kaylaButton.color == blinkOff)
				{
					kaylaButton.color = blinkOn;
				}
				else kaylaButton.color = blinkOff;

				blinkTimer = 0;
			}
		}
		else if (amyAvailable)
		{
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= blinkSpeed)
			{
				if (amyButton.color == blinkOff)
				{
					amyButton.color = blinkOn;
				}
				else amyButton.color = blinkOff;
			}
		}
		else
		{
			blinkTimer = 0;
			kaylaButton.color = blinkOff;
			amyButton.color = blinkOff;
		}
	}

	public void ConversationEnded()
	{
		if (kaylaOpen)
		{
			timesChattedKayla++;
			if (timesChattedKayla == 1)
			{
				amyAvailable = true;
			}
		}
		else if (amyOpen)
		{
			timesChattedAmy++;
		}

	}

	public void OpenKayla()
	{
		if (kaylaOpen) return;
		kaylaOpen = true;
		amyOpen = false;
		chatPanelKayla.SetActive(true);
		chatPanelAmy.SetActive(false);

		if (kaylaAvailable)
		{
			if (timesChattedKayla == 0)
			{
				inkControl.StartStory(kayla[0], chatContentKayla);
				kaylaAvailable = false;
			}
		}
	}

	public void OpenAmy()
	{
		if (amyOpen) return;
		kaylaOpen = false;
		amyOpen = true;
		chatPanelKayla.SetActive(false);
		chatPanelAmy.SetActive(true);

		if (amyAvailable)
		{
			if (timesChattedAmy == 0)
			{
				inkControl.StartStory(amy[0], chatContentAmy);
				amyAvailable = false;
			}
		}
	}
}
