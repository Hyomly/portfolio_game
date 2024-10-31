using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterCtrl;


public class MonsterCtrl : MonoBehaviour
{
    #region [Constants and Fields]
    public enum AIState
    {
        Idle,
        Attack,
        Chase,
        Damage,
        Max
    }
    [SerializeField, Header("AI Á¤º¸")]
    AIState m_state = AIState.Idle;

    [SerializeField]
    PlayerCtrl m_player;
    [SerializeField]
    MonsterAniCtrl m_monAniCtrl;
    NavMeshAgent m_navAgent;
    [SerializeField]
    float m_idleDuration = 5f;
    [SerializeField]
    float m_idleTime;
    [SerializeField]
    float m_detectDist;
    [SerializeField]
    float m_attackDist = 1.5f;

    #endregion [Constants and Fields]

    #region [Public Properties]
    public MonsterManager.MonsterType Type { get; set; }


    #endregion [Public Properties]

    #region [Animation Event Methods]



    #endregion [Animation Event Methos]

    #region [Methods]
    public void InitMonster(PlayerCtrl player)
    {
        m_player = player;
    }

    IEnumerator CoChaseToTarget(Transform target, int frame)
    {
        while (m_state == AIState.Chase)
        {
            m_navAgent.SetDestination(target.position);
            for (int i = 0; i < frame; i++)
            {
                yield return null;
            }
        }
    }

    bool CheckAttackArea(Vector3 targetPos, float dist)
    {
        var dir = targetPos - transform.position;
        if (dir.magnitude <= dist * dist)
        {
            return true;
        }
        return false;
    }
    #endregion [Methods]

    bool FindTarget(Transform target, float dist)
    {
        var start = transform.position + Vector3.up * 0.7f;
        var end = target.position + Vector3.up * 0.7f;
        RaycastHit hit;
        if (Physics.Raycast(start, (end - start).normalized, out hit, 1 << LayerMask.NameToLayer("Player")))
        {
            Debug.DrawRay(start, (end - start).normalized * hit.distance, Color.magenta, 0.5f);
            if (hit.transform.tag.Equals("Player"))
            {
                return true;
            }
        }
        else
        {
            Debug.DrawRay(start, (end - start).normalized * dist, Color.cyan, 0.5f);
        }
        return false;

    }

    void SetState(AIState state)
    {
        m_state = state;

    }
    void BehaviorProcess()
    {
        switch (m_state)
        {
            case AIState.Idle:
                if (m_idleTime > m_idleDuration)
                {
                    m_idleTime = 0f;

                    if (CheckAttackArea(m_player.transform.position, m_attackDist))
                    {
                        SetState(AIState.Attack);
                        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Attack);
                        return;
                    }
                    SetState(AIState.Chase);
                    m_monAniCtrl.Play(MonsterAniCtrl.Motion.Walk);
                    m_navAgent.stoppingDistance = m_attackDist;
                    StartCoroutine(CoChaseToTarget(m_player.transform, 30));
                    return;
                }
                m_idleTime += Time.deltaTime;
                break;
            case AIState.Attack:
                break;
            case AIState.Chase:
                if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                {
                    SetIdle(0.5f);
                }
                break;
            case AIState.Damage:
                break;
        }
    }
    void SetIdleDuration(float duration)
    {
        m_idleTime = m_idleDuration - duration;
    }
    void SetIdle(float duration)
    {
        SetState(AIState.Idle);
        SetIdleDuration(duration);
        m_monAniCtrl.Play(MonsterAniCtrl.Motion.Idle);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackDist);

    }
    #region [Unity Methods] 
    void Start()
    {
        m_monAniCtrl = GetComponent<MonsterAniCtrl>();
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        BehaviorProcess();
    }
    #endregion [Unity Methods]



}
