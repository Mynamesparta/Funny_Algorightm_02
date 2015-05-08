using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fortune_;
using System.Threading;

[System.Serializable]
public enum _NameAlgorithm{Fortune=0,Kyle_Kirkpatrick=3,Andrew_Edwin,Graham,Last };
[System.Serializable]
public struct Colors
{
	public Color start,end;
}
[System.Serializable]
public struct Colors_For_Algorithm
{
	public Colors Graham;
	public Colors Last;
	public Colors CoastLine;
	public Colors SweepLine;
	public Colors VoronoiEdge;
	public Colors DeloneEdge;
}
[System.Serializable]
public class RectScene
{
	public static float begin_minW, begin_maxW, begin_minH, begin_maxH;
	public float _minW,_maxW;
	public RectScene()
	{
		_minW = begin_minW;
		_maxW = begin_maxW;
	}
	public float minW
	{
		get{ return _minW;}
		set{ _minW = (value < begin_minW ||value==float.NaN? begin_minW : value);}
	}
	public float maxW
	{
		get{return _maxW;}
		set{_maxW=(value > begin_maxW||value==float.NaN ? begin_maxW : value);}
	}
	//public float  minH, maxH;
}
[System.Serializable]
public struct Option
{
	public bool FullBeachLine;
	public bool Clear_before_reBuild;
	public bool isRevertes;
	public bool VoloronoiEdge;
	public bool allEvent;
	public bool Delete;
	public bool OneEventForOneBranch;
	public bool addAllPointEvent;
	public float Epcilon_for_Point_event;
	public bool FullBuild;
	public float speedOfAlgorightm;
	public bool BuildParabols;
	public float Epcilon_for_Y;
	public bool normalizationEvent;
	[Range(1, 5)]
	public int LimitOf_reBuild;
	public Vector3[] GhostVertex;
	public bool PrintTime;
}
public class nameAlgorithm : MonoBehaviour {
	public Controller contr;
	public Text text_name;
	public Colors_For_Algorithm COLORS;
	public RectScene standartRect;
	public float maxH;
	public float minH;
	public float delta_X=1f;
	[System.NonSerialized]
	public Vertex vertex_for_test;
	public List<Vertex> list_of_Ghost_vertex;
	public Option OPTIONS;
	//public bool FullBeachLine=true;

	public delegate IEnumerator Algorightm();
	private _NameAlgorithm state;
	private Recorder record;
	private int lenght=6;
	private float lenght_of_SweepLine=1000;
	Algorightm algo;
	Line line,parabola;
	void Awake()
	{
		record = GameObject.FindGameObjectWithTag ("Recorder").GetComponent<Recorder> ();
		//setAlgorihtm (_NameAlgorithm.Graham);
		//_Test ();
		//RectScene.begin_maxH = standartRect.maxH;
		//RectScene.begin_minH = standartRect.minH;
		RectScene.begin_maxW = standartRect.maxW;
		RectScene.begin_minW = standartRect.minW;
		RectScene.begin_maxH = maxH;
		RectScene.begin_minH = minH;

		Fortune_.Event.NA = this;
		VoronoiEdge.NA = this;
		VoronoiVertex.NA = this;
		Fortune_.Function.isReverse = OPTIONS.isRevertes;
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
			StartCoroutine(algo ());
		else
			print ("algo=null");
		//record.Play();
	}
	static void _Test()
	{
	}
	public void Set()
	{
		float index = GetComponent<UnityEngine.UI.Scrollbar> ().value;
		if (index < 0.2f) 
		{
			text_name.text="Fortune";
			state=_NameAlgorithm.Fortune;
			algo=Fortune_algorithm;
			return;
		}
		if(index<0.4f)
		{
			text_name.text="Kyle Kirkpatrick";
			state=_NameAlgorithm.Kyle_Kirkpatrick;
			algo=Kyle_Kirkpatrick;
			return;
		}
		if(index<0.6f)
		{
			text_name.text="Andrew Edwin";
			state=_NameAlgorithm.Andrew_Edwin;
			algo=Andrew_Edwin;
			return;
		}
		if(index<0.8f)
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
			_fun = new List<Vector3> ();
			_fun.Add (begin.getPos ());
			_fun.Add (end.getPos ());
		}
		record.Add(line,state,_fun);
	}
	public void addLine(Line line, State_of_Line state, Vector3 begin,Vector3 end)
	{
		if (begin != null && end != null) 
		{
			//MonoBehaviour.print(""+begin.ToString()+"->"+end.ToString());
			_fun = new List<Vector3> ();
			_fun.Add (begin);
			_fun.Add (end);
		}
		record.Add(line,state,_fun);
	}
	public Line addLine(Vertex begin,Vertex end,State_of_Line state)
	{
		_fun=new List<Vector3>();//Vector3
		_fun.Add(begin.getPos());
		_fun.Add(end.getPos());
		Line line = contr.createLine ();
		record.Add(line,state,_fun);
		return line;
	}
	public Line addLine(Vector3 begin,Vector3 end,State_of_Line state)
	{
		_fun=new List<Vector3>();
		_fun.Add(begin);
		_fun.Add(end);
		Line line = contr.createLine ();
		record.Add(line,state,_fun);
		return line;
	}
	public Line addParabola(Fortune_.Fun fun,State_of_Line state,RectScene rect)
	{
		_fun = Fortune_.Function.buildFun (fun, rect,delta_X);
		//MonoBehaviour.print ("lenght_of_fun:"+_fun.Count);
		//if (_fun.Count < 10)
		//	return null;
		Line line = contr.createLine ();
		record.Add (line, state, _fun);
		return line;
	}
	public  Line addParabola(Line line,Fortune_.Fun fun,State_of_Line state,RectScene rect)
	{
		_fun = Fortune_.Function.buildFun (fun, rect,delta_X);
		//MonoBehaviour.print ("lenght_of_fun:"+_fun.Count);
		//Line line = contr.addLine ();
		record.Add (line, state, _fun);
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
	public void endOfAlgorightm()
	{
		record.StartPlay ();
		contr.StartPlaying ();
	}
	//=====================================Fortune================
	void Update()
	{
		//current_pos_line = -1000;
		//print (test.ToString () + " " + (current_pos_line < 1000).ToString ());


	}
	bool test=false;
	public delegate void reBuild(float y);
	public delegate void Build ();
	public event reBuild re_build;
	public event Build buildVoronoi;
	public event Build buildDelone;
	public event Build destroy;

	public float line_y = 0.05f;
	List<Fortune_.Event> list_of_Event;
	float cur_Y=float.MaxValue;
	List<Fortune_.VoronoiEdge> list_of_edge;
	List<Fortune_.Vertex_Event> list_of_vertexEvent;
	public void AddPointEvent(Fortune_.Vertex_Event ev)
	{
		//MonoBehaviour.print ("addPointEvent");
		if (list_of_vertexEvent == null) 
		{
			list_of_vertexEvent=new List<Vertex_Event>();
			//MonoBehaviour.print("list_of_Event==null");
			//return;
		}
		if(ev.Y>cur_Y)
		{
			if(OPTIONS.PrintTime)
				MonoBehaviour.print("ev.Y > current Y");
			if(OPTIONS.normalizationEvent)
				return;
		}
		if (list_of_vertexEvent.Count == 0) 
		{
			list_of_vertexEvent.Add(ev);
			//MonoBehaviour.print("Count:"+list_of_vertexEvent.Count);
			if(OPTIONS.allEvent)
			{
				LoadPointEvent();
			}
			return;
		}
		for (int i=0;i<list_of_vertexEvent.Count;i++) 
		{
			if(list_of_vertexEvent[i].Y<ev.Y)
			{
				list_of_vertexEvent.Insert(i,ev);
				//MonoBehaviour.print("Count:"+list_of_vertexEvent.Count);
				return;
			}
		}
		list_of_vertexEvent.Insert (list_of_vertexEvent.Count, ev);
		if(OPTIONS.allEvent)
		{
			LoadPointEvent();
		}
	}
	public void LoadPointEvent()
	{
		if (list_of_vertexEvent==null||list_of_vertexEvent.Count == 0)
			return;
		Fortune_.Vertex_Event ev=list_of_vertexEvent[list_of_vertexEvent.Count-1];
		list_of_vertexEvent.Clear ();
		//MonoBehaviour.print ("addPointEvent");
		if (list_of_Event == null) 
		{
			//MonoBehaviour.print("list_of_Event==null");
			return;
		}
		if(ev.Y>cur_Y)
		{
			//MonoBehaviour.print("ev.Y > current Y");
			return;
		}
		if (list_of_Event.Count == 0) 
		{
			list_of_Event.Add(ev);
			//MonoBehaviour.print("Count:"+list_of_Event.Count);
		}
		for (int i=0;i<list_of_Event.Count;i++) 
		{
			if(list_of_Event[i].Y<ev.Y)
			{
				list_of_Event.Insert(i,ev);
				//MonoBehaviour.print("Count:"+list_of_Event.Count);
				return;
			}
		}
		list_of_Event.Insert (list_of_Event.Count, ev);
	}
	public void addEdge(Fortune_.VoronoiEdge edge)
	{
		if(list_of_edge==null)
		{
			list_of_edge = new List<VoronoiEdge> ();
		}
		list_of_edge.Add (edge);
	}
	public IEnumerator Fortune_algorithm ()
	{
		buildDelone = null;
		buildVoronoi = null;
		re_build = null;
		destroy = null;
		list_of_Ghost_vertex=new List<Vertex>();
		Vertex ghost_vertex;
		for(int i=0;i<OPTIONS.GhostVertex.Length;i++)
		{
			ghost_vertex=contr.Add(OPTIONS.GhostVertex[i]);
			ghost_vertex.Index=-i-1;
			list_of_Ghost_vertex.Add(ghost_vertex );
		}
		Line Sweep= addLine (new Vector3 (-lenght_of_SweepLine/2, 300), new Vector3 (lenght_of_SweepLine/2, 300), State_of_Line.Flesh);
		Sweep.setColor (COLORS.SweepLine.start, COLORS.SweepLine.end);
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		vertexs = Sort (vertexs, _isGreaterKyle_Kirkpatrick);
		list_of_Event=new List<Fortune_.Event>();
		list_of_edge = new List<VoronoiEdge> ();

		Binary_Tree.Binary_search_tree BTree = new Binary_Tree.Binary_search_tree (this);

		//Binary_search_tree
		for (int i=vertexs.Length-1; i>=0; i--) 
		{
			var se = new Site_Event(vertexs[i]);
			if(se==null)
				print ("se==null");
			list_of_Event.Add(se);
		}
		Fortune_.Fun function;
		Fortune_.Site_Event siteE;
		Fortune_.Vertex_Event vertexE;
		int Index_of_Step = 0;
		bool isTimetoReBuild;
		float lastBuild=-1000f;
		cur_Y = 200;
		while(list_of_Event.Count>0) 
		{
			
			if(OPTIONS.PrintTime)
				MonoBehaviour.print("=================before="+Index_of_Step+"==============");
			isTimetoReBuild=true;
			Index_of_Step++;
			Fortune_.Event ev=list_of_Event[0];
			if(OPTIONS.FullBuild&&cur_Y-ev.Y>OPTIONS.speedOfAlgorightm)
			{
				cur_Y-=OPTIONS.speedOfAlgorightm;
				addLine(Sweep,State_of_Line.Flesh,new Vector3 (-lenght_of_SweepLine/2, cur_Y), new Vector3 (lenght_of_SweepLine/2, cur_Y));
				if(re_build!=null)
				{
					record.setWithoutPause(true);
					re_build(cur_Y);
					buildVoronoi();
					record.setWithoutPause(false);
				}
				yield return new WaitForSeconds(0.0f);
				continue;
			}
			list_of_Event.RemoveAt(0);
			cur_Y=ev.Y;
			//MonoBehaviour.print("Count:"+list_of_Event.Count);
			addLine(Sweep,State_of_Line.Flesh,new Vector3 (-lenght_of_SweepLine/2, ev.Y-0.05f), new Vector3 (lenght_of_SweepLine/2, ev.Y-0.05f));
			record.setWithoutPause(true);
			switch(ev.getEvent())
			{
			case Fortune_.EVENT.Site:
			{
				//====================Site=Event================
				if(OPTIONS.PrintTime)
					BTree.Test ();

				siteE=(Fortune_.Site_Event) ev;
				if(OPTIONS.PrintTime)
					MonoBehaviour.print("Site_Evenet:"+siteE.Y);
				BTree.Add(siteE,ev.Y);

				break;
				//===============================================
			}
			case Fortune_.EVENT.Vertex:
			{
				//============================Vertex=Event========
				if(OPTIONS.PrintTime)
					BTree.Test ();

				vertexE=(Fortune_.Vertex_Event)ev;
				if(OPTIONS.PrintTime)
					MonoBehaviour.print("Vertex_Event:"+vertexE.Y);
				if(OPTIONS.Delete)
					isTimetoReBuild=BTree.Delete(vertexE._branch,ev.Y);
				if(OPTIONS.PrintTime)
					BTree.Test ();
				break;
				//================================================
			}
			}
			if(OPTIONS.PrintTime)
			{
				BTree.Test ();
				MonoBehaviour.print("=================after===============");
				MonoBehaviour.print("=================rebuild=============");
			}
			if(isTimetoReBuild)
			{
				re_build(ev.Y-0.05f);
				lastBuild=ev.Y-0.05f;
				if(OPTIONS.PrintTime)
				{
					BTree.Test ();
					MonoBehaviour.print("=================build===============");
				}
				if (buildVoronoi != null)
				{
					buildVoronoi ();
				}
				record.setWithoutPause(false);
			}
			yield return new WaitForSeconds(0.0f);
		}
		//
		lastBuild += 1f;
		lastBuild = RectScene.begin_minH*37/64;
		for(;lastBuild>=RectScene.begin_minH*37/64;lastBuild--)
		{
			MonoBehaviour.print("=================The=Last=rebuild=============");
			addLine(Sweep,State_of_Line.Flesh,new Vector3 (-lenght_of_SweepLine/2, lastBuild), new Vector3 (lenght_of_SweepLine/2, lastBuild));
			record.setWithoutPause(true);
			re_build(lastBuild);
			BTree.Test ();
			MonoBehaviour.print("=================The=Last=Build=============");
			if (buildVoronoi != null)
			{
				buildVoronoi ();
			}
			record.setWithoutPause(false);
		}
		record.setPause (true);
		addLine (Sweep, State_of_Line.Flesh_End);
		record.setPause (false);
		if (buildDelone != null)
			buildDelone ();
		//BTree.Test ();
		//
		if (destroy != null)
			destroy ();
		endOfAlgorightm ();
	}
	//=====================================Kyle=Kirkpatrick================
	List<Vector3> _fun;
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
	public IEnumerator Kyle_Kirkpatrick()
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
			yield return new WaitForSeconds(0.0f);
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
			//yield return new YieldInstruction();
			goto end;
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
	end:;
		endOfAlgorightm ();
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
	public IEnumerator Andrew_Edwin()
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
			yield return new WaitForSeconds(0.0f);
		}
		addLine (_Piece_of_Cake, State_of_Line.End_Time);
		searchAndrew_Edwin (Up, Down [0]);
		Down.Insert (0,Up [0]);
		Down.Insert (Down.Count,Up[Up.Count-1]);
		searchAndrew_Edwin(Down,Up[1]);
		endOfAlgorightm ();

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
	public IEnumerator Graham()
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
			addLine(center,vertexs[i].getPos(),State_of_Line.Without_Restrictions).setColor(COLORS.Graham.start,COLORS.Graham.end);
			lines.Add(addLine(vertexs[i],vertexs[i+1],State_of_Line.Without_Restrictions));
		}
		addLine(center,vertexs[vertexs.Length-1].getPos(),State_of_Line.Without_Restrictions).setColor(COLORS.Graham.start,COLORS.Graham.end);
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
			yield return new WaitForSeconds(0.0f);
		}
		print (Iter);
		endOfAlgorightm ();
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
			line_1.setColor (COLORS.Last.start, COLORS.Last.end);
		}
		if(_Last (point, right, rightList))
		{
			line_2.setColor (COLORS.Last.start, COLORS.Last.end);
		}
		return true;
	}
	List<Vertex> resultLast;
	public IEnumerator Last()
	{
		Vertex[] vertexs=contr.getVertexs ().ToArray();
		vertexs = Sort (vertexs, _isGreaterAndrew_Edwin);
		List<Vertex> Up=new List<Vertex>(), Down=new List<Vertex>();
		Vertex left=vertexs[0], right=vertexs[vertexs.Length-1];
		addLine (left, right, State_of_Line.Without_Restrictions).setColor (COLORS.Last.start, COLORS.Last.end);
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
			yield return new WaitForSeconds(0.0f);
		}
		resultLast = new List<Vertex> ();
		_Last (left.getPos(), right.getPos(), Up);
		foreach(Vertex vertex in vertexs)
		{

		}
		_Last (right.getPos (), left.getPos (), Down);
		endOfAlgorightm ();
	}
}
