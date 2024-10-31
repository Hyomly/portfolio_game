using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaCtrl : MonoBehaviour
{
    bool m_usedBettleArea = false;

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.gameObject.CompareTag("Player"))
        {
            if (!m_usedBettleArea)
            {
                m_usedBettleArea = true;
                StartBettle();
            }
        }
    }
    void StartBettle()
    {
        CreateBettleWall();
        MonsterManager.Instance.CreateMonster();
    }
    void CreateBettleWall()
    {
        var obj = gameObject.transform.GetChild(0);
        obj.gameObject.SetActive(true);
    }
    void DeleteBettleWall()
    {
        var obj = gameObject.transform.GetChild(0);
        obj.gameObject.SetActive(false);
    }
}
