using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionHandler : MonoBehaviour
{
    private bool m_Selected = false;
    public Material sel_mat;
    private List<Material> obj_mat = new List<Material>();
    [SerializeField]
    private PlacementSystem placement;

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
    public void OnDeselection()
    {
        m_Selected = false;
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
                OnDeselection();
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
                    transform.Rotate(transform.up, -15f);
                }
            }
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
        }
    }
}
