using UnityEngine;

public class Food : MonoBehaviour
{
	public static float FoodHeight = 1.0F;

    private GameObject _player;

    private EnvironmentController _environment;
    private MeshRenderer _mr;

	public float Energy;
    private float _decayAmount;
    private float _destroyAt;
    private float _decayTime;
    private float _decayRate;

    private float _dist;
    private float _changeDist;

    private float _age;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    private void Awake()
    {
        Energy = Random.Range(5, 20);
    }

    private void Start()
    {
        
        _changeDist = transform.position.y;

        _player = GameObject.FindGameObjectWithTag("Player");

        name = "food";
        SettingsReader.GetInstance();

        _environment = EnvironmentController.GetInstance();

        _mr = GetComponent<MeshRenderer>();
        _mr.sharedMaterial = (Material) Resources.Load("Materials/food");

        Collider co = GetComponent<SphereCollider>();
        co.isTrigger = true;

        Random.Range(-1.0f, 1.0f);
        gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        _age += Time.deltaTime;
        //if (_age >= 150) Destroy();

        if (_player != null)
            _dist = _player.transform.position.y;

        var maxCamDist = 1f; // max distance of camera
        var colorStart = Color.green;
        var colorAlpha = Color.red;

        var l = _changeDist - maxCamDist;
        var h = _changeDist + maxCamDist;
        if (_dist < h && _dist > l)
            GetComponent<Renderer>().material.SetColor("_TintColor", colorAlpha);
        else
            GetComponent<Renderer>().material.SetColor("_TintColor", colorStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the player enters this objects
        if (other.tag == "Player")
        {
            //increase player energy
            other.GetComponent<PlayerStats>().AddFood(1, Energy);
            other.GetComponent<EffectTrigger>().PlayerEats(transform.position);
            // destroy this food
            Destroy();
        }
    }

    public void Destroy()
    {
        _environment.Removefood(gameObject);
        Destroy(gameObject);
    }
}
