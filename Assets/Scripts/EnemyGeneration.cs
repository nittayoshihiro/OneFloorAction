using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGeneration : MonoBehaviour
{
    [SerializeField] GameObject[] m_enemyObjects;
    AutoMappingV3.TileStatus[,] m_mapStatus;

    /// <summary>敵生成</summary>
    public void EnemyGenerator()
    {
        m_mapStatus = GameObject.FindObjectOfType<AutoMappingV3>().GetMappingData;
        for (int x = 1; x < m_mapStatus.GetLength(0)-1; x++)
        {
            for (int y = 1; y < m_mapStatus.GetLength(1)-1; y++)
            {
                if (m_mapStatus[x, y] == AutoMappingV3.TileStatus.Road && Random.Range(1,100) < 3)
                {
                    Instantiate(m_enemyObjects[m_enemyObjects.Length-1],new Vector3 ((float)x+0.5f,(float)y+0.5f,0),Quaternion.identity);
                }
            }
        }
       
    }
}
