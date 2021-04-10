using UnityEngine;
using System.Collections;

public class CollisionMediator : MonoBehaviour {

    // instance variables
    public static CollisionMediator Instance;
    public static GameObject Container;

    public EventHandler Evt;
    public ArrayList CollisionEventsStorer;
    public EnvironmentController EnvironmentController;
    public AudioManager AudioManager;
    private SettingsReader _settings;

    private float _energyPassage;
    private double _crossoverRate;
    private double _mutationRate;
    private float _mutationFactor;

    // create an instance of this script as an object
    public static CollisionMediator GetInstance()
    {
        if (Instance) return Instance;
        Container = new GameObject { name = "Collision Observer" };
        Instance = Container.AddComponent<CollisionMediator>();

        return Instance;
    }

    private void Start()
    {
        // Build references
        CollisionEventsStorer = new ArrayList();
        EnvironmentController = EnvironmentController.GetInstance();
        AudioManager = AudioManager.GetInstance();
        _settings = SettingsReader.GetInstance();

        // Read settings variables
        _energyPassage = _settings.CreatureEnergyToOffspring;
        _crossoverRate = (double) _settings.GeneticsCrossoverRate;
        _mutationRate = (double) _settings.GeneticsMutationRate;
        _mutationFactor = _settings.GeneticsMutationFactor; 
    }

    public void ObserveColliders(GameObject colliderA, GameObject colliderB)
    {
        // Create a new event handler reference
        CollisionEventsStorer.Add(new EventHandler(colliderA, colliderB));

        // Check if the event already exists
        var eventDuplicate = CheckIfEventExists(colliderA, colliderB);

        // If the event doesn't exists, create a new handler
        if (null != eventDuplicate)
        {
            CollisionEventsStorer.Clear();
            // Capture the location of the collision and create a new reference point 0.5 units away from it
            var eventPosition = (colliderA.transform.position - colliderB.transform.position) * 0.5F +
                                colliderB.transform.position;

            // Get reference to the Creature scripts on each collision actor (a and b)
            var creatureAScript = colliderA.transform.parent.parent.GetComponent<Creature>();
            var creatureBScript = colliderB.transform.parent.parent.GetComponent<Creature>();

            // Get reference to the energy of each collision actor (a and b)
            var aEnergy = creatureAScript.GetEnergy();
            var bEnergy = creatureBScript.GetEnergy();

            // Create A new crossover between each actors chromosome structure
            var newChromosome = GeneticsUtilities.Crossover(
                creatureAScript.ChromosomeComposition,
                creatureBScript.ChromosomeComposition,
                _crossoverRate);

            // Mutate the new chromosome
            newChromosome = GeneticsUtilities.Mutate(
                newChromosome,
                _mutationRate,
                _mutationFactor);

            // Transfer energy to child creature
            var creatureAEnergyToChild = aEnergy * _energyPassage;
            var creatureBEnergyToChild = bEnergy * _energyPassage;
            var newCreatureEnergy = creatureAEnergyToChild + creatureBEnergyToChild;

            // Spawn the child creature
            EnvironmentController.SpawnerOrganiser.Spawn(
                eventPosition, Vector3.zero,
                newCreatureEnergy,
                newChromosome
            );

            // Deduct energy from each parent creature
            creatureAScript.SetEnergy(aEnergy - creatureAEnergyToChild);
            creatureBScript.SetEnergy(bEnergy - creatureBEnergyToChild);

            // Increment each creatures offspring counter
            creatureAScript.Offspring++;
            creatureBScript.Offspring++;
        }
        else
        {
            // Create new event
            CollisionEventsStorer.Add(new EventHandler(colliderB, colliderA));
        }
    }

    private EventHandler CheckIfEventExists(GameObject a, GameObject b)
    {
        for (var index = 0; index < CollisionEventsStorer.Count; index++)
        {
            var eventA = (EventHandler) CollisionEventsStorer[index];
            var eventOne = eventA.GetColliders()[0];
            var eventTwo = eventA.GetColliders()[1];
            // if the object signalling the collision exists in another event - there is a duplicate
            if (b.GetInstanceID() != eventOne.GetInstanceID() &&
                a.GetInstanceID() != eventTwo.GetInstanceID()) continue;
            return eventA;
        }

        return null;
    }
}
