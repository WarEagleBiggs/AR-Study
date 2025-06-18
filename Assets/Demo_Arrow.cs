using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Demo_Arrow : MonoBehaviour
{
    public Vector2 GridDimenstions;
    public Transform GridAnchor;
    public GameObject GridSquare;
    public GameObject GridArrow;
    public int Spacing = 40;

    public GameObject ObjectForDestruction;
    public Image Answer;

    [Range(0.0f, 1.0f)]
    public float SquareFullness = 1.0f; 
    
    //stop watch
    public TextMeshProUGUI StopWatchTxt;
    public float StopWatchValue;
    public bool canTick;

    public InputActionReference ResetBtn;
    public InputActionReference RevealBtn;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        ClearGrid();
        canTick = true;

        Vector3 anchorLocalPos = transform.InverseTransformPoint(GridAnchor.position);

        // Pick forbidden direction 
        int randomAnswer = Random.Range(0, 4);

        // Pick one random square that is allowed to use forbidden direction
        int totalSquares = (int)GridDimenstions.x * (int)GridDimenstions.y;
        int allowedIndex = Random.Range(0, totalSquares);

        int currentIndex = 0;

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

                // Decide whether to place an arrow here based on fullness
                if (Random.value <= SquareFullness || currentIndex == allowedIndex)
                {
                    int randomRotation;

                    if (currentIndex == allowedIndex)
                    {
                        // This is the lucky square that gets the forbidden direction
                        randomRotation = randomAnswer;
                        Answer = square.GetComponent<Image>();
                    }
                    else
                    {
                        // Exclude forbidden direction for all other squares
                        List<int> possibleDirections = new List<int> { 0, 1, 2, 3 };
                        possibleDirections.Remove(randomAnswer);
                        randomRotation = possibleDirections[Random.Range(0, possibleDirections.Count)];
                    }

                    // Instantiate arrow
                    GameObject arrow = Instantiate(GridArrow, square.transform);
                    arrow.SetActive(true);
                    arrow.transform.localPosition = Vector3.zero;
                    arrow.transform.localRotation = Quaternion.Euler(0, 0, randomRotation * 90f);
                    Image arrowIm = arrow.GetComponent<Image>();
                    arrowIm.enabled = true;
                }
                // else no arrow is placed (square remains empty)

                currentIndex++;
            }
        }
    }

    public void ClearGrid()
    {
        foreach (Transform child in ObjectForDestruction.transform)
        {
            Destroy(child.gameObject);
            StopWatchValue = 0f;
        }
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GenerateGrid();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            RevealAnswer();
        }
        
        //updates stopwatch
        if (canTick)
        {
            StopWatchTxt.text = (StopWatchValue.ToString("f2"));
            StopWatchValue = StopWatchValue + 1f * Time.deltaTime;
        }
    }

    public void RevealAnswer()
    {
        Answer.color = Color.green;
        canTick = false;
    }
    
    //Buttons on phone
    private void OnEnable()
    {
        ResetBtn.action.performed += OnButtonPressed;
        ResetBtn.action.Enable();
    }

    private void OnDisable()
    {
        ResetBtn.action.performed -= OnButtonPressed;
        ResetBtn.action.Disable();
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        GenerateGrid();
    }

}
