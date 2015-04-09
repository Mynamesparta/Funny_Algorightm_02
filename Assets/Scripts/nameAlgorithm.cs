using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum _NameAlgorithm{Kyle_Kirkpatrick};

public class nameAlgorithm : MonoBehaviour {
	public Controller contr;
	public _NameAlgorithm state=_NameAlgorithm.Kyle_Kirkpatrick;
	public Text text_name;
	private Recorder record;
	void Awake()
	{
		record = GameObject.FindGameObjectWithTag ("Recorder").GetComponent<Recorder> ();
	}
	public void  Start_Algoritghm()//Vertex StartVertex)
	{
		switch (state) {
		case _NameAlgorithm.Kyle_Kirkpatrick:
			{
				Kyle_Kirkpatrick();
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
		if (index < 0.1666666f) 
		{
			return;
		}
		if(index<0.33333333333333333333333333333334f)
		{
			return;
		}
		if(index<0.5f)
		{
			text_name.text="Kyle Kirkpatrick";
			state=_NameAlgorithm.Kyle_Kirkpatrick;
			return;
		}
		if(index<0.66666666666666666666666666666666f)
		{
			return;
		}
		if(index<8.33333333333333333333333333333333f)
		{
			return;
		}

	}
	public void addLine(Line line,State_of_Line state,Vertex begin=null,Vertex end=null)
	{
		if (begin != null && end != null) 
		{
			fun = new List<Vector3> ();
			fun.Add (begin.getPos ());
			fun.Add (end.getPos ());
		}
		record.Add(line,state,fun);
	}
	public Line addLine(Vertex begin,Vertex end,State_of_Line state)
	{
		fun=new List<Vector3>();
		fun.Add(begin.getPos());
		fun.Add(end.getPos());
		Line line = contr.addLine ();
		record.Add(line,state,fun);
		return line;
	}
	//=====================================Kyle=Kirkpatrick================
	List<Vector3> fun;
	bool isRightRotation(Vector3 last,Vertex begin,Vertex end)
	{
		Vector3 a = begin.getPos() - last;
		Vector3 b = end.getPos() - begin.getPos();
		print ("angle:"+(a.x +"*"+ b.y +"-"+ a.y +"*"+ b.x));
		return a.x * b.y - a.y * b.x <= 0;
	}
	bool is_First=true;
	bool _Kyle_Kirkpatrick(List<Vertex> list,Vector3 last,int Index)
	{
		for(int i=Index+1;i<list.Count;i++)
		{
			if(isRightRotation(last,list[Index],list[i]))
			{
				//last=list[Index].getPos();
				Line line=addLine(list[Index],list[i],State_of_Line.Without_Restrictions);
				if(_Kyle_Kirkpatrick(list,list[Index].getPos(),i))
					return true;
				else
				{
					addLine(line,State_of_Line.End_Time);
				}
			}
			else
			{
				addLine(list[Index],list[i],State_of_Line.Const_Lenght);
				return false;
			}
		}
		return true;
	}
	public void Kyle_Kirkpatrick()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		List<Vertex> leftConer=new List<Vertex>(), rightConer=new List<Vertex>();
		bool isExit;
		Vertex _vertex;
		for(int i = 0; i < vertexs.Length; i++)
		{
			isExit=true;
			for(int j = 0; j < vertexs.Length - i -1 ; j++)
			{
				if(vertexs[j].transform.localPosition.y > vertexs[j + 1].transform.localPosition.y)
				{
					isExit=false;
					_vertex=vertexs[j];
					vertexs[j]=vertexs[j+1];
					vertexs[j+1]=_vertex;
				}
			}
			if(isExit)
				break;
		}
		float current_y=vertexs[0].transform.localPosition.y;
		Vertex min=vertexs[0], max=vertexs[0];
		for(int i=1 ;i <vertexs.Length;i++)
		{
			if(vertexs[i].transform.localPosition.y==current_y)
			{
				if(min.transform.localPosition.x>vertexs[i].transform.localPosition.x)
					min=vertexs[i];
				if(max.transform.localPosition.x<vertexs[i].transform.localPosition.x)
					max=vertexs[i];
			}
			else
			{
				leftConer.Add(min);
				rightConer.Add(max);
				if(min.transform.localPosition.x!=max.transform.localPosition.x)
				{
					//min.setColor(1);
					//max.setColor(3);
				}
				else
				{
					//min.setColor(2);
				}
				current_y=vertexs[i].transform.localPosition.y;
				min=vertexs[i];
				max=vertexs[i];
			}
		}
		leftConer.Add(min);
		rightConer.Add(max);
		if(min.transform.localPosition.x!=max.transform.localPosition.x)
		{
			//min.setColor(1);
			//max.setColor(3);
		}
		else
		{
			min.setColor(2);
		}
		if (leftConer.Count < 2) 
		{
			print ("Somithing Strange!!!");
			return ;
		}
		Vector3 last = (new Vector3 (1, 0,0))+leftConer[0].getPos();
		_Kyle_Kirkpatrick (leftConer,last, 0);
		rightConer.Reverse (0, rightConer.Count);
		if(leftConer[leftConer.Count-1].getPos().x!=rightConer[0].getPos().x)
		{
			addLine (leftConer[leftConer.Count-1], rightConer [0],State_of_Line.Without_Restrictions);
		}
		last=(new Vector3 (-1, 0,0))+rightConer[0].getPos();
		_Kyle_Kirkpatrick (rightConer,last, 0);
	}
	//===============================================================================
}
