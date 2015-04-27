using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Fortune_
{
	public delegate float Fun(float i);
	//----------------------------------------------Function----------------------------------------------------------------------------------
	public static class Function
	{
		public static List<Vector3> buildFun(Fun fun,RectScene rect,float deltaX)
		{
			try
			{
				//Fun fun= x=> cFun[0]*x*x+cFun[1]*x+cFun[2];
				List<Vector3> list=new List<Vector3>();
				float _x=rect.minW;
				float _y;
				while(_x<rect.maxW)
				{
					_y=fun(_x);
					if(_y>=rect.minH&&_y<=rect.maxH)
					{
						list.Add(new Vector3(_x,_y));
					}
					_x+=deltaX;
				}
				return list;
			}
			finally
			{
				//print("Lenght != 3");
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
			float[] cFun = new float[3];
			for (int i=0; i<cFun.Length; i++)
				cFun [i] = leftFun [i] - rightFun [i];
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

	public class Site_Event2:Event
	{
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
		
		public Site_Event(Vertex v) //:base()
		{
			_e = EVENT.Site;
			vertex = v;
			_y = vertex.getPos ().y;
			//NA.build += Build;		
		}
		public Site_Event()
		{
		}

		public Vector3 getVector()
		{
			return vertex.getPos ();
		}
	}
	public class Vertex_Event:Event
	{
		public Vertex_Event(float y)
		{
			_e=	EVENT.Vertex;
			_y = y;
		}
	}
}
