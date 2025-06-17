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
                //grid
                GameObject square = Instantiate(GridSquare, transform);
                square.SetActive(true);
                square.transform.localPosition = anchorLocalPos + new Vector3(x * Spacing, -y * Spacing, 0);

                //pick answer direction
                int randomAnswer = Random.Range(0, 4);

                //arrow
                GameObject arrow = Instantiate(GridArrow, square.transform);
                arrow.SetActive(true);
                arrow.transform.localPosition = Vector3.zero;
                float randomRotation = Random.Range(0, 4);
                if (randomRotation == randomAnswer)
                {
                    randomRotation = Random.Range(0, 4);
                }
                randomRotation = randomRotation * 90f;
                arrow.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            }
        }
    }
}