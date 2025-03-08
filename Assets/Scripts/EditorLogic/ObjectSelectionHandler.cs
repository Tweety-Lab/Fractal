using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionHandler : MonoBehaviour
{
    public int ID;
    private bool m_Selected = false;
    public Material sel_mat;
    private List<Material> obj_mat = new List<Material>();
    [SerializeField]
    private PlacementSystem placement;
    public OtherStuff stuffbase;
    public ObjDatabase database;

    private void Start()
    {
        placement = FindAnyObjectByType<PlacementSystem>();
        foreach(Transform child in transform)
        {
            obj_mat.Add(child.GetComponent<MeshRenderer>().material);
        }
    }
    public void OnSelection()
    {
        m_Selected = true;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material = sel_mat;
        }
    }
    public void OnDeselection(bool Forced)
    {
        m_Selected = false;
        if (!Forced)
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().material = obj_mat[i];
            }
    }
    private void Update()
    {
        if (m_Selected)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                placement.Undo += 1;
                gameObject.SetActive(false);
                OnDeselection(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position = new Vector3(transform.position.x - placement.GridStatus, transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + placement.GridStatus);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position = new Vector3(transform.position.x + placement.GridStatus, transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - placement.GridStatus);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    transform.Rotate(transform.right, 15f);
                }
            }
            else
                if (Input.GetKeyDown(KeyCode.R))
                {
                    transform.Rotate(transform.up, 15f);
                }
            if (Input.GetKey(KeyCode.F))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (database.objectsData[ID].isWall)
                {
                    if (CompareTag("Object"))
                    {
                        tag = "Mirror";
                        Material mat = stuffbase.stuffData.MirrorMats[Random.Range(0, stuffbase.stuffData.MirrorMats.Length)];
                        foreach (Transform child in transform)
                        {
                            child.GetComponent<MeshRenderer>().material = mat;
                        }
                        OnDeselection(true);
                    }
                    else
                    {
                        tag = "Object";
                        Material mat = database.objectsData[ID].Materials[Random.Range(0, database.objectsData[ID].Materials.Length)];
                        foreach (Transform child in transform)
                        {
                            child.GetComponent<MeshRenderer>().material = mat;
                        }
                        OnDeselection(true);
                    }
                }
            }
        }
    }
}
