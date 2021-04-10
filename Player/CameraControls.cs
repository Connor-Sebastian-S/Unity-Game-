using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour {

    public int PlayerId = 0; // The Rewired player id of this character
    private Player _player;  // The Rewired Player

    public Text crtName;
    public Text crtType;
    public Text crtState;
    public Text crtAge;
    public Text crtEnergy;
    public Text crtGenetics;

    public GameObject canvas;

    void Awake()
    {
        canvas.active = false;
        _player = ReInput.players.GetPlayer(PlayerId);
        _cc = GetComponent<CharacterController>();
    }

    public Vector3 MoveVector;
    private CharacterController _cc;

    private void Update()
    {
        GetInput_Windows();
        ProcessInput();

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 60))
            {
                if (hit.collider.transform.root.GetComponent<Creature>())
                {
                    GameObject crt = hit.collider.transform.root.gameObject;

                    Creature crtScript = crt.GetComponent<Creature>();
                    ChromosomeComposition csScript = crtScript.ChromosomeComposition;

                    crtName.text = "Name: " + crt.name;
                    crtType.text = "Type: " + crtScript.TypeOfCreature;
                    crtState.text = "State: " + crtScript.State;
                    crtAge.text = "Age: " + crtScript.Age;
                    crtEnergy.text = "Energy: " + crtScript.Energy;
                    crtGenetics.text = "Chromosome: " + "\n" +
                                       "\t Colour: " + csScript.GetColour() + "\n" +
                                       "\t Branches: " + csScript.GetBranchCount() + "\n" +
                                       "\t Limbs: " + csScript.NumRecurrences.Length;
                    canvas.active = true;
                }
                else
                {
                    canvas.active = false;
                }
            }
            else
            {
                canvas.active = false;
            }
        }
    }

    private void GetInput_Windows()
    {
        MoveVector.x = _player.GetAxis("Move Horizontal");
        MoveVector.y = _player.GetAxis("Move UpDown");
        MoveVector.z = _player.GetAxis("Move Vertical");
    }

    private void ProcessInput()
    {
        // Process movement
        if (MoveVector.x != 0.0f || MoveVector.y != 0.0f || MoveVector.z != 0.0f)
            _cc.Move(MoveVector * 5 * Time.deltaTime);
    }
}
