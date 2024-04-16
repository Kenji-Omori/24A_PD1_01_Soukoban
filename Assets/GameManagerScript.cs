using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  // 配列の宣言
  int[] map;

  void Start()
  {
    map = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 };
    // 追加。文字列の宣言と初期化
    string debugText = "";
    for (int i = 0; i < map.Length; i++)
    {
      // 変更。文字列に結合していく
      debugText += map[i].ToString() + ", ";
    }
    // 結合した文字列を出力
    Debug.Log(debugText);
  }


  // Update is called once per frame
  void Update()
  {

    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      // 見つからなかった時のために-1で初期化
      int playerIndex = -1;
      // 要素数はmap.Lengthで取得
      for (int i = 0; i < map.Length; i++)
      {
        if (map[i] == 1)
        {
          playerIndex = i;
          break;
        }
      }
      if (playerIndex < map.Length - 1)
      {
        map[playerIndex + 1] = 1;
        map[playerIndex] = 0;
      }

      string debugText = "";
      for (int i = 0; i < map.Length; i++)
      {
        debugText += map[i].ToString() + ", ";
      }
      Debug.Log(debugText);
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      // 見つからなかった時のために-1で初期化
      int playerIndex = -1;
      // 要素数はmap.Lengthで取得
      for (int i = 0; i < map.Length; i++)
      {
        if (map[i] == 1)
        {
          playerIndex = i;
          break;
        }
      }
      if (playerIndex > 0)
      {
        map[playerIndex - 1] = 1;
        map[playerIndex] = 0;
      }

      string debugText = "";
      for (int i = 0; i < map.Length; i++)
      {
        debugText += map[i].ToString() + ", ";
      }
      Debug.Log(debugText);
    }

  }
}
