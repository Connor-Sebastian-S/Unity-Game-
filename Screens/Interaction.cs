using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour {

    public Camera C;

    private TempDataTracker _dta;
    private Logger _lg;
    private SettingsReader _settingsReader;
    private GeneticsManager _gm;
    private CollisionMediator _co;
    private AudioManager _am;

    private ScreenFader _fadeScr;

    private GameObject _aperatus;
    private GameObject _cam;
    private EnvironmentController _environmentController;
    private float _timer = 1.1f;

    private string _sceneName;
    private float _speed = 0.001F;
    private float _fraction = 0;
    private GameStatusManager _sc;
    private float _everyN;
    private float _defaultN;
    private float _d;

    public Color lColour;

    void Awake()
    {
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        // Create actorA temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        _sceneName = currentScene.name;
        _sc = GameObject.Find("UI Controller").GetComponent<GameStatusManager>();
        print("-------- " + _sc.GameStatus.ToString());

        _dta = TempDataTracker.GetInstance();
        _settingsReader = SettingsReader.GetInstance();
        _cam = GameObject.Find("Main Camera");
        _environmentController = EnvironmentController.GetInstance();
        _gm = GeneticsManager.GetInstance();
        _co = CollisionMediator.GetInstance();
    }

    void Start () {
	    if (_sceneName == "Learner")
	    {
	        RenderSettings.skybox.SetColor("_Color1", lColour);
            print("In learning scene");
	    }
    }

    public void Exit()
    {
        print("retire");
        Time.timeScale = 1;
        Camera.main.GetComponent<Pause>().PauseMenu.SetActive(false);
        Destroy(this.gameObject);
        _fadeScr.EndScene(2);
    }
}
