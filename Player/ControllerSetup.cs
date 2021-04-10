using System;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class ControllerSetup : MonoBehaviour {

	public int PlayerId = 0; // The Rewired player id of this character

	private Player _player; // The Rewired Player
	private CharacterController _cc;
	public Vector3 MoveVector;
    private PlayerDataTracker _pt;
    public SettingsReader SettingsReader;
    Vector3 _ss;

    private void Awake()
    {
        _pt = PlayerDataTracker.GetInstance();
        SettingsReader = SettingsReader.GetInstance();
        _pt.SetSpeed(SettingsReader.PlayerSpeed);
        
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        _player = ReInput.players.GetPlayer(PlayerId);
        

        // Get the character controller
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (SettingsReader.Platform == RuntimePlatform.Android)
        {
            print("android");
            //GetInput_Android();
        }
        else if (SettingsReader.Platform == RuntimePlatform.WindowsPlayer)
        {
            print("windows");
            GetInput_Windows();
            ProcessInput();
        }

        if (_endReached)
        {
            if (_fraction < 1)
            {
                _fraction += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(_e, _s, _fraction);
            }
            else if (_fraction >= 1)
                _endReached = false;
        }
    }


    private void GetInput_Android()
    {
        //check if the screen is touched / clicked   
        if (Input.touchCount == 1 || (Input.GetMouseButtonDown(0)))
        {
            Vector3 l = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            l.z = l.y;
            l.y = transform.position.y;

            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                var forward = transform.TransformDirection(Vector3.forward);
                _cc.SimpleMove(forward * _pt.GetSpeed());
                transform.LookAt(l);

                _pt.SetPlayerX(transform.position.x);
                _pt.SetPlayerY(transform.position.y);
                _pt.SetPlayerZ(transform.position.z);

                print(l);
            }
        }
    }

    private void GetInput_Windows()
    {
        MoveVector.x = _player.GetAxis("Move Horizontal");
        MoveVector.y = _player.GetAxis("Move UpDown");
        MoveVector.z = _player.GetAxis("Move Vertical");

        _pt.SetPlayerX(transform.position.x);
        _pt.SetPlayerY(transform.position.y);
        _pt.SetPlayerZ(transform.position.z);

        print(MoveVector);
    }

    private bool _endReached;
    private float _speed = 0.6F;
    private float _fraction;
    private Vector3 _s;
    private Vector3 _e;

    public void AccelerateAway()
    {
        _fraction = 0;
        var d = transform.position.y + 50;
        _s = new Vector3(0, d, 0);
        _e = transform.position;
        _endReached = true;
    }

    private void ProcessInput()
    {
        // Process movement
        if (MoveVector.x != 0.0f || MoveVector.y != 0.0f || MoveVector.z != 0.0f)
            _cc.Move(MoveVector * _pt.GetSpeed() * Time.deltaTime);
    }
}