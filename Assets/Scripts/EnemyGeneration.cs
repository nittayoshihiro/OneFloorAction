using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵生成クラス
/// </summary>
public class EnemyGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] m_enemyObjects;
    AutoMappingV3.TileStatus[,] m_mapStatus;
    /// <summary>敵が生成される確率</summary>
    float m_ramenmyGenerationProbability = 0.03f;

    /// <summary>敵を生成する</summary>
    public void EnemyGenerator()
    {
        m_mapStatus = GameObject.FindObjectOfType<AutoMappingV3>().GetMappingData;
        //各タイルについて処理をする
        for (int x = 1; x < m_mapStatus.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < m_mapStatus.GetLength(1) - 1; y++)
            {
                //タイルが道だったら、確率で敵を生成する
                if (m_mapStatus[x, y] == AutoMappingV3.TileStatus.Road && Random.Range(0, 1f) < m_ramenmyGenerationProbability)
                {
                    Instantiate(m_enemyObjects[m_enemyObjects.Length - 1], new Vector3((float)x + 0.5f, (float)y + 0.5f, 0), Quaternion.identity);
                }
            }
        }

    }
}
