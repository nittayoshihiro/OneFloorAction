using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]Text m_scoreText;
    private int m_totalScore = 0;
   
    /// <summary>
    /// スコアを初期化します
    /// </summary>
    public void Initialize()
    {
        m_totalScore = 0;
        m_scoreText.text = string.Format("Score:{0:0000}",m_totalScore);
    }

    /// <summary>
    ///　スコアを加算するための関数
    /// </summary>
    /// <param name="point"></param>
    public void AddScore(int point)
    {
        m_totalScore += point;
        m_scoreText.text = string.Format("Score:{0:0000}", m_totalScore);
    }
}
