using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDepthHandler : MonoBehaviour {

    public List<Text> Text = new List<Text>();

    public static UIDepthHandler Instance;
    private static GameObject _container;
    public static UIDepthHandler GetInstance()
    {
        if (!Instance)
        {
            _container = new GameObject { name = "UIDepthHandler" };
            Instance = _container.AddComponent(typeof(UIDepthHandler)) as UIDepthHandler;
        }

        return Instance;
    }

    void Awake()
    {
        Text.Clear();
    }

    void Update()
    {
        Sort();
    }

    public void AddToCanvas(Text objectToAdd)
    {
        Text.Add(objectToAdd.GetComponent<Text>());
    }

    void Sort()
    {
        foreach (var textObject in Text)
        {
            GameObject creature = textObject.GetComponent<CreatureReference>().creatureReference;
            creature = creature.gameObject.transform.GetChild(0).transform.gameObject;

            if (creature != null)
            {
                if ((Camera.main.transform.position.y - creature.transform.position.y) <= 6 * 6)
                {
                    textObject.gameObject.SetActive(true);
                }
                else
                {
                    textObject.gameObject.SetActive(false);
                }

                if (creature.transform.position.y > Camera.main.transform.position.y)
                {
                    textObject.gameObject.SetActive(false);
                }
            }
        }
    }
}
