using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Demo_Arrow : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 GridDimenstions;   // (columns, rows) — kept name to not break existing Inspector refs
    public Transform GridAnchor;
    public GameObject GridSquare;
    public GameObject GridArrow;
    public int Spacing = 40;

    [Range(0.0f, 1.0f)]
    public float SquareFullness = 1.0f;

    [Header("Destroy Container")]
    public GameObject ObjectForDestruction;

    [Header("Reveal")]
    public Image Answer;

    [Header("Stopwatch")]
    public TextMeshProUGUI StopWatchTxt;
    public float StopWatchValue;
    public bool canTick;

    [Header("Input Actions")]
    public InputActionReference ResetBtn;
    public InputActionReference RevealBtn;

    [Header("UI Inputs (TMP)")]
    public TMP_InputField xInputField;   // columns
    public TMP_InputField yInputField;   // rows

    void Start()
    {
        ApplyGridSizeFromUI(); // read UI once on start if available
        //GenerateGrid();
    }

    void Update()
    {
        // Live-read UI (so changing the fields updates the size before hitting R)
        ApplyGridSizeFromUI();

        if (Keyboard.current != null)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
                GenerateGrid();

            if (Keyboard.current.eKey.wasPressedThisFrame)
                RevealAnswer();
        }

        if (canTick)
        {
            StopWatchValue += Time.deltaTime;
            if (StopWatchTxt) StopWatchTxt.text = StopWatchValue.ToString("f2");
        }
    }

    public void GenerateGrid()
    {
        // Guard + clamp
        int cols = Mathf.Clamp((int)Mathf.Max(1, GridDimenstions.x), 1, 200);
        int rows = Mathf.Clamp((int)Mathf.Max(1, GridDimenstions.y), 1, 200);

        ClearGrid();      // wipes previous
        Answer = null;    // reset answer reference
        canTick = true;
        StopWatchValue = 0f;

        Vector3 anchorLocalPos = transform.InverseTransformPoint(GridAnchor.position);

        // Pick forbidden direction (0,1,2,3 -> 0°,90°,180°,270°)
        int randomAnswer = Random.Range(0, 4);

        // Choose one random square that *does* use the forbidden direction
        int totalSquares = cols * rows;
        int allowedIndex = Random.Range(0, totalSquares);

        int currentIndex = 0;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Square
                GameObject square = Instantiate(GridSquare, transform);
                square.SetActive(true);
                square.transform.localPosition = anchorLocalPos + new Vector3(x * Spacing, -y * Spacing, 0);
                square.transform.SetParent(ObjectForDestruction.transform, true);

                Image squareIm = square.GetComponent<Image>();
                if (squareIm) squareIm.enabled = true;

                // Place arrow?
                bool placeArrow = (Random.value <= SquareFullness) || (currentIndex == allowedIndex);
                if (placeArrow)
                {
                    int rotationIndex;
                    if (currentIndex == allowedIndex)
                    {
                        // This square uses the forbidden direction (the answer)
                        rotationIndex = randomAnswer;
                        Answer = squareIm; // reveal will color this square
                    }
                    else
                    {
                        // Any direction except the forbidden one
                        List<int> options = new List<int> { 0, 1, 2, 3 };
                        options.Remove(randomAnswer);
                        rotationIndex = options[Random.Range(0, options.Count)];
                    }

                    GameObject arrow = Instantiate(GridArrow, square.transform);
                    arrow.SetActive(true);
                    arrow.transform.localPosition = Vector3.zero;
                    arrow.transform.localRotation = Quaternion.Euler(0, 0, rotationIndex * 90f);

                    Image arrowIm = arrow.GetComponent<Image>();
                    if (arrowIm) arrowIm.enabled = true;
                }

                currentIndex++;
            }
        }
    }

    public void RevealAnswer()
    {
        if (Answer != null)
            Answer.color = Color.green;
        canTick = false;
    }

    public void ClearGrid()
    {
        if (ObjectForDestruction == null) return;

        // Destroy all previous squares/arrows
        for (int i = ObjectForDestruction.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(ObjectForDestruction.transform.GetChild(i).gameObject);
        }
    }

    // Reads X/Y from the two TMP fields and writes into GridDimenstions (with clamping)
    private void ApplyGridSizeFromUI()
    {
        int cols = (int)GridDimenstions.x;
        int rows = (int)GridDimenstions.y;

        if (xInputField && int.TryParse(xInputField.text, out int xVal)) cols = xVal;
        if (yInputField && int.TryParse(yInputField.text, out int yVal)) rows = yVal;

        cols = Mathf.Clamp(Mathf.Max(1, cols), 1, 200);
        rows = Mathf.Clamp(Mathf.Max(1, rows), 1, 200);

        GridDimenstions = new Vector2(cols, rows);
    }

    // Phone buttons / actions
    private void OnEnable()
    {
        if (ResetBtn != null)
        {
            ResetBtn.action.performed += OnResetPressed;
            ResetBtn.action.Enable();
        }
        if (RevealBtn != null)
        {
            RevealBtn.action.performed += OnRevealPressed;
            RevealBtn.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (ResetBtn != null)
        {
            ResetBtn.action.performed -= OnResetPressed;
            ResetBtn.action.Disable();
        }
        if (RevealBtn != null)
        {
            RevealBtn.action.performed -= OnRevealPressed;
            RevealBtn.action.Disable();
        }
    }

    private void OnResetPressed(InputAction.CallbackContext ctx) => GenerateGrid();
    private void OnRevealPressed(InputAction.CallbackContext ctx) => RevealAnswer();
}
