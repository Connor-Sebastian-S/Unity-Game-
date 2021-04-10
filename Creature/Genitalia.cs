using UnityEngine;

public class Genitalia : MonoBehaviour {

    private Creature _myCreature;
    private SpawnerOrganiser _spawnerOrganiser;
    private SettingsReader _settingsReader;
    private Transform _transformT;
    private LineRenderer _lineRenderer;

    private float _lineLength			= 0.05F;
    private float _lineWidth  			= 0.05F;

    private double _timeCreated;
    private double _timeToEnableMating 	= 1.0F;

    private Eye _myEye;

    private void Start()
    {
        _settingsReader = SettingsReader.GetInstance();

        _transformT = transform;
        gameObject.tag = "Genital";
        _myCreature = (Creature) _transformT.parent.parent.gameObject.GetComponent("Creature");
        _myEye = _myCreature.Eye.gameObject.GetComponent<Eye>();

        _transformT = transform;
        _timeCreated = Time.time;

        _lineLength = _settingsReader.GenitaliaLineLength;
    }

    private void Update()
    {
        Pursuing();

        Reset();
    }

    private void Reset()
    {
        // If the creature has seen a mate
        if (!_myEye.TargetCreature || _myCreature.State != Creature.CurrentState.PersuingMate)
        {
            new Vector3(0, 0, 0);
            new Vector3(0, 0, -_lineLength);
        }
        else
        {
            new Vector3(_myEye.TargetCreature.Genital.transform.position.x,
                _myEye.TargetCreature.Genital.transform.position.y,
                _myEye.TargetCreature.Genital.transform.position.z
            );
            ResetStart();
        }
    }

    private void Pursuing()
    {
        // If creature is currently mating and the time has been reached for them to mate
        if (_myCreature.State != Creature.CurrentState.Mating ||
            !(Time.time > _timeCreated + _timeToEnableMating)) return;
        _myCreature.State = Creature.CurrentState.PersuingMate; // Set state to pursuing mate
        _timeCreated = Time.time;
    }


    private void ResetStart () {
		new Vector3(_transformT.position.x,_transformT.position.y,_transformT.position.z);
	}

}
