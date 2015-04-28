using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State_of_Controller {Play,Edit,Normal,Pick,_choseStartVertex};
[System.Serializable]
public struct Clone_of_Object
{
	public GameObject clone_of_Vertex;
	public GameObject clone_of_Edge;
	public GameObject clone_of_Line;
}
[System.Serializable]
public struct Parents
{
	public Transform point_parent;
	public Transform line_parent;
}
[System.Serializable]
public struct Scripts
{
	public GameObject canvas;
	public Recorder recorder;
	public nameAlgorithm algorightm;
	public Camera maincamera;
	public File_Controller Fcon;
}

public class Controller : MonoBehaviour {

	public Clone_of_Object CLONE;
	public Parents PARENTS;
	public Scripts SCRIPTS;
	//public GameObject evengameobject;
	//public GameObject button_Chose;
	//public Transform center;
	public uint maxNumberOfVertex=10;
	public bool isTimetoMagicSin=true;
	//public float radius_of_Arey=1;
	public float pixelH;
	public float pixelW;
	public _NameAlgorithm startAgorithm;
	public string startFile;
	public float exitTime;
	//public Vertex startVertex;
	//public Vertex endVertex;
	//public Vector2 CreateZone;
	//public int Active_State=1;
	//public int Pick_State=1;

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
	IEnumerator ExitTime()
	{
		yield return new WaitForSeconds (exitTime);
		print ("Byeee!!");
		Application.Quit ();
	}
	void Awake()
	{
		currendIndex=new Queue<int>(); 
		nextIndex = 1;
		vertexs = new List<Vertex> ();
		lines = new List<Line> ();
		itemsformove = new List<Vertex> ();
		//even = this.GetComponent<Event_System> ();
		//button_Chose.SetActive(false);
		_canvas = SCRIPTS.canvas.GetComponent<Canvas> ();
		Line.magicSin = isTimetoMagicSin;
		if(exitTime>0)
			StartCoroutine (ExitTime ());
	}
	void Start()
	{
		SCRIPTS.algorightm.setAlgorihtm (startAgorithm);
		SCRIPTS.Fcon.Read (startFile);
	}


	public void Edit()
	{
		switch (state) {
		case State_of_Controller.Edit:
		{
			state = State_of_Controller.Normal;
			_canvas.inScene(name_of_Button.NameAlgorithm,false);
			_canvas.inScene(name_of_Button.Play,true);
			GameObject[] edge= GameObject.FindGameObjectsWithTag("Edge");
			break;
		}
		case State_of_Controller.Normal:
		{
			state = State_of_Controller.Edit;
			_canvas.inScene(name_of_Button.NameAlgorithm,true);
			_canvas.inScene(name_of_Button.Play,false);
			break;
		}
		case State_of_Controller.Play:
		{
			state=State_of_Controller.Edit;
			SCRIPTS.recorder.toBegin();
			SCRIPTS.recorder.Pause();
			Delete_vertex(SCRIPTS.algorightm.vertex_for_test);
			_canvas.TimeToRecorder(false);
			_canvas.inScene(name_of_Button.NameAlgorithm,true);
			_canvas.inScene(name_of_Button.FileList,true);
			_canvas.inScene(name_of_Button.Recorder_speed,false);
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
		position = SCRIPTS.maincamera.ScreenToWorldPoint (position);
		position.z = 0;
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
	public Vertex Add()
	{
		Vertex _vertex;
		Vector3 position = getMousePosition ();
		int index = newIndex ();
		_vertex = (Object.Instantiate (CLONE.clone_of_Vertex, position, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
		if (_vertex != null) 
		{
			_vertex.transform.SetParent (PARENTS.point_parent);
			_vertex.setIndex (index);
			vertexs.Add (_vertex);
		}
		return _vertex;
	}
	public Vertex Add(Vector3 position)
	{
		if (position == null)
			return null;
		Vertex _vertex;
		int index = newIndex ();
		_vertex = (Object.Instantiate (CLONE.clone_of_Vertex, position, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
		print ("new Vertex:" + position.ToString ());
		_vertex.setIndex (index);
		_vertex.transform.SetParent (PARENTS.point_parent);
		vertexs.Add (_vertex);
		return _vertex;
	}
	public void Add(List<Vector3> poss)
	{
		int index = 0;
		Vertex _vertex;
		foreach(Vector3 pos in poss)
		{
			index=newIndex();
			_vertex = (Object.Instantiate (CLONE.clone_of_Vertex, pos, Quaternion.Euler (0, 0, 0))as GameObject).GetComponent<Vertex> ();
			_vertex.setIndex (index);
			_vertex.transform.SetParent (PARENTS.point_parent);
			vertexs.Add (_vertex);
		}
	}
	public Line addLine ()
	{
		Line line = (Object.Instantiate (CLONE.clone_of_Line)as GameObject).GetComponent<Line> ();
		line.transform.SetParent (PARENTS.line_parent);
		lines.Add (line);
		return line;
	}
	public void Delete_vertex(Vertex _vertex)
	{
		if (_vertex == null)
			return;
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
		SCRIPTS.recorder.StartCreate ();
		state = State_of_Controller.Play;
		_canvas.Edit (name_of_Button.Play, true);
		_canvas.inScene(name_of_Button.Recorder_speed,true);
		_canvas.inScene(name_of_Button.FileList,false);
		//_canvas.inScene(4,true);
		_canvas.TimeToRecorder (true);
		for (int i=0; i<lines.Count; i++)
			Object.Destroy (lines [i].gameObject);
		lines.Clear ();
		SCRIPTS.algorightm.Start_Algoritghm ();
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
	public int getLenghtOfVertexs()
	{
		return vertexs.Count;
	}
	
}
