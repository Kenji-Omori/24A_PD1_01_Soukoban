using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
  // �z��̐錾
  int[] map;

  void PrintArray()
  {
    string debugText = "";
    for (int i = 0; i < map.Length; i++)
    {
      debugText += map[i].ToString() + ", ";
    }
    Debug.Log(debugText);
  }

  int GetPlayerIndex()
  {
    for (int i = 0; i < map.Length; i++)
    {
      if (map[i] == 1)
      {
        return i;
      }
    }
    return -1;
  }

  bool MoveNumber(int number, int moveFrom, int moveTo)
  {
    if (moveTo < 0 || moveTo >= map.Length)
    {
      // �����Ȃ��������ɏ����A���^�[������B�������^�[��
      return false;
    }
    // �ړ����2(��)��������
    if (map[moveTo] == 2)
    {
      // �ǂ̕����ֈړ����邩���Z�o
      int velocity = moveTo - moveFrom;
      // �v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������B
      // ���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h��
      // �ĂсA�������ċA���Ă���B�ړ��s��bool�ŋL�^
      bool success = MoveNumber(2, moveTo, moveTo + velocity);
      // ���������ړ����s������A�v���C���[�̈ړ������s
      if (!success) { return false; }
    }



    map[moveTo] = number;
    map[moveFrom] = 0;
    return true;
  }


  void Start()
  {
    map = new int[] { 0, 2, 0, 1, 0, 2, 0, 2, 0, 2, 0, 0, 0 };
    // �ǉ��B������̐錾�Ə�����
    PrintArray();
  }


  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      // ������Ȃ��������̂��߂�-1�ŏ�����
      int playerIndex = GetPlayerIndex();
      MoveNumber(1, playerIndex, playerIndex + 1);
      PrintArray();
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      int playerIndex = GetPlayerIndex();
      MoveNumber(1, playerIndex, playerIndex - 1);
      PrintArray();
    }

  }
}
