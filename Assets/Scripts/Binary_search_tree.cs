﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Fortune_;
namespace Binary_Tree
{
	public class Binary_search_tree  
	{
		Branch tree;
		static nameAlgorithm NA;
		int lenght;
		public Binary_search_tree(nameAlgorithm algo)
		{
			NA = algo;
			Branch.NA = NA;
		}
		//===================private==========================
		void _normalized(Branch branch)
		{
			MonoBehaviour.print ("Hello ................");
			if(branch==null)
			{
				MonoBehaviour.print ("Branch==null");
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
				if(false&&parent.left_neighbour!=null)
				{
					parent.left_neighbour.coner.rightBranch=branch;
					parent.left_neighbour.coner.right_data=branch.coner.left_data;
				}
				//branch.right_neighbour=parent;
				//printEvent("parent.left:",parent.coner.left);
			}
			else
			{
				parent.coner.right_data=branch.coner.left_data;
				parent.coner.rightBranch=branch;
				if(false&&parent.right_neighbour!=null)
				{
					parent.right_neighbour.coner.leftBranch=branch;
					parent.right_neighbour.coner.left_data=branch.coner.left_data;
				}
				//branch.left_neighbour=parent;
				//printEvent("parent.right:",parent.coner.right);
			}
			int deep = 0;
			while(parent.isLeftBranch==isLeftBranch)
			{
				deep++;
				if(deep>100)
				{
					MonoBehaviour.print("DEEP!!!!!!!!!!!!!!!!");
					break;
				}
				//MonoBehaviour.print("go up");
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
					if(false&&parent.left_neighbour!=null)
					{
						parent.left_neighbour.coner.rightBranch=branch;
						parent.left_neighbour.coner.right_data=branch.coner.left_data;
					}
					//branch.left_neighbour=parent;
					//printEvent("parent.right:",parent.coner.right_data);
				}
				else
				{
					parent.coner.left_data=branch.coner.left_data;
					parent.coner.leftBranch=branch;
					if(false&&parent.left_neighbour!=null)
					{
						parent.left_neighbour.coner.rightBranch=branch;
						parent.left_neighbour.coner.right_data=branch.coner.left_data;
					}
					//branch.right_neighbour=parent;
					//printEvent("parent.left:",parent.coner.left_data);
				}
			}
		}
		/*/
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
		/*/
		//======================public=============================
		public void Test()
		{
			if (tree == null || tree.left == null)
				return;
			Branch parent = tree,children=parent.left;
			
			string text="|";
			string text_2 = "|";
			while (children.state==State_of_Branch.Node) 
			{
				parent=children;
				children=parent.left;
			}
			text+=(parent.coner.left_data.I).ToString()+"->"+parent.coner.right_data.I+"|";
			text_2 += parent.middle.ToString () + "|";
			//parent.printNeighbourVertex ();
			while (parent.right_neighbour!=null)
			{
				parent=parent.right_neighbour;
				text+=(parent.coner.left_data.I).ToString()+"->"+parent.coner.right_data.I+"|";
				text_2 += parent.middle.ToString () + "|";
				//MonoBehaviour.print("----------------");
				//parent.printNeighbourVertex ();
			}
			//text += parent.coner.right_data.I.ToString ();
			//MonoBehaviour.print ("train:"+text);
			//MonoBehaviour.print ("train_2:" + text_2);
		}
		bool isFirstAdd=true;
		public void Add(Fortune_.Site_Event data,float _y)
		{
			_Add (data, _y);
			return;
			if(data==null||data.vertex==null)
			{
				return ;
			}
			 //MonoBehaviour.print ("---------------------------------------------------------------");
			//MonoBehaviour.print ("hello new site:"+data.I);
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
				lenght=1;
				return ;
			}
			else
			{
				lenght++;
				Branch parent,children;
				parent=tree;
				int deep=0;
				while(true)
				{
					deep++;
					 //MonoBehaviour.print("deep:"+deep);
					if(deep>100)
					{
						MonoBehaviour.print("break..");
						return;
					}
					 //MonoBehaviour.print("State:"+parent.state.ToString());
					if(parent.state==State_of_Branch.Site)
					{
						/*/
						Branch left=new Branch(),right=new Branch();
						parent.left=left;
						parent.right=right;

						left.parent=right.parent=parent;

						parent.state=State_of_Branch.Node;
						left.state=right.state=State_of_Branch.Site;

						if(Key<parent.coner.left_data.X)
						{
							left.coner.left_data=data;
							right.coner.left_data=parent.coner.left_data;
						}
						else
						{
							left.coner.left_data=parent.coner.left_data;
							right.coner.left_data=data;
						}

						addNeighbour(parent);

						left.isLeftBranch=true;
						right.isLeftBranch=false;

						_normalized(left);
						_normalized(right);
						//return;
						addEvent(left);
						addEvent(right);
						if(Key<parent.coner.left_data.X)
						{
							addEvent(parent.left_neighbour);
						}
						else
						{
							addEvent(parent.right_neighbour);
						}
						return;
						/*/
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
						
						branch.right_neighbour=parent;
						parent.left_neighbour=branch;
						if(parent.parent!=null)
						{
							Branch _parent=parent.parent;
							if(parent.isLeftBranch)
							{
								if(_parent.left_neighbour!=null)
								{
									_parent.left_neighbour.right_neighbour=branch;
									branch.left_neighbour=_parent.left_neighbour;
								}
								_parent.left_neighbour=parent;
								parent.right_neighbour=_parent;
							}
							else
							{
								if(_parent.right_neighbour!=null)
								{
									_parent.right_neighbour.left_neighbour=parent;
									parent.left_neighbour=_parent.right_neighbour;
								}
								_parent.right_neighbour=branch;
								branch.left_neighbour=_parent;
							}
						}
						
						parent.left.isLeftBranch=true;
						parent.right.isLeftBranch=false;
						branch.left.isLeftBranch=true;
						branch.right.isLeftBranch=false;
						
						if(parent.coner.left_data.X<Key)
						{
							
						}
						else
						{
							//MonoBehaviour.print("Left");
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
						//branch.printNeighbour();
						//parent.printNeighbour();
						addEvent(branch.left);
						addEvent(branch.right);
						addEvent(parent.right);

						VoronoiEdge edge=new VoronoiEdge();
						branch.setVoronoiVertex(edge.first,_y);
						parent.setVoronoiVertex(edge.second,_y);
						NA.LoadPointEvent();
						return;
						//
					}
					else
					{
						MonoBehaviour.print(parent.left.getLimit(_y)+"<"+Key+"<"+parent.right.getLimit(_y));
						if(Key<parent.coner.leftBranch.getLimit(_y))
						{
							MonoBehaviour.print("Left:"+data.I+" "+Key+"<"+parent.left.getLimit(_y));
							parent=parent.left;
							continue;
						}
						if(parent.coner.rightBranch.getLimit(_y)<Key)
						{
							MonoBehaviour.print ("Right:"+data.I+" "+parent.right.getLimit(_y)+"<"+Key);
							parent=parent.right;
							continue;
						}
						 //MonoBehaviour.print("I need some magic");
						//MonoBehaviour.print("My neighbour vertex:"+parent.coner.left_data.I+","+parent.coner.right_data.I);
						//return;
						Branch leftbranch_of_left_coner=new Branch(),rightbranch_of_left_coner=new Branch();
						Branch leftbranch_of_right_coner=new Branch(),rightbranch_of_right_coner=new Branch();

						parent.coner.leftBranch.left=leftbranch_of_left_coner;
						parent.coner.leftBranch.right=rightbranch_of_left_coner;
						parent.coner.rightBranch.left=leftbranch_of_right_coner;
						parent.coner.rightBranch.right=rightbranch_of_right_coner;

						leftbranch_of_left_coner.parent=parent.coner.leftBranch;
						rightbranch_of_left_coner.parent=parent.coner.leftBranch;
						leftbranch_of_right_coner.parent=parent.coner.rightBranch;
						rightbranch_of_right_coner.parent=parent.coner.rightBranch;
						
						leftbranch_of_left_coner.state=leftbranch_of_right_coner.state=rightbranch_of_left_coner.state=rightbranch_of_right_coner.state=State_of_Branch.Site;
						parent.coner.leftBranch.state=parent.coner.rightBranch.state=State_of_Branch.Node;

						leftbranch_of_left_coner.coner.left_data=parent.coner.left_data;
						rightbranch_of_right_coner.coner.left_data=parent.coner.right_data;
						float limit=parent.getLimit(_y);
						if(Key<limit)
						{
							rightbranch_of_left_coner.coner.left_data=data;
							leftbranch_of_right_coner.coner.left_data=parent.coner.left_data;
						}
						else
						{
							rightbranch_of_left_coner.coner.left_data=parent.coner.right_data;
							leftbranch_of_right_coner.coner.left_data=data;
						}
						
						//------------------------------------------------------------------------
						//parent.printNeighbour();
						parent.coner.leftBranch.left_neighbour=parent.left_neighbour;
						if(parent.left_neighbour!=null)
							parent.left_neighbour.right_neighbour=parent.coner.leftBranch;
						parent.coner.rightBranch.right_neighbour=parent.right_neighbour;
						if(parent.right_neighbour!=null)
							parent.right_neighbour.left_neighbour=parent.coner.rightBranch;

						parent.coner.leftBranch.right_neighbour=parent;
						parent.coner.rightBranch.left_neighbour=parent;
						parent.left_neighbour=parent.coner.leftBranch;
						parent.right_neighbour=parent.coner.rightBranch;
						//------------------------------------------------------------------------
						leftbranch_of_left_coner.isLeftBranch=leftbranch_of_right_coner.isLeftBranch=true;
						rightbranch_of_left_coner.isLeftBranch=rightbranch_of_right_coner.isLeftBranch=false;

						_normalized(leftbranch_of_left_coner);
						_normalized(rightbranch_of_left_coner);
						_normalized(leftbranch_of_right_coner);
						_normalized(rightbranch_of_right_coner);

						parent.coner.right_data=leftbranch_of_right_coner.coner.left_data;
						parent.coner.rightBranch=leftbranch_of_right_coner;

						parent.coner.left_data=rightbranch_of_left_coner.coner.left_data;
						parent.coner.leftBranch=rightbranch_of_left_coner;
						/*/
						//leftbranch_of_left_coner.parent.printNeighbour();
						//parent.printNeighbour();
						//leftbranch_of_right_coner.parent.printNeighbour();
						//
						MonoBehaviour.print(leftbranch_of_left_coner.coner.left_data.I+"->"+
						                    rightbranch_of_left_coner.coner.left_data.I+"->"+
						                    leftbranch_of_right_coner.coner.left_data.I+"->"+
						                    rightbranch_of_right_coner.coner.left_data.I);
						/*/
						MonoBehaviour.print(Key+"<"+limit);
						VoronoiEdge edge=new VoronoiEdge();
						if(Key<limit)
						{
							addEvent(leftbranch_of_left_coner);
							addEvent(leftbranch_of_right_coner);
							leftbranch_of_left_coner.parent.setVoronoiVertex(edge.first,_y);
							rightbranch_of_right_coner.parent.setVoronoiVertex(parent.vVertex,_y);
						}
						else
						{
							addEvent(rightbranch_of_right_coner);
							addEvent(rightbranch_of_left_coner);
							leftbranch_of_left_coner.parent.setVoronoiVertex(parent.vVertex,_y);
							rightbranch_of_right_coner.parent.setVoronoiVertex(edge.first,_y);
						}
						parent.setVoronoiVertex(edge.second,_y);
						NA.LoadPointEvent();
						return ;
					}
				}
			}
			return ;
		}
		public void _Add(Fortune_.Site_Event data,float _y)
		{
			if (data == null || data.vertex == null) {
				return;
			}
			//MonoBehaviour.print ("---------------------------------------------------------------");
			//MonoBehaviour.print ("hello new site:" + data.I);
			float Key = data.X;
			if (data == null)
				MonoBehaviour.print ("data==null");
			if (isFirstAdd) 
			{
				isFirstAdd = false;
				//MonoBehaviour.print("new KING Site");
				tree = new Branch ();
				tree.data = data;
				tree.state = State_of_Branch.Site;
				lenght = 1;
				return;
			}
			else
			{
				Branch parent=tree;
				while(true)
				{
					if(parent.state==State_of_Branch.Node)
					{
						//MonoBehaviour.print(parent.left.getLimit(_y)+"->"+Key+"->"+parent.right.getLimit(_y));
						//MonoBehaviour.print(parent.left.getLimit(_y)+"->"+parent.getLimit(_y)+"->"+parent.right.getLimit(_y));
						//parent.printNeighbourVertex();
						//MonoBehaviour.print("parent:"+parent.getLimit(_y));
						if(Key<parent.getLimit(_y))
						{
							parent=parent.left;
							//MonoBehaviour.print("go left");
							continue;
						}
						else
						{
							parent=parent.right;
							//MonoBehaviour.print ("go right");
							continue;
						}
					}
					else
					{
						Branch grand_parent=parent.parent;
						bool isLeft=Key<parent.getLimit(_y);
						float leftLimit=float.NegativeInfinity,rightLimit=float.PositiveInfinity;
						if(grand_parent!=null)
						{
							if(isLeft)
							{
								if(grand_parent.left_neighbour!=null)
								{
									leftLimit=grand_parent.left_neighbour.getLimit(_y);
								}
								rightLimit=grand_parent.getLimit(_y);
							}
							else
							{
								if(grand_parent.right_neighbour!=null)
								{
									rightLimit=grand_parent.right_neighbour.getLimit(_y);
								}
								leftLimit=grand_parent.getLimit(_y);
							}
						}
						Branch branch;
						if(true)
						{
							branch=new Branch();
							branch.left=new Branch();
							branch.right=new Branch();
							parent.left=branch;
							parent.right=new Branch();
							
							parent.left.parent=parent;
							parent.right.parent=parent;
							branch.left.parent=branch;
							branch.right.parent=branch;

							branch.state=parent.state=State_of_Branch.Node;
							branch.left.state=branch.right.state=parent.right.state=State_of_Branch.Site;

							branch.isLeftBranch=branch.left.isLeftBranch=true;
							branch.right.isLeftBranch=parent.right.isLeftBranch=false;

							branch.left.data=parent.right.data=parent.data;
							branch.right.data=data;

							branch.coner.leftBranch=branch.left;
							branch.coner.rightBranch=branch.right;
							parent.coner.leftBranch=branch.right;
							parent.coner.rightBranch=parent.right;

							branch.right_neighbour=parent;
							parent.left_neighbour=branch;
							if(grand_parent!=null)
							{
								if(isLeft)
								{
									if(grand_parent.left_neighbour!=null)
									{
										grand_parent.left_neighbour.right_neighbour=branch;
										grand_parent.left_neighbour.coner.rightBranch=branch.left;
										branch.left_neighbour=grand_parent.left_neighbour;
									}
									grand_parent.left_neighbour=parent;
									grand_parent.coner.leftBranch=parent.right;
									parent.right_neighbour=grand_parent;
								}
								else
								{
									if(grand_parent.right_neighbour!=null)
									{
										grand_parent.right_neighbour.left_neighbour=parent;
										grand_parent.right_neighbour.coner.leftBranch=parent.right;
										parent.right_neighbour=grand_parent.right_neighbour;
									}
									grand_parent.right_neighbour=branch;
									grand_parent.coner.rightBranch=branch.left;
									branch.left_neighbour=grand_parent;
								}
							}
							//MonoBehaviour.print(leftLimit+"->"+Key+"->"+rightLimit);
							if(false&&grand_parent!=null)
							{
								if(Key-leftLimit<rightLimit-Key)
								{
									addEvent(branch.left);
								}
								else
								{
									addEvent(parent.right);
								}
							}
							addEvent(branch.left);
							addEvent(branch.right);
							addEvent(parent.right);
							if(branch.left_neighbour!=null)
							{
								addEvent(branch.left_neighbour.coner.leftBranch);
							}
							if(parent.right_neighbour!=null)
							{
								addEvent(parent.right_neighbour.coner.rightBranch);
							}
							VoronoiEdge edge =new VoronoiEdge();
							branch.setVoronoiVertex(edge.first,_y);
							parent.setVoronoiVertex(edge.second,_y);
							NA.LoadPointEvent();
							return;
						}
					}
				}
			}
		}
		public void addNeighbour(Branch branch)
		{
			Branch parent = branch.parent;
			bool b = branch.isLeftBranch;
			if (parent == null)
				return;
			if (b) 
			{
				branch.right_neighbour=parent;
				branch.left_neighbour=parent.left_neighbour;
				if(parent.left_neighbour!=null)
				{
					parent.left_neighbour.right_neighbour=branch;
				}
				parent.left_neighbour=branch;
			}
			else
			{
				branch.left_neighbour=parent;
				branch.right_neighbour=parent.right_neighbour;
				if(parent.right_neighbour!=null)
				{
					parent.right_neighbour.left_neighbour=branch;
				}
				parent.right_neighbour=branch;
			}
		}
		static public void addEvent(Branch branch)
		{
			if (NA.OPTIONS.OneEventForOneBranch&&branch._event != null)
			{
				branch._event.Clear ();
				branch._event=null;
			}
			float y;
			Fortune_.Vertex_Event ev;
			if(Fortune_.Function.Circle(branch,out y))
			{
				ev=new Fortune_.Vertex_Event(branch,y);
				//NA.deletePointEvent(ev);
				//MonoBehaviour.print ("_Y:" + y);
				branch._event=ev;
				NA.AddPointEvent(ev);
			}
		}
		public bool Delete(Branch branch,float _y)
		{
			return _Delete (branch, _y);
			/*/
			if (branch.coner.leftBranch == null) 
			{
				MonoBehaviour.print("delete null");
				return false;
			}
			MonoBehaviour.print ("delete:" + branch.coner.left_data.I);
			if (branch.state == State_of_Branch.Node) 
			{
				MonoBehaviour.print("branch.state== Node");
				return false;
			}
			Branch parent= branch.parent,parent_2 ;
			if (parent == null) 
			{
				tree=null;
				return true;
			}
			parent_2 = parent.parent;
			bool b = branch.isLeftBranch;
			if(parent.left_neighbour!=null)
			{
				parent.left_neighbour.right_neighbour=parent.right_neighbour;
				if(b)
				{
					//parent.left_neighbour.coner.right_data=parent.coner.right_data;
					parent.left_neighbour.coner.rightBranch=parent.coner.rightBranch;
				}
			}
			if(parent.right_neighbour!=null)
			{
				parent.right_neighbour.left_neighbour=parent.left_neighbour;
				if(!b)
				{
					//parent.right_neighbour.coner.left_data=parent.coner.left_data;
					parent.right_neighbour.coner.leftBranch=parent.coner.leftBranch;
				}
			}
			Vector3 position;
			VoronoiEdge edge = new VoronoiEdge ();
			if(b)
			{
				if(parent.left_neighbour!=null)
				{
					parent.left_neighbour.reBuild(_y);
					parent.reBuild(_y);
					parent.left_neighbour.vVertex.endSearch();
					parent.vVertex.endSearch();
					position=parent.vVertex.postion;
					parent.left_neighbour.vVertex=edge.first;
					edge.second.postion=position;
					edge.second.endSearch();
				}
			}
			else
			{
				if(parent.right_neighbour!=null)
				{
					parent.right_neighbour.reBuild(_y);
					parent.reBuild(_y);
					if(parent.vVertex!=null)
					{
						position=parent.vVertex.postion;
						parent.right_neighbour.vVertex.endSearch();
						parent.vVertex.endSearch();
						parent.right_neighbour.vVertex=edge.second;
						edge.first.postion=position;
						edge.first.endSearch();
					}
				}
			}
			//parent.ri
			branch.destroy ();
			if (b) 
			{
				branch=parent.right;
			}
			else
			{
				branch=parent.left;
			}
			if(parent_2==null)
			{
				tree=branch;
				//return;
			}
			else
			{
				branch.isLeftBranch=parent.isLeftBranch;
				if (parent.isLeftBranch) 
				{
					parent_2.left=branch;
				}
				else
				{
					parent_2.right=branch;
				}
			}
			branch.parent = parent_2;
			//
			//if(parent.left_neighbour!=null)
				//addEvent (parent.left_neighbour.coner.leftBranch);
			//if(parent.right_neighbour!=null)
				//addEvent (parent.right_neighbour.coner.rightBranch);
			//
			if (b) 
			{
				_normalized(parent.coner.rightBranch);
				//addEvent(parent.coner.rightBranch);
			}
			else
			{
				_normalized(parent.coner.leftBranch);
				//addEvent(parent.coner.leftBranch);
			}
			parent.destroy ();
			/*/
			return true;
		}
		
		public bool _Delete(Branch branch,float _y)
		{
			if (branch == null)
				return false;
			if (branch.isDestroy) 
			{
				//MonoBehaviour.print("delete null");
				return false;
			}
			//MonoBehaviour.print ("delete:" + branch.data.I);
			if (branch.state == State_of_Branch.Node) 
			{
				//MonoBehaviour.print("branch.state== Node");
				return false;
			}
			Branch parent= branch.parent,grand_parent ;
			if (parent == null) 
			{
				//MonoBehaviour.print("something wrong:lenght of bTree = 1");
				//tree=null;
				return false;
			}
			grand_parent = parent.parent;
			if(grand_parent==null)
			{
				//MonoBehaviour.print("something wrong:lenght of bTree = 2");
				return false;
			}
			if(parent.left_neighbour==null||parent.right_neighbour==null)
			{
				//MonoBehaviour.print("something wrong:...");
				return false;
			}
			parent.reBuild(_y,true);
			parent.left_neighbour.reBuild(_y,true);
			parent.right_neighbour.reBuild (_y,true);
			float d = branch.getDistance_of_Arc ();
			//MonoBehaviour.print("YOU CANT DELETE ME MYXAXAXAXAXAXAXAXAXA(distance:"+d+")");
			if(float.IsNaN(d)||d>NA.OPTIONS.Epcilon_for_Point_event)
			{
				//MonoBehaviour.print("MYXAXAXAXAXAXAXAXAXA");
				return false;
			}
			//MonoBehaviour.print ("okay :(");
			Branch new_branch;
			if(branch.isLeftBranch)
			{
				new_branch=parent.right;
			}
			else
			{
				new_branch=parent.left;
			}
			if(parent.isLeftBranch)
			{
				grand_parent.left=new_branch;
			}
			else
			{
				grand_parent.right=new_branch;
			}

			new_branch.parent = grand_parent;

			new_branch.isLeftBranch = parent.isLeftBranch;

			if(branch.isLeftBranch)
			{
				parent.left_neighbour.coner.rightBranch=parent.coner.rightBranch;
			}
			else
			{
				parent.right_neighbour.coner.leftBranch=parent.coner.leftBranch;
			}
			parent.left_neighbour.right_neighbour=parent.right_neighbour;
			parent.right_neighbour.left_neighbour=parent.left_neighbour;

			VoronoiEdge edge = new VoronoiEdge ();
			edge.first.position=new Vector3(parent.middle,branch.data.fun(parent.middle));
			edge.first.endSearch ();
			parent.vVertex.position=new Vector3(parent.middle,branch.data.fun(parent.middle));
			parent.vVertex.endSearch();
			if(branch.isLeftBranch)
			{
				parent.left_neighbour.vVertex.position=new Vector3(parent.middle,branch.data.fun(parent.middle));
				parent.left_neighbour.vVertex.endSearch();
				parent.left_neighbour.setVoronoiVertex(edge.second,_y);
			}
			else
			{
				parent.right_neighbour.vVertex.position=new Vector3(parent.middle,branch.data.fun(parent.middle));
				parent.right_neighbour.vVertex.endSearch();
				parent.right_neighbour.setVoronoiVertex(edge.second,_y);
			}
			if(branch.isLeftBranch)
			{
				addEvent (parent.coner.rightBranch);
				addEvent (parent.left_neighbour.coner.leftBranch);
			}
			else
			{
				addEvent (parent.coner.leftBranch);
				addEvent (parent.right_neighbour.coner.rightBranch);
			}
			addEvent (branch);

			//NA.LoadPointEvent ();
			parent.destroy ();
			return true;
		}

		bool getCenter(Branch node_1,Branch node_2,Fortune_.Site_Event data)
		{
			float mid = (node_1.middle + node_2.middle) / 2;
			return data.X < mid;
		}
		public void printEvent(string text,Fortune_.Site_Event ev)
		{
			//MonoBehaviour.print (text + ev.getVector ().ToString ());
		}

	}
	public enum State_of_Branch{Site,Node};
	public struct Coner
	{
		public Fortune_.Site_Event left_data
		{
			get{ return leftBranch.data;}
			set
			{ 
				if(leftBranch!=null)
					leftBranch.data = value;
			}
		}
		public Branch leftBranch;
		public Fortune_.Site_Event right_data
		{
			get{return rightBranch.data;}
			set
			{ 
				if(rightBranch!=null)
					rightBranch.data = value;
			}
		}
		public Branch rightBranch;
		public Line leftParabola, rightParabola;
		
	}
	public class Branch
	{
		bool isCheked;
		public State_of_Branch state;
		public bool isLeftBranch=true;
		public bool isDestroy=false;
		public Coner coner;
		Fortune_.Site_Event _data;
		public Fortune_.Site_Event data
		{
			get{ return _data;}
			set{ _data = value;}
		}
		public Fortune_.Vertex_Event _event;
		public float middle=float.NaN;
		public VoronoiVertex vVertex;
		/*/
		public VoronoiVertex vVertex
		{
			get{ return _vVertex;}
			set
			{
				if(coner.left_data!=null)
				{
					value.postion = new Vector3 (middle, coner.left_data.fun (middle));
					_vVertex = value;
				}
			}
		}
		/*/
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
			NA.destroy += Destroy;
			coner = new Coner ();
		}
		public void setVoronoiVertex(VoronoiVertex ver,float _y)
		{
			vVertex = ver;
			reBuild (_y);
		}
		public void destroy()
		{
			NA.re_build -= reBuild;
			NA.build -= Build;
			NA.destroy -= Destroy;
			if(coner.leftParabola!=null)
				coner.leftParabola.Clear ();
			if(coner.rightParabola!=null)
				coner.rightParabola.Clear ();
			parent = null;
			coner.leftBranch = coner.rightBranch = null;
			left = right = null;
			left_neighbour = right_neighbour = null;
			data = null;
			isDestroy = true;
		}
		public void Destroy()
		{
			NA.re_build -= reBuild;
			NA.build -= Build;
			NA.destroy -= Destroy;
		}
		public void reBuild(float _y,bool isTimetoPrint=false)
		{
			//MonoBehaviour.print("Hello new reBuild:" + _y.ToString ());
			if (state == State_of_Branch.Site)
			{
				if(NA.OPTIONS.addAllPointEvent)
					Binary_search_tree.addEvent(this);
				return;
			}
			//MonoBehaviour.print ("its node");
			if (coner.leftBranch == null||coner.rightBranch==null) 
			{
				//MonoBehaviour.print("coner.left==null");
				return;
			}
			//MonoBehaviour.print ("its norm");
			bool leftb = Fortune_.Function.buildParabola ( coner.left_data, _y);
			bool rightb = Fortune_.Function.buildParabola ( coner.right_data, _y);
			isCheked = !( leftb&&rightb);
			//MonoBehaviour.print ("isCheked:"+isCheked.ToString());
			if (true||!isCheked) 
			{
				middle = Fortune_.Function.getIntersection (coner.left_data, coner.right_data,isTimetoPrint);
				if(vVertex!=null)
				{
					vVertex.position=new Vector3(middle,coner.left_data.fun(middle));
					if(false&&middle<RectScene.begin_minW)
					{
						vVertex.endSearch();
						vVertex=null;
					}
				}
				 //MonoBehaviour.print("search midle:"+middle);
				 //MonoBehaviour.print(coner.left_data.I+"|"+coner.right_data.I);
			}
		}
		
		public void Build()
		{
			//MonoBehaviour.print ("========================");
			//MonoBehaviour.print ("build time");
			if (state == State_of_Branch.Site)
				return;
			if(coner.leftParabola!=null)
				coner.leftParabola.Clear();
			if(coner.rightParabola!=null)
				coner.rightParabola.Clear();
			//
			//NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, new RectScene());
			//NA.addParabola (coner.right_data.fun, State_of_Line.Flesh, new RectScene());
			RectScene rect=new RectScene();
			//printNeighbourVertex ();
			rect.minW=getNeighbourIntersection(false);
			rect.maxW=middle;
			//MonoBehaviour.print("left rect:"+rect.minW+"->"+rect.maxW);
			if (rect.minW == float.NaN)
				rect.minW = RectScene.begin_minW;
			if (rect.maxW == float.NaN)
				rect.maxW = RectScene.begin_maxW;
			if (NA.OPTIONS.FullBeachLine) 
			{
				if(coner.leftParabola==null)
				{
					coner.leftParabola=NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, rect);
					if(coner.leftParabola!=null)
						coner.leftParabola.setColor(NA.COLORS.CoastLine.start,NA.COLORS.CoastLine.end);
					else 
						MonoBehaviour.print("something wrong");
				}
				else
				{
					NA.addParabola (coner.leftParabola,coner.left_data.fun, State_of_Line.Flesh, rect);
				}
				if(right_neighbour==null)
				{
					rect.minW=middle;
					////MonoBehaviour.print("middle:"+middle);
					rect.maxW=getNeighbourIntersection(true);
					//MonoBehaviour.print("FullBeachLine right rect:"+rect.minW+"->"+rect.maxW);
					////MonoBehaviour.print("stage");
					if(coner.rightParabola==null)
					{
						coner.rightParabola=NA.addParabola (coner.right_data.fun, State_of_Line.Flesh, rect);
						if(coner.rightParabola!=null)
							coner.rightParabola.setColor(NA.COLORS.CoastLine.start,NA.COLORS.CoastLine.end);
						else 
							MonoBehaviour.print("something wrong");
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
				//return;
				if(coner.leftParabola==null)
				{
					coner.leftParabola=NA.addParabola (coner.left_data.fun, State_of_Line.Flesh, rect);
					coner.leftParabola.setColor(NA.COLORS.CoastLine.start,NA.COLORS.CoastLine.end);
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
					coner.rightParabola.setColor(NA.COLORS.CoastLine.start,NA.COLORS.CoastLine.end);
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
			if(state==State_of_Branch.Node)
			{
				reBuild (_y);
				return middle;
			}
			else
			{
				if(parent==null)
				{
					return data.X;
				}
				else
				{
					parent.reBuild(_y);
					return parent.middle;
				}
			}
		}
		public float getNeighbourIntersection(bool right)
		{
			if(right)
			{
				if(right_neighbour!=null)
				{
					//MonoBehaviour.print ("right_neighbour:"+coner.right_data.I+"->"+right_neighbour.coner.left_data.I);
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
		public void printNeighbour()
		{
			if(state==State_of_Branch.Site)
			{
				MonoBehaviour.print("its site");
				return;
			}
			MonoBehaviour.print ("############################");
			if (left_neighbour != null) 
			{
				MonoBehaviour.print ("left_neighbour:"+left_neighbour.coner.right_data.I+"->"+coner.left_data.I);
			}
			if(right_neighbour!=null)
			{
				MonoBehaviour.print ("right_neighbour:"+coner.right_data.I+"->"+right_neighbour.coner.left_data.I);
			}
		}
		public void printNeighbourVertex(bool b=true)
		{
			if(state==State_of_Branch.Site)
			{
				MonoBehaviour.print("its site");
				return;
			}
			MonoBehaviour.print ("############################");
			if (b&&left_neighbour != null) 
			{
				MonoBehaviour.print ("left_neighbour:"+left_neighbour.coner.left_data.I);
				MonoBehaviour.print ("left_neighbour:"+left_neighbour.coner.right_data.I);
			}
			if(coner.left_data!=null)
				MonoBehaviour.print ("current:" + coner.left_data.I);
			if(coner.right_data!=null)
				MonoBehaviour.print ("current:" + coner.right_data.I);
			if(b&&right_neighbour!=null)
			{
				MonoBehaviour.print ("right_neighbour:"+right_neighbour.coner.left_data.I);
				MonoBehaviour.print ("right_neighbour:"+right_neighbour.coner.right_data.I);
			}
		}
		public Vertex getVertex()
		{
			return data.vertex;
		}
		public Site_Event getLeftData()
		{
			if(state== State_of_Branch.Node)
			{
				return coner.left_data;
			}
			else
			{
				return data;
			}
		}
		public Site_Event getRightData()
		{
			if (state == State_of_Branch.Node) 
			{
				return coner.right_data;
			}
			else
			{
				return data;
			}
		}
		public float getDistance_of_Arc()
		{
			if (state == State_of_Branch.Node)
				return -1;
			float leftLimit=float.NegativeInfinity, rightLimit=float.PositiveInfinity;
			if(parent==null)
			{
				return float.PositiveInfinity;
			}
			if(isLeftBranch)
			{
				if(parent.left_neighbour!=null)
				{
					leftLimit=parent.left_neighbour.middle;
				}
				rightLimit=parent.middle;
			}
			else
			{
				if(parent.right_neighbour!=null)
				{
					rightLimit=parent.right_neighbour.middle;
				}
				leftLimit=parent.middle;
			}
			//MonoBehaviour.print ("Limit:" + leftLimit + "->" + data.X + "->" + rightLimit);
			return Math.Abs( rightLimit - leftLimit);
		}
		
	}
}
