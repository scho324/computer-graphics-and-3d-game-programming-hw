using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Score: " + ScoreManager.instance.amount;
    }
}
