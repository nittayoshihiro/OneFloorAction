﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoMappingClass : MonoBehaviour
{
    [SerializeField] Tilemap m_tilemap;
    /// <summary>mapのサイズX</summary>
    [SerializeField] int m_mapX = 100;
    /// <summary>mapのサイズY</summary>
    [SerializeField] int m_mapY = 100;
    /// <summary>部屋のxの長さ</summary>(偶数をいれる)
    [SerializeField] int m_roomX = 4;
    /// <summary>部屋のyの長さ</summary>(偶数をいれる)
    [SerializeField] int m_roomY = 4;
    [SerializeField] Vector3Int m_vector3Int = new Vector3Int(0, 0, 0);
    /// <summary>壁のタイル</summary>
    Tile[] m_wallTile;
    /// <summary>道のタイル</summary>
    Tile[] m_roadTile;
    // Start is called before the first frame update
    void Start()
    {
        //Resourcesフォルダーからタイルを読み込む
        m_roadTile = Resources.LoadAll<Tile>("RoadPalette");
        m_wallTile = Resources.LoadAll<Tile>("WallPalette");

        //CenterRoomMapping(m_wallTile[0], m_roadTile[0], m_vector3Int, m_roomX, m_roomY);
        RoomMapping(m_wallTile[0], m_roadTile[0], m_vector3Int, m_roomX, m_roomY);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>ポジションにを中心に部屋を作る</summary>
    void CenterRoomMapping(Tile wallTile, Tile roadTile, Vector3Int centerPosition, int roomSizeX, int roomSizeY)
    {
        int tileIndex = roomSizeX * roomSizeY;
        //roomX×roomYの部屋を想定(xの横並びで考える)
        Tile[] tile = new Tile[tileIndex];
        //置くtileを配列に収納
        for (int i = 0; i < tile.Length; i++)
        {
            if (i < roomSizeX || i % roomSizeX == 0 || i % roomSizeX == roomSizeX - 1 || i > tileIndex - roomSizeX)
            {
                tile[i] = wallTile;
            }
            else
            {
                tile[i] = roadTile;
            }
        }
        int putIndex = 0;
        //配列からtileを置く(xの横並びで考える)
        for (int y = 0; y < roomSizeY; y++)
        {
            for (int x = 0; x < roomSizeX; x++)
            {
                centerPosition = new Vector3Int(x - roomSizeX / 2, y - roomSizeY / 2, 0);
                m_tilemap.SetTile(centerPosition, tile[putIndex]);
                putIndex++;
            }
        }
    }

    /// <summary>ポジションを原点にXYを伸ばして作成</summary>
    void RoomMapping(Tile wallTile, Tile roadTile, Vector3Int position, int roomSizeX, int roomSizeY)
    {
        int tileIndex = roomSizeX * roomSizeY;
        //roomX×roomYの部屋を想定(xの横並びで考える)
        Tile[] tile = new Tile[tileIndex];
        //置くtileを配列に収納
        for (int i = 0; i < tile.Length; i++)
        {
            if (i < roomSizeX || i % roomSizeX == 0 || i % roomSizeX == roomSizeX - 1 || i > tileIndex - roomSizeX)
            {
                tile[i] = wallTile;
            }
            else
            {
                tile[i] = roadTile;
            }
        }
        int putIndex = 0;
        //配列からtileを置く(xの横並びで考える)
        for (int y = 0; y < roomSizeY; y++)
        {
            for (int x = 0; x < roomSizeX; x++)
            {
                m_tilemap.SetTile(position, tile[putIndex]);
                putIndex++;
                position.x++;
            }
            position.x -= roomSizeX;
            position.y++;
        }

    }
    public void MappingButtun()
    {
        m_tilemap.ClearAllTiles();
        PositionMapping(m_wallTile[0], m_roadTile[0], m_vector3Int, m_mapX, m_mapY);
    }
    void PositionMapping(Tile wallTile, Tile roadTile, Vector3Int mapPosition, int mapX, int mapY)
    {
        System.Random random = new System.Random();
        //部屋のXY
        int roomX = mapX, roomY = mapY;
        for (int i = 0; i < 3; i++)
        {
            //ランダムにX座標を決める
            int randPosX = random.Next(mapPosition.x + 5, mapX - 5);
            if (randPosX < mapX / 2)
            {
                //半分より小さいとき
                roomX = randPosX;
                RoomMapping(wallTile, roadTile, mapPosition, roomX, roomY);
                mapX -= roomX;
                mapPosition.x += randPosX;
                roomX = mapX;
            }
            else
            {
                //半分より大きいとき
                roomX = mapX - randPosX;
                mapPosition.x += randPosX;
                RoomMapping(wallTile, roadTile, mapPosition, roomX, roomY);
                mapX -= roomX;
                mapPosition.x -= randPosX;
                roomX = mapX;
            }
            //ランダムにY座標を決める
            int randPosY = random.Next(mapPosition.y + 5, mapY - 5);
            if (randPosY < mapY / 2)
            {
                //半分より小さいとき
                roomY = randPosY;
                RoomMapping(wallTile, roadTile, mapPosition, roomX, roomY);
                mapY -= roomY;
                mapPosition.y += randPosY;

            }
            else
            {
                //半分より大きいとき
                roomY = mapY - randPosY;
                mapPosition.y += randPosY;
                RoomMapping(wallTile, roadTile, mapPosition, roomX, roomY);
                mapY -= roomY;
                mapPosition.y -= randPosY;
            }



        }
    }
}
