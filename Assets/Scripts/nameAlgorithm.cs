using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum _NameAlgorithm{Test};

public class nameAlgorithm : MonoBehaviour {
	public Controller contr;
	public _NameAlgorithm state=_NameAlgorithm.Test;
	public Text text_name;
	private Recorder record;
	void Awake()
	{
		record = GameObject.FindGameObjectWithTag ("Recorder").GetComponent<Recorder> ();

	}
	public void  Start_Algoritghm()//Vertex StartVertex)
	{
		switch (state) {
		case _NameAlgorithm.Test:
			{
				break;
			}
		}
		record.StartPlay();
		//record.Play();
	}
	static void _Test()
	{
	}
	public void Set()
	{
		float index = GetComponent<UnityEngine.UI.Scrollbar> ().value;
		if (index < 0.1f) 
		{
			text_name.text="Test";
				state=_NameAlgorithm.Test;
			return;
		}
		if(index<0.2f)
		{
			return;
		}
		if(index<0.3f)
		{
			return;
		}
		if(index<0.4f)
		{
			return;
		}
		if(index<0.5f)
		{
			return;
		}
		if(index<0.6f)
		{
			return;
		}
		if(index<0.7f)
		{
			return;
		}
		if(index<0.8f)
		{
			return;
		}
		if(index<0.9f)
		{
			return;
		}

	}
}
