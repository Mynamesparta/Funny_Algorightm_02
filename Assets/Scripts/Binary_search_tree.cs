using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Binary_Tree
{
	public struct Coners
	{
		public Fortune_.Site_Event left;
		public Fortune_.Site_Event right;

	}
	public class Binary_search_tree  
	{
		Branch tree;
		static nameAlgorithm NA;
		public Binary_search_tree(nameAlgorithm algo)
		{
			NA = algo;
			Branch.NA = NA;
		}
		//===================private==========================
		void _normalized(Branch branch)
		{
			if(branch==null)
			{
				//MonoBehaviour.print ("Branch==null");
				return;
			}
			if (branch.left_coner == null)
				MonoBehaviour.print ("branch.left_coner==null");
			bool isLeftBranch = branch.isLeftBranch;
			MonoBehaviour.print ("=======================");
			MonoBehaviour.print ("isLeftBranch:"+isLeftBranch.ToString());
			if(branch.state==State_of_Branch.Node)
			{
				MonoBehaviour.print ("it s Node");
				return;
			}
			Branch parent = branch.parent;
			if(parent==null)
				MonoBehaviour.print("2:parent==null");
			while (parent!=null&&parent.isLeftBranch==isLeftBranch) 
			{
				MonoBehaviour.print("true");
				if(isLeftBranch)
				{
					MonoBehaviour.print ("leftConer");
					parent.left_coner=branch.left_coner;
				}
				else
				{
					MonoBehaviour.print ("rightConer");
					parent.right_coner=branch.left_coner;
				}
				parent=parent.parent;
			}

			if(parent!=null)
			{
				MonoBehaviour.print("parent!=null");
				if(isLeftBranch)
				{
					MonoBehaviour.print ("leftConer");
					parent.left_coner=branch.left_coner;
				}
				else
				{
					MonoBehaviour.print ("rightConer");
					parent.right_coner=branch.left_coner;
				}
			}
			else
			{
			}
		}
		float getLimit(Branch branch)
		{
			if(branch.state==State_of_Branch.Node)
			{
				if (branch.isLeftBranch) 
				{
					return branch.right_coner.X;
				}
				else
				{
					return branch.left_coner.X;
				}
			}
			return branch.left_coner.X;
		}
		//======================public=============================
		bool isFirstAdd=true;
		public void Add(Fortune_.Site_Event data,float _y)
		{
			MonoBehaviour.print ("hello new site");
			float Key = data.X;
			Coners coners=new Coners();
			bool isFind = false;
			if (data == null)
				MonoBehaviour.print ("data==null");
			if(isFirstAdd)
			{
				isFirstAdd=false;
				//MonoBehaviour.print("new KING Site");
				tree=new Branch();
				tree.left_coner=data;
				tree.state=State_of_Branch.Site;
				return ;
			}
			else
			{
				Branch parent,children;
				parent=tree;
				while(true)
				{
					MonoBehaviour.print("State:"+parent.state.ToString());
					if(parent.state==State_of_Branch.Site)
					{
						Branch branch=new Branch();
						parent.left=branch;
						parent.right=new Branch();
						branch.left=new Branch();
						branch.right=new Branch();

						parent.right.parent=parent;
						parent.left.parent=parent;
						branch.left.parent=branch;
						branch.right.parent=branch;

						parent.state = branch.state = State_of_Branch.Node;
						branch.left.state = branch.right.state = parent.right.state = State_of_Branch.Site;

						branch.left.left_coner=parent.right.left_coner=parent.left_coner;
						branch.right.left_coner=data;

						parent.left_neighbour=branch;
						branch.right_neighbour=parent;

						if(parent.left_coner.X<Key)
						{
							MonoBehaviour.print ("Right");
						}
						else
						{
							MonoBehaviour.print("Left");
						}
						parent.left.isLeftBranch=true;
						parent.right.isLeftBranch=false;
						branch.left.isLeftBranch=true;
						branch.right.isLeftBranch=false;
						if(parent.left_coner==null)
							MonoBehaviour.print("parent.left_coner==null");
						_normalized(branch.left);
						_normalized(branch.right);
						_normalized(parent.right);
						if(parent.left_coner==null||parent.right_coner==null)
							MonoBehaviour.print ("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBag!");
						break;
					}
					else
					{
						if(Key<parent.left.getLimit(_y))
						{
							parent=parent.left;
							MonoBehaviour.print("Left");
							continue;
						}
						if(parent.right.getLimit(_y)<Key)
						{
							parent=parent.right;
							MonoBehaviour.print ("Right");
							continue;
						}
						MonoBehaviour.print("I need some magic");
						isFind=true;
						return ;
					}
				}
			}
			return ;
		}

	}
	public enum State_of_Branch{Site,Node};
	public class Branch
	{
		bool isCheked;
		public State_of_Branch state;
		public bool isLeftBranch=true;
		public Fortune_.Site_Event left_coner;
		public Fortune_.Site_Event right_coner;
		public Line leftParabola, rightParabola;
		public float middle;
		//public Fortune_.Site_Event data;
		//float _key=0;
		public Branch parent;
		public Branch left;
		public Branch right;

		public Branch left_neighbour;
		public Branch right_neighbour;

		public static nameAlgorithm NA;
		public Branch()
		{
			NA.re_build += reBuild;
			NA.build += Build;
		}
		public void reBuild(float _y)
		{
			MonoBehaviour.print("Hello new reBuild:" + _y.ToString ());
			if (state == State_of_Branch.Site)
				return;
			MonoBehaviour.print ("its node");
			if (left_coner == null||right_coner==null) 
			{
				MonoBehaviour.print("left_coner==null");
				return;
			}
			MonoBehaviour.print ("its norm");
			isCheked = !(Fortune_.Function.buildParabola (ref left_coner, _y) ||
				Fortune_.Function.buildParabola (ref right_coner, _y));
			if (!isCheked) 
			{
				middle = Fortune_.Function.getIntersection (left_coner, right_coner);
				MonoBehaviour.print("search midle:"+middle);
			}
		}
		
		public void Build()
		{
			MonoBehaviour.print ("========================");
			MonoBehaviour.print ("build time");

			if (state == State_of_Branch.Site)
				return;
			//
			NA.addParabola (left_coner.fun, State_of_Line.Flesh, new RectScene());
			NA.addParabola (right_coner.fun, State_of_Line.Flesh, new RectScene());
			RectScene rect=new RectScene();
			//rect.minW=getNeighbourIntersection(false);
			rect.maxW=middle;
			if (NA.FullBeachLine) 
			{
				if(leftParabola==null)
				{
					leftParabola=NA.addParabola (left_coner.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (leftParabola,left_coner.fun, State_of_Line.Flesh, rect);
				}
				rect.minW=middle;
				MonoBehaviour.print("middle:"+middle);
				//rect.maxW=getNeighbourIntersection(true);
				if(right_neighbour==null)
				{
					MonoBehaviour.print("stage");
					if(rightParabola==null)
					{
						rightParabola=NA.addParabola (right_coner.fun, State_of_Line.Flesh, rect);
					}
					else
					{
						NA.addParabola (rightParabola,right_coner.fun, State_of_Line.Flesh, rect);
					}
				}
				else
				{
					MonoBehaviour.print("end");
				}
			}
			else
			{
				if(leftParabola==null)
				{
					leftParabola=NA.addParabola (left_coner.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (leftParabola,left_coner.fun, State_of_Line.Flesh, rect);
				}
				rect.minW=middle;
				rect.maxW=getNeighbourIntersection(true);
				if(rightParabola==null)
				{
					rightParabola=NA.addParabola (right_coner.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (rightParabola,right_coner.fun, State_of_Line.Flesh, rect);
				}
			}
			//
		}
		public float getLimit(float _y)
		{
			reBuild (_y);
			return middle;
		}
		public float getNeighbourIntersection(bool right)
		{
			if(right)
			{
				if(right_neighbour!=null)
				{
					return right_neighbour.middle;
				}
				else
				{
					return RectScene.begin_maxW;
				}
			}
			else
			{
				if(left_neighbour!=null)
				{
					return left_neighbour.middle;
				}
				else
				{
					return RectScene.begin_minW;
				}
			}
		}
	}
}
