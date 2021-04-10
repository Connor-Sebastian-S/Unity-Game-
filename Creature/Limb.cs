using UnityEngine;

public class Limb : MonoBehaviour {
    private HingeJoint _hj;
    public Color OriginalColour;

    private void Start()
    {
        tag = "Creature";
    }

    /// <summary>
    /// Get limb position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }

    /// <summary>
    ///  Get limb scale
    /// </summary>
    /// <returns></returns>
    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    /// <summary>
    /// Set limb colour
    /// </summary>
    /// <param name="c"></param>
    public void SetColour(Color c)
    {
        OriginalColour = c;
        gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Color");
        gameObject.GetComponent<MeshRenderer>().material.color = c;
    }

    /// <summary>
    /// Set limb position
    /// </summary>
    /// <param name="p"></param>
    public void SetPosition(Vector3 p)
    {
        transform.localPosition = p;
    }

    /// <summary>
    /// Set limb scale
    /// </summary>
    /// <param name="s"></param>
    public void SetScale(Vector3 s)
    {
        transform.localScale = s;
    }

}
