using UnityEngine;

public class CreatureCount : MonoBehaviour
{
    private int _creatureCapture;
	public int NumberOfCreatures;
	public static GameObject Container;
	public static CreatureCount Instance;


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

    public static CreatureCount GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject {name = "CreatureCount"};
            Instance = Container.AddComponent(typeof(CreatureCount)) as CreatureCount;
        }

        return Instance;
    }
}
