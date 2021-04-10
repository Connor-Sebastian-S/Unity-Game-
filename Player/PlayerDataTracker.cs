using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int EvolutionLevel = 1;
    public float PlayerXPosition;
    public float PlayerYPosition;
    public float PlayerZPosition;
    public int FoodEaten;
    public int TimesEaten;
    public float Energy;
    public float Speed;
}

public class PlayerDataTracker : MonoBehaviour
{
    public PlayerData Dt;
    public static PlayerDataTracker Instance;
    public static GameObject Container;

    private EffectTrigger _et;
    private SettingsReader _st;

    public static PlayerDataTracker GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject();
            Container.name = "PlayerDataTracker";
            Instance = Container.AddComponent(typeof(PlayerDataTracker)) as PlayerDataTracker;
        }

        return Instance;
    }

    private void Awake()
    {
        Dt = new PlayerData();
        _st = SettingsReader.GetInstance();
    }

    private void Start()
    {
        _et = GameObject.FindWithTag("Player").GetComponent<EffectTrigger>();
    }

    public void SetEvolutionLevel(int i)
    {
        Dt.EvolutionLevel = i;
        _et.PlayerEvolves(_et.gameObject.transform.position);
    }

    public int GetEvolutionLevel()
    {
        return Dt.EvolutionLevel;
    }

    public void SetEaten(int i)
    {
        Dt.TimesEaten += i;
    }

    public int GetEaten()
    {
        return Dt.TimesEaten;
    }

    public void SetEnergy(float e)
    {
        float l = Dt.Energy;
        l += e;
        Dt.Energy = l;
    }

    public void SetFood(int i)
    {
        Dt.FoodEaten += i;       
    }

    public int GetFood()
    {
        return Dt.FoodEaten;
    }

    public void RemoveEnergy(float i)
    {
        Dt.Energy -= i;
    }

    public float GetEnergy()
    {
        return Dt.Energy;
    }

    public void SetSpeed(float i)
    {
        Dt.Speed = i;
    }

    public float GetSpeed()
    {
        return Dt.Speed;
    }

    public float GetPlayerX()
    {
        return Dt.PlayerXPosition;
    }

    public float GetPlayerY()
    {
        return Dt.PlayerYPosition;
    }

    public float GetPlayerZ()
    {
        return Dt.PlayerZPosition;
    }

    public void SetPlayerX(float x)
    {
        Dt.PlayerXPosition = x;
    }

    public void SetPlayerY(float y)
    {
        Dt.PlayerYPosition = y;
    }

    public void SetPlayerZ(float z)
    {
        Dt.PlayerZPosition = z;
    }

    // debug ui
    private void OnGui()
    {
        if (_st.DevDebug.ToString() == "true")
        {
            GUI.Label(new Rect(0, 10, 150, 30), "---Player Debug---");
            if (GUI.Button(new Rect(10, 50, 150, 30), "Trigger Eaten"))
                SetEaten(1);
            if (GUI.Button(new Rect(10, 90, 150, 30), "Trigger Eat and Add 10 Energy"))
            {
                SetFood(1);
                SetEnergy(10);
            }

            GUI.Label(new Rect(10, 130, 150, 30), "Food eaten: " + GetFood());
            GUI.Label(new Rect(10, 170, 150, 30), "Times eaten: " + GetEaten());
            GUI.Label(new Rect(10, 210, 150, 30), "Energy: " + GetEnergy());
            GUI.Label(new Rect(10, 250, 150, 30), "Evolution level: " + GetEvolutionLevel());
        }
    }
}
