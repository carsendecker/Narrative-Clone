using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
	/*I tried to make this generic but it wasn't working for both sprite renderer and text for some reason :( */
	public float FlashSpeed = 0.5f;
	public bool EnabledOnAwake;
	public bool EndFlashVisible;

	private bool flashEnabled = false;
	private bool currentlyFlashing = false;
	private float counter;
	private Image sr;
	
	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<Image>();
		
		if (EnabledOnAwake)
		{
			flashEnabled = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (flashEnabled)
		{
			counter += Time.deltaTime;
			if (counter >= FlashSpeed && !currentlyFlashing)
			{
				sr.enabled = false;
				currentlyFlashing = true;
				counter = 0;
			}
			else if (counter >= FlashSpeed && currentlyFlashing)
			{
				sr.enabled = true;
				currentlyFlashing = false;
				counter = 0;
			}
		}
		else if (!flashEnabled && currentlyFlashing)
		{
			if (EndFlashVisible)
				sr.enabled = true;
			else
				sr.enabled = false;
			currentlyFlashing = false;
		}
	}

	public void FlashObject(bool active)
	{
		if (active)
			flashEnabled = true;
		else if (!active)
		{
			flashEnabled = false;
			sr.enabled = false;
			currentlyFlashing = false;
		}

	}

	public void FlashObject(bool active, int totalFlashes)
	{
		if (active || totalFlashes > 0)
		{
			flashEnabled = true;
			StartCoroutine(waitTime(totalFlashes * (FlashSpeed * 2)));
			flashEnabled = false;
		}
		else
			flashEnabled = false;
	}

	IEnumerator waitTime(float time)
	{
		yield return new WaitForSeconds(time);
	}

	
}
