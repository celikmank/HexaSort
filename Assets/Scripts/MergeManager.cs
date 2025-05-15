/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MergeManager : MonoBehaviour
{
    [Header(" Elements ")]
    private List<GridCell> updatedCells = new List<GridCell>();
    private void Awake()
    {
        StackController.OnStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.OnStackPlaced -= StackPlacedCallback;

    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        StartCoroutine(StackPalacedCoroutine(gridCell));
        
    }

    IEnumerator StackPalacedCoroutine(GridCell gridCell)
    {
        updatedCells.Add(gridCell);
        while (updatedCells.Count > 0)
        {
            yield return CheckForMerge(updatedCells[0]);
        }
        
    }

    IEnumerator CheckForMerge(GridCell gridCell)
    {
        updatedCells.Remove(gridCell);

        if (!gridCell.isOccupied)
        {
            yield break;
        }
        
        List<GridCell> neighbourGridCells = GetNeigbourGridCells(gridCell);
        
        if (neighbourGridCells.Count<=0)
        {
            Debug.Log("No neighbour grid cell found");
            yield break;
        }

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();
         
        List<GridCell> SimiliarneighbourGridCells=GetSimilarNeighbourGridCells(gridCellTopHexagonColor, neighbourGridCells.ToArray());

        if (SimiliarneighbourGridCells.Count <=0)
        {
            Debug.Log("No similair grid cell found");
            yield break;
        }
        
        updatedCells.AddRange(SimiliarneighbourGridCells);
        
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, neighbourGridCells.ToArray());
        
        RemoveHexagonsFromStack(hexagonsToAdd,SimiliarneighbourGridCells.ToArray());

        MoveHexagons(gridCell,hexagonsToAdd);
        yield return new WaitForSeconds(.2f + (hexagonsToAdd.Count + 1) * 0.1f);

       yield return  CheckForCompleteStack(gridCell,gridCellTopHexagonColor);
    }
    private List<GridCell> GetNeigbourGridCells(GridCell gridCell)
    {
        LayerMask gridCellMask =1 << gridCell.gameObject.layer;
       
        List<GridCell> neighbourGridCells = new List<GridCell>();
        
        Collider[] neighbourGridCellColliders=Physics.OverlapSphere(gridCell.transform.position,2,gridCellMask);
        
        foreach (Collider gridCellCollider in neighbourGridCellColliders)
        {
            GridCell neighbourGridCell = gridCellCollider.GetComponent<GridCell>();

            if (!neighbourGridCell.isOccupied)
            {
                continue;
            }

            if (neighbourGridCell==gridCell)
            {
                continue;
            }
            neighbourGridCells.Add(neighbourGridCell);
        } 
        return neighbourGridCells;
    }

    private List<GridCell> GetSimilarNeighbourGridCells(Color gridCellTopHexagonColor ,GridCell[] neighbourGridCells)
    {
        List<GridCell> SimiliarneighbourGridCells = new List<GridCell>();
        
        foreach (GridCell neighbourGridCell in neighbourGridCells)
        {
            Color neighbourCellTopHexagonColor = neighbourGridCell.Stack.GetTopHexagonColor();
            if (gridCellTopHexagonColor == neighbourCellTopHexagonColor)
            {
                SimiliarneighbourGridCells.Add(neighbourGridCell);
            }
            
        }
        return SimiliarneighbourGridCells;
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor ,GridCell[] SimiliarneighbourGridCells)
    {
       List<Hexagon> hexagonsToAdd= new List<Hexagon>();
        
        foreach (GridCell neighbourCell in SimiliarneighbourGridCells)
        { 
            HexStack neighbourCellHexStack = neighbourCell.Stack;
            for (int i = neighbourCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neighbourCellHexStack.Hexagons[i];
                if (hexagon.Color != gridCellTopHexagonColor)
                {
                    break;
                }
                hexagonsToAdd.Add(hexagon);
                hexagon.SetParrent(null);
            }
        }
        return hexagonsToAdd;
    }

    private void RemoveHexagonsFromStack(List<Hexagon> hexagonsToAdd,GridCell[] SimiliarneighbourGridCells)
    {
        foreach (GridCell neighbourCell in SimiliarneighbourGridCells)
        {
            HexStack stack = neighbourCell.Stack;  
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                {
                    stack.Remove(hexagon);
                }
            }    
        }
    }

    private void MoveHexagons(GridCell gridCell,List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.Stack.Hexagons.Count * .2f;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];
             
            float targetY = initialY + i *.2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;
             
            gridCell.Stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPosition);
          
        }
    }

    private IEnumerator CheckForCompleteStack(GridCell gridCell,Color topColor)
     { 
         if (gridCell.Stack.Hexagons.Count<10)
             yield break;

         List<Hexagon> similiarHexagons = new List<Hexagon>();

         for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
         {
             Hexagon hexagon = gridCell.Stack.Hexagons[i];

             if (hexagon.Color != topColor) 
                 break;
             
             similiarHexagons.Add(hexagon);
              
         }
         
         int similiarHexagonsCount = similiarHexagons.Count;
         
         if (similiarHexagons.Count<10)
         {
              yield break;
         }

         float delay = 0;
         while (similiarHexagons.Count>0)
         {
          similiarHexagons[0].SetParrent(null);
          similiarHexagons[0].Vanish(delay);
         // DestroyImmediate(similiarHexagons[0].gameObject);

         delay += 0.1f;
          
          gridCell.Stack.Remove(similiarHexagons[0]);
          similiarHexagons.RemoveAt(0);
         }
         
         updatedCells.Add(gridCell);

         yield return new WaitForSeconds(.2f + (similiarHexagonsCount + 1) * .01f);
     }
}
*/

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [Header("Elements")] 
    private List<GridCell> updatedCells = new List<GridCell>();
    private Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
    private bool isProcessingQueue = false;

    private void Awake()
    {
        StackController.OnStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.OnStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        actionQueue.Enqueue(StackPlacedCoroutine(gridCell));
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (isProcessingQueue || actionQueue.Count == 0)
            return;

        isProcessingQueue = true;
        StartCoroutine(ProcessQueueCoroutine());
    }

    private IEnumerator ProcessQueueCoroutine()
    {
        while (actionQueue.Count > 0)
        {
            yield return StartCoroutine(actionQueue.Dequeue());
        }
        isProcessingQueue = false;
    }

    private IEnumerator StackPlacedCoroutine(GridCell gridCell)
    {
        updatedCells.Add(gridCell);
        while (updatedCells.Count > 0)
        {
            yield return CheckForMerge(updatedCells[0]);
        }
    }

    private IEnumerator CheckForMerge(GridCell gridCell)
    {
        updatedCells.Remove(gridCell);

        if (!gridCell.isOccupied)
            yield break;

        List<GridCell> neighbourGridCells = GetNeigbourGridCells(gridCell);
        if (neighbourGridCells.Count <= 0)
            yield break;

        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();
        List<GridCell> similarNeighbourGridCells = GetSimilarNeighbourGridCells(gridCellTopHexagonColor, neighbourGridCells.ToArray());

        if (similarNeighbourGridCells.Count > 0)
        {
            updatedCells.AddRange(similarNeighbourGridCells);
            List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, neighbourGridCells.ToArray());
            RemoveHexagonsFromStack(hexagonsToAdd, similarNeighbourGridCells.ToArray());
            yield return MoveHexagons(gridCell, hexagonsToAdd);

            // Komşular bitene kadar tekrar kontrol et
            yield return CheckForMerge(gridCell);
            yield break;
        }

        // Artık komşu yoksa yok olma işlemi başlatılır
        yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
    }

    private List<GridCell> GetNeigbourGridCells(GridCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer;
        List<GridCell> neighbourGridCells = new List<GridCell>();
        Collider[] neighbourGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, gridCellMask);

        foreach (Collider gridCellCollider in neighbourGridCellColliders)
        {
            GridCell neighbourGridCell = gridCellCollider.GetComponent<GridCell>();
            if (!neighbourGridCell.isOccupied || neighbourGridCell == gridCell)
                continue;
            neighbourGridCells.Add(neighbourGridCell);
        }
        return neighbourGridCells;
    }

    private List<GridCell> GetSimilarNeighbourGridCells(Color gridCellTopHexagonColor, GridCell[] neighbourGridCells)
    {
        List<GridCell> similarNeighbourGridCells = new List<GridCell>();
        foreach (GridCell neighbourGridCell in neighbourGridCells)
        {
            Color neighbourCellTopHexagonColor = neighbourGridCell.Stack.GetTopHexagonColor();
            if (gridCellTopHexagonColor == neighbourCellTopHexagonColor)
                similarNeighbourGridCells.Add(neighbourGridCell);
        }
        return similarNeighbourGridCells;
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, GridCell[] similarNeighbourGridCells)
    {
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();
        foreach (GridCell neighbourCell in similarNeighbourGridCells)
        {
            HexStack neighbourCellHexStack = neighbourCell.Stack;
            for (int i = neighbourCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neighbourCellHexStack.Hexagons[i];
                if (hexagon.Color != gridCellTopHexagonColor)
                    break;
                hexagonsToAdd.Add(hexagon);
                hexagon.SetParrent(null);
            }
        }
        return hexagonsToAdd;
    }

    private void RemoveHexagonsFromStack(List<Hexagon> hexagonsToAdd, GridCell[] similarNeighbourGridCells)
    {
        foreach (GridCell neighbourCell in similarNeighbourGridCells)
        {
            HexStack stack = neighbourCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
            }
        }
    }

    private IEnumerator MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.Stack.Hexagons.Count * .2f;
        float baseDuration = 0.35f;
        float delayBetween = 0.05f; // Hexler arası küçük gecikme

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];
            float targetY = initialY + i * .2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;

            gridCell.Stack.Add(hexagon);

            // Her hexagonun animasyonu topluca başlar, aralarına küçük delay eklenir
            seq.Join(
                hexagon.transform.DOLocalMove(targetLocalPosition, baseDuration)
                    .SetEase(Ease.OutCubic)
                    .SetDelay(i * delayBetween)
            );
        }

        seq.Play();
        yield return seq.WaitForCompletion();
    }
    private IEnumerator CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            yield break;

        List<Hexagon> similarHexagons = new List<Hexagon>();
        for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];
            if (hexagon.Color != topColor)
                break;
            similarHexagons.Add(hexagon);
        }

        int count = similarHexagons.Count;
        if (count < 10)
            yield break;

        // Hexagon sayısına göre animasyon süresi ve delay ayarlanıyor
        float animDuration = Mathf.Max(0.08f, 0.18f - count * 0.005f);
        float delayPerHex = Mathf.Max(0.01f, 0.03f - count * 0.001f);

        Sequence vanishSequence = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            Hexagon hex = similarHexagons[i];
            hex.SetParrent(null);

            vanishSequence.Join(
                hex.transform.DOScale(Vector3.zero, animDuration)
                    .SetEase(Ease.InBack)
                    .SetDelay(i * delayPerHex)
            );

            // MaterialPropertyBlock ile fade
            vanishSequence.Join(
                DOTween.To(() => 1f, x => hex.FadeOut(animDuration, i * delayPerHex), 0f, animDuration)
            );
        }

        vanishSequence.Play();
        yield return vanishSequence.WaitForCompletion();

        foreach (var hex in similarHexagons)
        {
            gridCell.Stack.Remove(hex);
        }
        similarHexagons.Clear();

        updatedCells.Add(gridCell);
        yield return new WaitForSeconds(0.05f);
    }
    /*private IEnumerator CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            yield break;

        List<Hexagon> similarHexagons = new List<Hexagon>();
        for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];
            if (hexagon.Color != topColor)
                break;
            similarHexagons.Add(hexagon);
        }

        int similarHexagonsCount = similarHexagons.Count;
        if (similarHexagonsCount < 10)
            yield break;

        float delay = 0;
        while (similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParrent(null);
            similarHexagons[0].Vanish(delay);
            delay += 0.1f;
            gridCell.Stack.Remove(similarHexagons[0]);
            similarHexagons.RemoveAt(0);
        }

        updatedCells.Add(gridCell);
        yield return new WaitForSeconds(.2f + (similarHexagonsCount + 1) * .01f);
    }*/
}