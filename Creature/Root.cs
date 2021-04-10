using UnityEngine;

public class Root : MonoBehaviour {
    private Transform _t;
	
	public Creature MyCreature;	
	public GameObject MyEye;
	public GameObject MyMouth;
	public GameObject Genital;
	
	public MeshRenderer MeshRenderer;
	public Material Material;

    public Color OriginalColour;

    private void Start()
    {
        _t = transform;

        MeshRenderer = gameObject.GetComponent<MeshRenderer>(); // reference to mesh renderer

        MyCreature = _t.parent.gameObject.GetComponent<Creature>(); // reference to creature
        MyEye = MyCreature.Eye;
        MyMouth = MyCreature.Mouth;
        Genital = MyCreature.Genital;

        tag = "Creature";
    }

    /// <summary>
    /// Set root colour
    /// </summary>
    /// <param name="c"></param>
    public void SetColour(Color c)
    {
        OriginalColour = c;
        MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        MeshRenderer.material.shader = Shader.Find("Unlit/Color");
        MeshRenderer.material.color = c;
    }

    /// <summary>
    /// Set root scale
    /// </summary>
    /// <param name="scale"></param>
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (MyCreature.TypeOfCreature == "Herbiverous")
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(3,3,3));
        }
        else
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(3, 3, 3));
        }
    }
}
