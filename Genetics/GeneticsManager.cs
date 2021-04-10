using UnityEngine;
using System.Collections;

public class GeneticsManager : MonoBehaviour
{
	private EnvironmentController _environment;
	private SettingsReader _settingsReader;
	public static GameObject Container;
	public static GeneticsManager Instance;

    private ChromosomeComposition _chromosomeComposition;

    private int _startingCreatures;
    private float _wideSpread;
    private float _wideSpreadY;

    private Vector3 _maximumRootScale;
    private Vector3 _minimumRootScale;

    private Vector3 _maximumLimbScale;
    private Vector3 _minimumLimbScale;

    private int _recurrenceLimit;
    private int _branchLimit;
    private float _creatureInitEnergy;

    private void Start()
    {
        _settingsReader = SettingsReader.GetInstance();
        _environment = EnvironmentController.GetInstance();

        VariablesInit();
    }

    private void VariablesInit()
    {
// maximum scale of root object is equal to the max_root_scale x, y, and z values from the settings file
        _maximumRootScale = new Vector3
        {
            x = _settingsReader.RootMaxRootScaleX,
            y = _settingsReader.RootMaxRootScaleY,
            z = _settingsReader.RootMaxRootScaleZ
        };

        // minimum scale of root object is equal to the max_root_scale x, y, and z values from the settings file
        _minimumRootScale = new Vector3
        {
            x = _settingsReader.RootMinRootScaleX,
            y = _settingsReader.RootMinRootScaleY,
            z = _settingsReader.RootMinRootScaleZ
        };

        // maximum scale of limb object is equal to the max_root_scale x, y, and z values from the settings file
        _maximumLimbScale = new Vector3
        {
            x = _settingsReader.LimbMaxLimbScaleX,
            y = _settingsReader.LimbMaxLimbScaleY,
            z = _settingsReader.LimbMaxLimbScaleZ
        };

        // minimum scale of limb object is equal to the max_root_scale x, y, and z values from the settings file
        _minimumLimbScale = new Vector3
        {
            x = _settingsReader.LimbMinLimbScaleX,
            y = _settingsReader.LimbMinLimbScaleY,
            z = _settingsReader.LimbMinLimbScaleZ
        };

        // number of starting creatures is equal to the value starting_creatures from the settings file
        _startingCreatures = (int) _settingsReader.EnvironmentStartingCreatures;
        // the spread of creatures along the x and z axis is equal to value the wide_spread from the settings file
        _wideSpread = _settingsReader.FoodWideSpread;
        // the spread of creatures along the y axis is equal to the value wide_spread_y from the settings file
        _wideSpreadY = _settingsReader.FoodWideSpreadY;
        // creatures initial energy is equal to the init_energy value from the settings file
        _creatureInitEnergy = _settingsReader.CreatureInitEnergy;
        // creatures maximum limb count is equal to the branch_limit value from the settings file
        _branchLimit = (int) _settingsReader.CreatureBranchLimit;
        // creatures maximum limb segment count is equal to the recurrence_limit value from the settings file
        _recurrenceLimit = (int) _settingsReader.CreatureRecurrenceLimit;
    }

    public bool Init; // check status of initialisation

    private void Update()
    {
        if (_environment.SpawnerOrganiser.CrtCounterObservor != null)
        {
            // in the event of the creature count going below actorA playable limit, bring them back up to the starting count
            if (_environment.SpawnerOrganiser.CrtCounterObservor.NumberOfCreatures < _startingCreatures)
                for (var i = _environment.SpawnerOrganiser.CrtCounterObservor.NumberOfCreatures; i < _startingCreatures; i++)
                    // make actorA creature (or set it up)
                    SetupCreatures();
        }       
    }

    // intialise our scene
    public bool Initialise()
    {
        // for each creature below startup creature count
        for (var i = _environment.SpawnerOrganiser.CrtCounterObservor.NumberOfCreatures; i < _startingCreatures; i++) SetupCreatures();
        Init = true;  // initialisation complete
        return true; // return true on complete
    }

    // setup our creature, define colour, scale, limb count (and segments count), etc.
    private void SetupCreatures()
    {
        // create actorA new chromosome stgructure for our new creature
        _chromosomeComposition = new ChromosomeComposition();

        // random colours (unity colours seem to be defined as actorA value between 0 and 1
        var col = new Color(Random.Range(0.0F, 1.0F),
            Random.Range(0.0F, 1.0F),
            Random.Range(0.0F, 1.0F)
        );
        // set limb and root colour in our chromosome to our random colours
        _chromosomeComposition.SetColour(col.r, col.g, col.b);
        _chromosomeComposition.SetLimbColour(col.r, col.g, col.b);

        // define hunger threshold as the hunger_threshold value from the settings file
        _chromosomeComposition.HungerPoint = _settingsReader.CreatureHungerThreshold;

        // random root scale
        var rootScale = new Vector3(Random.Range(_minimumRootScale.x, _maximumRootScale.x),
            Random.Range(_minimumRootScale.y, _maximumRootScale.y),
            Random.Range(_minimumRootScale.z, _maximumRootScale.z)
        );
        // set root scale in chromosome to our random Vector 3
        _chromosomeComposition.SetRootScale(rootScale);

        // random initial limbs
        var bs = Random.Range(1, _branchLimit + 1); // random branch count
        _chromosomeComposition.SetNumberOfBranches(bs); // Set the number of branches in our chromosome 
        var branches = new ArrayList();

        for (var j = 0; j < bs; j++)
        {
            var limbs = new ArrayList();

            var recurrences = Random.Range(0, _recurrenceLimit); // limb recurrence (i.e. how many segments)
            _chromosomeComposition.NumRecurrences[j] = recurrences; // set limb segment (recurrence) in our chromosome

            // iterate through our segments
            for (var k = 0; k <= recurrences; k++)
            {
                // creat erandom scale for each segment
                var scale = new Vector3(Random.Range(_minimumLimbScale.x, _maximumLimbScale.x),
                    Random.Range(_minimumLimbScale.y, _maximumLimbScale.y),
                    Random.Range(_minimumLimbScale.z, _maximumLimbScale.z)
                );

                var position = Utility.RandomPointInsideCube(rootScale);

                var limb = new ArrayList();
                limb.Add(position);
                limb.Add(scale);
                limbs.Add(limb);
            }

            branches.Add(limbs);
        }

        _chromosomeComposition.SetFequency(Random.Range(3, 20));
        _chromosomeComposition.SetAmplitude(Random.Range(3, 6));
        _chromosomeComposition.SetPhase(Random.Range(0, 360));
        _chromosomeComposition.SetBranches(branches);

        var pos = Utility.RandomVector3(-_wideSpread, _wideSpreadY, _wideSpread);
        _environment.SpawnerOrganiser.Spawn(
            pos,
            Utility.RandomVector3(),
            _creatureInitEnergy,
            _chromosomeComposition);
    }

    public static GeneticsManager GetInstance()
    {
        if (Instance) return Instance;
        Container = new GameObject {name = "GeneticsManager"};
        Instance = Container.AddComponent<GeneticsManager>();

        return Instance;
    }
}
