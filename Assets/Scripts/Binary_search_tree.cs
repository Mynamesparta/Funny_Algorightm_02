using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Data
{
}
public class Binary_search_tree:MonoBehaviour  
{
	public Branch tree;
	public Branch Search(float Key)
	{
		Branch parent = tree;//, child;
		if(parent==null)
		{
			return null;
		}
		while(true)
		{
			//print("search:"+parent.key);
			if(parent.key==Key)
			{
				//зміна даних
				return parent;
			}
			if (parent.key > Key)
			{
				if(parent.left==null)
				{
					return null;
				}
				parent=parent.left;
			}
			else
			{
				if(parent.right==null)
				{
					return null;
				}
				parent=parent.right;
			}
		}
		return null;
	}
	public bool Add(float Key)//,Data data)
	{
		Branch parent = tree;//, child;
		if(parent==null)
		{
			tree= new Branch();
			tree.key=Key;
			return true;
		}
		while(true)
		{
			//print("add:"+parent.key);
			if(parent.key==Key)
			{
				//parent.data=data;
				return false;
			}
			if (parent.key > Key)
			{
				if(parent.left==null)
				{
					parent.left=new Branch();
					//parent.left.parent=parent;
					parent.left.key=Key;
					return true;
				}
				parent=parent.left;
			}
			else
			{
				if(parent.right==null)
				{
					parent.right=new Branch();
					//parent.right.parent=parent;
					parent.right.key=Key;
					return true;
				}
				parent=parent.right;
			}
		}
		return false;
	}
	public bool Delete(float Key)
	{
		Branch parent = tree;
		if(parent.key==Key)
		{
			Branch child=parent;
			if(child.left==null&&child.right==null)
			{
				tree=null;
				return true;
			}
			if(child.left!=null&&child.right==null)
			{
				tree=child.left;
				return true;
			}
			if(child.left==null&&child.right!=null)
			{
				tree=child.right;
				return true;
			}
			
			if(child.left!=null&&child.right!=null)
			{
				if(child.right.left==null)
				{
					child.right.left=child.left;
					tree=child.right;
					return true;
				}
				else
				{
					Branch p=child.right,c=p.left;
					while(c.left!=null)
					{
						p=c;
						c=p.left;
					}
					p.left=c.right;
					c.right=child.right;
					c.left=child.left;
					tree=c;
					
				}
			}
			return true;
		}
		else
		{
			Branch child;
			bool isLeft=false;
			if(parent.key>Key)
			{
				child=parent.left;
			}
			else
			{
				child=parent.right;
			}
			while(true)
			{
				if(child==null)
					return false;
				if(child.key==Key)
					break;
				if(child.key>Key)
				{
					parent=child;
					child=parent.left;
					isLeft=true;
				}
				else
				{
					parent=child;
					child=parent.right;
					isLeft=false;
				}
			}
			if(child.left==null&&child.right==null)
			{
				if(isLeft)
				{
					parent.left=null;
				}
				else
				{
					parent.right=null;
				}
				return true;
			}
			if(child.left!=null&&child.right==null)
			{
				if(isLeft)
				{
					parent.left=child.left;
				}
				else
				{
					parent.right=child.left;
				}
				return true;
			}
			if(child.left==null&&child.right!=null)
			{
				if(isLeft)
				{
					parent.left=child.right;
				}
				else
				{
					parent.right=child.right;
				}
				return true;
			}
			
			if(child.left!=null&&child.right!=null)
			{
				if(child.right.left==null)
				{
					child.right.left=child.left;
					if(isLeft)
					{
						parent.left=child.right;
					}
					else
					{
						parent.right=child.right;
					}
					return true;
				}
				else
				{
					Branch p=child.right,c=p.left;
					while(c.left!=null)
					{
						p=c;
						c=p.left;
					}
					p.left=c.right;
					c.right=child.right;
					c.left=child.left;
					if(isLeft)
					{
						parent.left=c;
					}
					else
					{
						parent.right=c;
					}

				}
			}

		}
		return true;
	}
	public void Print()
	{
		Queue<Branch> que=new Queue<Branch>();
		que.Enqueue (tree.left);
		que.Enqueue (tree.right);
		Branch left, right;
		string sleft, sright;
		while(que.Count>0)
		{
			left=que.Dequeue();
			right=que.Dequeue();
			if(left==null)
				sleft="null";
			else
			{
				sleft=left.key.ToString();
				que.Enqueue(left.left);
				que.Enqueue(left.right);
			}
			if(right==null)
				sright="null";
			else
			{
				sright=right.key.ToString();
				que.Enqueue(right.left);
				que.Enqueue(right.right);
			}
			print(sleft+" "+sright);
		}
	}
}
public class Branch
{
	public float key;
	public Data data;
	//float _key=0;
	//public Branch parent;
	public Branch left;
	public Branch right;
}
