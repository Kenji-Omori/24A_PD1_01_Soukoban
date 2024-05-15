using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class BeginBounth : MonoBehaviour
{
  Vector3 buildFrom;
  Vector3 buildTo;
  float timeTaken = 0.2f;
  float timeTakenFast = 0.02f;
  float timeErapse = 0;


  private void Awake()
  {
    buildFrom = transform.position;
  }

  public void SetBuildTo(Vector3 buildTo) 
  {
    this.buildTo = buildTo;
    Move move;
    if (!TryGetComponent(out move)) { return; }
    move.PositionsSetup(buildTo);
  }

  // Update is called once per frame
  void Update()
  {
    timeErapse += Time.deltaTime;
    // 実行開始時間と実行完了時間を0-1としたとき、現在時間がいくつか算出
    float taken = timeTaken;
    if (Input.anyKey) { taken = timeTakenFast; }
    float timeRate = timeErapse / taken;
    // 実行完了時間を超えるようであれば実行完了時間相当に丸める。
    if (timeRate > 1) { timeRate = 1; }
    // イージング用計算
    float easing = EasingBounth(timeRate);
    // 座標を算出
    Vector3 currentPosition = Vector3.Lerp(buildFrom, buildTo, easing);
    // 算出した座標をpositionに代入
    transform.position = currentPosition;

    if (timeErapse >= taken) { this.enabled = false; }

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

}
