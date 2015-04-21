using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Fortune_Fuc
{
	delegate float Fun(float i);
	public static List<Vector3> buildFun(float[] cFun,RectScene rect,float deltaX)
	{
		try
		{
			Fun fun= x=> cFun[0]*x*x+cFun[1]*x+cFun[2];
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
	public static float[] buildParabola(Vector2 ver,float _y)
	{

		float[] fun = new float[3];
		float delta_x = 1f;
		float y1=(delta_x*delta_x+ver.y*ver.y-_y*_y)/(2*(ver.y-_y));
		Vector2 vertex_of_parabola = new Vector2 (ver.x, _y + (ver.y - _y) / 2);
		fun [0] = (y1-vertex_of_parabola.y) / (delta_x*delta_x);
		fun [1] = -2 * fun [0] * vertex_of_parabola.x;
		fun [2] = vertex_of_parabola.y - fun [0] * vertex_of_parabola.x * vertex_of_parabola.x - fun [1] * vertex_of_parabola.x;
		return fun;
	}
}