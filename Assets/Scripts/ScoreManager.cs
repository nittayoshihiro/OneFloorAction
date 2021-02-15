using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text m_scoreText;
    [SerializeField] Text m_resultText;
    string m_fileName = "bestscore";//今後のため変数宣言
    private int m_score = 0;
    private int m_movepoint = 0;
    private int m_tutalScore = 0;
    private string m_messege;
    private int m_bestScore = 0;

    private void Start()
    {
        m_bestScore = LoadJson();
    }

    /// <summary>
    /// スコアを初期化します
    /// </summary>
    public void Initialize()
    {
        m_score = 0;
        m_scoreText.text = string.Format("Score:{0:0000}", m_score);
        m_movepoint = 0;
    }

    /// <summary>
    ///　スコアを加算するための関数
    /// </summary>
    /// <param name="point"></param>
    public void AddScore(int point)
    {
        m_score += point;
        m_scoreText.text = string.Format("Score:{0:0000}", m_score);
    }

    /// <summary>
    ///　移動回数に応じてスコアを加算する
    /// </summary>
    /// <param name="movecount">移動回数</param>
    public void MovePoint(int movecount)
    {
        m_movepoint += movecount;
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
        //jsonシリアライズ
        string json = JsonUtility.ToJson(bestScore);
        using (var writer = new StreamWriter(GetFilePath(), false))
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
            using (var reader = new StreamReader(GetFilePath(), false))
            {
                //jsonでシリアライズ
                saveDate = JsonUtility.FromJson<int>(reader.ToString());
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.Log($"{ex}がなかったのでテキストを作成します");
            File.Create(GetFilePath());
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
