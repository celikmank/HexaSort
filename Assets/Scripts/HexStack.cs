using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    public List<Hexagon> Hexagons { get; private set; }
    

    public void Add(Hexagon hexagon)
    {
        if (Hexagons == null)
        { 
            Hexagons = new List<Hexagon>();
        }
        
        Hexagons.Add(hexagon);
        hexagon.SetParrent(transform);
    }
    
    public void Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Add(transform.GetChild(i).GetComponent<Hexagon>());
            
            Place();
        }   
    }
    
    public Color GetTopHexagonColor()
    {
        return Hexagons.Last().Color;
    }
   
    public void Place()
    {
        foreach (Hexagon hexagon in Hexagons)
        {
            hexagon.DisableCollider();
        }
    }

    public bool Contains(Hexagon hexagon) => Hexagons.Contains(hexagon);

    public void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);
        if (Hexagons.Count == 0)
        {
         DestroyImmediate(gameObject);   
        }
    }

 
    public void AnimateHexesToSlots()
    {
        float slotHeight = Hexagons[0].GetComponent<Renderer>().bounds.size.y;

        for (int i = 0; i < Hexagons.Count; i++)
        {
            Vector3 targetPos = new Vector3(0, i * slotHeight, 0);
            Hexagons[i].MoveToLocal(targetPos, 0.3f, i * 0.05f);
        }
    }
    public Hexagon GetTopHexagon()
    {
        if (Hexagons == null || Hexagons.Count == 0) return null;
        return Hexagons.Last();
    }
}

        
    
