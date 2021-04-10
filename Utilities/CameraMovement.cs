using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private float _mainSpeed = 1f; // Regular speed.
    private float _shiftAdd = 0; // Multiplied by how long shift is held. Basically running.
    private float _maxShift = 5f; // Maximum speed when holding shift.
    private float _camSens = .15f; // Camera sensitivity by mouse input.
	private Vector3 _lastMouse = new Vector3(Screen.width/2, Screen.height/2, 0); // Kind of in the middle of the screen, rather than at the top (play).
	private float _totalRun= 1.0f;
    private EnvironmentController _eth;


    private void Start() {
		_eth = EnvironmentController.GetInstance();
	}

    private void Update () {
		// Mouse input.
		_lastMouse = Input.mousePosition - _lastMouse;
		_lastMouse = new Vector3(-_lastMouse.y * _camSens, _lastMouse.x * _camSens, 0 );
		_lastMouse = new Vector3(transform.eulerAngles.x + _lastMouse.x , transform.eulerAngles.y + _lastMouse.y, 0);
		transform.eulerAngles = _lastMouse;
		_lastMouse = Input.mousePosition;
		// Keyboard commands.
		Vector3 p = GetDirection();
		if (Input.GetKey (KeyCode.LeftShift)){
			_totalRun += Time.deltaTime;
			p = p * _totalRun * _shiftAdd;
			p.x = Mathf.Clamp(p.x, -_maxShift, _maxShift);
			p.y = Mathf.Clamp(p.y, -_maxShift, _maxShift);
			p.z = Mathf.Clamp(p.z, -_maxShift, _maxShift);
		}
		else{
			_totalRun = Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
			p = p * _mainSpeed;
		}
		p = p * Time.deltaTime;
		Vector3 newPosition = transform.position;
		if (Input.GetKey(KeyCode.V)){ //If player wants to move on X and Z axis only
			transform.Translate(p);
			newPosition.x = transform.position.x;
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		}
		else{
			transform.Translate(p);
		}

		// the next 6 if _Statements control what happens when the player reaches the maximum positive or negative x, y, 
		// or z position in the world. Crossing beyond these limits will send them to the equivelent position on the 
		// opposite side of the map

		// positive x
		if (this.transform.position.x > _eth.WideSpread+10)
			this.transform.position = new Vector3 (-_eth.WideSpread, this.transform.position.y, this.transform.position.z);

		// negative x
		if (this.transform.position.x < -_eth.WideSpread-10)
			this.transform.position = new Vector3 (_eth.WideSpread, this.transform.position.y, this.transform.position.z);

		// positive y
		if (this.transform.position.y > _eth.WideSpread+10)
			this.transform.position = new Vector3 (this.transform.position.x, -_eth.WideSpread, this.transform.position.z);

		// negative y
		if (this.transform.position.y < -_eth.WideSpread-10)
			this.transform.position = new Vector3 (this.transform.position.x, _eth.WideSpread, this.transform.position.z);

		// positive z
		if (this.transform.position.z > _eth.WideSpread+10)
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -_eth.WideSpread);

		// negative z
		if (this.transform.position.z < -_eth.WideSpread-10)
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, _eth.WideSpread);
		
	}
	private Vector3 GetDirection() {
		Vector3 pVelocity = new Vector3();
		if (Input.GetKey (KeyCode.W)){
			pVelocity += new Vector3(0, 0 , 1);
		}
		if (Input.GetKey (KeyCode.S)){
			pVelocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey (KeyCode.A)){
			pVelocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)){
			pVelocity += new Vector3(1, 0, 0);
		}
		if (Input.GetKey (KeyCode.R)){
			pVelocity += new Vector3(0, 1, 0);
		}
		if (Input.GetKey (KeyCode.F)){
			pVelocity += new Vector3(0, -1, 0);
		}
		return pVelocity;
	}
	public void ResetRotation(Vector3 lookAt) {
		transform.LookAt(lookAt);
	}
}