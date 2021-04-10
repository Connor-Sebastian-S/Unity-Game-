using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnvironmentController : MonoBehaviour
{	
	public static GameObject Container;
	public static EnvironmentController Instance;

	public static CreatureCounterObservor CreatureCounterObservor;

    private GameObject _food;

    private SettingsReader _settingsReader;
    public SpawnerOrganiser SpawnerOrganiser;
    public UIDepthHandler UiDepthHandler;
	public float TotalEnergy;
	public float Energy;
    private readonly float _initEnergyMin;
    private readonly float _initEnergyMax;
    private float _initScaleMin;
    private float _initScaleMax;

    private Vector3 _pos;

	public float 	WideSpread;
	public float 	WideSpreadY;
    private int		_startNumberfoods;
	public int 	Maxfoods;
    private int 	_currentFodbits;

    private int 	_sporeRange;

    public ArrayList Creatures;
	public ArrayList Foods;

    private void OnEnable ()
    {
        SpawnerOrganiser.CreatureSpawned += OnCreatureSpawned;
        Creature.CreatureDead += OnCreatureDeath;
    }

    private void OnDisable()
    {
        SpawnerOrganiser.CreatureSpawned -= OnCreatureSpawned;
        Creature.CreatureDead -= OnCreatureDeath;
    }

    private void Start ()
    {
		_food = (GameObject)Resources.Load("Prefabs/Foodbit");
		string name = this.name.ToLower();
		
		_settingsReader = SettingsReader.GetInstance();
        TempDataTracker.GetInstance();
        UiDepthHandler = UIDepthHandler.GetInstance();
        SpawnerOrganiser = SpawnerOrganiser.GetInstance();
		CreatureCounterObservor = CreatureCounterObservor.GetInstance ();

		SetupVariables();

        Energy = TotalEnergy;

        Creatures = new ArrayList();
		Foods = new ArrayList();
	}

    private void SetupVariables()
    {
        try
        {
            TotalEnergy = _settingsReader.EnvironmentTotalEnergy;
            _startNumberfoods = (int) _settingsReader.EnvironmentStartNumberFoodbits;
            Maxfoods = _startNumberfoods;
            _sporeRange = (int) _settingsReader.FoodSporeRange;
            WideSpread = _settingsReader.FoodWideSpread;
            WideSpreadY = _settingsReader.FoodWideSpreadY;

            _initScaleMin = _settingsReader.FoodInitScaleMin;
            _initScaleMax = _settingsReader.FoodInitScaleMax;
        }
        catch (System.FormatException e)
        {
            Debug.LogWarning(e);
        }
    }

    private bool _init;

    public EnvironmentController(float initEnergyMin, float initEnergyMax)
    {
        _initEnergyMin = initEnergyMin;
        _initEnergyMax = initEnergyMax;
    }

    private void Update() {
		if (_currentFodbits <= Maxfoods) {
			Vector3 pos = Utility.RandomVector3 (-WideSpread, WideSpreadY, WideSpread);
			Newfood (pos);
		}
	}

	public bool Initialise () {
		if (_currentFodbits <= Maxfoods) {
			Vector3 pos = Utility.RandomVector3(-WideSpread, WideSpreadY, WideSpread);
			Newfood(pos);
		}
		_init = true;
		return true;
	}

    private void OnCreatureSpawned (Creature c)
    {
        Creatures.Add(c);
    }

    private void OnCreatureDeath (Creature c)
    {
        Creatures.Remove(c);
    }

    public void Newfood(Vector3 pos)
    {
        var fb = Instantiate(_food, pos, Quaternion.identity);
        var fbS = fb.AddComponent<Food>();
        fbS.Energy = Random.Range(5,20);
        var scale = Utility.ConvertRange(fbS.Energy, _initEnergyMin, _initEnergyMax, _initScaleMin, _initScaleMax);
        fb.transform.localScale = new Vector3(scale, scale, scale);
        Foods.Add(fb);
        _currentFodbits += 1;
    }
	
	public void Removefood (GameObject fb)
    {
        Foods.Remove(fb);
	}
	
	public int GetfoodCount ()
    {
		return Foods.Count;
	}

    public static EnvironmentController GetInstance()
    {
        if (Instance) return Instance;
        Container = new GameObject();
        Container.name = "environment";
        Instance = Container.AddComponent(typeof(EnvironmentController)) as EnvironmentController;

        return Instance;
    }
}
