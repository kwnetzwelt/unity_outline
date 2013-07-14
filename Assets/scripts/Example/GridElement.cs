using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridElement : MonoBehaviour
{
	Color m_color;
	
	static Material m_sharedMaterial;
	static Material m_sharedMaterialSelected;
	static Shader m_usedShader;
	
	static Stack<Material>m_materialPool = new Stack<Material>();
	
	Material m_currentMaterial;
	Material m_lerpTargetMaterial;
	float m_lerpFraction = 1;
	
	public bool isSelected
	{ 
		get;
		private set;
	}
	
	public void Hover()
	{
		if(isSelected)
			return;
		
		CreateMaterialIfNeeded();
		m_currentMaterial.color = Color.white;
		m_lerpTargetMaterial = m_sharedMaterial;
		m_lerpFraction = 0;
	}
	public void Click()
	{
		CreateMaterialIfNeeded();
		
		isSelected = !isSelected;
		
		if(isSelected)
			m_lerpTargetMaterial = m_sharedMaterialSelected;
		else
			m_lerpTargetMaterial = m_sharedMaterial;
		
		
		m_lerpFraction = 0;
		
	}
	
	void CreateMaterialIfNeeded()
	{
		if(m_currentMaterial == null)
		{
			if(m_materialPool.Count > 0)
				m_currentMaterial = m_materialPool.Pop();
			else
				m_currentMaterial = new Material( m_usedShader );
			
			renderer.material = m_currentMaterial;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		if(m_sharedMaterial == null)
		{
			m_usedShader = Shader.Find("Diffuse");
			m_sharedMaterial = new Material( m_usedShader );
			m_sharedMaterial.color = Color.black;
			m_sharedMaterialSelected = new Material( m_usedShader );
			m_sharedMaterialSelected.color = Color.green;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_lerpFraction < 1)
		{
			m_currentMaterial.color = Color.Lerp(m_currentMaterial.color , m_lerpTargetMaterial.color, m_lerpFraction);
			
			m_lerpFraction += Time.deltaTime;
			
			if(m_lerpFraction >= 1)
			{
				if(m_currentMaterial != null)
				{
					m_materialPool.Push( m_currentMaterial );
					renderer.sharedMaterial = m_lerpTargetMaterial;
				}
				m_currentMaterial = null;
				
			}
			
		}
	}
}

