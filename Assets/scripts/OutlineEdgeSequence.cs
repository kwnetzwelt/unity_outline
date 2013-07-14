using System;
using System.Collections.Generic;
using UnityEngine;
public class OutlineEdgeSequence
{
	public List<OutlineEdge> edges = new List<OutlineEdge>();
	
	public OutlineEdgeSequence ()
	{
	}
	public void DrawDebug(Color _c)
	{
			foreach (OutlineEdge e in edges) {
				e.DrawDebug(_c);
			}
		
	}
	
	public void Initialize()
	{
		for (int i = 0; i < edges.Count; i++) {
			edges[i].FindConnectedEdges(edges);
		}
	}	
	
}

