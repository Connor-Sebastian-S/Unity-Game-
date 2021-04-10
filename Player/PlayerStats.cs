using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private PlayerController _pc;
    private ControllerSetup _cs;
    private PlayerDataTracker _pt;
    public string PlayerStatus;
    public Main Mn;

    private int _lastCheck;
    private int _curCheck = 0;

    public SettingsReader SettingsReader;

    private List<bool> _evolutonBools;
    private bool _reached;

    public void RemoveEnergy(float n)
    {
        _pt.RemoveEnergy(n);
    }

    public void AddFood(int n, float e)
    {
        _pt.SetFood(n);
        _pt.SetEnergy(e);
    }

    public void RemoveFood()
    {
        if (_pt.GetEvolutionLevel() == 2)
            _pt.Dt.FoodEaten = 5;
        if (_pt.GetEvolutionLevel() == 3)
            _pt.Dt.FoodEaten = 15;
        if (_pt.GetEvolutionLevel() == 4)
            _pt.Dt.FoodEaten = 25;
        if (_pt.GetEvolutionLevel() == 5)
            _pt.Dt.FoodEaten = 35;
    }

    public void AddDevour(int n)
    {
        _pt.SetEaten(n);
    }

    private void Awake()
    {
        _evolutonBools = new List<bool>();
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);
        _evolutonBools.Add(false);

        SettingsReader = SettingsReader.GetInstance();
        Mn = GameObject.Find("Controller").GetComponent<Main>();
        _pc = GetComponent<PlayerController>();
        _cs = GetComponent<ControllerSetup>();
        
        _pt = PlayerDataTracker.GetInstance();
    }

    private void Update()
    {
        if (SettingsReader && Mn && _pc && _cs && _pt)
        {

            if (_reached == false)
                EndGameHandler();
            else
                ClickerHandler();
            float l = _pt.GetEnergy();
            _pt.Dt.Energy -= Time.deltaTime;
            CheckStats();
            CheckEnergy();
        }
    }

    public Color LColour = Color.white;
    public Color RColour = Color.red;


    private void CheckEnergy()
    {
        _lastCheck = _pt.GetFood();
        if (_pt.GetEnergy() <= 100)
        {
            var lerpParam = Mathf.InverseLerp(100, 0, _pt.GetEnergy());
            foreach (var child in transform.GetComponentsInChildren<Renderer>())
            {
                child.material
                    .SetColor("_Color", Color.Lerp(LColour, RColour, lerpParam));
            }          
        }
        else
        {
            foreach (var child in transform.GetComponentsInChildren<Renderer>())
            {
                child.material
                    .SetColor("_Color", LColour);
            }
        }

        if (_pt.GetEnergy() <= 0)
        {
            _pc.DeathStart();
        }
    }

    // Handle what happens when the player reaches the end of the level (i.e. the bottom)
    private void EndGameHandler()
    {
        if (transform.position.y <= 0)
        {
            _reached = true;
            if (_reached)
            {
                print("end game");
                GameObject c = GameObject.Find("Controller");
                c.GetComponent<Metronome>().Bpm = 80;
                c.GetComponent<Metronome>().StopMetronome(true);
            }
        } 
    }

    private void ClickerHandler()
    {
        if (_reached == true && transform.position.y >= SettingsReader.FoodWideSpreadY)
        {
            Mn.Exit();
        }
    }

    private void Play()
    {
        GetComponent<EffectHandler>().TriggerAudio();
    }

    public void CheckStats()
    {
        if (_pt.GetFood() >= 0 && _pt.GetFood() < 10 && _evolutonBools[0] == false)
        {
            _pt.SetEvolutionLevel(1);
            _pt.SetSpeed(3);
            _evolutonBools[0] = true;
            _evolutonBools[1] = false;
            _evolutonBools[2] = false;
            _evolutonBools[3] = false;
            _evolutonBools[4] = false;
            _evolutonBools[5] = false;
            _evolutonBools[6] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(1);
        }

        if (_pt.GetFood() >= 10 && _pt.GetFood() < 20 && _evolutonBools[1] == false)
        {
            _pt.SetEvolutionLevel(2);
            _pt.SetSpeed(5);
            _evolutonBools[0] = false;
            _evolutonBools[1] = true;
            _evolutonBools[2] = false;
            _evolutonBools[3] = false;
            _evolutonBools[4] = false;
            _evolutonBools[5] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(2);
        }

        if (_pt.GetFood() >= 20 && _pt.GetFood() < 30 && _evolutonBools[2] == false)
        {
            _pt.SetEvolutionLevel(3);
            _pt.SetSpeed(6);
            _evolutonBools[0] = false;
            _evolutonBools[1] = false;
            _evolutonBools[2] = true;
            _evolutonBools[3] = false;
            _evolutonBools[4] = false;
            _evolutonBools[5] = false;
            _evolutonBools[6] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(3);
        }

        if (_pt.GetFood() >= 30 && _pt.GetFood() < 40 && _evolutonBools[3] == false)
        {
            _pt.SetEvolutionLevel(4);
            _pt.SetSpeed(7.5f);
            _evolutonBools[0] = false;
            _evolutonBools[1] = false;
            _evolutonBools[2] = false;
            _evolutonBools[3] = true;
            _evolutonBools[4] = false;
            _evolutonBools[5] = false;
            _evolutonBools[6] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(4);
        }

        if (_pt.GetFood() >= 40 && _pt.GetFood() < 50 && _evolutonBools[4] == false)
        {
            _pt.SetEvolutionLevel(5);
            _pt.SetSpeed(9);
            _evolutonBools[0] = false;
            _evolutonBools[1] = false;
            _evolutonBools[2] = false;
            _evolutonBools[3] = false;
            _evolutonBools[4] = true;
            _evolutonBools[5] = false;
            _evolutonBools[6] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(5);
        }

        if (_pt.GetFood() >= 50 && _pt.GetFood() < 60 && _evolutonBools[5] == false)
        {
            _pt.SetEvolutionLevel(6);
            _pt.SetSpeed(10);
            _evolutonBools[0] = false;
            _evolutonBools[1] = false;
            _evolutonBools[2] = false;
            _evolutonBools[3] = false;
            _evolutonBools[4] = false;
            _evolutonBools[5] = true;
            _evolutonBools[6] = false;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Evolve(6);
        }

        if (_pt.GetFood() >= 60&& _evolutonBools[6] == false)
        {
            _pt.SetEvolutionLevel(7);
            _evolutonBools[0] = false;
            _evolutonBools[1] = false;
            _evolutonBools[2] = false;
            _evolutonBools[3] = false;
            _evolutonBools[4] = false;
            _evolutonBools[5] = false;
            _evolutonBools[6] = true;
            Debug.Log(_pt.GetEvolutionLevel());
            _pc.Final();
        }

    }
}
