 using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum _NameAlgorithm{Fortune=0,Kyle_Kirkpatrick=3,Andrew_Edwin,Graham,Last };
[System.Serializable]
public struct Colors_For_Algorithm
{
	public Color sGraham, eGraham;
	public Color sLast,eLast;
}
[System.Serializable]
public struct RectScene
{
	public float minW, maxW, minH, maxH;
}
public class nameAlgorithm : MonoBehaviour {
	public Controller contr;
	public Text text_name;
	public Colors_For_Algorithm COLORS;
	public RectScene standartRect;
	public float delta_X=1f;
	[System.NonSerialized]
	public Vertex vertex_for_test;

	public delegate void Algorightm();
	private _NameAlgorithm state;
	private Recorder record;
	private int lenght=6;
	Algorightm algo;
	Line line,parabola;
	void Awake()
	{
		record = GameObject.FindGameObjectWithTag ("Recorder").GetComponent<Recorder> ();
		//setAlgorihtm (_NameAlgorithm.Graham);
		//_Test ();
	}
	public void  Start_Algoritghm()//Vertex StartVertex)
	{
		if(contr.getLenghtOfVertexs()==0)
		{
			print("vertexs:null");
			return;
		}
		record.StartCreate ();
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
			text_name.text="Fortune";
			state=_NameAlgorithm.Fortune;
			algo=Fortune;
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
		if(index<0.833333333333333333333333333333333f)
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
	public void addLine(Line line, State_of_Line state, Vector3 begin,Vector3 end)
	{
		if (begin != null && end != null) 
		{
			fun = new List<Vector3> ();
			fun.Add (begin);
			fun.Add (end);
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
	public Line addParabola(float[] cFun,State_of_Line state,RectScene rect)
	{
		fun = Fortune_Fuc.buildFun (cFun, rect,delta_X);
		Line line = contr.addLine ();
		record.Add (line, state, fun);
		return line;
	}
	public Line addParabola(Line line,float[] cFun,State_of_Line state,RectScene rect)
	{
		fun = Fortune_Fuc.buildFun (cFun, rect,delta_X);
		//Line line = contr.addLine ();
		record.Add (line, state, fun);
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
	public void setAlgorihtm(_NameAlgorithm name)
	{
		GetComponent<UnityEngine.UI.Scrollbar> ().value=((float)name)/lenght;
		Set ();
	}
	//=====================================Fortune================
	void Update()
	{
		//current_pos_line = -1000;
		//print (test.ToString () + " " + (current_pos_line < 1000).ToString ());


	}
	float current_pos_line =-200;
	bool test=false;
	public void Fortune ()
	{
		line = contr.addLine ();parabola=contr.addLine();
		test = true;
		//
		List<Vertex> list = contr.getVertexs ();
		float _y = current_pos_line;
		addLine (new Vector3 (-500, _y), new Vector3 (500, _y), State_of_Line.Flesh);
		for (int i=0; i<list.Count; i++) 
		{
			float[] fun = Fortune_Fuc.buildParabola (list [i].getPos (), _y);
			addParabola (fun, State_of_Line.Flesh, standartRect);
		}
		//
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
	float getAngel_inPolarCoordinateSystem (Vector3 ver)
	{
		if(ver.x>0&&ver.y>=0)
			return Mathf.Atan(ver.y/ver.x);
		if(ver.x>0&&ver.y<0)
			return Mathf.Atan(ver.y/ver.x)+Mathf.PI*2;
		if (ver.x < 0)
			return Mathf.Atan(ver.y/ver.x)+Mathf.PI;
		if (ver.x == 0)
		if (ver.y > 0)
			return Mathf.PI / 2;
		else
			return Mathf.PI * 3 / 2;
		return -1f;
	}
	bool _isGreaterGraham(Vertex first,Vertex second)
	{
		return getAngel_inPolarCoordinateSystem(first.getPos()-center) >
			getAngel_inPolarCoordinateSystem(second.getPos()-center);
	}
	bool _isGreaterGraham(Vector3 first,Vector3 second)
	{
		first = first - center;
		second = second - center;
		return first.x*second.y-first.y*second.x<0;
	}
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
			addLine(center,vertexs[i].getPos(),State_of_Line.Without_Restrictions).setColor(COLORS.sGraham,COLORS.eGraham);
			lines.Add(addLine(vertexs[i],vertexs[i+1],State_of_Line.Without_Restrictions));
		}
		addLine(center,vertexs[vertexs.Length-1].getPos(),State_of_Line.Without_Restrictions).setColor(COLORS.sGraham,COLORS.eGraham);
		lines.Add(addLine(vertexs[vertexs.Length-1],vertexs[0],State_of_Line.Without_Restrictions));
		List<Vector3> list_of_vertexs = new List<Vector3> ();
		for(int i=0;i<vertexs.Length;i++)
		{
			list_of_vertexs.Add(vertexs[i].getPos());
		}
		int Iter=0;
		//bool first_bool=it_in_Left_side(list_of_vertexs[0],list_of_vertexs[1],center);
		int _i=1000;
		while(Iter<list_of_vertexs.Count)
		{
			_i--;
			if(_i<=0)
				break;
			if(Iter==0)
			{
				print(list_of_vertexs[1].ToString()+list_of_vertexs[list_of_vertexs.Count-1].ToString()+list_of_vertexs[0].ToString());
				if(it_in_Left_side(list_of_vertexs[list_of_vertexs.Count-1],
				                    list_of_vertexs[1],
				                    list_of_vertexs[0]))
				{
					addLine(lines[lines.Count-1],State_of_Line.End_Time);
					addLine(lines[0],State_of_Line.End_Time);
					lines.RemoveAt(lines.Count-1);
					lines.RemoveAt(0);
					lines.Insert(lines.Count,addLine(list_of_vertexs[list_of_vertexs.Count-1],
					                                           list_of_vertexs[1],
					                                           State_of_Line.Without_Restrictions));
					list_of_vertexs.RemoveAt(0);
				}
				else
				{
					Iter++;
				}
				continue;
			}
			if(Iter==list_of_vertexs.Count-1)
			{
				if(it_in_Left_side(list_of_vertexs[list_of_vertexs.Count-2],
				                    list_of_vertexs[0],
				                    list_of_vertexs[list_of_vertexs.Count-1]))
				{
					addLine(lines[lines.Count-2],State_of_Line.End_Time);
					addLine(lines[lines.Count-1],State_of_Line.End_Time);
					lines.RemoveAt(lines.Count-1);
					lines.RemoveAt(lines.Count-1);
					lines.Insert(lines.Count,addLine(list_of_vertexs[list_of_vertexs.Count-2],
					                                           list_of_vertexs[0],
					                                           State_of_Line.Without_Restrictions));
					list_of_vertexs.RemoveAt(list_of_vertexs.Count-1);
					//Iter--;
				}
				else
				{
					Iter++;
				}
				continue;
			}
			if(it_in_Left_side(list_of_vertexs[Iter-1],
			                    list_of_vertexs[Iter+1],
			                    list_of_vertexs[Iter]))
			{
				addLine(lines[Iter-1],State_of_Line.End_Time);
				addLine(lines[Iter  ],State_of_Line.End_Time);
				lines.RemoveAt(Iter);
				lines.RemoveAt(Iter-1);
				lines.Insert(Iter-1,addLine(list_of_vertexs[Iter-1],list_of_vertexs[Iter+1],State_of_Line.Without_Restrictions));
				list_of_vertexs.RemoveAt(Iter);
				Iter--;
				if(Iter<0)
					Iter=0;
				continue;
			}
			Iter++;
		}
		print (Iter);
		//contr.Delete_vertex (ver);
	}
	//=================================Last==============================================
	public float Area (Vector3 a,Vector3 b)
	{
		return 0.5f * Mathf.Abs (a.x * b.y - a.y * b.x);
	}
	bool _Last(Vector3 left,Vector3 right,List<Vertex> vertexs)
	{
		if (vertexs.Count == 0)
			return false;
		Vector3 point;
		if(vertexs.Count==1)
		{
			vertexs[0].setColor(2);
			point = vertexs[0].getPos ();
			addLine (left, point, State_of_Line.Without_Restrictions);//.setColor (COLORS.sLast, COLORS.eLast);
			addLine (right, point, State_of_Line.Without_Restrictions);//.setColor (COLORS.sLast, COLORS.eLast);
			return true;
		}
		int Index_of_max=0;
		float maxArea=Area(left-vertexs[0].getPos(),right-vertexs[0].getPos());
		float area;
		for(int i=1; i<vertexs.Count;i++)
		{
			area=Area(left-vertexs[i].getPos(),right-vertexs[i].getPos());
			if(maxArea<area)
			{
				maxArea=area;
				Index_of_max=i;
			}
		}
		vertexs [Index_of_max].setColor (2);
		point = vertexs [Index_of_max].getPos ();
		vertexs.RemoveAt (Index_of_max);
		Line line_1 = addLine (left, point, State_of_Line.Without_Restrictions);
		Line line_2 = addLine (right, point, State_of_Line.Without_Restrictions);

		List<Vertex> leftList=new List<Vertex>(), rightList=new List<Vertex>();

		foreach(Vertex vertex in vertexs)
		{
			if(it_in_Left_side(left,point,vertex.getPos()))
			{
				leftList.Add(vertex);
			}
			else
			{
				if(!it_in_Left_side(right,point,vertex.getPos()))
				{
					rightList.Add(vertex);
				}
			}
		}//.setColor (COLORS.sLast, COLORS.eLast);
		if(_Last (left, point, leftList))//
		{
			line_1.setColor (COLORS.sLast, COLORS.eLast);
		}
		if(_Last (point, right, rightList))
		{
			line_2.setColor (COLORS.sLast, COLORS.eLast);
		}
		return true;
	}
	List<Vertex> resultLast;
	public void Last()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		vertexs = Sort (vertexs, _isGreaterAndrew_Edwin);
		List<Vertex> Up=new List<Vertex>(), Down=new List<Vertex>();
		Vertex left=vertexs[0], right=vertexs[vertexs.Length-1];
		addLine (left, right, State_of_Line.Without_Restrictions).setColor (COLORS.sLast, COLORS.eLast);
		for(int i=1;i<vertexs.Length-1;i++)
		{
			if(it_in_Left_side(left.getPos(),right.getPos(),vertexs[i].getPos()))
			{
				Up.Add(vertexs[i]);
				vertexs[i].setColor(1);
			}
			else
			{
				Down.Add(vertexs[i]);
				vertexs[i].setColor(3);
			}
		}
		resultLast = new List<Vertex> ();
		_Last (left.getPos(), right.getPos(), Up);
		foreach(Vertex vertex in vertexs)
		{

		}
		_Last (right.getPos (), left.getPos (), Down);
	}
}
