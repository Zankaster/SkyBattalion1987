using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersDisplay : MonoBehaviour
{
    public List<Sprite> symbols;
    protected int totalValue = 0;
    protected List<SpriteRenderer> numbers;
    void Awake() {
        numbers = new List<SpriteRenderer>();
        for (int i = 0; i < transform.childCount; i++) {
            numbers.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
        SetValue(0);
    }

    public void SetValue(int score) {
        totalValue = score;
        UpdateValue();
    }

    public void AddValue(int score) {
        totalValue += score;
        UpdateValue();
    }

    void UpdateValue() {
        string valueString = totalValue.ToString();
        var len = valueString.Length;
        int i;
        for (i = 0; i < len; i++) {
            numbers[i].sprite = symbols[int.Parse(valueString[i] + "")];
        }

        for (int j = i; j < numbers.Count; j++) {
            numbers[j].sprite = symbols[10];
        }
    }
}
