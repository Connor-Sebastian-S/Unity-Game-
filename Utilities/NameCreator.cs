using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class NameCreator
{
    private static ChromosomeComposition ccComposition;
    // The name of  a creature is derived from different aspects of its chromosome composition
    public static string Name(ChromosomeComposition cc)
    {
        ccComposition = cc;
        string name = "";

        
        name += DeriveNameFromLimbs();
        name += " ";
        name += DeriveNameFromDescription("");
        name += DeriveNameFromType();

        return name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

    }

    static string DeriveNameFromLimbs()
    {
        // read limb count
        int limbCount = ccComposition.GetBranchCount();
        List<string> latinNumbers = new List<string>();
        latinNumbers.Add("uni");
        latinNumbers.Add("duo");
        latinNumbers.Add("tri");
        latinNumbers.Add("quart");
        latinNumbers.Add("quint");
        latinNumbers.Add("se");
        latinNumbers.Add("sept");
        latinNumbers.Add("oct");

        string limbName = latinNumbers[limbCount-1];

        

        if (ccComposition.NumRecurrences.Length > 5 && ccComposition.NumRecurrences.Length < 9)
            limbName += "aliq"; // some
        else if (ccComposition.NumRecurrences.Length > 0 && ccComposition.NumRecurrences.Length <= 5)
            limbName += "pauc"; // few
        else if (ccComposition.NumRecurrences.Length >= 9)
            limbName += "mult"; // many

        limbName += "peri";

        return (limbName); // return and capitalise
    }

    static string DeriveNameFromType()
    {
        string typeName = "";
        if (ccComposition.GetTypeOfCreature() == "Carnivorous")
            typeName = "carnis";
        else if (ccComposition.GetTypeOfCreature() == "Herbiverous")
            typeName = "planta";
        return (typeName); // return and capitalise
    }

    static string DeriveNameFromDescription(string previous)
    {
        List<string> latinDesc = new List<string>();
        latinDesc.Add("creare"); // creature
        latinDesc.Add("bestia"); // beast
        latinDesc.Add("advena"); // strange
        latinDesc.Add("hospitus"); // strange
        latinDesc.Add("arcanus"); // mystery
        latinDesc.Add("aqua"); // water
        latinDesc.Add("altus"); // deep
        latinDesc.Add("cadere"); // downfall
        latinDesc.Add("pungere"); // bite, string
        latinDesc.Add("cito"); // quick
        latinDesc.Add("lente"); // slow
        latinDesc.Add("piscis"); // fish
        latinDesc.Add("parvus"); // small
        latinDesc.Add("magnus"); // large
        latinDesc.Add("robustus"); // strong
        latinDesc.Add("debilitare"); // weak
        latinDesc.Add("amicus"); // friend
        latinDesc.Add("hostis"); // foe

        // only select a description once!
        string temp = latinDesc[Random.Range(0, latinDesc.Count - 1)];
        while (temp == previous)
            temp = latinDesc[Random.Range(0, latinDesc.Count - 1)];
        return (temp);
    }
}
