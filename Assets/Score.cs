using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;
    private int enemyKillCount;

    private void Start()
    {
        enemyKillCount = 0;
    }
    public void AddScore()
    {
        enemyKillCount++;
        score.text=enemyKillCount.ToString();
    }
}
