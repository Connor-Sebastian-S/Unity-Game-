using UnityEngine;
using System.Collections;


public class ChromosomeComposition
{
	public Color Colour;
	public Color LimbColour;
	public Vector3 RootScale;
	public float JointFrequency;
	public float JointAmplitude;
	public float JointPhase;
	public float HungerPoint;
    public string Type;

    public int NumBranches;
    public int[] NumRecurrences;

	public ArrayList Branches;

    // define how many branches this creature has
    public void SetNumberOfBranches(int n)
    {
        NumBranches = n;
        if (NumRecurrences == null) InitialiseNumberOfRecurrences(n);
    }

    // define creature type
    public void SetTypeOfCreature(string t)
    {
        Type = t;
    }

    // return type of creature
    public string GetTypeOfCreature()
    {
        return Type;
    }

    // define how many sections make up actorA branch (limb)
    private void InitialiseNumberOfRecurrences(int n)
    {
        NumRecurrences = new int[n];
    }

    // return branch (limb) count
    public int GetBranchCount()
    {
        return Branches.Count;
    }

    // return ArrayList of limbs
    public ArrayList GetLimbList(int index)
    {
        return (ArrayList) Branches[index];
    }

    // return colour
    public Color GetColour()
    {
        return Colour;
    }

    // return limb colour (generally the same as body)
    public Color GetLimbColour()
    {
        return LimbColour;
    }

    // return scale of root
    public Vector3 GetRootScale()
    {
        return RootScale;
    }

    // return ArrayList of branches
    public ArrayList GetBranches()
    {
        return Branches;
    }

    // define branches from ArrayList
    public void SetBranches(ArrayList bs)
    {
        Branches = bs;
    }

    //define colour
    public void SetColour(float r, float g, float b)
    {
        Colour = new Color(r, g, b);
    }

    // define root colour
    public void SetLimbColour(float r, float g, float b)
    {
        LimbColour = new Color(r, g, b);
    }

    // define root scale
    public void SetRootScale(Vector3 rs)
    {
        RootScale = rs;
    }

    // define frequency in which physics follows
    public void SetFequency(float freq)
    {
        JointFrequency = freq;
    }

    // define aplitude in which physics follows
    public void SetAmplitude(float amp)
    {
        JointAmplitude = amp;
    }

    // define phase in which physics follows
    public void SetPhase(float phase)
    {
        JointPhase = phase;
    }
}
