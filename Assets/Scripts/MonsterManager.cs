using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    public enum MonsterType
    {
        Slime,
        Max
    }
    [SerializeField]
    PlayerCtrl m_player;
    [SerializeField]
    Transform[] m_spwanPos;
    [SerializeField]
    GameObject m_monsterPrefab;

    Dictionary<MonsterType, GameObjectPool<MonsterCtrl>> m_monsterPool = new Dictionary<MonsterType, GameObjectPool<MonsterCtrl>>();

    public void CreateMonster()
    {
        for(int i = 0; i < m_spwanPos.Length; i++)
        {
            MonsterType type = (MonsterType)Random.Range((int)MonsterType.Slime, (int)MonsterType.Max);
            var mon = m_monsterPool[type].Get();
            mon.gameObject.SetActive(true); 
            mon.transform.position = m_spwanPos[i].transform.position;
        }
    }

   
    // Start is called before the first frame update
    protected override void OnStart()
    {
        MonsterType type = MonsterType.Slime;
        var pool = new GameObjectPool<MonsterCtrl>(3, () =>
        {
            var obj = Instantiate(m_monsterPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var mon = obj.GetComponent<MonsterCtrl>();
            mon.Type = type;
            mon.InitMonster(m_player);
            return mon;
        });
        m_monsterPool.Add(type, pool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
