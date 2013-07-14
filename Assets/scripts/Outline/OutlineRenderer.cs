using System;
using UnityEngine;
using System.Collections.Generic;

public class OutlineRenderer : MonoBehaviour
{
	
	
	public Color[] colors;
	
	public Texture2D texture;
	
	public bool drawDebug = true;
	
	List<OutlineEdgeSequence> m_sequences;
	
	public Vector3 m_offset;
	
	Stack<GameObject>m_edges = new Stack<GameObject>();
	
	public void Recalculate(List<OutlineEdgeSequence> _sequences)
	{
		m_sequences = _sequences;
		// Simple Mesh Renderer
		int si = 0;
		
		while(m_edges.Count > 0)
		{
			GameObject.Destroy( m_edges.Pop() );
		}
		
		foreach(OutlineEdgeSequence s in m_sequences)
		{
			Mesh m = new Mesh();
			
			GameObject g = new GameObject("edges");
			g.transform.parent = this.transform;
			
			m_edges.Push(g);
			
			Vector3[] vertices = new Vector3[s.edges.Count * 4];
			int[] triangles = new int[s.edges.Count * 6];
			Vector2[] uvs = new Vector2[vertices.Length];
			Vector3[] normals = new Vector3[vertices.Length];
			
			int vi = 0;
			int ti = 0;
			foreach(OutlineEdge e in s.edges)
			{
				int viStart = vi;
				
				uvs[vi] = Vector2.zero ;
				normals[vi] = Vector3.up;
				vertices[vi++] = e.start;
				
				uvs[vi] = Vector2.right ;
				normals[vi] = Vector3.up;
				vertices[vi++] = e.end;
				
				
				uvs[vi] = (Vector2.up + Vector2.right) ;
				
				normals[vi] = Vector3.up;
				
				if(e.nextEdge != null && e.nextEdge.normal != e.normal)
				{
					vertices[vi++] = e.end - e.normal * 0.2f - e.nextEdge.normal * 0.2f;
					
				}
				
				else
				{
					vertices[vi++] = e.end - e.normal * 0.2f;
				}
				
				
				uvs[vi] = Vector2.up ;
				normals[vi] = Vector3.up;
				
				
				if(e.prevEdge != null && e.prevEdge.normal != e.normal)
				{
					vertices[vi++] = e.start - e.normal * 0.2f - e.prevEdge.normal * 0.2f;
				}
				else
				{
					vertices[vi++] = e.start - e.normal * 0.2f;
				}
				
				
				// start + end + end+n
				triangles[ti++] = viStart+1;
				triangles[ti++] = viStart;
				triangles[ti++] = viStart+2;
				
				
				// start + start+n + end+n
				triangles[ti++] = viStart;
				triangles[ti++] = viStart+3;
				triangles[ti++] = viStart+2;
				
				
			}
			
			m.vertices = vertices;
			m.triangles = triangles;
			m.uv = uvs;
			m.RecalculateNormals();
			
			MeshFilter f = g.AddComponent<MeshFilter>();
			MeshRenderer r = g.AddComponent<MeshRenderer>();
			r.material = new Material(Shader.Find("Particles/VertexLit Blended"));
			//r.material.color = colors[si++ % colors.Length];
			r.material.SetColor("_EmisColor",colors[si++ % colors.Length]);
			r.material.mainTexture = texture;
			
			f.mesh = m;
			g.transform.position = m_offset;
		}
		
		
		
	}
	void Update()
	{
		if(drawDebug)
		{
		
			if(m_sequences != null)
			{
				
				int ci = 0;
				
				foreach(OutlineEdgeSequence s in m_sequences)
				{
					s.DrawDebug(colors[(ci++ % colors.Length)]);
				}
			}
		}
	}
	
	
	
	
}

