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

    public string Circuit(bool input1, bool input2, bool input3)
    {
        string circuitString = "";
        bool[] inputs = { input1, input2, input3 };

        // Step 1
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = ApplyNotWithProbability(inputs[i], 0.35);
            if (notApplied)
            {
                circuitString += "NOT" + (i+1) + ",";
            }
        }

        // Step 2
        int selectedInput = random.Next(2);
        int selectedNeighborInput = (selectedInput + 1) % 3; // always two neighbors
        int[] selectedInputs = { selectedInput, selectedNeighborInput };
        Func<bool, bool, bool> logicGate = SelectLogicGate();
        bool logicGateOutput = logicGate(inputs[selectedInputs[0]], inputs[selectedInputs[1]]);
        logicGateOutput = ApplyNotWithProbability(logicGateOutput, 0.4);

        circuitString += gateUsed + ",";
        if (notApplied)
        {
            circuitString += "OutputNOT1,";
        }

        // Step 3
        int remainingInput = 3 - selectedInputs[0] - selectedInputs[1];
        inputs[remainingInput] = ApplyNotWithProbability(inputs[remainingInput], 0.3);
        if (notApplied)
        {
            circuitString += "MiddleNOT,";
        }

        // Step 4
        logicGate = SelectLogicGate();
        bool finalOutput = logicGate(logicGateOutput, inputs[remainingInput]);
        circuitString += gateUsed + ",";

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