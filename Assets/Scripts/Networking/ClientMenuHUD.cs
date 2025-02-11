﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClientMenuHUD : MonoBehaviour
{
    public ArsNetworkManager NetworkManager;
	public InputField PlayerName;
    public InputField PlayerEmail;
	public GameObject[] PossibleAvatars;
	public GameObject PlayerAvatar;
	public ToggleGroup CharacterToggleGroup;
	public GameObject TogglePrefab;

	// Use this for initialization
	void Start ()
    {
		if(NetworkManager == null)
			NetworkManager = GetComponent<ArsNetworkManager>();

	    PlayerName.text = PlayerPrefs.GetString("LocalName", string.Empty);
        PlayerEmail.text = PlayerPrefs.GetString("Email", string.Empty);
        //ShowAvatars ();

        if (NetworkManager.SkipStartScreen)
        {
			StartGame();
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*if (FindObjectOfType<ToggleGroup>() != null 
            && Camera.main.transform.childCount == 0)
        {
			ShowAvatars();
		}*/
	}
		

	//show possible avatars for selection
	/*void ShowAvatars()
    {
		float x = -3;
		var tg = FindObjectOfType<ToggleGroup> ();
		var i = 0;

		foreach (var obj in PossibleAvatars)
        {
			var o = Instantiate (obj);
			o.transform.SetParent(Camera.main.transform);
			o.transform.localPosition = new Vector3 (0 + x, 1, 5);
			o.name = obj.name;
			x += 3;
			
			//Setup toggle
			Vector2 v = Camera.main.WorldToScreenPoint (o.transform.position);
			var t = Instantiate (TogglePrefab);
			t.transform.localPosition = Vector3.zero;
			t.transform.position = new Vector3(v.x - 5, v.y - 40, 0);
			t.transform.SetParent(tg.transform, true);
			t.GetComponent<Toggle>().isOn = i++ == 0;
			t.GetComponentInChildren<Text>().text = o.name; 
			t.GetComponent<Toggle>().group = CharacterToggleGroup;
		}

		PlayerAvatar = PossibleAvatars [0];
	}*/


	public void StartGame()
    {
		/*var selectedAvatar = "";
		foreach (var t in CharacterToggleGroup.ActiveToggles ()) {
			selectedAvatar = t.GetComponentInChildren<Text>().text;
		}
		foreach (var av in PossibleAvatars.Where(av => av.name == selectedAvatar))
		{
		    PlayerAvatar = av;
		}*/

	    if (string.IsNullOrEmpty(PlayerName.text))
	    {
	        StartCoroutine(HighlightPlayerName());
	        return;
	    }

	    if (!EmailSaver.IsValidEmail(PlayerEmail.text))
	    {
	        StartCoroutine(HighlightPlayerEmail());
	        return;
	    }

	    PlayerPrefs.SetString("LocalName", PlayerName.text);
        PlayerPrefs.SetString("Email", PlayerEmail.text);
        PlayerPrefs.Save();
		NetworkManager.ArsStartClient();
	    enabled = false;
    }

    private IEnumerator HighlightPlayerName()
    {
        var originalColorBlock = PlayerName.colors;
        PlayerName.colors = new ColorBlock
        {
            normalColor = Color.red,
            disabledColor = Color.red,
            highlightedColor = Color.red,
            pressedColor = Color.red,
            colorMultiplier = 1f
        };
        yield return new WaitForSeconds(.5f);
        PlayerName.colors = originalColorBlock;
    }

    private IEnumerator HighlightPlayerEmail()
    {
        var originalColorBlock = PlayerEmail.colors;
        PlayerEmail.colors = new ColorBlock
        {
            normalColor = Color.red,
            disabledColor = Color.red,
            highlightedColor = Color.red,
            pressedColor = Color.red,
            colorMultiplier = 1f
        };
        yield return new WaitForSeconds(.5f);
        PlayerEmail.colors = originalColorBlock;
    }
}
