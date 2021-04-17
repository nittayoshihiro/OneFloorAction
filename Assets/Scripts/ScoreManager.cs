using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// スコア管理クラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>スコアテキスト</summary>
    [SerializeField] Text m_scoreText;
    /// <summary>結果テキスト</summary>
    [SerializeField] Text m_resultText;
    /// <summary>表時テキスト</summary>
    string m_fileName = "bestscore";//今後のため変数宣言
    /// <summary>現在のスコア</summary>
    private int m_score = 0;
    /// <summary>移動のポイント</summary>
    private int m_movepoint = 0;
    /// <summary>合計スコア</summary>
    private int m_tutalScore = 0;
    /// <summary>表示文字</summary>
    private string m_messege;
    /// <summary>1番のスコア</summary>
    private int m_bestScore = 0;
    /// <summary>プレイヤーのライフ</summary>
    private int m_playerlife;
    /// <summary>プレイヤーのライフアイコン</summary>
    private string m_lifeIcon = "@@@";

    private void Start()
    {
        m_bestScore = LoadJson();
        Debug.Log(GetFilePath());
    }

    /// <summary>
    /// ライフをアイコン化する
    /// </summary>
    /// <param name="playerlife"></param>
    /// <returns></returns>
    private string LifeIcon(int playerlife)
    {
        m_lifeIcon = "";
        for (int i = 0; i < playerlife; i++)
        {
            m_lifeIcon += "@";
        }
        return m_lifeIcon;
    }

    /// <summary>
    /// プレイヤーのライフを更新します
    /// </summary>
    /// <param name="playerlife">プレイヤーのライフを渡す</param>
    public void PlayerLife(int playerlife)
    {
        m_playerlife = playerlife;
        m_scoreText.text = string.Format("Score:{0:0000}\nlife:{1}", m_score, LifeIcon(m_playerlife));
    }

    /// <summary>
    /// スコアを初期化します
    /// </summary>
    public void Initialize()
    {
        m_score = 0;
        m_scoreText.text = string.Format("Score:{0:0000}\nlife:{1}", m_score, LifeIcon(m_playerlife));
        m_movepoint = 0;
    }

    /// <summary>
    ///　スコアを加算するための関数
    /// </summary>
    /// <param name="point"></param>
    public void AddScore(int point)
    {
        m_score += point;
        m_scoreText.text = string.Format("Score:{0:0000}\nlife:{1}", m_score, LifeIcon(m_playerlife));
    }

    /// <summary>
    ///　移動回数に応じてスコアを加算する
    /// </summary>
    /// <param name="movecount">移動回数</param>
    public void MovePoint(int movecount)
    {
        m_movepoint = 5000 - (100 * movecount);
        if (m_movepoint < 0)
        {
            m_movepoint = 0;
        }
    }

    /// <summary>
    /// リザルトテキスト作成
    /// </summary>
    public void ResultText()
    {
        m_tutalScore = m_score + m_movepoint;
        if (m_bestScore < m_tutalScore)
        {
            m_messege = "you new record";
            SaveJson(m_tutalScore);
            m_bestScore = m_tutalScore;
        }
        else
        {
            m_messege = "";
        }
        m_resultText.text = string.Format("score:{0:0000}\nmovepoint:{1:0000}\ntotalscore:{2:0000}\n{3}\nbestscore:{4:0000}",
            m_score, m_movepoint, m_tutalScore, m_messege, m_bestScore);
    }

    /// <summary>
    /// json形式で保存します
    /// </summary>
    /// <param name="bestScore"></param>
    private void SaveJson(int bestScore)
    {
        BestScore best = new BestScore(bestScore);
        //jsonシリアライズ
        string json = JsonUtility.ToJson(best);
        using (var writer = new StreamWriter(GetFilePath()))//eroor
        {
            writer.Write(json);
        }
    }

    /// <summary>
    /// json形式のテキストを読み取ります
    /// </summary>
    /// <returns></returns>
    private int LoadJson()
    {
        int saveDate = 0;
        try
        {
            using (var reader = new StreamReader(GetFilePath()))
            {
                //jsonでシリアライズ
                BestScore best = JsonUtility.FromJson<BestScore>(reader.ReadToEnd());
                saveDate = best.bestScore;
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log($"{ex}がなかったのでテキストを作成します");
            /*File.Create(GetFilePath());
            while (true)
            {
                if (File.Exists(GetFilePath()))
                {
                    break;
                }
            }*/
            SaveJson(m_tutalScore);
        }
        catch (ArgumentException ex)
        {
            Debug.Log(ex);
        }
        return saveDate;
    }

    /// <summary>
    /// ファイルパス
    /// </summary>
    /// <returns></returns>
    private string GetFilePath()
    {
        // Unity の場合はどこでもファイルの読み書きができるわけではないことに注意。Application.persistentDataPath を使って「読み書きできるところ」でファイル操作をすること。
        string filePath = Application.persistentDataPath + "\\" + (m_fileName == "" ? Application.productName : m_fileName) + ".json";
        return filePath;
    }
}

[Serializable]
public class BestScore
{
    public int bestScore;
    public BestScore(int bestScore)
    {
        this.bestScore = bestScore;
    }
}