using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour
{
    private Transform _t;
    public string TypeOfCreature;
    public GameObject TargetFood;

    private SettingsReader _settingsReader;
    private EnvironmentController _environment;

    public GameObject Root;
    public Root RootScript;

    private Vector3 _maxRootScale;
    private Vector3 _minRootScale;

    public GameObject Eye;
    public GameObject Mouth;
    public GameObject Genital;

    private readonly List<ConfigurableJoint> _joints = new List<ConfigurableJoint>();

    public double Age;
    public float Energy;
    public float LowEnergyThreshold;

    public ChromosomeComposition ChromosomeComposition;

    public double LineOfSight;
    public float MetabolicRate;
    private int _ageSexualMaturity;

    public int Offspring;
    public int FoodEaten;

    private ArrayList _limbs;
    private ArrayList _allLimbs;

    private float _jointFrequency;
    private float _jointAmplitude;
    private float _jointPhase;

    private float _forceScalar = 1000F;

    public delegate void CreatureState(Creature c);

    public static event CreatureState CreatureDead;


    // a state machine to control the current... state of each creature
    public enum CurrentState
    {
        PersuingFood, // is following food
        PersuingMate, // is following a mate
        SearchingForMate, // is looking for a mate
        Mating, // is mating
        Eating, // is eating
        SearchingForFood, // is looking for food
        Dead, // is dead :(
        Neutral // is just drifting
    }

    public CurrentState State; // current state
    private bool _stateLock; // lock our current state

    public Eye EyeScript;
    public Vector3 TargetDirection;
    private Quaternion _lookRotation;
    private float _sine;
    private Vector3 _direction;

    private bool _lowEnergyLock;
    private MeshRenderer[] _ms;

    public GameObject NameText;
    public GameObject Badge;

    private void Start()
    {
        var foodPreference = Random.Range(0, 100);
        if (foodPreference >= 0 && foodPreference < 30)
            TypeOfCreature = "Carnivorous"; 
        if (foodPreference >= 30 && foodPreference <= 100)
            TypeOfCreature = "Herbiverous";

        _t = transform; // default transform of creature (parent)
        //name = "creature" + gameObject.GetInstanceID(); // name of creature (planning on randomising this)
        
        _t.gameObject.layer = LayerMask.NameToLayer("Creature");

        _environment = EnvironmentController.GetInstance();
        _settingsReader = SettingsReader.GetInstance();

        // everything from here is to do with the transforms of the creature and its limbs
        _maxRootScale = new Vector3
        {
            x = _settingsReader.RootMaxRootScaleX,
            y = _settingsReader.RootMaxRootScaleY,
            z = _settingsReader.RootMaxRootScaleZ
        };

        _minRootScale = new Vector3
        {
            x = _settingsReader.RootMinRootScaleX,
            y = _settingsReader.RootMinRootScaleY,
            z = _settingsReader.RootMinRootScaleZ
        };

        _jointFrequency = ChromosomeComposition.JointFrequency;
        _jointAmplitude = ChromosomeComposition.JointAmplitude;
        _jointPhase = ChromosomeComposition.JointPhase;

        RootSetup();

        EyeSetup();

        MouthSetup();

        GenitalSetup();

        VariableSetup();

        InvokeRepeating("UpdateState", 0, 5f); // update state every 5 seconds
        InvokeRepeating("RandomDirection", 1F, 5F); // update direction every 5 seconds

        Root.GetComponent<Rigidbody>().SetDensity(4F);
        Root.AddComponent<AudioSource>();
        _ms = GetComponentsInChildren<MeshRenderer>();
        ChromosomeComposition.SetTypeOfCreature(TypeOfCreature);
        // setup a name
        name = NameCreator.Name(ChromosomeComposition);
        NameSetup();
    }

    // Setup floating name
    private void NameSetup()
    {
        GameObject uiCanvas = GameObject.Find("Canvas");
        NameText = new GameObject();
        NameText.name = name;
        NameText.AddComponent<CanvasRenderer>();
        NameText.AddComponent<RectTransform>();
        Text text = NameText.AddComponent<Text>();
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.gameObject.layer = 5;
        text.font = (Font) Resources.Load("Fonts/Flow");
        text.fontSize = 24;
        text.transform.SetParent(uiCanvas.transform);
        text.text = name;
        UIDepthHandler uiDepth = UIDepthHandler.GetInstance();
        uiDepth.AddToCanvas(text);
        NameText.AddComponent<CreatureReference>();
        NameText.GetComponent<CreatureReference>().creatureReference = this.gameObject;
    }

    // Update name position to match creature position
    private void Name()
    {
        Vector3 namePosition = Camera.main.WorldToScreenPoint(Root.transform.position);
       // Vector3 namePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
        NameText.transform.position = namePosition;
    }

    // Set up variables
    private void VariableSetup()
    {
        LineOfSight = (double) _settingsReader.CreatureLineOfSight;
        MetabolicRate = _settingsReader.CreatureMetabolicRate;
        _ageSexualMaturity = (int) _settingsReader.CreatureAgeSexualMaturity;
        // transform of limbs done :)

        _allLimbs = new ArrayList(); // create array from limbs
        SetupLimbs(); // call setup on limbs

        Age = 0.0; // define age as being 0, newly born
        ChangeState(CurrentState.Neutral); // default state is neutral
        FoodEaten = 0; // by default a newly born creature has eaten nothing
        Offspring = 0; // by default a newly born creature has no offspring
        LowEnergyThreshold = _settingsReader.CreatureLowEnergyThreshold;
    }

    // Setup genital
    private void GenitalSetup()
    {
        Genital = new GameObject {name = "Genital"};
        Genital.transform.parent = Root.transform;
        Genital.transform.eulerAngles = Root.transform.eulerAngles;
        Genital.transform.localPosition = new Vector3(0, 0, -.5F);
        Genital.AddComponent<Genitalia>();
    }

    // Setup mouth
    private void MouthSetup()
    {
        Mouth = new GameObject {name = "Mouth"};
        Mouth.transform.parent = Root.transform;
        Mouth.transform.eulerAngles = Root.transform.eulerAngles;
        Mouth.transform.localPosition = new Vector3(0, 0, .5F);
        Mouth.AddComponent<Mouth>();
    }

    // Setup eye
    private void EyeSetup()
    {
        Eye = new GameObject {name = "Eye"};
        Eye.transform.parent = Root.transform;
        Eye.transform.eulerAngles = Root.transform.eulerAngles;
        Eye.transform.position = Root.transform.position;
        EyeScript = Eye.AddComponent<Eye>();
    }

    // Setup root
    private void RootSetup()
    {
        Root = (TypeOfCreature == "Herbiverous")
            ? GameObject.CreatePrimitive(PrimitiveType.Sphere)
            : GameObject.CreatePrimitive(PrimitiveType.Cube);
        //root = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Root.name = "root";
        Root.transform.parent = _t;
        Root.transform.position = _t.position;
        Root.transform.eulerAngles = _t.eulerAngles;
        Root.AddComponent<Rigidbody>();
        RootScript = Root.AddComponent<Root>();
        RootScript.SetColour(ChromosomeComposition.GetColour());
        RootScript.SetScale(ChromosomeComposition.GetRootScale());
        Root.GetComponent<Rigidbody>().angularDrag =
            _settingsReader.CreatureAngularDrag;
        Root.GetComponent<Rigidbody>().drag = _settingsReader.CreatureDrag;
    }

    private void FixedUpdate()
    {
        _sine = Sine(_jointFrequency, _jointAmplitude, _jointPhase);
        for (var i = 0; i < _joints.Count; i++)
            _joints[i].targetRotation = Quaternion.Euler(_sine * new Vector3(5F, 0F, 0F));

        if (EyeScript.Goal)
            TargetDirection = (EyeScript.Goal.transform.position - Root.transform.position).normalized;

        if (TargetDirection != Vector3.zero) _lookRotation = Quaternion.LookRotation(TargetDirection);

        var absSine = Mathf.Abs(_sine);
        var posSine = Math.Max(_sine, 0);
        Root.transform.rotation =
            Quaternion.Slerp(Root.transform.rotation, _lookRotation, Time.deltaTime * (absSine * 3F));

        if (posSine == 0) _direction = Root.transform.forward;

        Root.GetComponent<Rigidbody>()
            .AddForce(Mathf.Abs(_forceScalar) * _direction * posSine * ChromosomeComposition.GetBranchCount());
    }

    private float Sine(float freq, float amplitude, float phaseShift)
    {
        return Mathf.Sin((float) Age * freq + phaseShift) * amplitude;
    }

    private float _metaboliseTimer = 10F;

    private void Update()
    {
        Age += Time.deltaTime;
        Name();
        _metaboliseTimer -= Time.deltaTime;
        CheckEnergy();

        var force = _forceScalar;
        var jointFrequency = _jointFrequency;
        if (State == CurrentState.Mating)
        {
            _jointFrequency = 0F;
            _forceScalar = 0F;
        }
        else
        {
            _jointFrequency = jointFrequency;
            _forceScalar = force;
        }

        if (State == CurrentState.Dead) Energy = 0;

        if (Energy <= 0) Kill();

        /*
        // the next 6 if statements control what happens when a creature reaches the maximum positive or negative x, y, 
        // or z position in the world. Crossing beyond these limits will send them to the equivelent position on the 
        // opposite side of the map

        // positive x
        if (transform.position.x > _environment.WideSpread)
            transform.position = new Vector3(-_environment.WideSpread, transform.position.y, transform.position.z);

        // negative x
        if (transform.position.x < -_environment.WideSpread)
            transform.position = new Vector3(_environment.WideSpread, transform.position.y, transform.position.z);

        // positive y
        if (transform.position.y > _environment.WideSpreadY)
            transform.position = new Vector3(transform.position.x, -_environment.WideSpreadY, transform.position.z);

        // negative y
        if (transform.position.y < -_environment.WideSpreadY)
            transform.position = new Vector3(transform.position.x, _environment.WideSpreadY, transform.position.z);

        // positive z
        if (transform.position.z > _environment.WideSpread)
            transform.position = new Vector3(transform.position.x, transform.position.y, -_environment.WideSpread);

        // negative z
        if (transform.position.z < -_environment.WideSpread)
            transform.position = new Vector3(transform.position.x, transform.position.y, _environment.WideSpread);
            */
    }

    private void CheckEnergy()
    {
        if (_metaboliseTimer <= 0 && State != CurrentState.Dead)
        {
            Metabolise();
            _metaboliseTimer = 1F;
        }

        if (Energy <= LowEnergyThreshold && !_lowEnergyLock)
        {
            _lowEnergyLock = true;
            StartCoroutine(SlowDown());
            StartCoroutine(Darken());
        }

        if (Energy > LowEnergyThreshold)
        {
            _stateLock = false;
            _lowEnergyLock = false;
            StopCoroutine(SlowDown());
            StopCoroutine(Darken());
            Lighten();
            ResetSpeed();
        }
    }

    private void RandomDirection()
    {
        TargetDirection = new Vector3(
            Random.Range(-1F, 1F),
            Random.Range(-1F, 1F),
            Random.Range(-1F, 1F)
        );
    }

    public void SetEnergy(float n)
    {
        Energy = n;
    }

    private void UpdateState()
    {
        if (State == CurrentState.Mating) return;
        if (Energy < ChromosomeComposition.HungerPoint)
        {
            ChangeState(EyeScript.TargetFood != null ? CurrentState.PersuingFood : CurrentState.SearchingForFood);
        }

        if (Energy >= ChromosomeComposition.HungerPoint && Age > _ageSexualMaturity)
            ChangeState(EyeScript.TargetCreature != null ? CurrentState.PersuingMate : CurrentState.SearchingForMate);
    }

    public void Invokechromosome(ChromosomeComposition gs)
    {
        ChromosomeComposition = gs;
    }

    public void ChangeState(CurrentState s)
    {
        if (!_stateLock) State = s;
    }

    public float GetEnergy()
    {
        return Energy;
    }

    public bool SubtractEnergy(float n)
    {
        Energy -= n;
        if (!(Energy <= 0.0)) return false;
        Energy = 0;
        State = CurrentState.Dead;
        _stateLock = true;

        return true;
    }

    private void Metabolise()
    {
        SubtractEnergy(MetabolicRate);
    }

    public void Kill()
    {
        if (CreatureDead != null)
        {

            CreatureDead(this);
        }

        UIDepthHandler.GetInstance().Text.Remove(NameText.GetComponent<Text>());
        Destroy(NameText);

        // create food on death
        /*
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = CommonUtilities.RandomVector3(1, 1, 1);
            var fb = Instantiate((GameObject) Resources.Load("Prefabs/Foodbit"), pos, Quaternion.identity);
            var fbS = fb.AddComponent<Food>();
            fbS.Energy = Random.Range(5, 20);
            var scale = CommonUtilities.ConvertRange(fbS.Energy, 0.5f, 0.8f, 0.8f, 2);
            fb.transform.localScale = new Vector3(scale, scale, scale);
        }
        */
        Destroy(gameObject);
    }

    private void SetupLimbs()
    {
        var numBranches = ChromosomeComposition.GetBranchCount();
        ChromosomeComposition.SetNumberOfBranches(numBranches);

        var angXDrive = new JointDrive {mode = JointDriveMode.Position};
        for (var i = 0; i < numBranches; i++)
        {
            _limbs = ChromosomeComposition.GetLimbList(i);
            var actualLimbs = new List<GameObject>();

            var effects = new List<GameObject>();
            for (var j = 0; j < _limbs.Count; j++)
            {
                Limb limbScript;
                var limb = SetupSubLimb(i, j, actualLimbs, angXDrive, out limbScript);

                effects.Add(Resources.Load("Prefabs/particles01") as GameObject);
                effects.Add(Resources.Load("Prefabs/particles02") as GameObject);
                effects.Add(Resources.Load("Prefabs/particles03") as GameObject);

                if (j == _limbs.Count-1)
                {
                    var fx = effects[Random.Range(0, effects.Count - 1)];
                    Instantiate(fx, limb.transform.position, limb.transform.rotation).transform.parent = limb.transform;
                    if (TypeOfCreature == "Herbiverous"
                        ? Instantiate(effects[1], limb.transform.position, limb.transform.rotation).transform.parent =
                            limb.transform
                        : Instantiate(effects[0], limb.transform.position, limb.transform.rotation).transform.parent =
                            limb.transform) ;
                }

                _allLimbs.Add(limbScript);
            }
        }
    }

    private GameObject SetupSubLimb(int i, int j, List<GameObject> actualLimbs, JointDrive angXDrive, out Limb limbScript)
    {
        var limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
        limb.layer = LayerMask.NameToLayer("Creature");
        limb.name = "limb_" + i + "_" + j;
        limb.transform.parent = _t;
        actualLimbs.Add(limb);
        limbScript = limb.AddComponent<Limb>();

        var attributes = (ArrayList) _limbs[j];
        limbScript.SetScale((Vector3) attributes[1]);
        limbScript.SetColour(ChromosomeComposition.GetLimbColour());

        if (j == 0)
        {
            limbScript.SetPosition((Vector3) attributes[0]);
            limb.transform.LookAt(Root.transform);
        }
        else
        {
            limbScript.SetPosition(actualLimbs[j - 1].transform.localPosition);
            limb.transform.LookAt(Root.transform);
            limb.transform.Translate(0, 0, -actualLimbs[j - 1].transform.localScale.z);
        }

        limb.AddComponent<Rigidbody>();
        limb.AddComponent<BoxCollider>();
        limb.GetComponent<Collider>().material = (PhysicMaterial) Resources.Load("Physics Materials/Creature");

        var joint = limb.AddComponent<ConfigurableJoint>();
        joint.axis = new Vector3(0.5F, 0F, 0F);
        joint.anchor = new Vector3(0F, 0F, 0.5F);
        if (j == 0)
            joint.connectedBody = Root.GetComponent<Rigidbody>();
        else
            joint.connectedBody = actualLimbs[j - 1].GetComponent<Rigidbody>();
        limb.GetComponent<Rigidbody>().drag = 1F;

        _joints.Add(joint);

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        angXDrive.positionSpring = 7F;
        angXDrive.maximumForce = 100000000F;
        joint.angularXDrive = angXDrive;
        joint.angularYZDrive = angXDrive;

        limb.GetComponent<Rigidbody>().SetDensity(1F);
        return limb;
    }

    private float _dCol = 0.01F;

    private IEnumerator Darken()
    {
        var col = 1F;
        while (col > 0.15F && Energy < LowEnergyThreshold)
        {
            foreach (var m in _ms)
                m.material.color -= new Color(_dCol, _dCol, _dCol);
            col -= _dCol;
            yield return new WaitForSeconds(0.025F);
        }
    }

    private float _dFreq = 0.01F;
    private float _dForce = 0.01F;

    private IEnumerator SlowDown()
    {
        var freq = _jointFrequency;
        while (freq > 0.15F && Energy < LowEnergyThreshold)
        {
            freq -= _dFreq;
            _jointFrequency = freq;
            if (_forceScalar > 0F)
                _forceScalar -= _dForce;
            yield return new WaitForSeconds(0.025F);
        }
    }

    /// <summary>
    /// Lighten limb
    /// </summary>
    private void Lighten()
    {
        Root.GetComponent<MeshRenderer>().material.color = RootScript.OriginalColour;
        foreach (Limb l in _allLimbs) l.GetComponent<MeshRenderer>().material.color = l.OriginalColour;
    }

    private void ResetSpeed()
    {
        _jointFrequency = ChromosomeComposition.JointFrequency;
        _forceScalar = 1F;
    }
}