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
    public GameObject CircuitBacklight;

    private bool[] Inputs = new bool[3];
    bool Result;
    public string Diagram;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved = false;
    private bool ModuleActivated = false;

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

        bool flipDiagram = false;
        // RNG to invert diagram vertically
        if (Rnd.Range(0, 2) == 0)
        {
            var colorA = InputTexts[0].color;
            var colorC = InputTexts[2].color;
            InputTexts[0].color = colorC;
            InputTexts[2].color = colorA;
            bool temp = Inputs[0];
            Inputs[0] = Inputs[2];
            Inputs[2] = temp;

            // flip diagramImage y
            var scale = diagramImage.transform.localScale;
            scale.y *= -1;
            diagramImage.transform.localScale = scale;
            flipDiagram = true;
        }
        
        Debug.Log(Inputs[0] + " " + Inputs[1] + " " + Inputs[2]);

        LogicCircuit logicCircuit = new LogicCircuit();
        Diagram = logicCircuit.Circuit(Inputs, flipDiagram);

        Result = Diagram[Diagram.Length - 1] == 'T';

        Debug.Log(Diagram);
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
            if (button != Buttons[i])
            {
                continue;
            }
            if ((i == 0 && !Result) || (i == 1 && Result))
            {
                ModuleSolved = true;
                Solve();
                return;
            }
            Strike();
        }
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 3; i++)
        {
            diagramImage.SetActive(false);
            for (int j = 0; j < InputTexts.Length; j++)
            {
                InputTexts[j].color = new Color(0, 0, 0);
            }
            OutputText.color = new Color(0, 0, 0);

            if (i == 2)
            {
                // change CircuitBacklight color to black
                CircuitBacklight.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
                break;
            }

            yield return new WaitForSeconds(0.5f);

            diagramImage.SetActive(true);
            for (int j = 0; j < InputTexts.Length; j++)
            {
                if (Inputs[j])
                {
                    InputTexts[j].color = new Color(1, 1, 1);
                }
            }
            OutputText.color = new Color(0.4f, 1, 0.4f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void Activate () {
        for (int i = 0; i < Inputs.Length; i++) // change True inputs to white
        {
            if (Inputs[i])
            {
                InputTexts[i].color = new Color(1, 1, 1);
            } else
            {
                InputTexts[i].color = new Color(0, 0, 0);
            }
        }
        OutputText.color = new Color(1, 1, 1);
        diagramImage.SetActive(true);
        ModuleActivated = true;
    }

    void Update()
    {
        if (ModuleSolved || !ModuleActivated)
        {
            return;
        }
        for (int i = 0; i < Inputs.Length; i++)
        {
            if (!Inputs[i])
            {
                continue;
            }
            var color = InputTexts[i].color;
            float pingPongValue = Mathf.PingPong(Time.time * 0.2f, 0.2f) + 0.8f;
            color.r = pingPongValue;
            color.g = pingPongValue;
            color.b = pingPongValue;
            InputTexts[i].color = color;
            OutputText.color = color;
        }
    }

    void Solve () {
        GetComponent<KMBombModule>().HandlePass();
        StartCoroutine(Blink());
    }

    void Strike () {
        GetComponent<KMBombModule>().HandleStrike();
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} T/True/F/False [Presses the specified button]";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
        if (Command.ToLowerInvariant().EqualsAny("t", "true"))
        {
            yield return null;
            Buttons[1].OnInteract();
        }
        else if (Command.ToLowerInvariant().EqualsAny("f", "false"))
        {
            yield return null;
            Buttons[0].OnInteract();
        }
    }

    IEnumerator TwitchHandleForcedSolve () {
        Buttons[!Result ? 0 : 1].OnInteract();
        yield return new WaitForSeconds(.1f);
    }
}