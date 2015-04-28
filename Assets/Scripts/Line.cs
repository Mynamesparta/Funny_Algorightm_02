using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum State_of_Line{Const_Lenght,Without_Restrictions,Flesh,Flesh_End,End_Time};
public class Line : MonoBehaviour {
	public float speed=100f;
	public float magnitude=10f;
	public float magnitude_speed=1f;
	public int const_length=10;
	public static bool magicSin=true;
	public static bool isTimeForSin=false;
	public State_of_Line state=State_of_Line.Without_Restrictions;
	//private Vector3[]
	private List<Vector3> list;
	private List<Vector3> function;
	private LineRenderer line_renderer;
	int Index;
	int lastIndex;
	int min_Index=0;
	float _speed;
	float distance;
	bool isChanged;
	bool isDestroyTime=false;
	bool isFleshTime=true;
	void Awake()
	{
		line_renderer = GetComponent<LineRenderer> ();
		list = new List<Vector3> ();
		function = new List<Vector3> ();
		Index = 1;
		/*/
		function.Add (new Vector3 (-100, -100, 0));
		function.Add (new Vector3 (0, 0, 0));
		function.Add (new Vector3 (100, 50, 0));
		function.Add (new Vector3 (-50, 100, 0));
		function.Add (new Vector3 (-70, 20, 0));
		function.Add (new Vector3 (10, 35, 0));
		function.Add (new Vector3 (100, 100, 0));
		Add (function [0]);
		/*/
	}
	public void Clear()
	{
		line_renderer.SetVertexCount (0);
		if(!isTimeForSin)
		{
			list.Clear();
		}
		else
		{
		}
	}
	public void Add(Vector3 vec)
	{
		line_renderer.SetVertexCount (list.Count);
		line_renderer.SetPosition (list.Count - 1, vec);
	}
	void Update() 
	{
		if(!magicSin)
		{
			list=function;
			isTimeForSin=false;
			return;
		}
		switch(state)
		{
		case State_of_Line.Without_Restrictions:
		{
			break;
		}
		case State_of_Line.Const_Lenght:
		{
			while(list.Count+1> const_length)
			{
				list.RemoveAt(0);
				min_Index++;
			}
			break;
		}
		case State_of_Line.End_Time:
		{
			if(list.Count==0)
			{
				toBegin();
				//function.Clear();

			}
			else
			{
				list.RemoveAt(0);
				min_Index++;

			}
			break;
		}
		case State_of_Line.Flesh:
		{
			if(isFleshTime)
			{
				Build();
				isFleshTime=false;
			}
			return;
		}
		case State_of_Line.Flesh_End:
		{
			if(isFleshTime)
			{
				toBegin();
				isFleshTime=false;
			}
			break;
		}
		}
		if (Index >= function.Count||function.Count<=0)
		{
			if(isDestroyTime&&state==State_of_Line.Const_Lenght)
				state=State_of_Line.End_Time;
			return;
		}
		if (list.Count <= 0)
			return;
		lastIndex=list.Count-1;
		_speed = speed * 0.0197486f;//*Time.deltaTime;
		//print (list [lastIndex].ToString() + function [Index].ToString());
		distance = Vector3.Distance (list [lastIndex], function [Index]);
		//print (distance+">"+Time.deltaTime);
		isChanged = false;
		while(distance<_speed)
		{
			_speed-=distance;
			isChanged=true;
			Index++;
			if(Index==function.Count)
			{
				Add(function[Index-1]);
				return;
			}
			distance = Vector3.Distance (function [Index-1], function [Index]);
		}
		if(!isChanged)
		{
			list.Add (Vector3.MoveTowards(list[lastIndex],function[Index],_speed));
		}
		else
		{
			list.Add(Vector3.MoveTowards(function[Index-1],function[Index],_speed));
		}
	}
	int Index_of_Wave=1;
	void LateUpdate()
	{
		//Clear ();
		if (Index_of_Wave > list.Count - 1)
			Index_of_Wave = list.Count - 1;
		if (Index_of_Wave < 1)
			Index_of_Wave = 1;
		/*/
		for(int i=0;i<list.Count;i++)
		{
			line_renderer.SetPosition(i,list[i]);
		}
		return;
		/*/
		if (list.Count < 3)
			return;
		line_renderer.SetVertexCount (list.Count);
		line_renderer.SetPosition (0, list [0]);
		line_renderer.SetPosition (list.Count-1, list[list.Count-1]);
		isDestroyTime = true;
		if(isTimeForSin)
		{
			if(Index_of_Wave<list.Count-1)
			{
				if(state==State_of_Line.Flesh)
				{
					Index_of_Wave=list.Count-1;
				}
				else
					Index_of_Wave++;
			}
			for(int i=1;i<Index_of_Wave;i++)//
			{
				line_renderer.SetPosition  (i,getCrazyPoint(list[i],list[i+1]-list[i-1],-Mathf.Sin(i+min_Index+Time.time*magnitude_speed)));
			}
		}
		else
		{
			if(Index_of_Wave>1)
			{
				if(state==State_of_Line.Flesh)
				{
					Index_of_Wave=1;
				}
				else
					Index_of_Wave--;
			}
			for(int i=1;i<list.Count-Index_of_Wave;i++)
			{
				line_renderer.SetPosition(i,list[i]);
			}
			for(int i=list.Count-Index_of_Wave;i<list.Count-1;i++)//
			{
				line_renderer.SetPosition  (i,getCrazyPoint(list[i],list[i+1]-list[i-1],-Mathf.Sin(i+min_Index+Time.time*magnitude_speed)));
			}
		}
	}
	float _x;
	Vector3 getCrazyPoint(Vector3 point,Vector3 vec,float sin)
	{
		//
		_x = vec.x;
		vec.x = -vec.y;
		vec.y = _x;
		//
		return point+vec*sin/vec.magnitude*magnitude;
	}
	void toBegin()
	{
		isDestroyTime=false;
		isFleshTime = true;
		Index=1;
		Index_of_Wave=1;
		min_Index=0;
		line_renderer.SetVertexCount(0);
		list.Clear();
	}
	public void addFuctionPoint(Vector3 vec)
	{
		function.Add (vec);
		if (function.Count == 1)
			list.Add(vec);
		//print(list[0].ToString());
	}
	public void addFunctionPoints(List<Vector3> _list)
	{
		if (_list == null || _list.Count == 0)
			return;
		function = _list;
		toBegin ();
		list.Add (_list [0]);
	}
	public void setColor(Color begin,Color end)
	{
		line_renderer.SetColors (begin, end);
	}
	public void FleshTime()
	{
		isFleshTime = true;
	}
	void Build()
	{
		while (true) 
		{
			if (Index >= function.Count || function.Count <= 0) {
				if (isDestroyTime && state == State_of_Line.Const_Lenght)
					state = State_of_Line.End_Time;
				return;
			}
			if (list.Count <= 0)
				return;
			lastIndex = list.Count - 1;
			_speed = speed * 0.0197486f;//*Time.deltaTime;
			//print (list [lastIndex].ToString() + function [Index].ToString());
			distance = Vector3.Distance (list [lastIndex], function [Index]);
			//print (distance+">"+Time.deltaTime);
			isChanged = false;
			while (distance<_speed) {
				_speed -= distance;
				isChanged = true;
				Index++;
				if (Index == function.Count) {
					Add (function [Index - 1]);
					return;
				}
				distance = Vector3.Distance (function [Index - 1], function [Index]);
			}
			if (!isChanged) {
				list.Add (Vector3.MoveTowards (list [lastIndex], function [Index], _speed));
			} else {
				list.Add (Vector3.MoveTowards (function [Index - 1], function [Index], _speed));
			}
		}
	}

}
