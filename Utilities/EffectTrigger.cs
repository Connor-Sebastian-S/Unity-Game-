using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    private AudioManager _am;
    private AudioSource _audioSource;

    private void Awake()
    {
        _am = AudioManager.GetInstance();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayerEaten(Vector3 playerPosition)
    {
        Debug.Log("Triggered Player Eaten Effect");
        CreateEffect("Eaten", playerPosition);
    }

    public void PlayerEats(Vector3 playerPosition)
    {
        Debug.Log("Triggered Player Eats Effect");
        CreateEffect("Eat", playerPosition);
    }

    public void PlayerEvolves(Vector3 playerPosition)
    {
        Debug.Log("Triggered Player Evolves Effect");
        CreateEffect("Evolve", playerPosition);
    }

    private void CreateEffect(string effectName, Vector3 effectPosition)
    {
        GameObject effect = Resources.Load(effectName) as GameObject;
        Instantiate(effect, effectPosition, Quaternion.identity);
        if (effectName == "Eat")
        {
            _audioSource.priority = 0;
            _audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/red"), 1.3F);
        } 
        else if (effectName == "Evolve")
        {
            _audioSource.priority = 0;
            _audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/blue"), 1.3F);
        }
        else
            TriggerAudio();
    }

    public void TriggerAudio()
    {
        AudioClip currentAudioClip = _am.ViolinAudioClips[Random.Range(0, _am.ViolinAudioClips.Count-1)];
        if (currentAudioClip != null)
        {
            _audioSource.volume = 0.5f;
            _audioSource.PlayOneShot(currentAudioClip, 0.5F);
            Debug.Log("Played audio clip : " + currentAudioClip.name);
        }
    }
}
