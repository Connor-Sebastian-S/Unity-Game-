using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using Random = System.Random;

public class MarkovMusic : MonoBehaviour {

	public static System.Random R { get; private set; }
    private Metronome _mn;
    private int _requiredWordCount;
    private SettingsReader _st;

    private void Awake()
    {
        _st = SettingsReader.GetInstance();
        _mn = GetComponent<Metronome>();
        R = new Random();

        //var folder = new DirectoryInfo(Application.dataPath + "/");
        //var files = folder.GetFiles("*.notes", SearchOption.AllDirectories);
        //if (files.Length == 0) throw new Exception("No sample files found.");
        TextAsset prnFile = Resources.Load("input") as TextAsset;


        var sample = new Markov<string>(" ");
        var content = new List<string>();

        char[] archDelim = new char[] { ' '};
        string txt = prnFile.text;
        content = txt.Split(archDelim, StringSplitOptions.RemoveEmptyEntries).ToList();

        //using (var sampleFile = new StreamReader(t.FullName))
       // {
            //while (!sampleFile.EndOfStream)
           // {
                //var line = sampleFile.ReadLine().Trim();
            //    foreach (var entry in line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
              //      content.Add(entry);
            //}

            //sampleFile.Close();
        //}

        if (content.Count > 4) sample.Train(content, 4);

        _requiredWordCount = (int) _st.AudioLength;
        var result = sample.GeneratePiece(_requiredWordCount, true);
        var resultString = new StringBuilder();
        foreach (var entry in result) resultString.Append(entry + " ");
        _mn.BuildNoteList(result);
    }

    // debug ui
    private void OnGui()
    {
        if (_st.DevDebug.ToString() == "true")
        {
            GUI.Label(new Rect(0, 380, 150, 30), "---Music Debug---");
            GUI.Label(new Rect(10, 420, 150, 30), "Current step: " + _mn.CurrentStep);
            GUI.Label(new Rect(10, 460, 150, 30), "BPM: " + _mn.Bpm);
            GUI.Label(new Rect(10, 500, 150, 30), "Metronome base: " + _mn.Base);
            GUI.Label(new Rect(10, 540, 150, 30), "Metronome step: " + _mn.Step);
            GUI.Label(new Rect(10, 580, 250, 30),
                "Current note: " + "Tick: " + _mn.Counter + "/" + _mn.Bpm + ": " + _mn.CurrentNote);
        }
    }
}