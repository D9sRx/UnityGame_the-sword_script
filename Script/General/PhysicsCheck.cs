using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("���״̬")]
    public bool isGround;
    [Header("���״̬")]
    public float checkRaduis;
    public LayerMask groundLayer;
    public Vector2 bottomOffet;

    private void Update()
    {
        Check();
    }
    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffet,checkRaduis,groundLayer);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffet, checkRaduis);
    }
}


