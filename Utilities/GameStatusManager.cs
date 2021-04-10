using System.Collections.Generic;
using System.IO;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


/// <summary>
/// tracks the status of the game based on the existence of saved games and which button was pressed
/// </summary>
public class GameStatusManager : MonoBehaviour
{
    // 0 = default, null, error
    // 1 = new game started
    // 2 = game objects loaded
    // 3 = game save exists
    // 4 = no game save exists
    public int GameStatus;

    public GameObject NewButton;
    public GameObject LoadButton;
    public GameObject ExitButton;
    public GameObject SaveButton;
    public GameObject LearnerButton;
    public GameObject C;

    public static GameObject Container;
    public static GameStatusManager Instance;

    private ScreenFader _fadeScr;

    private EventSystem _mEventSystem;

    public static GameStatusManager GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject { name = "GameStatusManager" };
            Instance = Container.AddComponent<GameStatusManager>();
        }

        return Instance;
    }

    public List<GameObject> Buttons;

    private void Awake ()
    {
        CheckSaves();
		
        DontDestroyOnLoad(this);
        
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            
            if (GameStatus == 3)
            {
                NewButton.active = true;
                LoadButton.active = true;
                
            }

            if (GameStatus == 4)
            {
                NewButton.active = true;
                LoadButton.active = false;
                LearnerButton.GetComponent<RectTransform>().localPosition = ExitButton.GetComponent<RectTransform>().localPosition;
                ExitButton.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);
                
            }
        }
		
		Buttons.Add(NewButton);
        Buttons.Add(LoadButton);
        Buttons.Add(ExitButton);

        _mEventSystem = EventSystem.current;

        //_mEventSystem.SetSelectedGameObject(Buttons[0], null);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_mEventSystem) ControllerInput();
    }

    private void CheckSaves()
    {
        if (Directory.GetFiles(Application.dataPath + "/saves/", "*.bin").Length == 0)
        {
            Debug.Log("NO SAVE FILES FOUND");
            GameStatus = 4;
            print("clicked 1.2");
        }
        else
        {
            Debug.Log("SAVE FILES FOUND");
            GameStatus = 3;
            print("clicked 2.2");
        }
    }

    private void SetSelected(GameObject selectableObject)
    {
        // Set the currently selected GameObject
        _mEventSystem.SetSelectedGameObject(selectableObject);
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

    public void NewGame ()
    {
        print("clicked 1");
        GameStatus = 4;
        print("clicked 1.1");
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        _fadeScr.EndScene(3);
    }

    public void LoadGame()
    {
        print("clicked 2");
        GameStatus = 3;
        print("clicked 2.1");
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        _fadeScr.EndScene(3);
    }

    public void LearnGame()
    {
        print("clicked 3");
        print("clicked 3.1");
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        _fadeScr.EndScene(4);
    }

    public void ExitGame()
    {
        print("retire");
        Application.Quit();
    }
}
