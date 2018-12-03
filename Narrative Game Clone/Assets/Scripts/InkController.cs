using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using TMPro;
using Random = UnityEngine.Random;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class InkController : MonoBehaviour
{
	public TextAsset inkJSONAsset;
	public GameObject choicePanel;
	public GameObject scrollView;
	public float minTypeDelay, maxTypeDelay, minTypeTime, maxTypeTime;
	public AudioClip typingSound, sendSound, receivedSound;


	private ScrollRect scrollBar;
	private Story story;
	private ChatWindowControl chatControl;
	private AudioSource aso;
	private bool typing, doneDisplayingContent;
	
	// UI Prefabs
	[SerializeField]
	private TextMeshProUGUI textPrefab;
	[SerializeField]
	private Button buttonPrefab;
	
	void Awake () {
		//StartStory();
		chatControl = GetComponent<ChatWindowControl>();
		aso = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (doneDisplayingContent && !typing)
		{
			DisplayChoices();
			doneDisplayingContent = false;
		}
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory () {
		story = new Story (inkJSONAsset.text);
		RefreshView();
	}

	public void StartStory(TextAsset conversation, GameObject convoPanel)
	{
		inkJSONAsset = conversation;
		scrollView = convoPanel;
		scrollBar = scrollView.GetComponentInParent<ScrollRect>();
		story = new Story (inkJSONAsset.text);
		RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		
		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();

			if (story.currentTags.Contains("me"))
			{
				text = "<#FF452F>Me: </color>" + text;
			}
			else if (story.currentTags.Contains("Kayla"))
			{
				text = "<#676DF1>Kayla: </color>" + text;
			}
			else if (story.currentTags.Contains("Amy"))
			{
				text = "<#00C0A3>Amy: </color>" + text;
			}
			
			//If the text is not from the player, make them "type"
			if (!story.currentTags.Contains("me") 
			    && !story.currentTags.Contains("dont_type") 
			    && story.currentTags.Count != 0)
			{
				StartCoroutine(StartTyping(text));
			}
			else
			{
				CreateContentView(text);
			}
		}

		doneDisplayingContent = true;
	}

	void DisplayChoices()
	{
		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished! 
		else {
			chatControl.ConversationEnded();
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		aso.PlayOneShot(sendSound);
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a button showing the choice text
	void CreateContentView (string text) {
		TextMeshProUGUI storyText = Instantiate (textPrefab);
		storyText.text = text;
		storyText.transform.SetParent (scrollView.transform, false);
		StartCoroutine(ScrollDown());
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (choicePanel.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = choicePanel.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy(choicePanel.transform.GetChild(i).gameObject);
		}
	}

	IEnumerator ScrollDown()
	{
		yield return new WaitForEndOfFrame();
		scrollBar.verticalNormalizedPosition = 0;
	}

	IEnumerator StartTyping(String responseText)
	{
		typing = true;
		yield return new WaitForSeconds(Random.Range(minTypeDelay, maxTypeDelay));
		
		aso.PlayOneShot(typingSound);
		TextMeshProUGUI typingText = Instantiate (textPrefab);
		typingText.text = "<#7B7777><i> " + story.currentTags[0] + " is typing... </i></color>";
		typingText.transform.SetParent (scrollView.transform, false);
		StartCoroutine(ScrollDown());
		yield return new WaitForSeconds(Random.Range(minTypeTime, maxTypeTime));
		
		Destroy(typingText.gameObject);
		CreateContentView(responseText);
		aso.PlayOneShot(receivedSound);
		typing = false;
	}
}