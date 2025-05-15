
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackSpawner : MonoBehaviour
{
   [Header("Elements")] 
   [SerializeField] private Transform stackPositionParent;
   [SerializeField] Hexagon  hexagonPrefab;
   [SerializeField] HexStack HexagonStackPrefab;

   [Header("Settings")] [NaughtyAttributes.MinMaxSliderAttribute(2, 8)] 
   [SerializeField] private Vector2Int minMaxHexCount;
   [SerializeField] private Color[] colors;

   private int stackCounter;
   private void Awake()
   {
       Application.targetFrameRate = 60; 
       StackController.OnStackPlaced += StackPlacedCallback;
   }

   private void OnDestroy()
   {
       StackController.OnStackPlaced-=StackPlacedCallback;
   }

   private void StackPlacedCallback(GridCell gridCell)
   {
       stackCounter++;
       if (stackCounter >=3)
       {
           stackCounter = 0;
           GenerateStacks();
       }
   }

   void Start()
   {
      GenerateStacks();
   }

   private void GenerateStacks()
   {
      for (int i = 0; i < stackPositionParent.childCount; i++)
         GenerateStack(stackPositionParent.GetChild(i));
   }

   private void GenerateStack(Transform parent)
   { 
      HexStack hexStack = Instantiate(HexagonStackPrefab, parent.position, Quaternion.identity,parent);
      hexStack.name = $" Stack{parent.GetSiblingIndex()}";
      
      int amount=Random.Range(minMaxHexCount.x, minMaxHexCount.y);


      int firstColorHexagonCount = Random.Range(0, amount);
      
      Color[] colorArray = getRandomColors();
      
      for (int i = 0; i < amount; i++)
      {
         Vector3 hexagonLocalPos = Vector3.up * i * .2f;
         Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);
         
        Hexagon hexagonInstace = Instantiate(hexagonPrefab,spawnPosition , Quaternion.identity, hexStack.transform);

        hexagonInstace.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];

        hexagonInstace.Configure(hexStack);
        
        hexStack.Add(hexagonInstace);
      }
   }

   private Color[] getRandomColors()
   {
      List<Color> colorList = new List<Color>();
      colorList.AddRange(colors);

      if (colorList.Count <= 0)
      {
         Debug.LogError("No random colors available");
         return null;
      }

      Color firstColor = colorList.OrderBy(x => Random.value).First();
      colorList.Remove(firstColor);
      
      if (colorList.Count <= 0)
      {
         Debug.LogError("No random colors available");
         return null;
      }
      Color SecondColor = colorList.OrderBy(x => Random.value).First();
      return new Color[] {firstColor, SecondColor};
   }
}

/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;

    [SerializeField] private Color[] colors;
    [NaughtyAttributes.MinMaxSlider(2,8)]
    [SerializeField] private Vector2Int minMaxHexCount;

    private int _stackCounter;
    private void Awake() => Application.targetFrameRate = 60;
    private void StackPlaced(GridCell cell)
    {
        _stackCounter++;

        if(_stackCounter >= 3)
        {
            _stackCounter = 0;
            GenerateStacks();
        }
    }
    void Start()
    {
        GenerateStacks();
    }
    private void GenerateStacks()
    {
        for (int i = 0; i < stackPositionsParent.childCount; i++)
        {
            GenerateStacks(stackPositionsParent.GetChild(i));
        }
    }

    private void GenerateStacks(Transform parent)
    {
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex() }";

        int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);

        Color[] colorArray = GetRandomColors();

        int firstColorHexagonCount = Random.Range(0, amount);

        for (int i = 0; i < amount;i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .2f;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, hexStack.transform);

            hexagonInstance.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];
            hexagonInstance.Configure(hexStack);
            hexStack.Add(hexagonInstance);
        }
    }
    private Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if(colorList.Count <= 0) 
        {
            Debug.LogError("No color found");
            return null;
        }

        Color firstColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(firstColor);

        if (colorList.Count <= 0)
        {
            Debug.LogError("No color found");
            return null;
        }
        Color secondColor = colorList.OrderBy(x => Random.value).First();

        return new Color[] { firstColor, secondColor };
    }
}*/