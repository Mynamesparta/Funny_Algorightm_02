using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class File_Controller : MonoBehaviour {
	public GameObject clone_of_file_name;
	public Transform transform_list;
	public InputField name;
	public Scrollbar scrollbar;
	public float number_of_file_name;
	public float size_of_file_name;
	public float deltaRefresh=10f;
	public Animator anim;
	public Controller contr;

	private float size_of_Panel;
	private VerticalLayoutGroup verticalLG;
	private List<File_Input> list;
	private DirectoryInfo directory;
	private File_Input current;
	private float current_time;
	private bool refresh_time=true;
	private bool isFirst=true;
	void Awake()
	{
		verticalLG = GetComponent<VerticalLayoutGroup> ();
		directory = new DirectoryInfo ("Assets//docs//Points_information");
		list = new List<File_Input> ();
		StartCoroutine ("_RefreshFile");
	}
	void Update()
	{
		if(false&&refresh_time)
		{
			current_time += Time.deltaTime;
			if(current_time>deltaRefresh)
			{
				current_time=0;
				RefreshFile();
			}
		}
		if(isFirst)
		{
			isFirst=false;
			RefreshFile();
		}
	}
	IEnumerator _RefreshFile()
	{
		yield return new WaitForSeconds(2f);
		RefreshFile();
	}
	void Refresh()
	{
		float y = 0;
		for(int i=0;i<list.Count;i++)
		{
			y=-size_of_file_name*i;
			list[i].transform.localPosition=new Vector3(list[i].transform.localPosition.x,
			                                            y,
			                                            list[i].transform.localPosition.z);
		}
	}
	string getInputText()
	{
		string text = name.text;
		name.text = "";
		if (text == "")
			text = "None name";
		return text;
	}
	void size_of_listChanged()
	{
		scrollbar.size = number_of_file_name / (number_of_file_name > list.Count ? number_of_file_name : list.Count);
		size_of_Panel = list.Count * size_of_file_name;
		scrollbar.value = 0;
		Refresh ();
	}
	public void RefreshFile()
	{
		for(int i=0;i<list.Count;i++)
			Object.Destroy(list[i].gameObject);
		list.Clear();
		if(directory.Exists)
		{
			FileInfo[] files = directory.GetFiles ("*.txt");
			File_Input file_name;
			for(int i=0 ; i<files.Length;i++)
			{
				file_name = (Object.Instantiate (clone_of_file_name)as GameObject).GetComponent<File_Input>();
				file_name.transform.SetParent (transform_list);
				file_name.transform.localPosition = new Vector3 (0, 0, 0);
				file_name.transform.localScale = new Vector3 (1, 1, 1);
				file_name.setFile(files[i]);
				list.Add(file_name);
			}
			size_of_listChanged();
		}
	}
	public void Add()
	{
		if(!directory.Exists)
			directory.Create ();
		File_Input file_name = (Object.Instantiate (clone_of_file_name)as GameObject).GetComponent<File_Input>();
		file_name.transform.SetParent (transform_list);
		file_name.transform.localPosition = new Vector3 (0, 0, 0);
		file_name.transform.localScale = new Vector3 (1, 1, 1);
		if(file_name.setName (getInputText(),directory))
		{
			list.Insert (0,file_name);
			size_of_listChanged ();
			Save(file_name);
		}
		else
		{
			Object.Destroy(file_name.gameObject);
		}
	}
	public void scrollbarChanged()
	{
		float y = size_of_Panel * scrollbar.value *(1 - scrollbar.size);
		y = (y >= 0 ? y : 0);
		transform_list.localPosition = new Vector3 (transform_list.localPosition.x,
		                                        	y,
		                                         	transform_list.localPosition.z);
	}
	public void setCurrent(File_Input fi)
	{
		if (current != null)
			current.setToCurrentFile (false);
		current = fi;
	}
	public void deleteCurrent()
	{
		if (current == null)
			return;
		list.Remove (current);
		current.Delete ();
		Object.Destroy (current.gameObject);
		current = null;
		size_of_listChanged ();
	}
	void Save(File_Input file_input)
	{
		List<Vector3> list = contr.getPosVertexs ();
		file_input.Write (list);

	}
	public void saveCurrent()
	{
		if(current!=null)
		{
			List<Vector3> list = contr.getPosVertexs ();
			current.Write (list);
		}
	}
	public void renameCurrent()
	{
		if(current!=null)
		{
			string name = getInputText ();
			current.setName(name,directory);
		}
	}
	public void setActive()
	{
		bool b = !anim.GetBool ("inScene");
		anim.SetBool("inScene",b);
		gameObject.SetActive (b);
	}
	public void  readCurrent()
	{
		if(current!=null)
		{
			List<Vector3> list=current.Read();
			contr.setPosVertexs (list);
		}
	}

}
