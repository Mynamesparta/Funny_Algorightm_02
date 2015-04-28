using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Binary_Tree
{
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
			if (branch.coner.left_data == null)
				MonoBehaviour.print ("branch.coner.left==null");
			bool isLeftBranch = branch.isLeftBranch;
			//MonoBehaviour.print ("=======================");
			//MonoBehaviour.print ("isLeftBranch:"+isLeftBranch.ToString());
			if(branch.state==State_of_Branch.Node)
			{
				MonoBehaviour.print ("it s Node");
				return;
			}
			Branch parent = branch.parent;
			if(parent==null)
				MonoBehaviour.print("2:parent==null");
			if (isLeftBranch) 
			{
				parent.coner.left_data=branch.coner.left_data;
				parent.coner.leftBranch=branch;
				//printEvent("parent.left:",parent.coner.left);
			}
			else
			{
				parent.coner.right_data=branch.coner.left_data;
				parent.coner.rightBranch=branch;
				//printEvent("parent.right:",parent.coner.right);
			}
			int deep = 0;
			while(parent.isLeftBranch==isLeftBranch)
			{
				deep++;
				if(deep>100)
					break;
				MonoBehaviour.print("go up");
				if(parent.parent==null)
					break;
				parent=parent.parent;
			}
			parent=parent.parent;
			if (parent != null) 
			{
				if (isLeftBranch) 
				{
					parent.coner.right_data=branch.coner.left_data;
					parent.coner.rightBranch=branch;
					printEvent("parent.right:",parent.coner.right_data);
				}
				else
				{
					parent.coner.left_data=branch.coner.left_data;
					parent.coner.leftBranch=branch;
					printEvent("parent.left:",parent.coner.left_data);
				}
			}
		}
		float getLimit(Branch branch)
		{
			if(branch.state==State_of_Branch.Node)
			{
				if (branch.isLeftBranch) 
				{
					return branch.coner.right_data.X;
				}
				else
				{
					return branch.coner.left_data.X;
				}
			}
			return branch.coner.left_data.X;
		}
		//======================public=============================
		bool isFirstAdd=true;
		public void Add(Fortune_.Site_Event data,float _y)
		{
			MonoBehaviour.print ("hello new site");
			float Key = data.X;
			bool isFind = false;
			if (data == null)
				MonoBehaviour.print ("data==null");
			if(isFirstAdd)
			{
				isFirstAdd=false;
				//MonoBehaviour.print("new KING Site");
				tree=new Branch();
				tree.coner.left_data=data;
				tree.state=State_of_Branch.Site;
				return ;
			}
			else
			{
				Branch parent,children;
				parent=tree;
				int deep=0;
				while(true)
				{
					deep++;
					MonoBehaviour.print("deep:"+deep);
					if(deep>100)
					{
						MonoBehaviour.print("break..");
						return;
					}
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

						branch.left.coner.left_data=parent.right.coner.left_data=parent.coner.left_data;
						branch.right.coner.left_data=data;

						parent.left_neighbour=branch;
						branch.right_neighbour=parent;

						parent.left.isLeftBranch=true;
						parent.right.isLeftBranch=false;
						branch.left.isLeftBranch=true;
						branch.right.isLeftBranch=false;

						if(parent.coner.left_data.X<Key)
						{
							MonoBehaviour.print ("Right");
						}
						else
						{
							MonoBehaviour.print("Left");
						}
						if(parent.coner.left_data==null)
							MonoBehaviour.print("parent.coner.left==null");
						//MonoBehaviour.print("=======branch=left=====");
						_normalized(branch.left);
						//MonoBehaviour.print("=======branch=right=====");
						_normalized(branch.right);
						//MonoBehaviour.print("=======parent=right=====");
						_normalized(parent.right);
						if(parent.coner.left_data==null||parent.coner.right_data==null)
							MonoBehaviour.print ("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBag!");
						//MonoBehaviour.print ("==================");
						//printEvent("branch left:",branch.coner.left);
						//printEvent("branch right:",branch.coner.right);
						//printEvent("parent left:",parent.coner.left);
						//printEvent("parent right:",parent.coner.right);
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
						//return;
						Branch leftbranchL=new Branch(),rightbranchL=new Branch();
						Branch leftbranchR=new Branch(),rightbranchR=new Branch();

						parent.coner.leftBranch.left=leftbranchL;
						parent.coner.leftBranch.right=rightbranchL;
						parent.coner.rightBranch.left=leftbranchR;
						parent.coner.rightBranch.right=rightbranchR;

						leftbranchL.parent=parent.coner.leftBranch;
						rightbranchL.parent=parent.coner.leftBranch;
						leftbranchR.parent=parent.coner.rightBranch;
						rightbranchR.parent=parent.coner.rightBranch;
						
						leftbranchL.state=leftbranchR.state=rightbranchL.state=rightbranchR.state=State_of_Branch.Site;
						parent.coner.leftBranch.state=parent.coner.rightBranch.state=State_of_Branch.Node;

						leftbranchL.coner.left_data=parent.coner.left_data;
						rightbranchR.coner.left_data=parent.coner.right_data;
						if(Key<parent.getLimit(_y))
						{
							leftbranchR.coner.left_data=data;
							rightbranchL.coner.left_data=parent.coner.left_data;
							MonoBehaviour.print("Left");
						}
						else
						{
							leftbranchR.coner.left_data=parent.coner.right_data;
							rightbranchL.coner.left_data=data;
							MonoBehaviour.print ("Right");
						}

						parent.coner.leftBranch.left_neighbour=parent.left_neighbour;
						parent.coner.rightBranch.right_neighbour=parent.right_neighbour;
						parent.coner.leftBranch.right_neighbour=parent.coner.rightBranch.left_neighbour=parent;
						parent.left_neighbour=parent.coner.leftBranch;
						parent.right_neighbour=parent.coner.rightBranch;

						leftbranchL.isLeftBranch=leftbranchR.isLeftBranch=true;
						rightbranchL.isLeftBranch=rightbranchR.isLeftBranch=false;

						_normalized(leftbranchL);
						_normalized(rightbranchL);
						_normalized(leftbranchR);
						_normalized(rightbranchR);

						return ;
					}
				}
			}
			return ;
		}
		
		public void printEvent(string text,Fortune_.Site_Event ev)
		{
			//MonoBehaviour.print (text + ev.getVector ().ToString ());
		}

	}
	public enum State_of_Branch{Site,Node};
	public struct Coner
	{
		public Fortune_.Site_Event left_data;
		public Branch leftBranch;
		public Fortune_.Site_Event right_data;
		public Branch rightBranch;
		public Line leftParabola, rightParabola;
		
	}
	public class Branch
	{
		bool isCheked;
		public State_of_Branch state;
		public bool isLeftBranch=true;
		public Coner coner;
		public float middle=float.NaN;
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
			coner = new Coner ();
		}
		public void reBuild(float _y)
		{
			//MonoBehaviour.print("Hello new reBuild:" + _y.ToString ());
			if (state == State_of_Branch.Site)
				return;
			//MonoBehaviour.print ("its node");
			if (coner.left_data == null||coner.right_data==null) 
			{
				//MonoBehaviour.print("coner.left==null");
				return;
			}
			//MonoBehaviour.print ("its norm");
			bool leftb = Fortune_.Function.buildParabola (ref coner.left_data, _y);
			bool rightb = Fortune_.Function.buildParabola (ref coner.right_data, _y);
			isCheked = !( leftb&&rightb);
			//MonoBehaviour.print ("isCheked:"+isCheked.ToString());
			if (true||!isCheked) 
			{
				middle = Fortune_.Function.getIntersection (coner.left_data, coner.right_data);
				//MonoBehaviour.print("search midle:"+middle);
				//MonoBehaviour.print(coner.left.getVector().ToString()+coner.right.getVector().ToString());
			}
		}
		
		public void Build()
		{
			//MonoBehaviour.print ("========================");
			//MonoBehaviour.print ("build time");

			if (state == State_of_Branch.Site)
				return;
			//
			NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, new RectScene());
			NA.addParabola (coner.right_data.fun, State_of_Line.Flesh, new RectScene());
			RectScene rect=new RectScene();
			rect.minW=getNeighbourIntersection(false);
			rect.maxW=middle;
			MonoBehaviour.print("left rect:"+rect.minW+"->"+rect.maxW);
			if (rect.minW == float.NaN)
				rect.minW = RectScene.begin_minW;
			if (rect.maxW == float.NaN)
				rect.maxW = RectScene.begin_maxW;
			if (NA.FullBeachLine) 
			{
				if(coner.leftParabola==null)
				{
					coner.leftParabola=NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (coner.leftParabola,coner.left_data.fun, State_of_Line.Flesh, rect);
				}
				rect.minW=middle;
				////MonoBehaviour.print("middle:"+middle);
				rect.maxW=getNeighbourIntersection(true);
				if(right_neighbour==null)
				{
					MonoBehaviour.print("FullBeachLine right rect:"+rect.minW+"->"+rect.maxW);
					////MonoBehaviour.print("stage");
					if(coner.rightParabola==null)
					{
						coner.rightParabola=NA.addParabola (coner.right_data.fun, State_of_Line.Flesh, rect);
					}
					else
					{
						NA.addParabola (coner.rightParabola,coner.right_data.fun, State_of_Line.Flesh, rect);
					}
					//MonoBehaviour.print("end");
				}
			}
			else
			{
				if(coner.leftParabola==null)
				{
					coner.leftParabola=NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (coner.leftParabola,coner.left_data.fun, State_of_Line.Flesh, rect);
				}
				rect.minW=middle;
				rect.maxW=getNeighbourIntersection(true);
				//MonoBehaviour.print("right rect:"+rect.minW+"-"+rect.maxW);
				if(coner.rightParabola==null)
				{
					coner.rightParabola=NA.addParabola (coner.right_data.fun, State_of_Line.Flesh, rect);
				}
				else
				{
					NA.addParabola (coner.rightParabola,coner.right_data.fun, State_of_Line.Flesh, rect);
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
		public void printEvent(string text,Fortune_.Site_Event ev)
		{
			MonoBehaviour.print (text + ev.getVector ().ToString ());
		}
	}
}
