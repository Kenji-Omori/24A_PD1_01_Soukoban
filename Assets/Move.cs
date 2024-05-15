using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
  // 完了までにかかる時間
  private float timeTaken = 0.2f;
  // 経過時間
  private float timeErapsed;
  // 目的地
  private Vector3 destination;
  // 出発地
  private Vector3 origin;

  //private void Start()
  //{
  //  // 目的地・出発地を現在地で初期化
  //  destination = transform.position;
  //  origin = destination;
  //}

  public void PositionsSetup(Vector3 startPosition)
  {
    destination = startPosition;
    origin = startPosition;
  }

  public void MoveTo(Vector3 newDestination)
  {
    gameObject.GetComponent<AudioSource>().Play();
    // 実行開始時間を記録
    timeErapsed = 0;
    // 移動中の可能性があるので、現在地とpositionに前回移動の目的地を代入
    origin = destination;
    transform.position = origin;
    // 新しい目的地を代入
    destination = newDestination;
  }
  private void Update()
  {
    // 目的地に到着していたら処理しない
    if (origin == destination) { return; }
    timeErapsed += Time.deltaTime;
    // 実行開始時間と実行完了時間を0-1としたとき、現在時間がいくつか算出
    float timeRate = timeErapsed / timeTaken;
    // 実行完了時間を超えるようであれば実行完了時間相当に丸める。
    if (timeRate > 1) { timeRate = 1; }
    // イージング用計算
    float easing = 1-Mathf.Pow(1-timeRate,3);
    // 座標を算出
    Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
    // 算出した座標をpositionに代入
    transform.position = currentPosition;
  }

  float EasingBounth(float x)
  {
    float n1 = 7.5625f;
    float d1 = 2.75f;

    if (x < 1 / d1)
    {
      return n1 * x * x;
    }
    else if (x < 2 / d1)
    {
      return n1 * (x -= 1.5f / d1) * x + 0.75f;
    }
    else if (x < 2.5 / d1)
    {
      return n1 * (x -= 2.25f / d1) * x + 0.9375f;
    }
    else
    {
      return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }
  }

  float EaseOutElastic(float x)
  {
    float c4 = (2 * Mathf.PI) / 3;

    return x == 0
      ? 0
      : x == 1
      ? 1
      : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
  }
}
