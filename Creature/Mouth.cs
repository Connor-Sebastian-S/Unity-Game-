using UnityEngine;

public class Mouth : MonoBehaviour {
    private Food _food;
    private Creature _myCreature;
    private Eye _myEye;

    private Transform _transformM;
    private LineRenderer _lineRenderer;

    private float _lineLength 	= 0.05F;
    private float _lineWidth 	= 0.05F;

    private int _foodDetectionRange = 40;

    private GameObject _creatureTarget;

    private void Start()
    {
        _transformM = transform;
        _myCreature = (Creature) _transformM.parent.parent.gameObject.GetComponent("Creature");
        _myEye = _myCreature.Eye.GetComponent<Eye>();
    }

    private void Update()
    {
        _creatureTarget = _myEye.TargetFood;
        PusueFood();
    }

    /// <summary>
    /// Set and follow food
    /// </summary>
    private void PusueFood()
    {
        if (_creatureTarget && _myCreature.State == Creature.CurrentState.PersuingFood)
        {
            new Vector3(_creatureTarget.transform.position.x,
                _creatureTarget.transform.position.y,
                _creatureTarget.transform.position.z
            );
            ResetStart();
        }
        else
        {
            new Vector3(0, 0, _lineLength);
        }
    }

    private void ResetStart()
    {
        new Vector3(_transformM.position.x, _transformM.position.y, _transformM.position.z);
    }

    /// <summary>
    /// Return detection scale
    /// </summary>
    /// <returns></returns>
    private float GetDetectRadius()
    {
        return _foodDetectionRange;
    }

}
