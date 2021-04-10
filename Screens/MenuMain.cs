using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour {

    public Camera C;

    private ScreenFader _fadeScr;

    private TempDataTracker _d;
    private Logger _lg;
    private SettingsReader _settingsReader;
    private GeneticsManager _gm;
    private CollisionMediator _co;
    private AudioManager _am;

    private GameObject _aperatus;
    private GameObject _cam;
    private EnvironmentController _environmentController;
    private float _timer = 1.1f;
    public GameObject PlayerPrefab;
    private PlayerData _pd;

    private string _sceneName;
    private float _speed = 0.004F;
    private float _fraction;
    private Vector3 _s;
    private Vector3 _e = new Vector3(0, 0, 0);

    private void Awake()
    {
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();

        // Create actorA temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        _sceneName = currentScene.name;

        _d = TempDataTracker.GetInstance();
        _settingsReader = SettingsReader.GetInstance();
        _cam = GameObject.Find("Main Camera");
        _environmentController = EnvironmentController.GetInstance();
        _gm = GeneticsManager.GetInstance();
        _co = CollisionMediator.GetInstance();
        _am = AudioManager.GetInstance();

        C = Camera.main;
        C.transform.parent = PlayerPrefab.transform;
    }

    private void Start()
    {
        
        var d = _settingsReader.FoodWideSpreadY;
        _s = new Vector3(0, d, 0);
    }


    private void Update()
    {
        if (_sceneName == "Menu")
        {
            if (_fraction < 1)
            {
                _fraction += Time.deltaTime * _speed;
                PlayerPrefab.transform.position = Vector3.Lerp(_s, _e, _fraction);
            }
        }
    }

    // Save current game
    public void Save()
    {
        StorageHandler sh = new StorageHandler();
        PlayerData dt = _pd;
        if (_pd != null)
            sh.SaveData(dt, "player");
    }

    // Load actorA saved game
    public void Load()
    {
        StorageHandler sh = new StorageHandler();
        PlayerData dp = (PlayerData)sh.LoadData("player") as PlayerData;
        PlayerDataTracker pdt = PlayerDataTracker.GetInstance();
        pdt.Dt = dp;
        PlayerPrefab = Instantiate(PlayerPrefab, new Vector3(pdt.GetPlayerX(), pdt.GetPlayerY(), pdt.GetPlayerZ()), Quaternion.identity);
        PlayerPrefab.gameObject.name = "Player";
        this.gameObject.GetComponent<EnvironmentColours>().Player = PlayerPrefab;

        _d = TempDataTracker.GetInstance();
        _settingsReader = SettingsReader.GetInstance();
        _cam = GameObject.Find("Main Camera");
        _environmentController = EnvironmentController.GetInstance();
        _gm = GeneticsManager.GetInstance();
        _co = CollisionMediator.GetInstance();
        _am = AudioManager.GetInstance();
    }
}