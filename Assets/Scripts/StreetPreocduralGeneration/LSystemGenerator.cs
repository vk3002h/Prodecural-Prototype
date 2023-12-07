using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    public Rules[] rules;
    public string rootSentence;
    [Range(0, 10)]
    public int iterationLimit = 1;
    [SerializeField]
    private bool RandomRuleIgnore = true;
    [SerializeField]
    [Range(0, 1)]
    private float RuleIgnoreChance = 0.2f;

    public void Start()
    {
        Debug.Log(GenerateSentence());
    }
    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = rootSentence;
        }
        return GrowRecursive(word);
    }
    public string GrowRecursive(string word, int iterationIndex = 0)
    {
        if (iterationIndex >= iterationLimit)
        {
            return word;
        }
        StringBuilder newword = new StringBuilder();

        foreach (var c in word)
        {
            newword.Append(c);
            ProcessRules(newword, c, iterationIndex);
        }

        return newword.ToString();
    }

    public void ProcessRules(StringBuilder newword, char c, int iterationIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == c.ToString())
            {
                if (RandomRuleIgnore)
                {
                    if (Random.Range(0.0f, 1.0f) > RuleIgnoreChance)
                    {
                        return;
                    }
                }
                newword.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}
