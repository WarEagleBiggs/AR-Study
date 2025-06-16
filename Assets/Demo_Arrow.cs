using UnityEngine;

public class Demo_Arrow : MonoBehaviour
{
    public Vector2 GridDimenstions;
    public GameObject GridSquare;
    public GameObject GridArrow;
    public int Spacing = 40;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < (int)GridDimenstions.x; x++)
        {
            for (int y = 0; y < (int)GridDimenstions.y; y++)
            {
                Vector3 position = new Vector3(x * Spacing, y * Spacing, 0);
                Instantiate(GridSquare, position, Quaternion.identity, transform);
            }
        }
    }
}
