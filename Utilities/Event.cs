using UnityEngine;

public class EventHandler {

    public EventHandler(GameObject a, GameObject b)
    {
        _a = a;
        _b = b;
    }

    private readonly GameObject _a;
	private readonly GameObject _b;

    public GameObject[] GetColliders()
    {
        var evt = new GameObject[2];
        evt[0] = _a;
        evt[1] = _b;
        return evt;
    }
}
