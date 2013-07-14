using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridTest : MonoBehaviour {
	
	public GameObject m_elementPrefab;
	
	public Dictionary<Vector2,GridElement> m_elements = new Dictionary<Vector2, GridElement>();
	
	bool m_recalculateOutline = false;
	
	const int kGridSizeX = 20;
	const int kGridSizeY = 20;
	
	OutlineGenerator m_og;
	public OutlineRenderer m_outlineRenderer;
	// Use this for initialization
	void Start () {
	
		m_og = new OutlineGenerator();
		m_og.tileScale = 1;
		
		StartCoroutine(
			CreateGrid());
		
	}
	
	IEnumerator CreateGrid()
	{
		
		for(int y = 0;y < kGridSizeY;y++)
		{
			for(int x = 0; x < kGridSizeY;x++)
			{
				CreateElement(x,y);
				yield return 0;
			}
		}
		
	}
	void CreateElement(int _x, int _y)
	{
		GameObject go = GameObject.Instantiate(m_elementPrefab) as GameObject;
		go.name = _x + " " + _y;
		go.transform.parent = this.transform;
		go.transform.position = new Vector3(_x, 0 ,_y);
		m_elements.Add(new Vector2(_x,_y),go.GetComponent<GridElement>());
		
	}
	
	Ray r;
	Plane p;
	float enter;
	Vector3 pos;
	Vector2 entryPos;
	
	void Update () {
	
		r = Camera.main.ScreenPointToRay(Input.mousePosition);
		p = new Plane(Vector3.up, Vector3.zero);
		if(p.Raycast(r, out enter))
		{
			// snap to grid
			pos = r.GetPoint(enter);
			entryPos = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.z));
			if(m_elements.ContainsKey(	entryPos ))
			{
				if(Input.GetMouseButtonUp(0))
				{
					m_elements[entryPos].Click();
					m_recalculateOutline = true;
				}
				else
					m_elements[entryPos].Hover();
			}
		}
		
		if(m_recalculateOutline)
		{
			m_recalculateOutline = false;
			List<Vector3> points = new List<Vector3>();
			foreach(var el in m_elements)
			{
				if(el.Value.isSelected)
				{
					Vector3 point = el.Value.transform.position;
					point.x = Mathf.Round(point.x);
					point.z = Mathf.Round(point.z);
					point.y = 0;
					Debug.Log(point.x + " " + point.y + " " + point.z);
					points.Add(point);
					
				}
			}
			
			m_og.SetPoints(points);
			m_og.GenOutlineEdges();
			m_outlineRenderer.Recalculate(m_og.edgeSequences);
		}
		
	}
}
