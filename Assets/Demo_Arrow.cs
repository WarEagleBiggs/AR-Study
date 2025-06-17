using UnityEngine;

public class Demo_Arrow : MonoBehaviour
{
    public Vector2 GridDimenstions;
    public Transform GridAnchor;
    public GameObject GridSquare;
    public GameObject GridArrow;
    public int Spacing = 40;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Vector3 anchorLocalPos = transform.InverseTransformPoint(GridAnchor.position);

        for (int x = 0; x < (int)GridDimenstions.x; x++)
        {
            for (int y = 0; y < (int)GridDimenstions.y; y++)
            {
                // Create the grid square
                GameObject square = Instantiate(GridSquare, transform);
                square.SetActive(true);
                square.transform.localPosition = anchorLocalPos + new Vector3(x * Spacing, -y * Spacing, 0);

                // Create the arrow as child of the square
                GameObject arrow = Instantiate(GridArrow, square.transform);
                arrow.SetActive(true);
                arrow.transform.localPosition = Vector3.zero;

                // Random rotation (0, 90, 180, 270 degrees)
                float randomRotation = 90f * Random.Range(0, 4);
                arrow.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            }
        }
    }
}