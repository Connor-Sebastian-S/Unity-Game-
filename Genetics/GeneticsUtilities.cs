using UnityEngine;
using System.Collections;

/// <summary>
/// Various utilities (crossover (transfer of genetics between creatures), mutation (random changing of genetics for 
/// an offspring), and various colour checking functions used by the creatures
/// </summary>
public class GeneticsUtilities
{
    private static double _rand;
    private static readonly System.Random Rnd = new System.Random();

    public static ChromosomeComposition Mutate(ChromosomeComposition currentCreatureComposition, double mutationRate, float mutationFactor)
    {
        // Mutate colour
        var newColour = new float[3]; // new 3 bit colour, r, g, and b
        var currentColour = currentCreatureComposition.GetColour(); // get the current colour of the creature
        // map current colours rgb values to the new colour
        newColour[0] = currentColour.r; 
        newColour[1] = currentColour.g;
        newColour[2] = currentColour.b;

        for (var i = 0; i < 3; i++) {
            _rand = Rnd.NextDouble();
            if (_rand < mutationRate)
            {
                newColour[i] += RandomiseThisGene(mutationFactor);
            }
        }

        // Assign new colour to this creature
        currentCreatureComposition.SetColour(newColour[0], newColour[1], newColour[2]);

        // Mutate root scale
        var currentRootScale = currentCreatureComposition.GetRootScale();
       
        // Check that the current scale (of x, y and z) is currently greater than 1
        if (currentRootScale.x > 1F && currentRootScale.y > 1F && currentRootScale.z > 1F)
        {// Create new 3 bit root scale (x,y, and z)
            var newRootScale = new float[3];
            newRootScale[0] = currentRootScale.x;
            newRootScale[1] = currentRootScale.y;
            newRootScale[2] = currentRootScale.z;

            for (var i = 0; i < 3; i++)
            {
                _rand = Rnd.NextDouble();
                if (_rand < mutationRate)
                {
                    // Randomise scale
                    newRootScale[i] += RandomiseThisGene(mutationFactor);
                }
            }

            // Assign new root scale to creature
            var rootScale = new Vector3(newRootScale[0], newRootScale[1], newRootScale[2]);
            currentCreatureComposition.SetRootScale(rootScale);
        }

        // Mutate limbs colour
        currentColour = currentCreatureComposition.GetLimbColour();
        newColour[0] = currentColour.r;
        newColour[1] = currentColour.g;
        newColour[2] = currentColour.b;
        for (var i = 0; i < 3; i++)
        {
            _rand = Rnd.NextDouble();
            if (_rand < mutationRate)
                newColour[i] += RandomiseThisGene(mutationFactor);
        }

        // Assign new colour to creature
        currentCreatureComposition.SetLimbColour(newColour[0], newColour[1], newColour[2]);

        var currentBranches = currentCreatureComposition.Branches;
        
        foreach (var branch in currentBranches)
        {// get a list of limbs
            var currentLimbs = (ArrayList) branch;
            foreach (var limb in currentLimbs)
            {
                var limbs = (ArrayList) limb;
                var v = (Vector3) limbs[1];
                for (var k = 0; k < 3; k++)
                {
                    _rand = Rnd.NextDouble();
                    if (_rand < mutationRate)
                    {
                        v[k] += RandomiseThisGene(mutationFactor);
                    }
                }
            }
        }

        // mutate base frequency and amplitude
        _rand = Rnd.NextDouble();
        if (_rand < mutationRate) currentCreatureComposition.JointAmplitude += RandomiseThisGene(mutationFactor);

        _rand = Rnd.NextDouble();
        if (_rand < mutationRate) currentCreatureComposition.JointFrequency += RandomiseThisGene(mutationFactor);

        _rand = Rnd.NextDouble();
        if (_rand < mutationRate) currentCreatureComposition.JointPhase += RandomiseThisGene(mutationFactor);

        _rand = Rnd.NextDouble();
        if (_rand < mutationRate) currentCreatureComposition.HungerPoint += RandomiseThisGene(mutationFactor);

        currentCreatureComposition.SetBranches(currentBranches);
        return currentCreatureComposition;
    }

    /// <summary>
    /// Construct a new creeature from the two parent creatures. 
    /// Really it is just a randomly chosen value from either creature which is mutated slightly to make it more unique. 
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public static ChromosomeComposition Crossover(ChromosomeComposition c1, ChromosomeComposition c2, double rate)
    {
        var newChromsome = new ChromosomeComposition();

        // Crossover colour
        var chromosome1Colour = c1.GetColour();
        var chromosome2Colour = c2.GetColour();

        var r = .5F * chromosome1Colour.r + .5F * chromosome2Colour.r;
        var g = .5F * chromosome1Colour.g + .5F * chromosome2Colour.g;
        var b = .5F * chromosome1Colour.b + .5F * chromosome2Colour.b;

        newChromsome.SetColour(r, g, b);

        var chromosome1LimbColour = c1.GetLimbColour();
        var chromosome2LimbColour = c2.GetLimbColour();

        r = .5F * 
            chromosome1LimbColour.r + 
            .5F * 
            chromosome2LimbColour.r;

        g = .5F * 
            chromosome1LimbColour.g + 
            .5F * 
            chromosome2LimbColour.g;

        b = .5F * 
            chromosome1LimbColour.b + 
            .5F * 
            chromosome2LimbColour.b;

        // Assign new colour
        newChromsome.SetLimbColour(r, g, b);

        // Crossover limbs
        var chromome1Branches = c1.Branches;
        var chromosome2Branches = c2.Branches;
        ArrayList newBranches;

        // Select the parent from which the child will take its limb structure
        var randomSelect = Random.Range(0, 2); // 1 or the other 
        ArrayList chosenParentBranches;

        if (randomSelect == 0)
        {
            newBranches = chromome1Branches;
            chosenParentBranches = chromosome2Branches;
        }
        else
        {
            newBranches = chromosome2Branches;
            chosenParentBranches = chromome1Branches;
        }

        randomSelect = Random.Range(0, 2);
        // If 0 select parent 1 branch scale, if 1 select parent 2 branch scale
        newChromsome.SetRootScale(randomSelect == 0 ? c1.GetRootScale() : c2.GetRootScale());

        // Select attributes from the selected parents limbs to assign to child limbs

        newChromsome.NumRecurrences = new int[newBranches.Count];
        for (var i = 0; i < newBranches.Count; i++)
        {
            var parentLimbs = (ArrayList) newBranches[i];
            newChromsome.NumRecurrences[i] = parentLimbs.Count;

            for (var j = 1; j < parentLimbs.Count; j++)
            {
                var parentAttributes = (ArrayList) parentLimbs[j];

                // Select random segment from parent creature
                var index = Random.Range(0, chosenParentBranches.Count);
                var otherParentLimbs = (ArrayList) chosenParentBranches[index];

                index = Random.Range(0, otherParentLimbs.Count);
                var otherParentAttributes = (ArrayList) otherParentLimbs[index];

                // Get parent scale
                var parentScale = (Vector3) parentAttributes[1];
                var otherParentScale = (Vector3) otherParentAttributes[1];
                for (var s = 0; s < 3; s++)
                {
                    _rand = Rnd.NextDouble();
                    if (_rand < rate)
                    {
                        parentScale[s] = otherParentScale[s];
                    }
                }

                // Select random  segment from other creature
                otherParentLimbs = (ArrayList) chosenParentBranches[Random.Range(0, chosenParentBranches.Count)];
                otherParentAttributes = (ArrayList) otherParentLimbs[Random.Range(0, otherParentLimbs.Count)];

                var parentPosition = (Vector3) parentAttributes[0];
                var otherParentPosition = (Vector3) otherParentAttributes[0];
                for (var p = 0; p < 3; p++)
                {
                    _rand = Rnd.NextDouble();
                    if (_rand < rate)
                    {
                        parentPosition[p] = otherParentPosition[p];
                    }
                }
            }

            newBranches[i] = parentLimbs;
            newChromsome.NumBranches = newBranches.Count;
        }

        // Crossover frequency and amplitude
        newChromsome.JointAmplitude = c1.JointAmplitude;
        newChromsome.JointFrequency = c1.JointFrequency;
        newChromsome.JointPhase = c1.JointPhase;

        _rand = Rnd.NextDouble();
        if (_rand < 0.5f)
        {
            newChromsome.JointAmplitude = c2.JointAmplitude;
        }

        _rand = Rnd.NextDouble();
        if (_rand < 0.5f)
        {
            newChromsome.JointFrequency = c2.JointFrequency;
        }

        _rand = Rnd.NextDouble();
        if (_rand < 0.5f) newChromsome.JointPhase = c2.JointPhase;
        {

        }

        _rand = Rnd.NextDouble();
        // New creature hunger point is one of either the two parents hunger points
        newChromsome.HungerPoint = _rand < 0.5f ? c2.HungerPoint : c1.HungerPoint;

        // Assign new branches
        newChromsome.SetBranches(newBranches);

        return newChromsome;
    }

    /// <summary>
    /// Checks that two colours are within a certain distance from each other -similarity
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <returns></returns>
	public static float CheckSimilarColour (ChromosomeComposition c1, ChromosomeComposition c2)
    {
        // Get colour for 1
		var colourOne = c1.GetColour();
        // Get colour from 2
		var colourTwo = c2.GetColour();
		
		return Mathf.Abs(
		    (colourOne.r * colourTwo.r) - 
		    (colourOne.g * colourTwo.g) - 
		    (colourOne.b * colourTwo.g));
	}
	/// <summary>
    /// Randomise a gene by a factor of n
    /// </summary>
    /// <param name="randomisationFactor"></param>
    /// <returns></returns>
	private static float RandomiseThisGene(float randomisationFactor)
    {
		return (float) 
		       Rnd.NextDouble() * 
		       ( Mathf.Abs(
		           randomisationFactor-(
		               -randomisationFactor)) ) + 
		       (-randomisationFactor);
	}	
}
