using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  // 配列の宣言
  int[,] map;
  GameObject[,] field;
  public GameObject playerPrefab;
  public GameObject boxPrefab;
  public GameObject wallPrefab;
  public GameObject goalPrefab;
  public GameObject clearText;
  public GameObject particlePrefab;
  private List<Vector2Int> goals = new List<Vector2Int>();
  private float buildErapse = 0;
  private float buildInterval = 0.1f;
  private float buildIntervalFast = 0.01f;
  private int buildedNum = 0;
  bool isBuilded { get { return buildedNum >= map.Length; } }

  Vector2Int GetPlayerIndex()
  {
    for (int y = 0; y < field.GetLength(0); y++)
    {
      for (int x = 0; x < field.GetLength(1); x++)
      {
        if (field[y, x] == null) { continue; }
        if (field[y, x].tag == "Player")
        {
          return new Vector2Int(x, y);
        }
      }
    }
    return new Vector2Int(-1, -1);
  }

  bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo, int power = 1)
  {
    if (power < 0) { return false; }
    if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
    if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
    if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall") { return false; }
    // 移動先に2(箱)が居たら

    if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
    {
      // どの方向へ移動するかを算出
      Vector2Int velocity = moveTo - moveFrom;
      // プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
      // 箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
      // 呼び、処理が再帰している。移動可不可をboolで記録
      bool success = MoveNumber("Box", moveTo, moveTo + velocity, power);
      // もし箱が移動失敗したら、プレイヤーの移動も失敗
      if (!success) { return false; }
    }

    Vector3 moveToPosition = new Vector3(
      moveTo.x - map.GetLength(1) / 2,
      -moveTo.y + map.GetLength(0) / 2, 0
      );
    field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
    //field[moveFrom.y, moveFrom.x].transform.position =
    //  new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
    field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
    field[moveFrom.y, moveFrom.x] = null;

    return true;
  }

  private void BuildSequense()
  {
    if (buildedNum >= map.GetLength(0) * map.GetLength(1)) { return; }
    buildErapse += Time.deltaTime;
    if (Input.anyKey)
    {
      buildInterval = buildIntervalFast;
    }
    if (buildErapse < buildInterval) { return; }
    do {
      int x = buildedNum % map.GetLength(1);
      int y = (int)(buildedNum / map.GetLength(1));
      Vector3 buildFrom= new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 10);
      Vector3 buildTo= new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0);
      GameObject prefab;
      switch (map[y, x])
      {
        case 0:
          buildedNum++;
          continue;
        case 1:
          prefab = playerPrefab;
          break;
        case 2:
          prefab = boxPrefab;
          break;
        case 3:
          prefab = wallPrefab;
          break;
        case 4:
          prefab = goalPrefab;
          break;
        default:
          return;
      }
      field[y, x] = Instantiate(prefab, buildFrom, Quaternion.identity);
      field[y, x].GetComponent<BeginBounth>().SetBuildTo(buildTo);
      buildErapse -= buildInterval;
      buildedNum++;
    } while (buildErapse>= buildInterval);
  }

  void Start()
  {
    Screen.SetResolution(1280, 720, false);
    map = new int[,] {
     { 3,3,3,3,3,3,3,3,3,3,3,3,3,3 },
     { 3,0,0,0,3,0,0,0,0,0,0,0,0,3 },
     { 3,0,0,2,0,0,0,0,0,0,0,0,0,3 },
     { 3,0,0,2,0,1,3,0,0,0,0,4,3,3 },
     { 3,0,0,2,0,0,0,0,0,0,0,4,3,3 },
     { 3,0,0,0,0,0,0,0,0,0,0,4,3,3 },
     { 3,0,0,0,0,0,0,0,0,0,0,0,0,3 },
     { 3,0,0,0,3,0,0,0,0,0,0,0,0,3 },
     { 3,3,3,3,3,3,3,3,3,3,3,3,3,3 }
    };


    field = new GameObject
    [
      map.GetLength(0),
      map.GetLength(1)
    ];

    for (int y = 0; y < map.GetLength(0); y++)
    {
      for (int x = 0; x < map.GetLength(1); x++)
      {
        if (map[y, x] == 4)
        {
          goals.Add(new Vector2Int(x, y));
        }
      }
    }
  }




  // Update is called once per frame
  void Update()
  {
    BuildSequense();
    if (!isBuilded) { return; }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      // 見つからなかった時のために-1で初期化
      Vector2Int playerIndex = GetPlayerIndex();
      MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
      if (IsCleard())
      {
        Debug.Log("Clear!");
        clearText.SetActive(true);
      }
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      Vector2Int playerIndex = GetPlayerIndex();
      MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
      if (IsCleard())
      {
        Debug.Log("Clear!");
        clearText.SetActive(true);
      }
    }

    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      // 見つからなかった時のために-1で初期化
      Vector2Int playerIndex = GetPlayerIndex();
      MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
      if (IsCleard())
      {
        Debug.Log("Clear!");
        clearText.SetActive(true);
      }
    }

    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      Vector2Int playerIndex = GetPlayerIndex();
      MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
      if (IsCleard())
      {
        Debug.Log("Clear!");
        clearText.SetActive(true);
      }
    }
  }

  bool IsCleard()
  {
    for (int i = 0; i < goals.Count; i++)
    {
      GameObject f = field[goals[i].y, goals[i].x];
      if (f == null || f.tag != "Box")
      {
        return false;
      }
    }
    return true;
  }
}
