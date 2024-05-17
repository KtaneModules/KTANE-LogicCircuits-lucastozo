using System;

public class LogicCircuit
{
    private static Random random = new Random();

    private static bool notApplied = false;
    private static string gateUsed = "";

    private static bool NotGate(bool a)
    {
        return !a;
    }

    private static bool AndGate(bool a, bool b)
    {
        return a && b;
    }

    private static bool NandGate(bool a, bool b)
    {
        return !(a && b);
    }

    private static bool OrGate(bool a, bool b)
    {
        return a || b;
    }

    private static bool NorGate(bool a, bool b)
    {
        return !(a || b);
    }

    private static bool XorGate(bool a, bool b)
    {
        return a ^ b;
    }

    private static bool ApplyNotWithProbability(bool input, double probability)
    {
        notApplied = false;
        if (random.NextDouble() < probability)
        {
            notApplied = true;
            return NotGate(input);
        }
        return input;
    }

    private static Func<bool, bool, bool> SelectLogicGate()
    {
        gateUsed = "";
        var gates = new Func<bool, bool, bool>[]
        {
            AndGate,
            OrGate,
            NorGate,
            XorGate,
            NandGate
        };
        var select = random.Next(gates.Length);
        gateUsed = gates[select].Method.Name;
        return gates[select];
    }

    public string Circuit(bool[] input, bool flipDiagram)
    {
        string circuitString = "";

        bool[] inputs = new bool[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            inputs[i] = input[i];
        }

        // Step 1
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = ApplyNotWithProbability(inputs[i], 0.35);
            if (notApplied)
            {
                if (flipDiagram) // 1 -> 3, 3 -> 1
                {
                    circuitString += "NOT" + (3-i) + ",";
                }
                else
                {
                    circuitString += "NOT" + (i+1) + ",";
                }
                notApplied = false;
            }
        }

        circuitString += "CurrentInputs: " + (inputs[0] ? "T" : "F") + (inputs[1] ? "T" : "F") + (inputs[2] ? "T" : "F") + ",";

        // Step 2
        int selectedInput = 0;
        if (flipDiagram)
        {
            selectedInput = 1; // B + C inputs
        }
        int[] selectedInputs = { selectedInput, selectedInput+1 };
        Func<bool, bool, bool> logicGate = SelectLogicGate();
        bool logicGateOutput = logicGate(inputs[selectedInputs[0]], inputs[selectedInputs[1]]);

        circuitString += gateUsed + ",";
        if (logicGateOutput)
        {
            circuitString += "OutputT,";
        }
        else
        {
            circuitString += "OutputF,";
        }

        logicGateOutput = ApplyNotWithProbability(logicGateOutput, 0.4);
        if (notApplied)
        {
            circuitString += "OutputNOT1,";
        }

        // Step 3
        int remainingInput = 2;
        if (flipDiagram)
        {
            remainingInput = 0; // A input
        }
        inputs[remainingInput] = ApplyNotWithProbability(inputs[remainingInput], 0.3);
        if (notApplied)
        {
            circuitString += "MiddleNOT,";
        }

        // Step 4
        logicGate = SelectLogicGate();
        bool finalOutput = logicGate(logicGateOutput, inputs[remainingInput]);
        circuitString += gateUsed + ",";
        if (finalOutput)
        {
            circuitString += "OutputT,";
        }
        else
        {
            circuitString += "OutputF,";
        }

        // Step 5
        finalOutput = ApplyNotWithProbability(finalOutput, 0.5);
        if (notApplied)
        {
            circuitString += "OutputNOT2,";
        }

        if (finalOutput)
        {
            circuitString += "T";
        }
        else
        {
            circuitString += "F";
        }
        
        return circuitString;
    }
}