using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static GameObject Container;
	public static AudioManager Instance;

	// available audio to play 
	public List<AudioClip> PianoAudioClips;
    public List<AudioClip> ViolinAudioClips;

    public static AudioManager GetInstance()
    {
        if (Instance) return Instance;
        Container = new GameObject {name = "Audio"};
        Instance = Container.AddComponent<AudioManager>();

        return Instance;
    }

    private void Awake ()
    {
        ViolinAudioClips = new List<AudioClip>();
        PianoAudioClips = new List<AudioClip>();
        foreach (AudioClip g in Resources.LoadAll("Sound/piano", typeof(AudioClip)))
        {
            PianoAudioClips.Add(g);
        }

        print("Piano clips found: " + PianoAudioClips.Count);

        foreach (AudioClip g in Resources.LoadAll("Sound/violin", typeof(AudioClip)))
        {
            ViolinAudioClips.Add(g);
        }

        print("Violin clips found: " + ViolinAudioClips.Count);

    }
}
