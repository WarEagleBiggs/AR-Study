using System.Text;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Demo_Text : MonoBehaviour
{
    [Header("Text Generation")]
    [SerializeField] private int wordCount = 50;
    public string generatedText;        // formatted (with TMP tags after reveal)
    private string generatedTextRaw;    // plain text copy for searching
    public TextMeshProUGUI outputTxt;

    private readonly string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "z" };
    private readonly string[] vowels     = { "a", "e", "i", "o", "u", "y" };

    [Header("Stopwatch")]
    public TextMeshProUGUI stopWatchTxt;
    public float stopWatchValue;
    public bool canTick;

    [Header("Input Actions")]
    public InputActionReference resetBtn;
    public InputActionReference revealBtn;

    [Header("Visual Effects")]
    public Image backgroundImage;

    [Header("Control Panel")]
    public TMP_InputField wordCountInput;   // word count
    public TMP_InputField letterInput;      // letter to highlight
    public TMP_InputField occurrenceInput;  // Nth occurrence (e.g., "3")
    public bool caseSensitive = false;

    void Update()
    {
        // Word count
        if (wordCountInput != null && int.TryParse(wordCountInput.text, out int wc) && wc > 0)
            wordCount = wc;

        // Keyboard fallback
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame) GenerateText();
            if (Keyboard.current.eKey.wasPressedThisFrame) RevealText();
        }

        // Stopwatch
        if (canTick)
        {
            stopWatchValue += Time.deltaTime;
            if (stopWatchTxt != null) stopWatchTxt.text = stopWatchValue.ToString("F2") + "s";
        }

        // UI
        if (outputTxt != null) outputTxt.SetText(generatedText);
    }

    public void GenerateText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < wordCount; i++)
        {
            sb.Append(GenerateWord(Random.Range(3, 8))); // 3–7 letters
            if (i < wordCount - 1) sb.Append(' ');
        }

        generatedTextRaw = sb.ToString();
        generatedText    = generatedTextRaw;

        if (backgroundImage != null) backgroundImage.color = Color.white;

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
        // Stop timer
        canTick = false;

        // Source text fallback
        if (string.IsNullOrEmpty(generatedTextRaw))
            generatedTextRaw = generatedText;

        // Get target letter (first char only)
        char? target = null;
        if (letterInput != null && !string.IsNullOrWhiteSpace(letterInput.text))
            target = letterInput.text.Trim().FirstOrDefault();

        // Get occurrence level (defaults to 1)
        int level = 1;
        if (occurrenceInput != null && int.TryParse(occurrenceInput.text, out int parsed) && parsed > 0)
            level = parsed;

        // If no letter, keep as-is
        if (target == null || target == '\0')
        {
            generatedText = generatedTextRaw;
            return;
        }

        string source = generatedTextRaw;
        string needle = target.Value.ToString();

        var cmp = caseSensitive ? System.StringComparison.Ordinal
                                : System.StringComparison.OrdinalIgnoreCase;

        int idx = GetNthOccurrenceIndex(source, needle, level, cmp);

        if (idx < 0)
        {
            // Nth occurrence not found, do nothing
            generatedText = generatedTextRaw;
            return;
        }

        // Wrap that single character: green + bold (no background)
        const string openTag  = "<color=#00FF00><b>";
        const string closeTag = "</b></color>";

        var sb = new StringBuilder(source.Length + openTag.Length + closeTag.Length);
        sb.Append(source, 0, idx);
        sb.Append(openTag);
        sb.Append(source[idx]);
        sb.Append(closeTag);
        if (idx + 1 < source.Length)
            sb.Append(source, idx + 1, source.Length - (idx + 1));

        generatedText = sb.ToString();

        if (backgroundImage != null)
            backgroundImage.color = Color.white; // keep background normal
    }

    // Returns the 0-based index of the Nth occurrence of 'needle' in 'source' using comparison 'cmp'; -1 if not found
    private static int GetNthOccurrenceIndex(string source, string needle, int n, System.StringComparison cmp)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(needle) || n <= 0) return -1;

        int count = 0;
        int start = 0;

        while (start < source.Length)
        {
            int hit = source.IndexOf(needle, start, cmp);
            if (hit < 0) break;

            count++;
            if (count == n) return hit;

            start = hit + needle.Length; // advance past this match
        }
        return -1;
    }

    // Input system actions
    private void OnEnable()
    {
        if (resetBtn != null)
        {
            resetBtn.action.performed += OnResetPressed;
            resetBtn.action.Enable();
        }
        if (revealBtn != null)
        {
            revealBtn.action.performed += OnRevealPressed;
            revealBtn.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (resetBtn != null)
        {
            resetBtn.action.performed -= OnResetPressed;
            resetBtn.action.Disable();
        }
        if (revealBtn != null)
        {
            revealBtn.action.performed -= OnRevealPressed;
            revealBtn.action.Disable();
        }
    }

    private void OnResetPressed(InputAction.CallbackContext context) => GenerateText();
    private void OnRevealPressed(InputAction.CallbackContext context) => RevealText();
}
