using UnityEngine;

public class CreatureCounterObservor : MonoBehaviour
{
    private int _creatureCapture;
	public int NumberOfCreatures;
	public static GameObject Container;
	public static CreatureCounterObservor Instance;


    private void OnEnable ()
    {
        SpawnerOrganiser.CreatureSpawned += OnSpawn; // increment spawn count
        Creature.CreatureDead += OnDeath; // increment death count
    }

    private void OnDisable()
    {
        SpawnerOrganiser.CreatureSpawned -= OnSpawn; // decrement spawn count
        Creature.CreatureDead += OnDeath; // increment death count
    }

	public void OnSpawn (Creature x)
    {
        NumberOfCreatures += 1;
    }

	public void OnDeath (Creature x)
    {
        NumberOfCreatures -= 1;
    }

    // create an instance of this script as an object
    public static CreatureCounterObservor GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject {name = "CreatureCounterObservor"};
            Instance = Container.AddComponent(typeof(CreatureCounterObservor)) as CreatureCounterObservor;
        }

        return Instance;
    }
}
