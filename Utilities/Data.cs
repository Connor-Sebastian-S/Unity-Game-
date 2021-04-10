using UnityEngine;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public delegate void Count();
    public static event Count DataUpdated;

    public static GameObject Container;
    public static Data Instance;

    private EnvironmentController _environment;

    public List<int> FoodAvailable;

    public static Data GetInstance()
    {
        if (Instance) return Instance;
        Container = new GameObject {name = "Data"};
        Instance = Container.AddComponent<Data>();

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

    internal float TotalfoodEnergy()
    {
        float result = 0;
        foreach (GameObject f in _environment.Foods) result += f.GetComponent<Food>().Energy;
        return result;
    }
}
