using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class LogicCircuits : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    public KMSelectable[] Buttons;
    public TextMesh[] InputTexts;
    public TextMesh OutputText;
    public GameObject diagramImage;

    bool[] Inputs = new bool[3];
    bool Result;
    public string Diagram;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    void Awake () {
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;

        foreach (KMSelectable button in Buttons) 
        {
            button.OnInteract += delegate () { ButtonPress(button); return false; };
        }

        for (int i = 0; i < Inputs.Length; i++)
        {
            InputTexts[i].color = new Color(0, 0, 0);
            if (Rnd.Range(0, 2) == 0)
            {
                Inputs[i] = false;                
            }
            else
            {
                Inputs[i] = true;
            }
        }
        OutputText.color = new Color(0, 0, 0);

        diagramImage.SetActive(false);

        // RNG to invert diagram vertically
        if (Rnd.Range(0, 2) == 0)
        {
            var colorA = InputTexts[0].color;
            var colorC = InputTexts[2].color;
            bool temp = Inputs[0];
            Inputs[0] = Inputs[2];
            Inputs[2] = temp;
            InputTexts[0].color = colorC;
            InputTexts[2].color = colorA;

            // flip diagramImage y
            var scale = diagramImage.transform.localScale;
            scale.y *= -1;
            diagramImage.transform.localScale = scale;

        }

        LogicCircuit logicCircuit = new LogicCircuit();

        Diagram = logicCircuit.Circuit(Inputs[0], Inputs[1], Inputs[2]);
        Debug.Log(Inputs[0] + " " + Inputs[1] + " " + Inputs[2]);
        Debug.Log(Diagram);
        Result = Diagram[Diagram.Length - 1] == 'T';
    }

    void ButtonPress(KMSelectable button)
    {
        button.AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
        if (ModuleSolved)
        {
            return;
        }
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (button == Buttons[i])
            {
                if ((i == 0 && !Result) || (i == 1 && Result))
                {
                    ModuleSolved = true;
                    Solve();
                }
                else
                {
                    Strike();
                }
            }
        }
    }

    void OnDestroy () {
      
    }

    void Activate () {
        for (int i = 0; i < Inputs.Length; i++) // "liga" as luzes dos textos
        {
            if (Inputs[i])
            {
                InputTexts[i].color = new Color(1, 1, 1);
            }
        }
        OutputText.color = new Color(1, 1, 1);

        diagramImage.SetActive(true);
    }

    void Start () {

    }

    void Update () { 

    }

    void Solve () {
        GetComponent<KMBombModule>().HandlePass();
    }

    void Strike () {
        GetComponent<KMBombModule>().HandleStrike();
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve () {
        yield return null;
    }
}
