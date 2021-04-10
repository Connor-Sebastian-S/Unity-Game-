using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MetronomeEvent(Metronome metronome);

// crude implimentation of actorA metronome, it isn't incredibly accurate but accurate enough for our needs
public class Metronome : MonoBehaviour {
	public int Counter;
	private SettingsReader _settingsReader;
	public List<string> Result;

   //Signature 
    public int Base;
    public int Step;
	
	//Beats Per Minute
    public float Bpm;
	//keep track of what step/measure we're on
    public int CurrentStep = 1;
    public int CurrentMeasure;

	//interval between notes
    private float _interval;
	
	//relative moment of when the beat will actually occur
    private float _nextTime;

    public event MetronomeEvent OnTick;
    public event MetronomeEvent OnNewMeasure;

    private bool _isIndexReached = false;
    private int _newIndex;
	public int Index;
    public string CurrentNote;

    private AudioManager _am;
    private AudioSource _audioSource;

    private bool _endReached;
    private float _speed = 0.1F;
    private float _fraction;
    private Vector3 _s;
    private Vector3 _e = new Vector3(0, 0, 0);

    public GameObject Player;

    public void BuildNoteList(List<string> l)
    {
        Result.Clear();
        Result = l;
    }

    private void Awake()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        _am = AudioManager.GetInstance();
        _settingsReader = SettingsReader.GetInstance();
        Bpm = (int) _settingsReader.AudioBpm;
        Base = (int) _settingsReader.AudioBase;
        Step = (int) _settingsReader.AudioStep;

        var d = _settingsReader.FoodWideSpreadY;
        _s = new Vector3(0, d, 0);
        
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        StartMetronome();
    }

    public void StartMetronome()
    {
        StopCoroutine("DoTick");
        CurrentStep = 1;
        var multiplier = Base / 4f;
        var tmpInterval = 60f / Bpm;
        _interval = tmpInterval / multiplier;
        _nextTime = Time.time;
        StartCoroutine("DoTick");
    }


    public void StopMetronome(bool state)
    {
        if (state == true)
        {
            StopCoroutine("DoTick");
            Index = 0;
            CurrentStep = 1;
            Counter = 0;
            var multiplier = Base / 4f;
            var tmpInterval = 60f / Bpm;
            _interval = tmpInterval / multiplier;
            _nextTime = Time.time;
            _endReached = true;
            StartCoroutine("DoEndTick");
        }
        else if (state == false)
        {
            StopCoroutine("DoEndTick");
        }
    }

    private void Update()
    {
        if (_endReached)
        {
            if (_fraction < 1)
            {
                _fraction += Time.deltaTime * _speed;
                Player.transform.position = Vector3.Lerp(_e, _s, _fraction);
            }
        }
    }

    private IEnumerator DoEndTick()
    {
        for (;;)
        {
            if (CurrentStep == 1 && OnNewMeasure != null)
                OnNewMeasure(this);
            if (OnTick != null)
                OnTick(this);
            _nextTime += _interval;
            yield return new WaitForSeconds(_nextTime - Time.time);
            CurrentStep++;
            if (CurrentStep > Step)
            {
                CurrentStep = 1;
                CurrentMeasure++;
            }

            Counter += 1;

           if (Index == 10) StopMetronome(false); // loop our piece

            CurrentNote = Result[Index];
            AudioClip currentAudioClip = _am.ViolinAudioClips.Find(x => x.name == CurrentNote);
            if (currentAudioClip != null)
            {
                _audioSource.volume = 0.5f;
                _audioSource.PlayOneShot(currentAudioClip, 0.5F);
            }
            else
            {
            }

            if (Counter == Bpm)
                Counter = 0;
            Index += 1;
        }
    }


    private IEnumerator DoTick()
    {
        // common time (4/4) 60 BPM (beats per minute) indicate that 
        // there should be 60 quarter notes per minute
        for (;;)
        {
            if (CurrentStep == 1 && OnNewMeasure != null)
                OnNewMeasure(this);
            if (OnTick != null)
                OnTick(this);
            _nextTime += _interval;
            yield return new WaitForSeconds(_nextTime - Time.time);
            CurrentStep++;
            if (CurrentStep > Step)
            {
                CurrentStep = 1;
                CurrentMeasure++;
            }

            Counter += 1;

            if (Index == Result.Count) Index = 0; // loop our piece

            CurrentNote = Result[Index];
            AudioClip currentAudioClip = _am.PianoAudioClips.Find(x => x.name == CurrentNote);
            if (currentAudioClip != null)
            {
                //Debug.Log("On currrent note " + _currentNote + ", we found it");
                _audioSource.volume = 0.5f;
                _audioSource.PlayOneShot(currentAudioClip, 0.3F);
            }
            else
            {
                //Debug.Log("On currrent note " + _currentNote + ", we didn't find it");
            }

            if (Counter == Bpm)
                Counter = 0;
            Index += 1;
        }
    }
}