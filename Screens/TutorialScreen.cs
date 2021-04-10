using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using System;

public class TutorialScreen : MonoBehaviour {
    private ScreenFader _fadeScr;
    private int _sceneNumb = 1;

    public Button Btn;
    public Text Cont;

    private EventSystem _mEventSystem;
	public GameObject GoButton;
    public List<GameObject> Buttons;
	public int PlayerId = 0; // The Rewired player id of this character

    public Color LColour;

    private void Awake () {
		Buttons.Add(GoButton);
        _mEventSystem = EventSystem.current;
        // m_EventSystem.SetSelectedGameObject(buttons[0], null);
        RenderSettings.skybox.SetColor("_Color1", LColour);
    }

    private void Update () {
		if (_mEventSystem) ControllerInput();
	}

    private void ControllerInput()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        ReInput.players.GetPlayer(0);

        //int curIndex = buttons.IndexOf(m_EventSystem.currentSelectedGameObject.gameObject);
        if (Rewired.ReInput.players.GetPlayer(0).GetButton("UIVertical"))
        {
            //SetSelected(buttons[curIndex+1]);
        }
    }
	
    // Use this for initialization
    private void Start()
    {
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        StartCoroutine(DoTheDance());
    }

    public IEnumerator DoTheDance()
    {
        yield return new WaitForSeconds(5f); // waits 3 seconds
        ShowContinue();
    }

    private void ShowContinue()
    {
        Cont.enabled = true;
        Btn.GetComponent<Button>().enabled = true;
    }

    public void Continue()
    {
        try
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/saves/");
            // if there's not actorA save game, show the tutorial
            if (Directory.GetFiles(Application.dataPath + "/saves/", "*.bin").Length == 0)
                _fadeScr.EndScene(1);
            // else just skip it and load the main menu
            else
                _fadeScr.EndScene(2);
        }
        catch (Exception e)
        {
            Application.Quit();
        }
    } 
     
}
