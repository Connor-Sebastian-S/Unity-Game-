using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour {

    private string[] cheatCode;
    private int index;
    public GameObject textPrefab;
    bool friendCheat = false;
    public Texture2D tex;

    void Start()
    {
        // Code is "tilt", user needs to input this in the right order
        cheatCode = new string[] { "f", "r", "i", "e", "n", "d" };
        index = 0;
    }

    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Check if the next key in the code is pressed
            if (Input.GetKeyDown(cheatCode[index]))
            {
                index++;
            }
            // Wrong key entered, we reset code typing
            else
            {
                index = 0;
            }
        }

        // If index reaches the length of the cheatCode string, 
        // the entire code was correctly entered
        if (index == cheatCode.Length)
        {
            Debug.Log("Typed");
            index = 0; // reset cheat code counter
            Mod();
        }

        if (friendCheat)
        {
            //Mod();
        }
    }

    void Mod()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Creature"))
        {
            if (g.name == "root")
            {
                g.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
                g.GetComponent<Renderer>().material.mainTexture = tex;
            }
        }
    }

    void Friend()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Creature"))
        {
            if (g.name == "root") {

                Transform[] ts = g.GetComponentsInChildren<Transform>();
                bool hasIt = false;
                foreach (Transform t in ts) {              
                    if (t.name == "jokeName")
                    {
                        hasIt = true;
                    }
                }

                if (!hasIt)
                {
                    Vector3 screenPos = g.transform.position;
                    GameObject l = Instantiate(textPrefab);
                    l.transform.position = screenPos;
                    l.transform.parent = g.transform;
                    l.transform.LookAt(Camera.main.transform);
                    l.GetComponent<Text>().text = g.name;
                }
            }
        } 
    }
}
