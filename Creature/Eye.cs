using UnityEngine;

public class Eye : MonoBehaviour
{
    private Creature _myCreature;
    private Food _fbit;
	public Creature TargetCreature;
    private CollisionMediator _collisionHandler;
    private SettingsReader _settingsReader;

    public GameObject TargetFood;

    public float CurrentDistance;
    private float _eyeRefreshRate;

    private double _creatureMateRange;
    private double _creatureEatRange;
    
    private double _lineOfSight;
	
	public Collider[] ColliderCloud;

    private Transform _myTransform;

    
    private Creature _otherCreature;

	public GameObject Goal;
	public float DistanceToGoal;
    private Transform _root;

    private void Start ()
    {
		_myTransform = transform;
		
		_myCreature = _myTransform.parent.parent.gameObject.GetComponent<Creature>();

		_collisionHandler = CollisionMediator.GetInstance();
		_settingsReader = SettingsReader.GetInstance();

        InitialiseVariables();

        _root = _myTransform.parent;

		InvokeRepeating("RefreshVision", 0, _eyeRefreshRate); // Refresh vision every n seconds
	}

    private void InitialiseVariables()
    {
        if (!_settingsReader)
        {
            _settingsReader = SettingsReader.GetInstance();
        }
        else
        {
            _creatureMateRange = (double) _settingsReader.CreatureMateRange;
            _creatureEatRange = (double) _settingsReader.CreatureEatRange;
            _eyeRefreshRate = _settingsReader.CreatureEyeRefreshRate;
            _lineOfSight = _myCreature.LineOfSight;
        }
    }

    private void RefreshVision ()
    {
        TargetCreature = null;
		switch (_myCreature.State)
        {
		case Creature.CurrentState.PersuingMate:
		    IdentifySimilarCreature();
			break;
		case Creature.CurrentState.SearchingForMate:
		    IdentifySimilarCreature();
			break;
		case Creature.CurrentState.PersuingFood:
			IdentifyClosestfood();
			break;
		case Creature.CurrentState.SearchingForFood:
			IdentifyClosestfood();
			break;
		}
	}

    private void IdentifySimilarCreature()
    {
		TargetCreature 			= null;	// Reference to the script of the closest creature
		GameObject target 		= null;
        float similarity		= Mathf.Infinity;
        ColliderCloud = Physics.OverlapSphere(_myTransform.position, (float)_lineOfSight);

		if (ColliderCloud.Length == 0)
        {
			target = null;
			return;
		}

		foreach (Collider col in ColliderCloud)
        {
			var currentCollider = col.transform.gameObject; // Reference of the current collider
			if (currentCollider && currentCollider.gameObject.name == "root" && currentCollider != _myCreature.Root.gameObject)
            {
				_otherCreature = currentCollider.transform.parent.GetComponent<Creature>();
				var currentSimilarity = GeneticsUtilities.CheckSimilarColour (_myCreature.ChromosomeComposition, _otherCreature.ChromosomeComposition);

				if (currentSimilarity < similarity)
                {
					target = currentCollider.transform.parent.gameObject;
					similarity = currentSimilarity;
				}

				Vector3 locationDifference = currentCollider.transform.position - _myTransform.position;
				if (locationDifference.magnitude < (float)_creatureMateRange)
                {
					_otherCreature = currentCollider.transform.parent.GetComponent<Creature>();
					Genitalia otherGenital = _otherCreature.Genital.GetComponent<Genitalia>();

                    if (_myCreature.State != Creature.CurrentState.PersuingMate &&
                        _otherCreature.State != Creature.CurrentState.PersuingMate)
                    {
                    }
                    else
                    {
                        _collisionHandler.ObserveColliders(_myCreature.Genital.gameObject, otherGenital.gameObject);
                        _otherCreature.ChangeState(Creature.CurrentState.Mating);
                        _myCreature.ChangeState(Creature.CurrentState.Mating);
                    }

                    similarity = currentSimilarity;
				}
			}

			DistanceToGoal = 0F;
			Goal = null;
            if (!target) continue;
            TargetCreature = target.GetComponent<Creature>();
            Goal = TargetCreature.Root;
            DistanceToGoal = DistanceUntilGoal();
        }
	}

    private void ClosestCreature ()
    {
		TargetCreature 				= null;	// Reference to the script of the closest creature
		GameObject target 		= null;
        var dist 				= Mathf.Infinity;
		ColliderCloud = Physics.OverlapSphere(_myTransform.position, (float)_lineOfSight);

        if (ColliderCloud.Length != 0)
        {
            foreach (Collider col in ColliderCloud)
            {
                var currentCollider 			= col.transform.gameObject; // Reference to the current collider being looked at
                if (currentCollider && currentCollider.gameObject.name == "root" &&
                    currentCollider != _myCreature.Root.gameObject)
                {
                    Vector3 diff = currentCollider.transform.position - _myTransform.position;
                    CurrentDistance = diff.magnitude;
                    _otherCreature = currentCollider.transform.parent.GetComponent<Creature>();


                    if (CurrentDistance < dist)
                    {
                        target = currentCollider.transform.parent.gameObject;
                        dist = CurrentDistance;
                    }

                    if (CurrentDistance < (float) _creatureMateRange)
                    {
                        _otherCreature = currentCollider.transform.parent.GetComponent<Creature>();
                        Genitalia otherGenital = _otherCreature.Genital.GetComponent<Genitalia>();
                        if (_myCreature.State == Creature.CurrentState.PersuingMate ||
                            _otherCreature.State == Creature.CurrentState.PersuingMate)
                        {
                            _collisionHandler.ObserveColliders(_myCreature.Genital.gameObject, otherGenital.gameObject);
                            _otherCreature.ChangeState(Creature.CurrentState.Mating);
                            _myCreature.ChangeState(Creature.CurrentState.Mating);
                        }

                        dist = CurrentDistance;
                    }
                }

                DistanceToGoal = 0F;
                Goal = null;
                if (!target) continue;
                TargetCreature = target.GetComponent<Creature>();
                Goal = TargetCreature.Root;
                DistanceToGoal = DistanceUntilGoal();
            }
        }
        else
        {
            target = null;
            return;
        }
    }

    private void IdentifyClosestfood ()
    {
        // we need to adjust the line of sight for carnivorous creatures as opposed to
        // herbivorous creatures -simply to ensure that they can find the player
		TargetFood 		= null;	// reference to the script of the closest food
		GameObject closest 	= null;
		float dist 			= Mathf.Infinity;
        if (_myCreature.TypeOfCreature == "Carnivorous")
        {
            ColliderCloud = Physics.OverlapSphere(_myTransform.position, (float)_lineOfSight);

            int l = Random.Range(0, 100);
            // 70% chance that the creature will just target the player (resets every 5 seconds)
            if (l >= 0 && l < 70)
            {
                GameObject f = GameObject.FindGameObjectWithTag("Player");
                Vector3 locationDifference = f.transform.position - _myTransform.position;
                float currDist = locationDifference.magnitude;
                if (currDist < dist)
                {
                    closest = f;
                    dist = currDist;
                }

                if (f.tag == "Player")
                {
                    EffectTrigger et = f.GetComponent<EffectTrigger>();
                    et.TriggerAudio();
                }

                float _tempRange = 2.5f;
                if (currDist < _tempRange && (_myCreature.State == Creature.CurrentState.PersuingFood))
                {
                    if (f.tag == "Player")
                    {
                        PlayerStats ps = f.GetComponent<PlayerStats>();
                        EffectTrigger et = f.GetComponent<EffectTrigger>();
                        _myCreature.Energy += 100f;
                        ps.RemoveEnergy(20f);
                        et.PlayerEaten(f.transform.position);
                        f.GetComponent<ControllerSetup>().AccelerateAway();
                        ps.RemoveFood();
                        _myCreature.FoodEaten++;
                        ps.AddDevour(1);
                    }
                }
            }
            else
            {
                foreach (Collider c in ColliderCloud)
                {
                    GameObject f = c.gameObject;

                    if (f)
                    {
                        f = f.transform.root.gameObject;
                        if (f.tag == "Creature" || f.tag == "Player")
                        {
                            _myCreature.TargetFood = f.gameObject;
                            Vector3 locationDifference = f.transform.position - _myTransform.position;
                            float currDist = locationDifference.magnitude;
                            if (currDist < dist)
                            {
                                closest = f;
                                dist = currDist;
                            }

                            if (f.tag == "Player")
                            {
                                EffectHandler et = f.GetComponent<EffectHandler>();
                                et.TriggerAudio();
                            }

                            float tempRange = 2.5f;
                            if (currDist < tempRange && (_myCreature.State == Creature.CurrentState.PersuingFood))
                            {
                                if (f.tag == "Player")
                                {
                                    PlayerStats ps = f.GetComponent<PlayerStats>();
                                    EffectHandler et = f.GetComponent<EffectHandler>();
                                    _myCreature.Energy += 100f;
                                    ps.RemoveEnergy(20f);
                                    et.PlayerEaten(f.transform.position);
                                    f.GetComponent<ControllerSetup>().AccelerateAway();
                                    ps.RemoveFood();
                                    _myCreature.FoodEaten++;
                                    ps.AddDevour(1);
                                }
                                else if (f.GetComponent<Creature>().TypeOfCreature == "Herbiverous")
                                {
                                    Creature ct = f.GetComponent<Creature>();
                                    ct.Energy -= 100f;
                                    _myCreature.Energy += 100f;
                                }
                            }

                            if (currDist < (float) _creatureEatRange && (_myCreature.State == Creature.CurrentState.PersuingFood))
                            {
                                if (f.name == "food" && _myCreature.TypeOfCreature == "Herbiverous")
                                {
                                    _fbit = f.GetComponent<Food>();
                                    _myCreature.Energy += _fbit.Energy;
                                    _fbit.Destroy();
                                    _myCreature.FoodEaten++;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            ColliderCloud = Physics.OverlapSphere(_myTransform.position, (float)_lineOfSight);

            foreach (Collider c in ColliderCloud)
            {
                GameObject f = c.gameObject;

                if (f && f.name == "food" || f)
                {
                    _myCreature.TargetFood = f.gameObject;
                    Vector3 diff = f.transform.position - _myTransform.position;
                    float currDist = diff.magnitude;
                    if (currDist < dist)
                    {
                        closest = f;
                        dist = currDist;
                    }

                    if (currDist < (float)_creatureEatRange && (_myCreature.State == Creature.CurrentState.PersuingFood))
                    {
                        if (f.name == "food" && _myCreature.TypeOfCreature == "Herbiverous")
                        {
                            _fbit = f.GetComponent<Food>();
                            _myCreature.Energy += _fbit.Energy;
                            _fbit.Destroy();
                            _myCreature.FoodEaten++;
                        }
                    }
                }
            }
        }
		
        DistanceToGoal = 0F;
		if (closest)
        {
			TargetFood = closest.gameObject;
		}

		Goal = TargetFood;
		if (Goal)
        {
            DistanceToGoal = DistanceUntilGoal();
		}
	}

	public float DistanceUntilGoal ()
    {
		if (Goal)
			return Vector3.Distance(_root.position, Goal.transform.position);
		else
			return 0F;
	}
}
