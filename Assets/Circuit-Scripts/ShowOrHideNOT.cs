using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOrHideNOT : MonoBehaviour {

	public SpriteRenderer First_NOT_A;
	public SpriteRenderer First_NOT_B;
	public SpriteRenderer First_NOT_C;

	public SpriteRenderer Second_NOT_C;

	public SpriteRenderer First_NOT_Output;
	public SpriteRenderer Second_NOT_Output;

	public Sprite NOT;

	public LogicCircuits logicCircuits;

    void Start () {
        var diagram = logicCircuits.Diagram;
		var parts = diagram.Split(',');
		for (int i = 0; i < parts.Length; i++)
		{
			switch(parts[i])
			{
                case "NOT1":
                    First_NOT_A.sprite = NOT;
                    break;
                case "NOT2":
                    First_NOT_B.sprite = NOT;
                    break;
                case "NOT3":
                    First_NOT_C.sprite = NOT;
                    break;
                case "OutputNOT1":
                    First_NOT_Output.sprite = NOT;
                    break;
                case "OutputNOT2":
                    Second_NOT_Output.sprite = NOT;
                    break;
                case "MiddleNOT":
                    Second_NOT_C.sprite = NOT;
                    break;
            }
		}

    }
}
