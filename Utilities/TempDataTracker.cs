using UnityEngine;
using System.Collections.Generic;

public class TempDataTracker : MonoBehaviour
{
    public delegate void Count();
    public static event Count DataUpdated;

    public static GameObject Container;
    public static TempDataTracker Instance;

    private EnvironmentController _environment;

    public List<int> FoodAvailable;

    public TempDataTracker(float logTime)
    {
    }

    // create an instance of this script as an object
    public static TempDataTracker GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject {name = "TempDataTracker"};
            Instance = Container.AddComponent<TempDataTracker>();
        }

        return Instance;
    }

    private void Start ()
    {
        _environment = EnvironmentController.GetInstance();
        FoodAvailable = new List<int>();
    }

    public float TotalCreatureEnergy()
    {
        float result = 0;
        foreach (Creature c in _environment.Creatures) result += c.Energy;
        return result;
    }

    internal float TotalFoodEnergy()
    {
        float result = 0;
        foreach (GameObject f in _environment.Foods) result += f.GetComponent<Food>().Energy;
        return result;
    }
}
