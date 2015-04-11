﻿using UnityEngine;
using System.Collections;
using System;
[System.Serializable]
public enum name_of_Button{NameAlgorithm,Edit,Play,Pause,Rewinding01,Rewinding02,toBegin,toEnd,FileList};
public class Canvas : MonoBehaviour {

	public Recorder recorder;
	public Animator[] anim;
	public Animator[] anim_recorder;
	private bool[] editTime;

	void Awake()
	{
		anim = GetComponentsInChildren<Animator> ();
		editTime=new bool[anim.Length];
		for(int i=0;i<editTime.Length;i++)
		{
			editTime[i]=false;
		}
		inScene (name_of_Button.Edit,true);
		inScene (name_of_Button.Play, true);
		inScene (name_of_Button.FileList, true);
	}
	int getNumber_of_Button(name_of_Button name)
	{
		switch(name)
		{
		case name_of_Button.NameAlgorithm:
		{
			return 0;
		}
		case name_of_Button.Edit:
		{
			return 1;
		}
		case name_of_Button.Play:
		{
			return 2;
		}
		case name_of_Button.Pause:
		{
			return 3;
		}
		case name_of_Button.Rewinding01:
		{
			return 4;
		}
		case name_of_Button.Rewinding02:
		{
			return 5;
		}
		case name_of_Button.toBegin:
		{
			return 6;
		}
		case name_of_Button.toEnd:
		{
			return 7;
		}
		case name_of_Button.FileList:
		{
			return 8;
		}

		}
		print ("something wrong:name of Button null");
		return -1;
	}
	public name_of_Button fromStringToButton(string _name)
	{
		return (name_of_Button)Enum.Parse (typeof(name_of_Button), _name);
	}
	public void Edit(name_of_Button name)
	{
		int index = getNumber_of_Button (name);
		if (editTime[index]) 
		{
			editTime[index]=false;
			anim[index].SetBool("isActive",false);
		}
		else
		{
			editTime[index]=true;
			anim[index].SetBool("isActive",true);
		}
	}
	public void Edit(name_of_Button name,bool b)
	{
		int index=getNumber_of_Button(name);
		editTime[index]=b;
		anim[index].SetBool("isActive",b);
	}
	public void Edit(string name)
	{
		int index = getNumber_of_Button (fromStringToButton(name));
		if (editTime[index]) 
		{
			editTime[index]=false;
			anim[index].SetBool("isActive",false);
		}
		else
		{
			editTime[index]=true;
			anim[index].SetBool("isActive",true);
		}
	}
	public void Edit(string name,bool b)
	{
		int index=getNumber_of_Button(fromStringToButton(name));
		editTime[index]=b;
		anim[index].SetBool("isActive",b);
	}
	public void inScene(name_of_Button name,bool set)
	{
		int index=getNumber_of_Button(name);
		anim [index].SetBool ("inScene", set);//!anim [index].GetBool ("inScene"));
	}
	public void TimeToRecorder(bool isTimeToRecorder)
	{
		for (int i =0; i<anim_recorder.Length; i++) 
		{
			anim_recorder[i].SetBool("inScene",isTimeToRecorder);
		}
	}
	void LateUpdate()
	{
		if(recorder.is_inPlaying_state()&&!!recorder.isPlaying())
		{
			//Edit(3,false);
			//Edit(4,true);
		}
	}

}
