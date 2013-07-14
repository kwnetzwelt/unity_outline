using System;
using UnityEngine;

public class OutlineRenderer : MonoBehaviour
{
	
	OutlineGenerator gen;
	
	public Color[] colors;
	
	public Texture2D texture;
	
	public bool drawDebug = true;
	
	
	void Start()
	{
		gen = new OutlineGenerator();
		// Ugly LineRenderer Approach
		/*
		int ci = 0;
		
		foreach(OutlineEdgeSequence s in gen.edgeSequences)
		{
			GameObject g = new GameObject("edges",typeof(LineRenderer));
			
			LineRenderer r = g.GetComponent<LineRenderer>();
			
			r.material = new Material(Shader.Find("Particles/Additive"));
			Color c = colors[(ci++ % colors.Length)];
			r.SetColors(c,c);
			r.SetWidth(0.2f,0.2f);
			
			
			r.SetVertexCount(s.edges.Count+1);
			
			for (int i = 0; i < s.edges.Count; i++) 
			{
				r.SetPosition(i,s.edges[i].start);
			}
			
			r.SetPosition(s.edges.Count,s.edges[s.edges.Count-1].end);
		}
		*/
		
		
		// Simple Mesh Renderer
		int si = 0;
		
		foreach(OutlineEdgeSequence s in gen.edgeSequences)
		{
			Mesh m = new Mesh();
			
			GameObject g = new GameObject("edges");
			g.transform.parent = this.transform;
			
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
				
				if(e.nextEdge.normal != e.normal)
				{
					vertices[vi++] = e.end - e.normal * 0.2f - e.nextEdge.normal * 0.2f;
					
				}
				
				else
				{
					vertices[vi++] = e.end - e.normal * 0.2f;
				}
				
				
				uvs[vi] = Vector2.up ;
				normals[vi] = Vector3.up;
				
				if(e.prevEdge.normal != e.normal)
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
		}
		
		
		
	}
	void Update()
	{
		if(drawDebug)
		{
		
			if(gen != null && gen.edgeSequences != null)
			{
				
				int ci = 0;
				
				foreach(OutlineEdgeSequence s in gen.edgeSequences)
				{
					s.DrawDebug(colors[(ci++ % colors.Length)]);
				}
			}
		}
	}
	
	
	
	
}

