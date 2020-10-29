using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoMaping : MonoBehaviour
{
    [SerializeField] Tilemap m_tilemap;
    /// <summary>壁のタイル</summary>
    Tile []m_wallTile;
    /// <summary>道のタイル</summary>
    Tile []m_roadTile;
    // Start is called before the first frame update
    void Start()
    {
        //Resourcesフォルダーからタイルを読み込む
        m_roadTile = Resources.LoadAll<Tile>("RoadPalette");
        m_wallTile = Resources.LoadAll<Tile>("WallPalette");

        //タイルをタイルマップに置く
        Vector3Int roadPosition = new Vector3Int(1, 1, 0);
        m_tilemap.SetTile(roadPosition, m_roadTile[0]);
        Vector3Int wallPosition = new Vector3Int(1, 2, 0);
        m_tilemap.SetTile(wallPosition, m_wallTile[0]);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
