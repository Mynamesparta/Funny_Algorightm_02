using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum _NameAlgorithm{Kyle_Kirkpatrick,Andrew_Edwin,Graham,Last  };

public class nameAlgorithm : MonoBehaviour {
	public Controller contr;
	public _NameAlgorithm state=_NameAlgorithm.Kyle_Kirkpatrick;
	public Text text_name;
	public delegate void Algorightm();
	public Vertex vertex_for_test;
	public Color start_for_algorithm, end_for_algorithm;
	private Recorder record;
	Algorightm algo;
	void Awake()
	{
		record = GameObject.FindGameObjectWithTag ("Recorder").GetComponent<Recorder> ();
		algo = Kyle_Kirkpatrick;
	}
	public void  Start_Algoritghm()//Vertex StartVertex)
	{
		if(contr.getLenghtOfVertexs()==0)
		{
			print("vertexs:null");
			return;
		}
		if (algo != null)
			algo ();
		else
			print ("algo=null");
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
			text_name.text="Kyle Kirkpatrick";
			state=_NameAlgorithm.Kyle_Kirkpatrick;
			return;
		}
		if(index<0.33333333333333333333333333333334f)
		{text_name.text="Kyle Kirkpatrick";
			state=_NameAlgorithm.Kyle_Kirkpatrick;
			return;
		}
		if(index<0.5f)
		{
			text_name.text="Kyle Kirkpatrick";
			state=_NameAlgorithm.Kyle_Kirkpatrick;
			algo=Kyle_Kirkpatrick;
			return;
		}
		if(index<0.66666666666666666666666666666666f)
		{
			text_name.text="Andrew Edwin";
			state=_NameAlgorithm.Andrew_Edwin;
			algo=Andrew_Edwin;
			return;
		}
		if(index<8.33333333333333333333333333333333f)
		{
			text_name.text="Graham ";
			state=_NameAlgorithm.Graham ;
			algo=Graham;
			return;
		}
		text_name.text="Last ";
		state=_NameAlgorithm.Last ;
		algo=Last;
		
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
		fun=new List<Vector3>();//Vector3
		fun.Add(begin.getPos());
		fun.Add(end.getPos());
		Line line = contr.addLine ();
		record.Add(line,state,fun);
		return line;
	}
	public Line addLine(Vector3 begin,Vector3 end,State_of_Line state)
	{
		fun=new List<Vector3>();
		fun.Add(begin);
		fun.Add(end);
		Line line = contr.addLine ();
		record.Add(line,state,fun);
		return line;
	}
	public delegate bool isGreater(Vertex one,Vertex two); 
	public Vertex[] Sort(Vertex[] vertexs,isGreater ig)
	{
		bool isExit;
		Vertex _vertex;
		for(int i = 0; i < vertexs.Length; i++)
		{
			isExit=true;
			for(int j = 0; j < vertexs.Length - i -1 ; j++)
			{
				if(ig(vertexs[j], vertexs[j + 1]))
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
		return vertexs;
	}
	//=====================================Kyle=Kirkpatrick================
	List<Vector3> fun;
	bool isRightRotation(Vector3 last,Vertex begin,Vertex end)
	{
		Vector3 a = begin.getPos() - last;
		Vector3 b = end.getPos() - begin.getPos();
		//print ("angle:"+(a.x +"*"+ b.y +"-"+ a.y +"*"+ b.x));
		return a.x * b.y - a.y * b.x <= 0;
	}
	bool isRightRotation(Vector3 last,Vector3 begin,Vector3 end)
	{
		Vector3 a = begin - last;
		Vector3 b = end - begin;
		//print ("angle:"+(a.x +"*"+ b.y +"-"+ a.y +"*"+ b.x));
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
	bool _isGreaterKyle_Kirkpatrick(Vertex first,Vertex second)
	{
		return first.getPos ().y > second.getPos ().y;
	}
	public void Kyle_Kirkpatrick()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		List<Vertex> leftConer=new List<Vertex>(), rightConer=new List<Vertex>();
		vertexs = Sort (vertexs, _isGreaterKyle_Kirkpatrick);
		/*/
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
		/*/
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
					min.setColor(1);
					max.setColor(3);
				}
				else
				{
					min.setColor(2);
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
			min.setColor(1);
			max.setColor(3);
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
		if(rightConer[leftConer.Count-1].getPos().x!=leftConer[0].getPos().x)
		{
			addLine (rightConer[leftConer.Count-1], leftConer [0],State_of_Line.Without_Restrictions);
		}
	}
	//=================================Andrew=Edwin==============================================
	bool _isGreaterAndrew_Edwin(Vertex first,Vertex second)
	{
		return first.getPos ().x > second.getPos ().x;
	}
	bool it_in_Left_side(Vector3 ver_1,Vector3 ver_2,Vector3 ver)
	{
		Vector3 vec = ver_2 - ver_1;
		if (vec.x != 0)
			if(vec.x>0)
				return ver.y > (ver_1.y + vec.y / vec.x * (ver.x - ver_1.x));
			else
				return ver.y < (ver_1.y + vec.y / vec.x * (ver.x - ver_1.x));
		else
			return ver.x <= ver_1.x;
	}
	public void searchAndrew_Edwin(List<Vector3> list,Vector3 first)
	{
		int i, j, k;
		bool first_side = it_in_Left_side (list [0], list [1], first);
		bool is_norm_Edge;
		Line line;
		for(i=0;i<list.Count;i++)
		{
			for(j=i+1;j<list.Count;j++)
			{
				line=addLine(list[i],list[j],State_of_Line.Without_Restrictions);
				is_norm_Edge=true;
				for(k=0;k<list.Count;k++)
				{
					if(k!=i&&k!=j)
					{
						if(first_side!=it_in_Left_side(list[i],list[j],list[k]))
						{
							addLine(line,State_of_Line.End_Time);
							is_norm_Edge=false;
							break;
						}

					}
					else
					{
						continue;
					}
				}
				if(is_norm_Edge)
					break;
			}
		}
	}
	public void Andrew_Edwin()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		List<Vertex> leftConer=new List<Vertex>(), rightConer=new List<Vertex>();
		vertexs = Sort (vertexs, _isGreaterAndrew_Edwin);
		Line _Piece_of_Cake = addLine (vertexs [0], vertexs [vertexs.Length - 1], State_of_Line.Without_Restrictions);
		List<Vector3> Up=new List<Vector3>(),Down=new List<Vector3>();
		Vector3 point_1=vertexs [0].getPos(), point_2=vertexs [vertexs.Length - 1].getPos(), point_3;
		for(int i=0;i<vertexs.Length;i++)
		{
			point_3=vertexs[i].getPos();
			if(it_in_Left_side(point_1,point_2,point_3))
			{
				Up.Add(point_3);
				vertexs[i].setColor(1);
			}
			else
			{
				Down.Add(point_3);
				vertexs[i].setColor(3);
			}
		}
		addLine (_Piece_of_Cake, State_of_Line.End_Time);
		searchAndrew_Edwin (Up, Down [0]);
		Down.Insert (0,Up [0]);
		Down.Insert (Down.Count,Up[Up.Count-1]);
		searchAndrew_Edwin(Down,Up[1]);

	}
	//=================================Graham==============================================
	Vector3 center;
	bool _isGreaterGraham(Vertex first,Vertex second)
	{
		Vector3 _first = center-first.getPos () ;
		Vector3 _second = second.getPos()- center;
		return _first.x*_second.y-_first.y*_second.x>0;
	}
	bool _isGreaterGraham(Vector3 first,Vector3 second)
	{
		first = first - center;
		second = second - center;
		return first.x*second.y-first.y*second.x<0;
	}
	int _i=1000;
	public void Graham()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		Vector3 min, max;
		min = max = vertexs [0].getPos ();
		for(int i=1;i<vertexs.Length;i++)
		{
			if(min.x>vertexs[i].getPos().x)
				min=vertexs[i].getPos();
			if(max.x<vertexs[i].getPos().x)
				max=vertexs[i].getPos();
		}
		center = min + (max - min) / 2;
		print (center.ToString ());
		vertex_for_test=contr.Add (center);
		vertex_for_test.setColor (1);
		vertexs = Sort (vertexs, _isGreaterGraham);
		//vertexs = Sort (vertexs, _isGreaterGraham);
		List<Line> lines = new List<Line> ();
		for (int i=0; i<vertexs.Length-1; i++) 
		{
			addLine(center,vertexs[i].getPos(),State_of_Line.Without_Restrictions).setColor(start_for_algorithm,end_for_algorithm);
			lines.Add(addLine(vertexs[i],vertexs[i+1],State_of_Line.Without_Restrictions));
		}
		addLine(center,vertexs[vertexs.Length-1].getPos(),State_of_Line.Without_Restrictions).setColor(start_for_algorithm,end_for_algorithm);
		lines.Add(addLine(vertexs[vertexs.Length-1],vertexs[0],State_of_Line.Without_Restrictions));
		List<Vector3> list_of_vertexs = new List<Vector3> ();
		for(int i=0;i<vertexs.Length-1;i++)
		{
			list_of_vertexs.Add(vertexs[i].getPos());
		}
		int Iter=1;
		//bool first_bool=it_in_Left_side(list_of_vertexs[0],list_of_vertexs[1],center);
		while(Iter<list_of_vertexs.Count)
		{
			_i--;
			if(_i<=0)
				break;
			if(false&&Iter==0)
			{
				if(!it_in_Left_side(list_of_vertexs[list_of_vertexs.Count-1],
				                    list_of_vertexs[1],
				                    list_of_vertexs[0]))
				{
					addLine(lines[list_of_vertexs.Count],State_of_Line.End_Time);
					addLine(lines[0],State_of_Line.End_Time);
					lines.RemoveAt(list_of_vertexs.Count);
					lines.RemoveAt(0);
					lines.Insert(lines.Count,addLine(list_of_vertexs[list_of_vertexs.Count-1],
					                                           list_of_vertexs[1],
					                                           State_of_Line.Without_Restrictions));
					list_of_vertexs.RemoveAt(0);
				}
				continue;
			}
			if(Iter==list_of_vertexs.Count-1)
			{
				if(!it_in_Left_side(list_of_vertexs[list_of_vertexs.Count-2],
				                    list_of_vertexs[0],
				                    list_of_vertexs[list_of_vertexs.Count-1]))
				{
					addLine(lines[list_of_vertexs.Count-1],State_of_Line.End_Time);
					addLine(lines[list_of_vertexs.Count],State_of_Line.End_Time);
					lines.RemoveAt(list_of_vertexs.Count);
					lines.RemoveAt(list_of_vertexs.Count-1);
					lines.Insert(lines.Count,addLine(list_of_vertexs[list_of_vertexs.Count-1],
					                                           list_of_vertexs[0],
					                                           State_of_Line.Without_Restrictions));
					list_of_vertexs.RemoveAt(list_of_vertexs.Count-1);
					//Iter--;
				}
				continue;
			}
			print(Iter+"<"+list_of_vertexs.Count);
			if(!it_in_Left_side(list_of_vertexs[Iter+1],
			                    list_of_vertexs[Iter-1],
			                    list_of_vertexs[Iter]))
			{
				addLine(lines[Iter-1],State_of_Line.End_Time);
				addLine(lines[Iter  ],State_of_Line.End_Time);
				lines.RemoveAt(Iter);
				lines.RemoveAt(Iter-1);
				lines.Insert(Iter-1,addLine(list_of_vertexs[Iter-1],list_of_vertexs[Iter+1],State_of_Line.Without_Restrictions));
				list_of_vertexs.RemoveAt(Iter);
				Iter--;
				if(Iter<=0)
					Iter=1;
				continue;
			}
			Iter++;
		}
		//contr.Delete_vertex (ver);
	}
	//=================================Last==============================================
	public void Last()
	{
	}
}
