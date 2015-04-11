using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ListofScenario
{
	public List<Vector3> function;
	public State_of_Line state;
	public Line line;
	public Vertex vertex;
	public int color;
	//---------------------------------
	public bool WithoutPause=false;
	public void Play()
	{
		if(line!=null)
		{
			line.state = state;
			if(state!=State_of_Line.End_Time)
				line.addFunctionPoints (function);
		}
		if(vertex!=null)
		{
			vertex.setColor(color);
		}
	}
	public void backPlay()
	{
		if(line!=null)
		{
			line.state = State_of_Line.End_Time;
			//line.addFunctionPoints (function);
		}
		if(vertex!=null)
		{
			vertex.setColor(0);
		}
	}
}
enum State_of_Recorder{Create,Play,Block};
public class Recorder : MonoBehaviour {
	public float PauseTime;
	public int Speed=2;

	private State_of_Recorder state = State_of_Recorder.Play;

	private List<ListofScenario> Scenario;
	private ListofScenario current_list;
	private int Iteration=0;
	private bool isTimetoPlay=false;
	private bool withoutPause=false;

	void Awake()
	{
		Scenario = new List<ListofScenario> ();
	}
	void Update()
	{
		if(Input.GetButtonDown("isTimeForSin"))
		{
			Line.isTimeForSin=(Line.isTimeForSin?false:true);
		}
	}
	IEnumerator  _Block(float time)
	{
		State_of_Recorder _state = new State_of_Recorder();
		_state = state;
		state = State_of_Recorder.Block;
		yield return new WaitForSeconds (time);
		state = _state;
	}
	IEnumerator _Play()
	{
		//print ("Test");
		for(;Iteration<Scenario.Count;Iteration++)
		{
			Scenario[Iteration].Play();
			//print ("iteration:"+Iteration);
			if(Scenario[Iteration].WithoutPause)
			{
				continue;
			}
			yield return new WaitForSeconds(PauseTime);
			while(!isTimetoPlay)
				yield return new WaitForSeconds(0.1f);
		}
		//isTimetoPlay = false;
	}
	public void StartCreate()
	{
		StopAllCoroutines ();
		if(Scenario!=null)
			toBegin ();
		if(Scenario==null)
			Scenario=new List<ListofScenario>();
		else
			Scenario.Clear ();
		state = State_of_Recorder.Create;
	}
	public void StartPlay()
	{
		if(current_list!=null)
			Scenario.Insert(Scenario.Count,current_list);
		current_list = null;
		isTimetoPlay = true;
		state = State_of_Recorder.Play;
		Iteration = 0;
	}
	public void Block(float time)
	{
		StartCoroutine(_Block(time));
	}
	//
	public void Add(Line line,State_of_Line _state,List<Vector3> function)
	{
		if (state != State_of_Recorder.Create)
			return;
		current_list=new ListofScenario();
		current_list.line = line;
		current_list.function = function;
		current_list.state = _state;
		Scenario.Insert(Scenario.Count,current_list);
		current_list = null;
	}
	public void Add(Vertex vertex,int color)
	{
		if (state != State_of_Recorder.Create)
			return;
		current_list=new ListofScenario();
		current_list.vertex = vertex;
		current_list.color = color;
		Scenario.Insert(Scenario.Count,current_list);
		current_list = null;
	}
	/*/
	public void Add(Vertex vertex,string distance)
	{
		if (state != State_of_Recorder.Create)
			return;
		if(current_list==null)
		{
			current_list=new ListofScenario();
			current_list._vertex=vertex;
			current_list.distance=distance;
		}
		else
		{
			if(current_list._edge==null)
			{
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=new ListofScenario();
				current_list._vertex=vertex;
				current_list.distance=distance;
			}
			else
			{
				current_list._vertex=vertex;
				current_list.distance=distance;
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=null;
			}
		}
	}
	//
	public void Add(Edge edge,int color,int right_left)
	{
		if (state != State_of_Recorder.Create)
			return;
		if(current_list==null)
		{
			current_list=new ListofScenario();
			current_list._edge=edge;
			current_list._color_of_Edge=color;
			current_list._Left_Right=right_left;
		}
		else
		{
			if(current_list._vertex==null)
			{
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=new ListofScenario();
				current_list._edge=edge;
				current_list._color_of_Edge=color;
				current_list._Left_Right=right_left;
			}
			else
			{
				current_list._edge=edge;
				current_list._color_of_Edge=color;
				current_list._Left_Right=right_left;
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=null;
			}
		}

	}
	public void Add(Edge edge,string text)
	{
		if (state != State_of_Recorder.Create)
			return;
		if(current_list==null)
		{
			current_list=new ListofScenario();
			current_list._edge=edge;
			current_list.stream=text;
		}
		else
		{
			if(current_list._vertex==null)
			{
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=new ListofScenario();
				current_list._edge=edge;
				current_list.stream=text;
			}
			else
			{
				current_list._edge=edge;
				current_list.stream=text;
				current_list.WithoutPause=withoutPause;
				Scenario.Insert(Scenario.Count,current_list);
				current_list=null;
			}
		}
	}
	/*/
	public void Play()
	{
		if (state != State_of_Recorder.Play)
			return;
		//print ("Scenario:" + Scenario.Count);
		if (Iteration >= Scenario.Count)
			return;
		if (Iteration <= -1)
			Iteration = 0;
		if (isTimetoPlay)
		{
			for(int i=0;i<Speed;i++)
				StartCoroutine ("_Play");
		}
		else
		{
			isTimetoPlay=true;
		}
	}
	public void Pause()
	{
		isTimetoPlay = false;
	}
	public bool isCreateRecord()
	{
		if (state == State_of_Recorder.Create)
			return true;
		else 
			return false;
	}
	public bool is_inPlaying_state ()
	{
		if (state == State_of_Recorder.Play)
			return true;
		else
			return false;
	}
	public bool isPlaying()
	{
		if (Scenario == null)
			return false;
		if (state==State_of_Recorder.Play&&isTimetoPlay&&Iteration < Scenario.Count) 
		{
			return true;
		}
		else
			return false;
	}
	public void toBegin()
	{
		if (Iteration <= -1)
			return;
		if (Iteration >= Scenario.Count)
			Iteration = Scenario.Count - 1;
		if(Iteration==Scenario.Count)
		{
			Iteration=Scenario.Count-1; 
		}
		for(;Iteration>=0;Iteration--)
		{
			Scenario[Iteration].backPlay();
		}

	}
	public void toEnd()
	{
		if (Iteration <= -1)
			Iteration = 0;
		if(Iteration==Scenario.Count)
		{
			Iteration=Scenario.Count-1; 
		}
		for(;Iteration<Scenario.Count;Iteration++)
		{
			Scenario[Iteration].Play();
		}
	}
	
	public void Rewind()
	{
		if (Iteration <= -1)
			return;
		if (Iteration >= Scenario.Count)
			Iteration = Scenario.Count - 1;
		Scenario [Iteration].backPlay ();
		Iteration--;
		if(Iteration>=0)
			Scenario [Iteration].Play ();
	}
	public void Foward()
	{
		if (Iteration >= Scenario.Count)
			return;
		if (Iteration <= -1)
			Iteration = 0;
		Scenario [Iteration].Play ();
		Iteration++;
	}
	public void setWithoutPause(bool b)
	{
		/*if (state != State_of_Recorder.Create)
			return;*/
		withoutPause = b;
	}



}
