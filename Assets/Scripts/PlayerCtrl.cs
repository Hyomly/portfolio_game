using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region [Constants and Fields]
    Vector3 m_dir;
    float m_speed = 4;
    int hash_Move;
    PlayerAniCtrl m_animCtrl;
    SkillCtrl m_skillCtrl;
    //CharacterController m_charCtrl;
    GameObject m_battleArea;

    #endregion [Constants and Fields]

    #region [Public  Properties]
    PlayerAniCtrl.Motion GetMotion { get { return m_animCtrl.GetMotion; } }  //current Motion

    #endregion [Public Properties]

    #region [Animation Event Methods]

    void AnimEvent_Attack()
    {
       
    }

    void AnimEvent_AttackFinished()
    {
        bool isCombo = false;
        if(m_skillCtrl.CommandCount > 0)
        {
            m_skillCtrl.GetCommand();
            isCombo = true; 
            if(m_skillCtrl.CommandCount > 0)
            {
                m_skillCtrl.ClearKeyBuffer();
                isCombo = false;
            }
        }
        if(isCombo)
        {
            m_animCtrl.Play(m_skillCtrl.GetCombo());
        }
        else
        {
            m_skillCtrl.ResetCombo();
            m_animCtrl.Play(PlayerAniCtrl.Motion.Idle);
        }
       
    }
    #endregion [Animation Event Methos]

    #region [Methods]

   /* private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        //trigger
        if (hit.gameObject.CompareTag("BattleArea"))
        {
            m_battleArea = hit.gameObject;
            var obj = m_battleArea.transform.GetChild(0);
            obj.gameObject.SetActive(true);
        }
    }*/
   /* private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BattleArea"))
        {
            m_battleArea = other.gameObject;
            var obj = m_battleArea.transform.GetChild(0);
            obj.gameObject.SetActive(true);
        }
    }*/
    IEnumerator CoCreateBettleBarrier(GameObject hit)
    {

        yield return null;
    }
    #endregion [Methods]

    #region [Unity Methods] 
    void Start()
    {
        m_animCtrl = GetComponent<PlayerAniCtrl>();
        m_skillCtrl = GetComponent<SkillCtrl>();
        //m_charCtrl = GetComponent<CharacterController>();
        hash_Move = Animator.StringToHash("IsMove");
    }

    void Update()
    {
        //Attack Combo
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(GetMotion == PlayerAniCtrl.Motion.Idle || GetMotion == PlayerAniCtrl.Motion.Walk)
            {
                m_animCtrl.Play(PlayerAniCtrl.Motion.Attack1);
            }
            else
            {
                m_skillCtrl.AddCommand(KeyCode.Space);
            }
        }
       

        //Move Charactor
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(m_dir != Vector3.zero)
        {
           
            m_animCtrl.SetBool(hash_Move, true);
            transform.forward = m_dir;
        }
        else
        {
            m_animCtrl.SetBool(hash_Move, false);
        }
        transform.position += m_dir * m_speed  * Time.deltaTime;
        //m_charCtrl.Move(m_dir * m_speed * Time.deltaTime);
    }
    #endregion [Unity Methods]



}
