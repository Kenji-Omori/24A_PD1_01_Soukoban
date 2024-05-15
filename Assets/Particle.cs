using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Particle : MonoBehaviour
{
  private float lifeTime;
  private float leftLifeTime;
  private Vector3 velocity;
  private Vector3 defaultScale;


  // Start is called before the first frame update
  void Start()
  {
    lifeTime = 0.3f;
    leftLifeTime = lifeTime;
    defaultScale = transform.localScale;
    float randomRange = 5;
    velocity = new Vector3(
        Random.Range(-randomRange, randomRange),
        Random.Range(-randomRange, randomRange),
        0
      );
  }

  // Update is called once per frame
  void Update()
  {
    // 残り時間をカウントダウン
    leftLifeTime -= Time.deltaTime;
    // 自身の座標を移動
    transform.position += velocity * Time.deltaTime;
    // 残り時間により徐々にScaleを小さくする
    transform.localScale = Vector3.Lerp
    (
      new Vector3(0, 0, 0),
      defaultScale,
      leftLifeTime / lifeTime
    );
    // 残り時間が0以下になったら自身のゲームオブジェクトを消滅
    if (leftLifeTime <= 0) {
      Destroy(gameObject);
    }
  }
}


