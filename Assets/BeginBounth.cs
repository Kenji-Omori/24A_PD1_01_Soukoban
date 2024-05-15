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
    // ���s�J�n���ԂƎ��s�������Ԃ�0-1�Ƃ����Ƃ��A���ݎ��Ԃ��������Z�o
    float taken = timeTaken;
    if (Input.anyKey) { taken = timeTakenFast; }
    float timeRate = timeErapse / taken;
    // ���s�������Ԃ𒴂���悤�ł���Ύ��s�������ԑ����Ɋۂ߂�B
    if (timeRate > 1) { timeRate = 1; }
    // �C�[�W���O�p�v�Z
    float easing = EasingBounth(timeRate);
    // ���W���Z�o
    Vector3 currentPosition = Vector3.Lerp(buildFrom, buildTo, easing);
    // �Z�o�������W��position�ɑ��
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
