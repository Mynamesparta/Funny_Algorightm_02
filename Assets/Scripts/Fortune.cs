using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Binary_Tree;

namespace Fortune_
{
	public delegate float Fun(float i);
	//----------------------------------------------Function----------------------------------------------------------------------------------
	public static class Function
	{
		public static bool isReverse=false;
		public static List<Vector3> buildFun(Fun fun,RectScene rect,float deltaX)
		{
			try
			{
				if(isReverse&&rect.maxW<rect.minW)
				{
					float a=rect.maxW;
					rect.maxW=rect.minW;
					rect.minW=a;
				}
				List<Vector3> list=new List<Vector3>();
				float _x=rect.minW;
				float _y=fun(_x);
				list.Add(new Vector3(_x,_y));
				//MonoBehaviour.print("minW:"+_x);
				bool b=true;
				//int Index=0;
				while(true)
				{
					if(_x>10000)
						break;
					if(_x>rect.maxW)
					{
						_x=rect.maxW;
						_y=fun(_x);
						//if(_y<RectScene.begin_maxH)
						list.Add(new Vector3(_x,_y));
						break;
					}
					_y=fun(_x);
					if(float.IsNaN(_y))
					{
						//_x+=deltaX;
						break;
					}
					if(_y>RectScene.begin_maxH)
					{
						if(b)
						{
							_x+=deltaX;
							continue;
						}
						else
						{
							list.Add(new Vector3(_x,_y));
							break;
						}
					}
					else
						b=false;
					list.Add(new Vector3(_x,_y));
					_x+=deltaX;
				}
				return list;
			}
			finally
			{
				//MonoBehaviour.print("BOOOOOOOOMMMMMMMM!!!!");
			}
			return null;
		}
		public static Fun buildParabola(Vector2 ver,float _y)
		{
			if (ver.y == _y)
				return null;
			float[] fun = new float[3];
			float delta_x = 1f;
			float y1=(delta_x*delta_x+ver.y*ver.y-_y*_y)/(2*(ver.y-_y));
			Vector2 vertex_of_parabola = new Vector2 (ver.x, _y + (ver.y - _y) / 2);
			fun [0] = (y1-vertex_of_parabola.y) / (delta_x*delta_x);
			fun [1] = -2 * fun [0] * vertex_of_parabola.x;
			fun [2] = vertex_of_parabola.y - fun [0] * vertex_of_parabola.x * vertex_of_parabola.x - fun [1] * vertex_of_parabola.x;
			return x=> fun[0]*x*x+fun[1]*x+fun[2];
		}
		public static void buildParabola(Vector2 ver,float _y,out float[] fun)
		{
			fun = new float[3];
			if (ver.y == _y)
				return ;
			float delta_x = 1f;
			float y1=(delta_x*delta_x+ver.y*ver.y-_y*_y)/(2*(ver.y-_y));
			Vector2 vertex_of_parabola = new Vector2 (ver.x, _y + (ver.y - _y) / 2);
			fun [0] = (y1-vertex_of_parabola.y) / (delta_x*delta_x);
			fun [1] = -2 * fun [0] * vertex_of_parabola.x;
			fun [2] = vertex_of_parabola.y - fun [0] * vertex_of_parabola.x * vertex_of_parabola.x - fun [1] * vertex_of_parabola.x;
			//return x=> fun[0]*x*x+fun[1]*x+fun[2];
		}
		public static bool buildParabola(ref Site_Event sevent,float _y)
		{
			if (_y == sevent.current_line_y)
				return false;
			if (sevent == null) 
			{
				return false;
			}
			float[] fun=new float[3];
			buildParabola (sevent.getVector (), _y, out fun);
			if(fun==null)
			{
				return false;
			}
			sevent.CFun = fun;
			sevent.current_line_y = _y;
			return true;
		}
		public static float getIntersection (Site_Event left,Site_Event right)
		{
			if (left == null || right == null) 
			{
				return float.MaxValue;
			}
			float[] leftFun=left.CFun, rightFun=right.CFun;
			if(leftFun==null)
			{
				MonoBehaviour.print("left fuction null");
				return float.NaN;
			}
			if(rightFun==null)
			{
				MonoBehaviour.print("right fuction null");
				return float.NaN;
			}
			float[] cFun = new float[3];
			for (int i=0; i<cFun.Length; i++)
				cFun [i] = leftFun [i] - rightFun [i];
			if (cFun [0] == 0) 
			{
				return -cFun[2]/cFun[1];
			}
			float x_1, x_2;
			float D = cFun [1] * cFun [1] - 4 * cFun [0] * cFun [2];
			if(D<0)
			{
				Console.WriteLine("Something wrong D<0 hm......");
				return 0f;
			}
			D = (float)Math.Sqrt (D);
			x_1 = (-cFun [1] + D) / (2*cFun [0]);
			x_2 = (-cFun [1] - D) / (2*cFun [0]);
			return((left.Y>right.Y?x_2<x_1:x_1<x_2) ? x_2 : x_1);
		}
		//public static float 
		public static Vector3 getCenter(Vector3 a,Vector3 b, Vector3 c)
		{
			Vector3 vec1=b-a, vec2=c-b;
			float col_1=vec2.x/vec1.x,col_2=vec2.y/vec1.y;
			//if(col_1==col_2||col_1==-col_2);
			//	return new Vector3 (float.NaN, float.NaN, float.NaN);
			float m_a = (b.y - a.y) / (b.x - a.x);
			float m_b = (c.y - b.y) / (c.x - b.x);
			float x = (m_a * m_b * (a.y - c.y) + m_b * (a.x + b.x) - m_a * (b.x + c.x)) / (2 * (m_b - m_a));
			float y;
			if (m_a != 0)
				y = -1 / m_a * (x - (a.x + b.x) / 2) + (a.y + b.y) / 2;
			else
				y = (a.y + b.y) / 2;
			return new Vector3 (x, y);
		}
		public static bool Circle(Branch branch,out float y)
		{
			y = float.NaN;
			if (branch == null)
				return false;
			Branch parent = branch.parent;
			if (parent == null)
				return false;
			Site_Event left, right;
			if (branch.isLeftBranch) 
			{
				if(parent.left_neighbour==null)
					return false;
				left = parent.left_neighbour.coner.left_data;
				right = parent.coner.right_data;
			}
			else
			{
				if(parent.right_neighbour==null)
				{
					return false;
				}
				left=parent.coner.left_data;
				right=parent.right_neighbour.coner.right_data;
			}
			MonoBehaviour.print (left.I + "+" + branch.coner.left_data.I + "+" + right.I);
			Vector3 center = getCenter (left.getVector(), branch.getVertex ().getPos (), right.getVector());
			y = center.y - Vector3.Distance (center, branch.getVertex ().getPos ());
			if (float.IsNaN (y))
				return false;
			if (float.IsInfinity (y))
				return false;
			MonoBehaviour.print ("Y:" + y);
			return true;
		}
	}
	//---------------------------------------------------------------------------------------------------------------------------------------
	public enum EVENT{Site,Vertex};
	public class Event
	{
		protected EVENT _e;
		protected float _y;
		public static nameAlgorithm NA;
		public EVENT getEvent()
		{
			return _e;
		}
		public float Y
		{
			get{return _y;}
		}
	}	

	public class Site_Event:Event
	{
		public Vertex vertex;
		private float[] _CFun;
		private Line function;
		public float[] CFun
		{
			get{ return _CFun;}
			set
			{
				if(value==null||value.Length!=3)
				{
//					print ("value==null||value.Lenght!=3");
					return;
				}
				_fun= x=> value[0]*x*x+value[1]*x+value[2];
				_CFun=value;
			}
		}

		private Fun _fun;
		public Fun fun
		{
			get
			{
				return _fun;
			}

		}
		public float X
		{
			get{ return vertex.getPos ().x;}
		}
		public float minX=-1000, maxX=1000;
		public float current_line_y=float.MinValue;
		public int I
		{
			get
			{
				return Index;
			}
		}
		int Index;
		static int _Index = 0;
		public Site_Event(Vertex v) //:base()
		{
			_e = EVENT.Site;
			vertex = v;
			_y = vertex.getPos ().y;
			//NA.build += Build;	
			Index = _Index;
			_Index++;

		}
		public Site_Event()
		{
		}

		public Vector3 getVector()
		{
			if (vertex != null)
				return vertex.getPos ();
			else
				return new Vector3();
		}
	}
	public class Vertex_Event:Event
	{
		public Vertex_Event(Vertex a,Vertex b, Vertex c,Binary_Tree.Branch branch)
		{
			_e=	EVENT.Vertex;
			center = Function.getCenter (a.getPos(), b.getPos(), c.getPos());
			_y = center.y-Vector3.Distance(a.getPos(),center);
			_branch = branch;
		}
		public Vertex_Event(Branch branch,float y)
		{
			_e = EVENT.Vertex;
			_y = y;
			_branch = branch;
		}
		public Binary_Tree.Branch _branch;
		public Vector3 center;
	}
	public class VoronoiEdge
	{
		bool isTimeToEnd=false;
		Line line;
		public VoronoiVertex first,second;
		public static nameAlgorithm NA;
		public static Controller contr;
		public VoronoiEdge():base()
		{
			first = new VoronoiVertex (this,true);
			second = new VoronoiVertex (this,false);
			NA.build += Build;
			line = contr.createLine ();
			line.setColor (NA.COLORS.VoronoiEdge.start, NA.COLORS.VoronoiEdge.end);
			NA.addEdge (this);
		}
		public void _Awake()
		{
			line.awake -= _Awake;
		}
		/*/
		public Vector3 first
		{
			get{ return _first.postion;}
		}
		public Vector3 second
		{
			get{ return _second.postion;}
		}
		/*/
		public void endSearch()
		{
			if (isTimeToEnd)
				NA.build -= Build;
			else
				isTimeToEnd = true;
		}
		public void Build()
		{
			//MonoBehaviour.print ("build Voronoi Edge");
			//MonoBehaviour.print (first.postion.ToString () + "->" + second.postion.ToString ());
			NA.addLine (line, State_of_Line.Flesh, first.postion, second.postion);
		}
	}
	public class VoronoiVertex
	{
		VoronoiEdge parent;
		bool isFirst;
		bool isEditable=true;
		Vector3 _position;
		public Vector3 postion
		{
			get{ return _position;}
			set{ _position = (isEditable ? value : _position);}
		}
		public VoronoiVertex(VoronoiEdge edge,bool b)
		{
			parent = edge;
			isFirst = b;
		}

		public void endSearch()
		{
			isEditable = false;
			parent.endSearch ();
		}
		/*public*/ void startSearch()
		{
			isEditable = true;
		}
	}
}
