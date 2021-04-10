using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
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
	public GameObject PlayerPrefab;
    private PlayerData _pd;

    private string _sceneName;
    private float _speed = 0.001F;
    private float _fraction = 0;
    private GameStatusManager _sc;
    private float _everyN;
    private float _defaultN;
    private float _d;

    private void Awake ()
    {
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        // Create actorA temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        _sceneName = currentScene.name;
        _sc = GameObject.Find("UI Controller").GetComponent<GameStatusManager>();
        print("-------- " + _sc.GameStatus.ToString());

        if (_sc.GameStatus == 4)
        {
            print("First start...");
            _dta = TempDataTracker.GetInstance();
            _settingsReader = SettingsReader.GetInstance();
            _cam = GameObject.Find("Main Camera");
            _environmentController = EnvironmentController.GetInstance();
            _gm = GeneticsManager.GetInstance();
            _co = CollisionMediator.GetInstance();
            _am = AudioManager.GetInstance();
            PlayerDataTracker pdt = PlayerDataTracker.GetInstance();
            _pd = pdt.Dt;

            var s = _settingsReader.FoodWideSpreadY;
            _defaultN = s;

            PlayerPrefab = Instantiate(PlayerPrefab, new Vector3(0, s, 0), Quaternion.identity);
            PlayerPrefab.gameObject.name = "Player";

            pdt.SetEnergy(_settingsReader.PlayerStartEnergy);

            this.gameObject.GetComponent<EnvironmentColours>().Player = PlayerPrefab;
        }
        else if (_sc.GameStatus == 3)
        {
            Load();
        }

        C = Camera.main;
        C.transform.position = new Vector3(PlayerPrefab.transform.position.x, PlayerPrefab.transform.position.y + 10, PlayerPrefab.transform.position.z);
        C.transform.parent = PlayerPrefab.transform;
    }

    private void Start()
    {
        StartCoroutine("DoCheck");
    }

    private IEnumerator DoCheck()
    {
        for (; ; )
        {
            Save();
            yield return new WaitForSeconds(10);
        }
    }

    // Save current game
    public void Save()
    {
        StorageHandler sh = new StorageHandler();
        PlayerData dt = _pd;
        if (dt != null)
        {
            print("saving!");
            sh.SaveData(dt, "player");
            print("saved!");
        }
    }

    public void Exit()
    {
        print("retire");
        Time.timeScale = 1;
        Camera.main.GetComponent<Pause>().PauseMenu.SetActive(false);
        Destroy(GameObject.Find("UI Controller"));
        _fadeScr.EndScene(2);
    }

    // Load actorA saved game
    public void Load ()
    {
        print("Loading...");
        StorageHandler sh = new StorageHandler();
        PlayerData dp = (PlayerData)sh.LoadData("player") as PlayerData;
        _pd = dp;
        PlayerDataTracker pdt = PlayerDataTracker.GetInstance();
        pdt.Dt = _pd;
        PlayerPrefab = Instantiate(PlayerPrefab, new Vector3(pdt.GetPlayerX(), pdt.GetPlayerY(), pdt.GetPlayerZ()), Quaternion.identity);

        PlayerPrefab.gameObject.name = "Player";
        this.gameObject.GetComponent<EnvironmentColours>().Player = PlayerPrefab;

        C.transform.position = new Vector3(pdt.GetPlayerX(), pdt.GetPlayerY() + 10, pdt.GetPlayerZ());
        C.transform.parent = PlayerPrefab.transform;

        _dta = TempDataTracker.GetInstance();
        _settingsReader = SettingsReader.GetInstance();
        _cam = GameObject.Find("Main Camera");
        _environmentController = EnvironmentController.GetInstance();
        _gm = GeneticsManager.GetInstance();
        _co = CollisionMediator.GetInstance();
        _am = AudioManager.GetInstance();
    }


}