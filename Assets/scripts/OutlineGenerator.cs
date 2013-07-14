using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineGenerator
{

	public enum ENext
	{
		TOP,
		LEFT,
		RIGHT,
		BOTTOM
	}
	
	
	
	public List<Vector3> points;
	
	public List<OutlineEdge> edges;
	
	public List<OutlineEdgeSequence> edgeSequences;
	
	public float tileScale = 1.0f;
	
	public OutlineGenerator () 
	{
	
		points = new List<Vector3>();
		
		// ####
		//##  #
		//######
		// #
		// ####
		
		points.Add(new Vector3(1,0,4));
		points.Add(new Vector3(2,0,4));
		points.Add(new Vector3(3,0,4));
		points.Add(new Vector3(4,0,4));
		
		points.Add(new Vector3(0,0,3));
		points.Add(new Vector3(1,0,3));
		points.Add(new Vector3(4,0,3));
		
		points.Add(new Vector3(0,0,2));
		points.Add(new Vector3(1,0,2)); // Only non-edge point in this construct
		points.Add(new Vector3(2,0,2));
		points.Add(new Vector3(3,0,2));
		points.Add(new Vector3(4,0,2));
		points.Add(new Vector3(5,0,2));
		
		
		points.Add(new Vector3(1,0,1));
		
		points.Add(new Vector3(1,0,0));
		points.Add(new Vector3(2,0,0));
		points.Add(new Vector3(3,0,0));
		points.Add(new Vector3(4,0,0));
		points.Add(new Vector3(5,0,0));
		
		
		
		
		
		
		// Generate Edge Objects
		
		GenOutlineEdges();
		
		
	}
	
	protected void GenOutlineEdges()
	{
		
		
		edges = new List<OutlineEdge>();
		
		// Step 1 Iterate through points
		
		// Step 2 If point has less than 4 valid neighbors
		/// Step 3 add generated edge to list of edges
		
		
		for (int i = 0; i < points.Count; i++) 
		{
			
			if(!HasPoint(GetNeighbor(i,ENext.RIGHT)))
			{
				edges.Add(GetEdge(i,ENext.RIGHT));
			}
			
			if(!HasPoint(GetNeighbor(i,ENext.LEFT)))
			{
				edges.Add(GetEdge(i,ENext.LEFT));
			}
			
			if(!HasPoint(GetNeighbor(i,ENext.TOP)))
			{
				edges.Add(GetEdge(i,ENext.TOP));
			}
			
			if(!HasPoint(GetNeighbor(i,ENext.BOTTOM)))
			{
				edges.Add(GetEdge(i,ENext.BOTTOM));
			}
		
			
		}
		
		// Step 4 generate edge sequences
		List<OutlineEdge> leftEdges = new List<OutlineEdge>(edges);
		edgeSequences = new List<OutlineEdgeSequence>();
		
		while(leftEdges.Count > 0)
		{
			OutlineEdgeSequence s = new OutlineEdgeSequence();
			
			OutlineEdge e = leftEdges[0];
			
			while(e != null)
			{
				leftEdges.Remove(e);
				s.edges.Add(e);
				
				OutlineEdge found = leftEdges.Find(delegate(OutlineEdge obj) {
					return obj.end == e.end || obj.start == e.end
						||
							obj.end == e.start || obj.start == e.start
						;
				});
				
				if(found != null)
				{
					e = found;
					continue;
				}
				break;
			}
			s.Initialize();
			edgeSequences.Add(s);
			
		}
		
		Debug.Log(edgeSequences.Count + " edge Sequences");
		
		// Step 5 smooth edge sequences
		
		
		
	}
				
	protected OutlineEdge GetEdge(int _Index, ENext _net)
	{
		OutlineEdge e = new OutlineEdge();
		if(_net == ENext.RIGHT || _net == ENext.TOP)
		{
			e.start = GetNeighbor(_Index, _net);
			e.end = GetEndOfEdge(_Index, _net);	
		}else
		{
			e.start = points[_Index];
			e.end = GetEndOfEdge(_Index, _net);	
		}
		
			e.normal = GetNormal(_net);
		
		
		return e;
	}
	
	protected bool IsEdgePoint(int _Index)
	{
		Vector3 p = points[_Index];
		int count = 0;	
		// Top Neighbor
		Vector3 top = GetNeighbor(_Index,ENext.TOP);
		
		// Left Neighbor
		Vector3 left = GetNeighbor(_Index,ENext.LEFT);
		
		// Right Neighbor
		Vector3 right = GetNeighbor(_Index,ENext.RIGHT);
		
		// Bottom Neighbor
		Vector3 bottom = GetNeighbor(_Index,ENext.BOTTOM);
		
		return !(HasPoint(top) && HasPoint(bottom) && HasPoint(left) && HasPoint(right));
	}    
	
	protected bool HasPoint(Vector3 _point)
	{
		return points.Contains(_point);
	}
	
	
	protected Vector3 GetEndOfEdge(int _Index, ENext _nei)
	{
		if(_nei == ENext.RIGHT || _nei == ENext.TOP)
		{
			return points[_Index] + GetNormal(ENext.RIGHT) * tileScale + GetNormal(ENext.TOP) * tileScale;
		}
		if(_nei == ENext.BOTTOM)
		{
			return points[_Index] + GetNormal(ENext.RIGHT) * tileScale;
		}
		
		return points[_Index] + GetNormal(ENext.TOP) * tileScale;
	}
	
	protected Vector3 GetNeighbor(int _Index, ENext _nei)
	{
		return points[_Index] + GetNormal(_nei) * tileScale;
		
	}
	protected Vector3 GetNormal(ENext _nei)
	{
		switch(_nei)
		{
			case ENext.TOP: return Vector3.forward;
			case ENext.BOTTOM: return Vector3.back;
			case ENext.LEFT: return Vector3.left;
			case ENext.RIGHT: return Vector3.right;
		}
		return Vector3.up;
	}
}
	