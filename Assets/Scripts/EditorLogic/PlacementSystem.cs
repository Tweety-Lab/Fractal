using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator, cellIndicator;
    [SerializeField]
    private InputManager inputmanager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private ObjDatabase database;
    private int selectIndex = -1;
    [SerializeField]
    public static List<GameObject> PlacedObjects = new List<GameObject>();
    [SerializeField]
    internal List<GameObject> TempObjects = new List<GameObject>();
    [SerializeField]
    private int UndoLimit;
    internal int Undo;
    [SerializeField]
    internal float GridStatus = 1;
    private Material GridMat;
    private GameObject Cached_Selection;
    [SerializeField]
    private GameObject GridParent;
    private float ElevateDelta;
    private float Angle, VertAngle;

    [SerializeField]
    private GameObject gridVisualisation;
    private bool PlacementMode;

    private void Start()
    {
        ElevateDelta = GridParent.transform.position.y;
        inputmanager.OnClicked += Select;
        GridMat = gridVisualisation.GetComponent<MeshRenderer>().material;
        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true);
        StopPlacement();
    }
    public void ToggleGrid(bool value)
    {
        gridVisualisation.SetActive(value);
    }
    public void StartPlacement(int id)
    {
        if (inputmanager.SelectedObj != null)
            Deselect();
        StopPlacement();
        Angle = 0;
        VertAngle = 0;
        selectIndex = database.objectsData.FindIndex(data => data.ID == id);
        if (selectIndex < 0)
        {
            Debug.LogError($"No ID found {id}");
            return;
        }
        PlacementMode = true;
        cellIndicator.transform.localScale = new Vector3(database.objectsData[selectIndex].Size.y, 1, database.objectsData[selectIndex].Size.x);
        cellIndicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        inputmanager.OnClicked -= Select;
        inputmanager.OnClicked += PlaceStructure;
        inputmanager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputmanager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputmanager.GetSelectedMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        GameObject newObject = Instantiate(database.objectsData[selectIndex].Prefab);
        newObject.name = database.objectsData[selectIndex].name;
        newObject.transform.position = grid.CellToWorld(gridPos);
        newObject.transform.Rotate(transform.up, Angle);
        newObject.transform.Rotate(transform.right, VertAngle);
        PlacedObjects.Add( newObject );
        if (database.objectsData[selectIndex].isWall)
        {
            foreach (Transform child in newObject.transform)
            {
                child.GetComponent<MeshRenderer>().material = database.objectsData[selectIndex].Materials[UnityEngine.Random.Range(0, database.objectsData[selectIndex].Materials.Length)];
            }
        }
    }

    private void StopPlacement()
    {
        cellIndicator.transform.localScale = new Vector3(GridStatus, 1, GridStatus);
        inputmanager.OnClicked += Select;
        cellIndicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
        selectIndex = -1;
        PlacementMode = false;
        inputmanager.OnClicked -= PlaceStructure;
        inputmanager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (PlacementMode == true)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    VertAngle += 90;
                    if (VertAngle == 360)
                    {
                        VertAngle = 0;
                    }
                    if (VertAngle > 360)
                    {
                        VertAngle -= 360;
                    }
                    cellIndicator.transform.Rotate(transform.right, 90);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Angle += 90;
                    if (Angle == 360)
                    {
                        Angle = 0;
                    }
                    if (Angle > 360)
                    {
                        Angle -= 360;
                    }
                    cellIndicator.transform.Rotate(transform.up, 90);
                }
            }
        }
        //if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z))
        //    UndoAction();
        if (Input.GetKeyDown(KeyCode.Z))
            UndoAction();
        if (Input.GetKeyDown(KeyCode.Y))
            RedoAction();
        if (Input.GetKeyDown(KeyCode.Equals))
            IncreaseGrid();
        if (Input.GetKeyDown(KeyCode.Minus))
            DecreaseGrid();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ElevateGrid();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            DescendGrid();
        }
        Vector3 mousePos = inputmanager.GetSelectedMapPos();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        indicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
    public void ElevateGrid()
    {
        ElevateDelta += GridStatus;
        GridParent.transform.position = new Vector3(transform.position.x, ElevateDelta, transform.position.z);
    }
    public void DescendGrid()
    {
        ElevateDelta -= GridStatus;
        GridParent.transform.position = new Vector3(transform.position.x, ElevateDelta, transform.position.z);
    }
    private void IncreaseGrid()
    {
        if (GridStatus == 1)
        {
            GridStatus += 1;
            GridMat.SetVector("_Size", new Vector4(0.5f, 0.5f, 0, 0));
        }
        else if (GridStatus == 0.5)
        {
            GridStatus += 0.5f;
            GridMat.SetVector("_Size", new Vector4(1f, 1f, 0, 0));
        }
        else
        {
            return;
        }
        grid.cellSize = new Vector3(GridStatus, GridStatus, GridStatus);
        if (selectIndex < 0)
            cellIndicator.transform.localScale = new Vector3(GridStatus, 1, GridStatus);
    }
    private void DecreaseGrid()
    {
        if (GridStatus == 1) { 
            GridStatus -= 0.5f;
            GridMat.SetVector("_Size", new Vector4(2f, 2f, 0, 0));
        }
        else if (GridStatus == 2){
            GridStatus -= 1; 
            GridMat.SetVector("_Size", new Vector4(1f, 1f, 0, 0));
        }
        else return;
        grid.cellSize = new Vector3(GridStatus, GridStatus, GridStatus);
        if (selectIndex < 0)
            cellIndicator.transform.localScale = new Vector3(GridStatus, 1, GridStatus);
    }
    private void UndoAction()
    {
        if (Undo >= UndoLimit)
        {
            return;
        }
        Undo += 1;
        if (PlacedObjects[PlacedObjects.Count - 1].activeSelf != false)
        {
            ManipulateObj(0, false);
        }
        else
        {
            ManipulateObj(0, true);
        }
    }
    private void ManipulateObj(int mode, bool state)
    {
        if (mode == 0)
        {
            GameObject TempObject = PlacedObjects[PlacedObjects.Count - 1];
            TempObjects.Add(TempObject);
            TempObject.SetActive(state);
            PlacedObjects.Remove(TempObject);
        }
        else
        {
            GameObject TempObject = TempObjects[TempObjects.Count - 1];
            PlacedObjects.Add(TempObject);
            TempObject.SetActive(state);
            TempObjects.Remove(TempObject);
        }
    }
    private void RedoAction()
    {
        if (Undo <= 0)
        {
            return;
        }
        Undo -= 1;
        if (TempObjects[TempObjects.Count - 1].activeSelf != false)
        {
            ManipulateObj(1, false);
        }
        else
        {
            ManipulateObj(1, true);
        }
    }
    private void Select()
    {
        if (inputmanager.SelectedObj == null || PlacedObjects.Contains(inputmanager.SelectedObj) != true || selectIndex >= 0)
        {
            return;
        }
        Deselect();
        inputmanager.OnExit += Deselect;
        Cached_Selection = inputmanager.SelectedObj;
        inputmanager.SelectedObj.GetComponent<ObjectSelectionHandler>().OnSelection();
    }
    private void Deselect()
    {
        if (Cached_Selection != null)
        {
            Cached_Selection.GetComponent<ObjectSelectionHandler>().OnDeselection(false);
        }
        inputmanager.SelectedObj.GetComponent<ObjectSelectionHandler>().OnDeselection(false);
        try
        {
            inputmanager.OnExit -= Deselect;
        }
        catch (Exception)
        {
            print("there is no Deselect action being added");
        }
    }
}
