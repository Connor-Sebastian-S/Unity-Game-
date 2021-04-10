using UnityEngine;

public class Catch : MonoBehaviour {
    private EnvironmentController _environment;
    private Creature _myCreature;

    private void OnTriggerEnter(Collider col)
    {
        _environment = GameObject.Find("environment").GetComponent<EnvironmentController>();
        if (col.gameObject.name == "root")
        {
            _myCreature = col.transform.parent.gameObject.GetComponent<Creature>();
            _myCreature.Kill();
        }
    }
}
