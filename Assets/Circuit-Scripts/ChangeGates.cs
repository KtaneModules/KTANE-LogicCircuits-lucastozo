using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGates : MonoBehaviour {

	public SpriteRenderer First_Gate;
	public SpriteRenderer Second_Gate;

	public Sprite AND;
	public Sprite NAND;
	public Sprite OR;
	public Sprite NOR;
	public Sprite XOR;

    public LogicCircuits logicCircuits;

    void Start () {
        var diagram = logicCircuits.Diagram;
        int currentGate = 0;
        var parts = diagram.Split(',');
        for (int i = 0; i < parts.Length; i++)
        {
            bool spriteAssigned = false;

            switch (parts[i])
            {
                case "AndGate":
                    if (currentGate == 0)
                    {
                        First_Gate.sprite = AND;
                    }
                    else
                    {
                        Second_Gate.sprite = AND;
                    }
                    spriteAssigned = true;
                    break;
                case "NandGate":
                    if (currentGate == 0)
                    {
                        First_Gate.sprite = NAND;
                    }
                    else
                    {
                        Second_Gate.sprite = NAND;
                    }
                    spriteAssigned = true;
                    break;
                case "OrGate":
                    if (currentGate == 0)
                    {
                        First_Gate.sprite = OR;
                    }
                    else
                    {
                        Second_Gate.sprite = OR;
                    }
                    spriteAssigned = true;
                    break;
                case "NorGate":
                    if (currentGate == 0)
                    {
                        First_Gate.sprite = NOR;
                    }
                    else
                    {
                        Second_Gate.sprite = NOR;
                    }
                    spriteAssigned = true;
                    break;
                case "XorGate":
                    if (currentGate == 0)
                    {
                        First_Gate.sprite = XOR;
                    }
                    else
                    {
                        Second_Gate.sprite = XOR;
                    }
                    spriteAssigned = true;
                    break;
            }

            if (spriteAssigned)
            {
                currentGate++;
            }
        }
    }
}
