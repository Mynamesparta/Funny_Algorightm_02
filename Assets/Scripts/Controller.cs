using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum State_of_Controller {Play,Edit,Normal,Pick,_choseStartVertex};
public class Controller : MonoBehaviour {

	public GameObject clone_of_Vertex;
	public GameObject clone_of_Edge;
	public GameObject clone_of_Line;
	public GameObject evengameobject;
	public GameObject canvas;
	public GameObject button_Chose;
	public Transform point_parent;
	public Transform line_parent;
	public Recorder recorder;
	public nameAlgorithm algorightm;
	public Camera maincamera;
	//public Transform center;
	public uint maxNumberOfVertex=10;
	public float radius_of_Arey=1;
	public float pixelH;
	public float pixelW;
	
	public Vertex startVertex;
	public Vertex endVertex;
	public Vector2 CreateZone;
	public int Active_State=1;
	public int Pick_State=1;

	private State_of_Controller state=State_of_Controller.Normal;
	private Queue<int> currendIndex;
	private List<Vertex> vertexs;
	private List<Line> lines;
	private List<Vertex> itemsformove;
	private int nextIndex;
	private Vertex first_vertex;
	private Vector3 LastPositionMouse;
	private Event even;
	private bool IsPick=false;
	private bool choseEndVertex = false;
	private nameAlgorithm currentAlgorithm;
	private Canvas _canvas;
	//=====================private=function======================================
	void Awake()
	{
		currendIndex=new Queue<int>(); 
		nextIndex = 1;
		vertexs = new List<Vertex> ();
		lines = new List<Line> ();
		itemsformove = new List<Vertex> ();
		//even = this.GetComponent<Event_System> ();
		//button_Chose.SetActive(false);
		_canvas = canvas.GetComponent<Canvas> ();

	}


	public void Edit()
	{
		switch (state) {
		case State_of_Controller.Edit:
			{
				state = State_of_Controller.Normal;
				//button_Chose.SetActive(false);
				_canvas.inScene(0,false);
				//_canvas.inScene(2,false);
				_canvas.inScene(2,true);
				GameObject[] edge= GameObject.FindGameObjectsWithTag("Edge");
					break;
			}
		case State_of_Controller.Normal:
			{
				state = State_of_Controller.Edit;
				_canvas.inScene(0,true);
				//_canvas.inScene(2,true);
				_canvas.inScene(2,false);
				break;
			}
		case State_of_Controller.Play:
		{
			state=State_of_Controller.Edit;
			recorder.toBegin();
			recorder.Pause();
			_canvas.inScene(0,true);
			_canvas.inScene(4,false);
			_canvas.inScene(2,true);
			_canvas.inScene(3,false);
			break;
		}
		}
	}
	//===================================Update=fuction=====================
	void Update()
	{
		switch(state)
		{
		case State_of_Controller.Edit://|State_of_Controller.Pick:
		{
			if(Input.GetButtonDown("Pick"))
			{
				state=State_of_Controller.Pick;
			}
			break;
		}
		}
	}
	void LateUpdate()
	{
	}
	//==========================================Create_of_Destroy=function===============
	//
	int newIndex()
	{
		int index;
		if(currendIndex.Count>0)
		{
			index= currendIndex.Dequeue();
		}
		else
		{
			index=nextIndex;
			nextIndex++;
		}
		if(index>maxNumberOfVertex)
		{
			return -1;
		}
		//print ("newIndex:"+index);
		return index;
	}
	//
	public Vector3 getMousePosition()
	{
		Vector3 position = Input.mousePosition;
		position = maincamera.ScreenToWorldPoint (position);
		position.z = 0;
		/*/
		Vector3 position = new Vector3(Input.mousePosition.x*pixelW,Input.mousePosition.y*pixelH);
		//print (position.y / Camera.main.pixelHeight);
		position = position - new Vector3 (0.5f*Camera.main.pixelWidth*pixelW,0.5f*Camera.main.pixelHeight*pixelH,0f );
		/*/
		return position;
	}
	public Vector3 getDeltaMousePosition()
	{
		//return new Vector3 ();
		//
		Vector3 position;
		if(LastPositionMouse==null||Input.GetMouseButtonDown(0))
		{
			position=new Vector3();
			LastPositionMouse=getMousePosition();
		}
		else
		{
			position=(getMousePosition()-LastPositionMouse);
			LastPositionMouse=getMousePosition();
		}
		//print (position);
		position = new Vector3(position.x,position.y);
		return position;
		//
		//print (even.delta);
		//return position;
	}
	//
	public void Add(Vertex _vertex)
	{

	}
	//
	public void Add()
	{
		Vertex _vertex;
		Vector3 position = getMousePosition ();
		int index = newIndex ();
		_vertex = (Object.Instantiate (clone_of_Vertex, position, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
		if (_vertex != null) 
		{
			_vertex.transform.SetParent (point_parent);
			_vertex.setIndex (index);
			vertexs.Add (_vertex);
		}
	}
	public void Add(Vector3 position)
	{
		Vertex _vertex;
		int index = newIndex ();
		_vertex = (Object.Instantiate (clone_of_Vertex, position, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
		_vertex.setIndex (index);
		_vertex.transform.SetParent (point_parent);
		vertexs.Add (_vertex);
	}
	public void Add(List<Vector3> poss)
	{
		int index = 0;
		Vertex _vertex;
		foreach(Vector3 pos in poss)
		{
			index=newIndex();
			_vertex = (Object.Instantiate (clone_of_Vertex, pos, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
			_vertex.setIndex (index);
			_vertex.transform.SetParent (point_parent);
			vertexs.Add (_vertex);
		}
	}
	public Line addLine ()
	{
		Line line = (Object.Instantiate (clone_of_Line)as GameObject).GetComponent<Line> ();
		line.transform.SetParent (line_parent);
		lines.Add (line);
		return line;
	}
	public void Delete_vertex(Vertex _vertex)
	{
		//Vector3 position = getMousePosition ();
//		currendIndex.Enqueue(_vertex.Index);
		vertexs.Remove(_vertex);
		_vertex.Destroy();
	}
	public void Delete_vertexs()
	{
		foreach(Vertex vertex in vertexs)
		{
			vertex.Destroy();
		}
		vertexs.Clear ();
	}
	/*/
	public void PickAction()
	{
		if(!Input.GetButton("Pick"))
			if(Input.GetMouseButtonUp(0))
		{
			foreach(Vertex _vertex in itemsformove)
				_vertex.setColor(0);
			itemsformove.Clear();
			//print("PickAction removes:"+itemsformove.Count);
			state=State_of_Controller.Edit;
		}
		else
		{
			if(IsPick)
			{
				Vector3 dpos=getDeltaMousePosition();
				//print(dpos);
				foreach(Vertex _vertex in itemsformove)
				{
					_vertex.transform.position=_vertex.transform.position+dpos;
				}
			}
		}

	}
	//
	public void PickAction(Vertex _vertex)
	{
		//Vector3 position= getMousePosition ();
		//
		foreach (Vertex vertex in vertexs) 
		{
			if(Vector3.Distance(vertex.gameObject.transform.position,position)<radius_of_Arey)
			{
				//Area_Empty=false;
		//
				if(Input.GetButton("Pick"))
				{
					if(Input.GetMouseButtonDown(0))
					{
						if(itemsformove.IndexOf(_vertex)==-1)
						{
							itemsformove.Add(_vertex);
							_vertex.setColor(Pick_State);
							//print("PickAction add:"+itemsformove.Count);
						}
					}
					else
					{
						if(Input.GetMouseButtonDown(1))
						{
							itemsformove.Remove(_vertex);
							_vertex.setColor(0);
							//print("PickAction remove:"+itemsformove.Count);
						}
					}
				}
				else
				{
					if(Input.GetMouseButton(0))
					{
						if(itemsformove.IndexOf(_vertex)==-1)
							return;
						IsPick=true;
						Vector3 dpos=getDeltaMousePosition();
						//print(dpos);
						foreach(Vertex vertex in itemsformove)
						{
							vertex.transform.position=vertex.transform.position+dpos;
						}
					}
					else
					{
						IsPick=false;
						if(Input.GetMouseButtonDown(1))
						{
							foreach(Vertex vertex in itemsformove)
							{
								vertexs.Remove(vertex);
								vertex.Destroy();
							}
							itemsformove.Clear();
							state=State_of_Controller.Edit;
							//print("PickAction remove:"+itemsformove.Count);
						}
					}
				}
				return;
		//
			}
		}
		//
		if(!Input.GetButton("Pick"))
			if(Input.GetMouseButtonUp(0))
			{
				foreach(Vertex _vertex in itemsformove)
					_vertex.setColor(0);
				itemsformove.Clear();
				//print("PickAction removes:"+itemsformove.Count);
				state=State_of_Controller.Edit;
			}
			else
			{
				if(IsPick)
				{
					Vector3 dpos=getDeltaMousePosition();
					//print(dpos);
					foreach(Vertex _vertex in itemsformove)
					{
						_vertex.transform.position=_vertex.transform.position+dpos;
					}
				}
			}
		//
	}
	/*/
	public void setAlgorithm(nameAlgorithm na)
	{
		currentAlgorithm = na;
	}
	/*/
	public void choseStartVertex()
	{
		if (state == State_of_Controller.Edit) 
		{
			state = State_of_Controller._choseStartVertex;
			choseEndVertex=false;
		}
		else
			state = State_of_Controller.Edit;
	}
	public void searchStartVertex(Vertex vertex)
	{
		//Vector3 position= getMousePosition ();
		//
		foreach (Vertex vertex in vertexs) 
		{
			if(Vector3.Distance(vertex.gameObject.transform.position,position)<radius_of_Arey)
			{
		//
				if(Input.GetMouseButtonDown(0))
				{
					if(!choseEndVertex)
					{
						//print("heelo");
						if(startVertex!=null)
						{
							startVertex.setStartVertex(0);
						}
						startVertex=vertex;
						vertex.setStartVertex(1);
						choseEndVertex=true;
					}
					else
					{
						if(startVertex==vertex)
							return;
						if(endVertex!=null)
						{
							endVertex.setStartVertex(0);
						}
						endVertex=vertex;
						endVertex.setStartVertex(2);
						choseStartVertex();
						_canvas.Edit(2);
						//_canvas.inScene(2,false);
					}
				}
				return;
		//
			}
		}
		//
	}
	/*/
	public void Play()
	{
		if (state != State_of_Controller.Normal)
			return;
		/*/
		foreach (Vertex vertex in vertexs) 
		{
			vertex.unCheked();
			vertex.AwakeTreeIndex();
			vertex.resetDistanse();
		}
		/*/
		recorder.StartCreate ();
		state = State_of_Controller.Play;
		_canvas.Edit (4, true);
		_canvas.inScene(4,true);
		_canvas.TimeToRecorder ();
		for (int i=0; i<lines.Count; i++)
			Object.Destroy (lines [i].gameObject);
		lines.Clear ();
		algorightm.Start_Algoritghm ();
		/*/
		GameObject[] edge= GameObject.FindGameObjectsWithTag("Edge");
		for(int i=0;i<edge.Length;i++)
		{
			if(edge[i]!=null)
			{
				if(edge[i].GetComponent<Edge>()!=null)
				{
					edge[i].GetComponent<Edge>().unCheked();
					edge[i].GetComponent<Edge>().weight.setEdit(false);
					if(algorightm.state==_NameAlgorithm.Ford_Fulkerson||algorightm.state==_NameAlgorithm.Edmonds_Karp)
						edge[i].GetComponent<Edge>().showStreamField();
				}
			}
		}
		/*/
	}
	public State_of_Controller getState()
	{
		State_of_Controller _state = new State_of_Controller ();
		_state = state;
		return _state;
	}
	public List<Vertex> getVertexs()
	{
		List<Vertex> _vers=new List<Vertex>();
		for(int i=0;i<vertexs.Count;i++)
		{
			_vers.Insert(i,vertexs[i]);
		}
		return _vers;
	}
	public List<Vector3> getPosVertexs()
	{
		List<Vector3> _vers=new List<Vector3>();
		for(int i=0;i<vertexs.Count;i++)
		{
			_vers.Insert(i,vertexs[i].transform.position);
		}
		return _vers;
	}
	public void setPosVertexs(List<Vector3> list)
	{
		Delete_vertexs ();
		Add (list);
	}
	
}
