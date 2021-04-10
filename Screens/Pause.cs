using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{

    public GameObject PauseMenu;

    private bool _paused;

    public int PlayerId = 0; // The Rewired player id of this character

    private Player _player; // The Rewired Player

    private EventSystem _mEventSystem;

    public GameObject SaveButton;
    public GameObject ExitButton;
    public List<GameObject> Buttons;

    private void Awake()
    {
        Buttons.Add(SaveButton);
        Buttons.Add(ExitButton);
        _mEventSystem = EventSystem.current;
       // m_EventSystem.SetSelectedGameObject(buttons[0], null);

        PauseMenu.SetActive(false);
        _player = ReInput.players.GetPlayer(PlayerId);
    }

    private void ControllerInput()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        _player = ReInput.players.GetPlayer(0);

        //int curIndex = buttons.IndexOf(m_EventSystem.currentSelectedGameObject.gameObject);
        if (Rewired.ReInput.players.GetPlayer(0).GetButton("UIVertical"))
        {
            //SetSelected(buttons[curIndex+1]);
        }
    }

    private void Update()
    {
        if (_mEventSystem) ControllerInput();

        if (_player.GetButtonDown("Pause"))
        {
            if (_paused == false)
            {
                print("paused");
                _paused = true;
                PauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                print("un-paused");
                _paused = false;
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
