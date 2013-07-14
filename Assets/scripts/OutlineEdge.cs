using System;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEdge
{
	public OutlineEdge ()
	{
	}
	
	public Vector3 start;
	
	public Vector3 end;
	
	public Vector3 normal;
	
	public void DrawDebug(Color _c)
	{
		
		Debug.DrawLine(start,end,_c);
		
		// draw normal
		Vector3 normStart = ((end - start) / 2.0f) + start;
		Debug.DrawLine(normStart,normStart + normal*0.2f,Color.cyan);
		
	}
	
	public void FindConnectedEdges(List<OutlineEdge> _edges)
	{
		
		int found = 0;
		
		foreach (OutlineEdge item in _edges) {
			if(item !=this)
			{
				if(item.start == this.end || item.end == this.end)
				{
					nextEdge = item;
					found++;
				}
				if(item.end == this.start || item.start == this.start)
				{
					prevEdge = item;
					found++;
				}
				
				if(found == 2)
					break;
			}
		}
	}
	
	public OutlineEdge nextEdge;
	
	public OutlineEdge prevEdge;
	
	
	
}

