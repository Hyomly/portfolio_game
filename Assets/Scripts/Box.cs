using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    int m_boxHp = 10;

    void SetDamage(int damage )
    {
        m_boxHp -= damage;
        if( m_boxHp <= 0 )
        {
            Destroy(gameObject);
        }
    }
       
    void OntriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {

        }

    }
}
