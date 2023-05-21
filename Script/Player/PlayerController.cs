using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;
    private Vector2 originalOffset;
    private Vector2 originalSize;   
    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f ;
    public bool isCrouch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();   
        originalOffset = coll.offset;
        originalSize = coll.size;
        inputControl = new PlayerInputControl();

        inputControl.Gameplay.Jump.started += Jump;// += 为注册，即将jump方法给到start触发器。
        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButtom.performed += ctx =>
        {
            if (physicsCheck.isGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.WalkButtom.canceled += ctx =>
        {
            if (physicsCheck.isGround)
                speed = runSpeed;
        };
        #endregion 

    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
        
    }
    public void Move()
    {
        if (!isCrouch)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
        //人物翻转
        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x < 0)
            faceDir = -1;
        if (inputDirection.x > 0)
            faceDir = 1;

        transform.localScale = new Vector3(faceDir,1,1);

        isCrouch = inputDirection.y < -0.1 && physicsCheck.isGround;
        if (isCrouch)
        {
            //改变碰撞体的参数
            coll.offset = new Vector2(-0.08129874f, 0.754f);
            coll.size = new Vector2(0.7113967f, 1.51f);
            rb.velocity = new Vector2(inputDirection.x * 0 * Time.deltaTime, rb.velocity.y);//修复了先按走路再按蹲还会移动的bug

        }
        else
        {
            //还原碰撞体参数
            coll.offset = originalOffset;
            coll.size = originalSize;
        }


    }
    private void Jump(InputAction.CallbackContext obj)//角色跳跃
    {
        if (physicsCheck.isGround)
        rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
    }
}
