using UnityEngine;
using System.Collections;
public enum State_of_Background{ Background,Vertex,Edge,Block}
public class Background : MonoBehaviour {
	public State_of_Background state;
	public Vertex _this_vertex;
	public Edge _this_edge;
	public float Scele_const;
	public Vector3 pos;
	public float _up_down, _left_right;
	public Vector3 scale;
	public bool _inProcent_up_down, _inProcent_left_right;
	public Camera camera;
	private nameAlgorithm algorithm;
	private Controller contr;
	void Awake()
	{
		contr = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Controller>();
		algorithm = GameObject.FindGameObjectWithTag ("nameAlgorithm").GetComponent<nameAlgorithm> ();
		switch(state)
		{
		case State_of_Background.Vertex:
		{
			_this_vertex=GetComponentInParent<Vertex> ();
			break;
		}
		case State_of_Background.Edge:
		{
			_this_edge=GetComponentInParent<Edge> ();
			break;
		}
		}
	}
	//
	void OnMouseEnter()
	{
		switch(state)
		{
		case State_of_Background.Vertex:
		{
			switch(contr.getState())
			{
			case State_of_Controller.Play:
			{
				break;
			}
			}
			break;
		}
		}
	}
	void OnMouseOver()
	{
		switch(state)
		{
		case State_of_Background.Vertex:
		{
			switch(contr.getState())
			{
			case State_of_Controller.Edit:
			{
				if (Input.GetMouseButton (0))
				{
					contr.Add(_this_vertex);
				}
				if(Input.GetMouseButton(1))
					contr.Delete_vertex(_this_vertex);
				break;
			}
			case State_of_Controller.Pick:
			{
				//contr.PickAction(_this_vertex);
				break;
			}
			case State_of_Controller._choseStartVertex:
			{
				//contr.searchStartVertex(_this_vertex);
				break;
			}
				/*/
			case State_of_Controller.Play:
			{
				break;
			}
				/*/
			}
			break;
		}
		case State_of_Background.Background:
		{
			switch(contr.getState())
			{
			case State_of_Controller.Edit:
			{
				if (Input.GetMouseButtonDown (0))
					contr.Add();
				break;
			}
			case State_of_Controller.Pick:
			{
				//contr.PickAction();
				break;
			}
			}
			break;
		}
		case State_of_Background.Edge:
		{

			switch(contr.getState())
			{
			case State_of_Controller.Edit:
			{
				break;
			}
			case State_of_Controller.Pick:
			{
				break;
			}
			}
			break;
		}
		}
	}
	//
	void Update()
	{
		if(state==State_of_Background.Block)
		{
			if(camera==null)
				return;
			Vector3 new_pos = new Vector3 (_left_right*camera.pixelWidth, 
			                               _up_down*camera.pixelHeight, 0);
			new_pos=camera.ScreenToWorldPoint(new_pos);
			transform.localPosition = new_pos + pos;
			new_pos=new Vector3();
			new_pos.x=(_inProcent_left_right?Scele_const*camera.pixelWidth:scale.x);
			new_pos.y=(_inProcent_up_down?Scele_const*camera.pixelHeight:scale.y);
			new_pos.z=transform.localScale.z;
			transform.localScale=new_pos;
		}
	}
}
