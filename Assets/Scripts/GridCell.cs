using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    [Header("Elements ")] [SerializeField] private Hexagon hexagonPrefab;
    
    [Header("Settings")]
    [OnValueChanged("GenerateInıtalHexagons")]
    [SerializeField] private Color[] hexagonColors;
    
    public HexStack Stack { get; private set;}
    public bool isOccupied
    {
        get => Stack !=null; 
        private set {}
    }

    private void Start()
    {
        if (hexagonColors.Length>1)
        GenerateInıtalHexagons();
        
    }

    public void AssignStack(HexStack stack)
    {
        Stack = stack;
    }

    private void GenerateInıtalHexagons()
    {
        while (transform.childCount > 1)
        {
            Transform  t = transform.GetChild(1);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }
        Stack = new GameObject("Inıtıal Stack").AddComponent<HexStack>();
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up * .2f;

        for (int i = 0; i < hexagonColors.Length; i++)
        {
            Vector3 SpawnPosition = Stack.transform.TransformPoint(Vector3.up * i * .2f);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, SpawnPosition, quaternion.identity);
            hexagonInstance.Color=hexagonColors[i];
            
            Stack.Add(hexagonInstance);
        }
    }
}
