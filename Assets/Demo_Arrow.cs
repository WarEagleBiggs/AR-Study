using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class Demo_Arrow : MonoBehaviour
{
    public Vector2 GridDimenstions;
    public Transform GridAnchor;
    public GameObject GridSquare;
    public GameObject GridArrow;
    public int Spacing = 40;

    public GameObject ObjectForDestruction;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        ClearGrid();

        Vector3 anchorLocalPos = transform.InverseTransformPoint(GridAnchor.position);

        // Pick the forbidden direction ONCE for the whole grid
        int randomAnswer = Random.Range(0, 4);

        for (int x = 0; x < (int)GridDimenstions.x; x++)
        {
            for (int y = 0; y < (int)GridDimenstions.y; y++)
            {
                // Instantiate grid square
                GameObject square = Instantiate(GridSquare, transform);
                square.SetActive(true);
                square.transform.localPosition = anchorLocalPos + new Vector3(x * Spacing, -y * Spacing, 0);
                square.transform.parent = ObjectForDestruction.transform;
                Image im = square.GetComponent<Image>();
                im.enabled = true;

                // Generate possible directions excluding forbidden one
                List<int> possibleDirections = new List<int> { 0, 1, 2, 3 };
                possibleDirections.Remove(randomAnswer);
                int randomRotation = possibleDirections[Random.Range(0, possibleDirections.Count)];

                // Instantiate arrow
                GameObject arrow = Instantiate(GridArrow, square.transform);
                arrow.SetActive(true);
                arrow.transform.localPosition = Vector3.zero;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, randomRotation * 90f);
                Image arrowIm = arrow.GetComponent<Image>();
                arrowIm.enabled = true;
            }
        }
    }

    public void ClearGrid()
    {
        foreach (Transform child in ObjectForDestruction.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GenerateGrid();
        }
    }
}
