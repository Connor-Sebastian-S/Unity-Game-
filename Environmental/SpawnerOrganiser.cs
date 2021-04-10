using UnityEngine;

public class SpawnerOrganiser : MonoBehaviour
{	
	public static SpawnerOrganiser Instance;
    private Logger _lg;
    public CreatureCounterObservor CrtCounterObservor;
    private GameObject _crt;
    private static GameObject _container;
    private Vector3 _pos;

    public delegate void Crt(Creature c);
    public static event Crt CreatureSpawned;

    private void Start()
    {
        TempDataTracker.GetInstance();
        EnvironmentController.GetInstance();
        CrtCounterObservor = CreatureCounterObservor.GetInstance();
    }

    public static SpawnerOrganiser GetInstance()
    {
        if (!Instance)
        {
            _container = new GameObject {name = "Spawner"};
            Instance = _container.AddComponent(typeof(SpawnerOrganiser)) as SpawnerOrganiser;
        }

        return Instance;
    }

    public void Spawn(Vector3 pos, Vector3 rot, float energy, ChromosomeComposition chromosomeComposition)
    {
        var child = new GameObject();
        child.transform.localPosition = pos;
        child.transform.eulerAngles = Utility.RandomRotationalVector();
        var crtScript = child.AddComponent<Creature>();
        child.tag = "Creature";
        crtScript.Invokechromosome(chromosomeComposition);
        crtScript.SetEnergy(energy);
        if (CreatureSpawned != null) CreatureSpawned(crtScript);
    }
}
