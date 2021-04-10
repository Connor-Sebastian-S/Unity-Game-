using UnityEngine;
using System.IO;
using LitJson;

public class SettingsReader : MonoBehaviour
{

    public int AudioBpm = 45;
    public int AudioStep = 4;
    public int AudioBase = 4;
    public int AudioLength = 600;

    public float PlayerStartEnergy = 30.0f;
    public float PlayerSpeed = 30.0f;

    public bool DevDebug = false;

    public float EnvironmentTotalEnergy = 50000.0f;
    public float EnvironmentStartNumberFoodbits = 250f;
    public float EnvironmentStartingCreatures = 90f;
    public float EnvironmentCreatureSpread = 1000f;
    public float EnvironmentCreatureSpreadY = 200f;
    public float EnvironmentStartingHunters = 3f;

    public float FoodInitScaleMin = 0.8f;
    public float FoodInitScaleMax = 0.8f;
    public float FoodDestroyAt = 1.0f;
    public float FoodSporeTime = 0.2f;
    public float FoodSporeRange = 100f;
    public float FoodSporeRate = 2.0f;
    public float FoodWideSpread = 200.0f;
    public float FoodWideSpreadY = 1000.0f;

    public float CreatureInitEnergy = 200.0f;
    public float CreatureHungerThreshold  = 150.0f;
    public float CreatureLowEnergyThreshold = 10.0f;
    public float CreatureLineOfSight = 20.0f;
    public float CreatureEnergyToOffspring = 100.0f;
    public float CreatureMetabolicRate = 0.5f;
    public float CreatureMateRange = 1.5f;
    public float CreatureEatRange = 0.75f;
    public float CreatureEyeRefreshRate = 5f;
    public float CreatureAgeSexualMaturity = 90f;
    public float CreatureAngularDrag = 7.5f;
    public float CreatureDrag = 2.0f;
    public float CreatureBranchLimit = 8f;
    public float CreatureRecurrenceLimit = 12f;

    public float RootMaxRootScaleX = 1.0f;
    public float RootMaxRootScaleY = 1.0f;
    public float RootMaxRootScaleZ = 1.0f;

    public float RootMinRootScaleX = 0.3f;
    public float RootMinRootScaleY = 0.3f;
    public float RootMinRootScaleZ = 0.3f;

    public float LimbMaxLimbScaleX = 0.3f;
    public float LimbMaxLimbScaleY = 0.3f;
    public float LimbMaxLimbScaleZ = 1.0f;

    public float LimbMinLimbScaleX = 0.05f;
    public float LimbMinLimbScaleY = 0.05f;
    public float LimbMinLimbScaleZ = 0.6f;

    public float GenitaliaLineLength = 0.1f;

    public float GeneticsCrossoverRate = 0.5f;
    public float GeneticsMutationRate = 0.1f;
    public float GeneticsMutationFactor = 0.3f;
    
    public static GameObject Container;
    public static SettingsReader Instance;

	public SettingsReader () {

	}

    public void Awake()
    {
    }

    // create an instance of this script as an object
    public static SettingsReader GetInstance()
    {
        if (!Instance)
        {
            Container = new GameObject();
            Container.name = "SettingsReader";
            Instance = Container.AddComponent(typeof(SettingsReader)) as SettingsReader;
        }

        return Instance;
    }

    // redundant, was going to be used to create an android version of the game -not implemented
    public RuntimePlatform Platform
    {
        get
        {
            #if UNITY_ANDROID
                            return RuntimePlatform.Android;
            #elif UNITY_IOS
                                return RuntimePlatform.IPhonePlayer;
            #elif UNITY_STANDALONE_OSX
                                return RuntimePlatform.OSXPlayer;
            #elif UNITY_STANDALONE_WIN
                                return RuntimePlatform.WindowsPlayer;
            #endif
        }
    }
}
