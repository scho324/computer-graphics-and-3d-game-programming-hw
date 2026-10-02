using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int amount;
    public static ScoreManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.Log("Duplicated ScoreManager.", gameObject);
        }
    }
}
