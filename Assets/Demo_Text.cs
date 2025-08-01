using System;
using System.Linq;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Demo_Text : MonoBehaviour
{
    [SerializeField] private int wordCount = 50;
    public string generatedText;
    public TextMeshProUGUI outputTxt;

    private string[] loremWords = new string[]
    {
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
        "magna", "aliqua", "ut", "enim", "ad", "minim", "veniam", "quis", "nostrud",
        "exercitation", "ullamco", "laboris", "nisi", "ut", "aliquip", "ex", "ea",
        "commodo", "consequat", "duis", "aute", "irure", "dolor", "in", "reprehenderit",
        "in", "voluptate", "velit", "esse", "cillum", "dolore", "eu", "fugiat", "nulla",
        "pariatur"
    };

    private void Update()
    {
        outputTxt.SetText(generatedText);
    }

    void Start()
    {
        generatedText = GenerateLoremIpsum(wordCount);
    }

    private string GenerateLoremIpsum(int count)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < count; i++)
        {
            string word = loremWords[Random.Range(0, loremWords.Length)];
            sb.Append(word);
            if (i < count - 1) sb.Append(" ");
        }
        return sb.ToString();
    }
}
