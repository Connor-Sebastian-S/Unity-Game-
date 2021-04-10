using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private ControllerSetup _pc;
    private PlayerDataTracker _pt;

	// limb list
	public List<Transform> Limbs = new List<Transform>();

    private Vector3 _torque;

    public List<Transform> Limbs0 = new List<Transform>();
    public List<Transform> Limbs1 = new List<Transform>();
    public List<Transform> Limbs2 = new List<Transform>();
    public List<Transform> Limbs3 = new List<Transform>();
    public List<Transform> Limbs4 = new List<Transform>();
    public List<Transform> Limbs5 = new List<Transform>();
    public List<Transform> Limbs6 = new List<Transform>();
    public List<Transform> Limbs7 = new List<Transform>();

    private EnvironmentController _environment;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 3);
    }

    private void Awake()
    {
        _environment = EnvironmentController.GetInstance();

        _pt = PlayerDataTracker.GetInstance();
        _pc = GetComponent<ControllerSetup>();
        // create list of limbs
        var t = transform;
        foreach (Transform child in t)
        {
            if (child.gameObject.name.Contains("limb_"))
            {
                Limbs.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_0"))
                    Limbs0.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_1"))
                    Limbs1.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_2"))
                    Limbs2.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_3"))
                    Limbs3.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_4"))
                    Limbs4.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_5"))
                    Limbs5.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_6"))
                    Limbs6.Add(child.gameObject.transform);
                if (child.gameObject.name.Contains("limb_7"))
                    Limbs7.Add(child.gameObject.transform);
            }
        }
    }

    private void ManageLimits()
    {
        // positive x
        if (transform.position.x > _environment.WideSpread)
            transform.position = new Vector3(-_environment.WideSpread, transform.position.y, transform.position.z);

        // negative x
        if (transform.position.x < -_environment.WideSpread)
            transform.position = new Vector3(_environment.WideSpread, transform.position.y, transform.position.z);

        // positive y
        if (transform.position.y > _environment.WideSpreadY+10)
            transform.position = new Vector3(transform.position.x, _environment.WideSpreadY-1, transform.position.z);


        // positive z
        if (transform.position.z > _environment.WideSpread)
            transform.position = new Vector3(transform.position.x, transform.position.y, -_environment.WideSpread);

        // negative z
        if (transform.position.z < -_environment.WideSpread)
            transform.position = new Vector3(transform.position.x, transform.position.y, _environment.WideSpread);
    }

    private void Update()
    {
        ManageLimits();
    }

    public void DeathStart()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        foreach (var l in Limbs)
            if (l.GetComponent<ConfigurableJoint>())
                l.GetComponent<ConfigurableJoint>().breakForce = 0;
        yield return new WaitForSeconds(4);
        GetComponent<PlayerStats>().Mn.Exit();
    }

    public void Final()
    {
        foreach (var l in Limbs)
            l.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Evolve(int evLevel)
    {
        foreach (var l in Limbs)
            l.gameObject.GetComponent<MeshRenderer>().enabled = true;
        for (int i = evLevel; i <= Limbs0.Count; i++)
            foreach (var e in Limbs0.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs1.Count; i++)
            foreach (var e in Limbs1.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs2.Count; i++)
            foreach (var e in Limbs2.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs3.Count; i++)
            foreach (var e in Limbs3.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs4.Count; i++)
            foreach (var e in Limbs4.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs5.Count; i++)
            foreach (var e in Limbs5.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs6.Count; i++)
            foreach (var e in Limbs6.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (int i = evLevel; i <= Limbs7.Count; i++)
            foreach (var e in Limbs7.Skip(i))
                e.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        foreach (Transform t in Limbs)
            if (_pc.MoveVector != Vector3.zero)
            {
                t.gameObject.GetComponent<Rigidbody>().AddForce(-_pc.MoveVector * 2);
            }
            else
            {
                t.gameObject.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 2);
            }
    }
}
