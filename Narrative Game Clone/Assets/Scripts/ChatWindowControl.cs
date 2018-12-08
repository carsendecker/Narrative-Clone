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
	public GameObject choicePanel;
	public TextAsset[] kayla, amy;
	public Image kaylaButton, amyButton;
	public GameObject confetti;
	public AudioClip cheerSound, notifySound, openSound;
	public Flash cursorScript;
	
	private InkController inkControl;
	private int timesChattedKayla, timesChattedAmy;
	private bool kaylaOpen, amyOpen;
	private bool kaylaAvailable, amyAvailable;
	private bool kaylaChatting, amyChatting;
	private bool soundPlayed;
	private float blinkTimer;
	private AudioSource aso;
	
	
	// Use this for initialization
	void Start ()
	{
		inkControl = GetComponent<InkController>();
		//StartCoroutine(StartDelay());
		kaylaAvailable = true;
		kaylaButton.color = blinkOff;
		amyButton.color = blinkOff;
		aso = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (kaylaAvailable)
		{
			BlinkButton(kaylaButton);
		}
		else if (amyAvailable)
		{
			BlinkButton(amyButton);
		}
		else
		{
			blinkTimer = 0;
			kaylaButton.color = blinkOff;
			amyButton.color = blinkOff;
			soundPlayed = false;
		}
	}

	public void ConversationEnded()
	{
		if (kaylaOpen)
		{
			kaylaChatting = false;
			timesChattedKayla++;
			if (timesChattedKayla == 1)
			{
				amyAvailable = true;
			}
		}
		else if (amyOpen)
		{
			amyChatting = false;
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
		aso.PlayOneShot(openSound);

		if (amyChatting)
		{
			choicePanel.SetActive(false);
			cursorScript.FlashObject(false);
			cursorScript.gameObject.SetActive(false);
		}
		else if (kaylaChatting)
		{
			choicePanel.SetActive(true);
			cursorScript.gameObject.SetActive(true);
			cursorScript.FlashObject(true);
		}

		if (kaylaAvailable)
		{
			if (timesChattedKayla == 0)
			{
				inkControl.StartStory(kayla[0], chatContentKayla);
				choicePanel.SetActive(true);
				cursorScript.gameObject.SetActive(true);
				kaylaAvailable = false;
				kaylaChatting = true;
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
		aso.PlayOneShot(openSound);

		if (kaylaChatting)
		{
			choicePanel.SetActive(false);
			cursorScript.FlashObject(false);
			cursorScript.gameObject.SetActive(false);
		}
		else if (amyChatting)
		{
			choicePanel.SetActive(true);
			cursorScript.gameObject.SetActive(true);
			cursorScript.FlashObject(true);
		}
		
		if (amyAvailable)
		{
			if (timesChattedAmy == 0)
			{
				inkControl.StartStory(amy[0], chatContentAmy);
				amyChatting = true;
				amyAvailable = false;
			}
		}
	}

	void BlinkButton(Image button)
	{
		if (!soundPlayed)
		{
			aso.PlayOneShot(notifySound);
			soundPlayed = true;
		}
		blinkTimer += Time.deltaTime;
		if (blinkTimer >= blinkSpeed)
		{
			if (button.color == blinkOff)
			{
				button.color = blinkOn;
			}
			else button.color = blinkOff;

			blinkTimer = 0;
		}
	}

	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(3f);
		kaylaAvailable = true;
	}
	
	
	public void DropConfetti()
	{
		ParticleSystem[] ps = confetti.GetComponentsInChildren<ParticleSystem>();
		foreach (var color in ps)
		{
			color.Play();
		}
		aso.PlayOneShot(cheerSound);
	}
	
}
