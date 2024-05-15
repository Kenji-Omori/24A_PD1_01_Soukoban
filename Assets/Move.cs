using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
  // �����܂łɂ����鎞��
  private float timeTaken = 0.2f;
  // �o�ߎ���
  private float timeErapsed;
  // �ړI�n
  private Vector3 destination;
  // �o���n
  private Vector3 origin;

  //private void Start()
  //{
  //  // �ړI�n�E�o���n�����ݒn�ŏ�����
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
    // ���s�J�n���Ԃ��L�^
    timeErapsed = 0;
    // �ړ����̉\��������̂ŁA���ݒn��position�ɑO��ړ��̖ړI�n����
    origin = destination;
    transform.position = origin;
    // �V�����ړI�n����
    destination = newDestination;
  }
  private void Update()
  {
    // �ړI�n�ɓ������Ă����珈�����Ȃ�
    if (origin == destination) { return; }
    timeErapsed += Time.deltaTime;
    // ���s�J�n���ԂƎ��s�������Ԃ�0-1�Ƃ����Ƃ��A���ݎ��Ԃ��������Z�o
    float timeRate = timeErapsed / timeTaken;
    // ���s�������Ԃ𒴂���悤�ł���Ύ��s�������ԑ����Ɋۂ߂�B
    if (timeRate > 1) { timeRate = 1; }
    // �C�[�W���O�p�v�Z
    float easing = 1-Mathf.Pow(1-timeRate,3);
    // ���W���Z�o
    Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
    // �Z�o�������W��position�ɑ��
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
