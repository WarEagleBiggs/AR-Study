using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Demo_Text : MonoBehaviour
{
    [Header("Text Generation")]
    [SerializeField] private int wordCount = 50;
    public string generatedText;
    public TextMeshProUGUI outputTxt;

    private readonly string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "z" };
    private readonly string[] vowels = { "a", "e", "i", "o", "u", "y" };

    [Header("Stopwatch")]
    public TextMeshProUGUI stopWatchTxt;
    public float stopWatchValue;
    public bool canTick;

    [Header("Input Actions")]
    public InputActionReference resetBtn;
    public InputActionReference revealBtn;

    [Header("Visual Effects")]
    public Image backgroundImage;
    
    
    //control panel
    public TMP_InputField inputField;
    

    void Start()
    {
        GenerateText();
        canTick = true;
    }

    void Update()
    {
        //control panel updates
        wordCount = int.Parse(inputField.text);

        
        
        // Keyboard fallback (optional)
        if (Keyboard.current.rKey.wasPressedThisFrame)
            GenerateText();
        if (Keyboard.current.eKey.wasPressedThisFrame)
            RevealText();

        // Stopwatch update
        if (canTick)
        {
            stopWatchValue += Time.deltaTime;
            stopWatchTxt.text = stopWatchValue.ToString("F2") + "s";
        }

        // Update UI text
        outputTxt.SetText(generatedText);
    }

    public void GenerateText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < wordCount; i++)
        {
            sb.Append(GenerateWord(Random.Range(3, 8))); // Word length: 3–7
            if (i < wordCount - 1)
                sb.Append(" ");
        }
        generatedText = sb.ToString();

        // Reset visuals
        if (backgroundImage != null)
            backgroundImage.color = Color.white;

        stopWatchValue = 0f;
        canTick = true;
    }

    private string GenerateWord(int length)
    {
        StringBuilder word = new StringBuilder();
        bool startWithConsonant = Random.value > 0.5f;

        for (int i = 0; i < length; i++)
        {
            if ((i % 2 == 0 && startWithConsonant) || (i % 2 == 1 && !startWithConsonant))
                word.Append(consonants[Random.Range(0, consonants.Length)]);
            else
                word.Append(vowels[Random.Range(0, vowels.Length)]);
        }

        return word.ToString();
    }

    public void RevealText()
    {
        if (backgroundImage != null)
            backgroundImage.color = Color.green;

        canTick = false;
    }

    // Input system actions
    private void OnEnable()
    {
        resetBtn.action.performed += OnResetPressed;
        resetBtn.action.Enable();

        revealBtn.action.performed += OnRevealPressed;
        revealBtn.action.Enable();
    }

    private void OnDisable()
    {
        resetBtn.action.performed -= OnResetPressed;
        resetBtn.action.Disable();

        revealBtn.action.performed -= OnRevealPressed;
        revealBtn.action.Disable();
    }

    private void OnResetPressed(InputAction.CallbackContext context)
    {
        GenerateText();
    }

    private void OnRevealPressed(InputAction.CallbackContext context)
    {
        RevealText();
    }
}
