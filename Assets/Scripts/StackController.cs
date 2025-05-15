
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackController : MonoBehaviour
{
   [Header("Settings")] [SerializeField] private LayerMask hexagonLayerMask;
   [SerializeField] private LayerMask gridHexagonLayerMask;
   [SerializeField] private LayerMask GroundLayerMask;
   
   private HexStack currentStack;
   private Vector3 currentStackInitialPosition;

   [Header("Data")] private GridCell targetCell;
   
   [Header("Actions")]
   public static Action<GridCell> OnStackPlaced;  
   
   [Header(" StackList ")]
   public List<HexStack> stacks = new List<HexStack>();
   
   private void Update()
   {
      ManageControl();
   }
   //chat gpt trial 
   public HexStack GetLeftNeighbour(HexStack stack)
   {
      int index = stacks.IndexOf(stack);
      if (index > 0)
         return stacks[index - 1];
      return null;
   }

   public HexStack GetRightNeighbour(HexStack stack)
   {
      int index = stacks.IndexOf(stack);
      if (index < stacks.Count - 1)
         return stacks[index + 1];
      return null;
   }
   

   private void ManageControl()
   {
      if (Input.GetMouseButtonDown(0))
         ManageMouseDown();
      else if (Input.GetMouseButton(0) && currentStack != null)
         ManageMouseDrag();
      else if (Input.GetMouseButtonUp(0) && currentStack != null)
         ManageMouseUp();
   }
   private void ManageMouseDown()
   {
      RaycastHit hit;
      Physics.Raycast(GetClickedRay(), out hit,500, hexagonLayerMask);

      if (hit.collider == null)
      {
         Debug.Log("We have not detected hexagon");
         return;
      }

      currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
      currentStackInitialPosition=currentStack.transform.position;
   }
   private void ManageMouseDrag()
   {
    
      RaycastHit hit;
      Physics.Raycast(GetClickedRay(), out hit,500, gridHexagonLayerMask);

      if (hit.collider == null)
         DragginAboveGround();
      else
         DraggingAboveGridCell(hit);
   }
   private void DragginAboveGround()
   {
      RaycastHit hit;
      Physics.Raycast(GetClickedRay(), out hit,500, GroundLayerMask);

      if (hit.collider == null)
      {
         Debug.Log("No ground detected");
         return;
      }
      

      Vector3 currentStackTargetPos = hit.point.With(y: 2);
      currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position,currentStackTargetPos,Time.deltaTime*30);

      targetCell = null;
   }
   
   private void DraggingAboveGridCell(RaycastHit hit)
   {
      GridCell gridCell = hit.collider.GetComponent<GridCell>();

      if (gridCell.isOccupied)
      {
         DragginAboveGround();
      }
      else
      {
         DraggingAboveNonOccupiedGridCell(gridCell);
      }
   }

   private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
   {
      Vector3 currentStackTargetPos = gridCell.transform.position.With(y:2);
      currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position,currentStackTargetPos,Time.deltaTime*30);
      
      targetCell= gridCell;
   }
   
   private void ManageMouseUp()
   {
      if (targetCell ==null)
      {
         currentStack.transform.position = currentStackInitialPosition;
         currentStack = null;
         return;
      }

      currentStack.transform.position = targetCell.transform.position.With(y: .2f);
      currentStack.transform.SetParent(targetCell.transform);
      currentStack.Place();
      
      targetCell.AssignStack(currentStack);
      
      OnStackPlaced?.Invoke(targetCell);
      
      targetCell = null;
      
      currentStack = null;
   }
   
   private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
   
}