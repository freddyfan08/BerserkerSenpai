using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour {
    
    /*
    =========================================================================================================================== 
    (10/06)為了方便管理，此腳本僅用於描述、執行動作，不可限制動作(硬直、無法操作的狀態等)、發出指令，
    那是EnemyAI工作範疇。    
    ===========================================================================================================================
    */
    
    //變數宣告
    #region
    bool GoingLeft,GoingRight,Jumping,Attacking;
    [Header("跳躍力")]
    public float JumpForce;
    [Header("移動速度")]
    public float MoveSpeed;
    [Header("發射碰撞器的力道")]
    public float AttackForce;
    [Header("碰撞器生存時間")]
    public float BulletLifeTime;
    public GameObject Attack1,Attack2;
    public bool FaceToRight;
    private float Ox, Oy, Oz, Nx;
    #endregion
    // Use this for initialization
    void Start ()
    {
        #region 調用區塊
        轉向設定START();

        #endregion
    }

    // Update is called once per frame
    void Update () {
        //=================== 
        #region 來自其他腳本的變數
        //===================
        Jumping = GetComponent<EnemyAI>().ToJump;
        GoingLeft = GetComponent<EnemyAI>().GoLeft;
        GoingRight = GetComponent<EnemyAI>().GoRight;
        Attacking = GetComponent<EnemyAI>().ToAttack;
        #endregion

        #region 調用區塊
        轉向設定ING();
        Jump();
        Walk();
        Attack();
        #endregion
    }
    private void 轉向設定START()
    {
        Ox = transform.localScale.x;//取得原Scale值
        Oy = transform.localScale.y;
        Oz = transform.localScale.z;
        Nx = -Ox;
    }
    public void 轉向設定ING()
    {


        if (FaceToRight == false)
        {
            transform.localScale = new Vector3(Nx, Oy, Oz);//向左乘以-1
            
        }
        if (FaceToRight == true)
        {
            transform.localScale = new Vector3(Ox, Oy, Oz);
            
        }
    }
    private void Jump()
    {
        
        if (Jumping)
        {
           // Debug.Log("J");
            GetComponent<Rigidbody2D>().AddForce(transform.up * JumpForce);
            Jumping = false;
        } 
    }
    private void Walk()
    {
        
        if (GoingLeft)
        {
            FaceToRight = false;
            transform.Translate(transform.right * MoveSpeed * -Time.deltaTime);
            //transform.eulerAngles = new Vector3(0, 180, 0);//跟著歐拉角180度 10/15因受力問題停用
        }
        
        if (GoingRight)
        {
            FaceToRight = true;
            transform.Translate(transform.right * MoveSpeed * Time.deltaTime);
           // transform.eulerAngles = new Vector3(0, 0, 0);//跟著歐拉角0度
        }
        if (GoingLeft && GoingRight)
        {
            GoingLeft = false;
            GoingRight = false;
        }
    }
    private void Attack()
    {
       
        if (Attacking)
        {
            var go1 = Instantiate(Attack1, transform.position, Quaternion.identity) as GameObject;
            go1.GetComponent<Rigidbody2D>().AddForce(transform.right * AttackForce);
            Destroy(go1, BulletLifeTime);
        }
    }


}
