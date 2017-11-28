using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    //宣告區
    #region
        [Header("玩家物件")]
    public GameObject Player;
    [Header("我是否生存?")]
    public bool Alife;
    [Header("我是否能動?")]
    public bool Activable;
    /*[Header("牆壁偵測器")]
    public GameObject Detector;*/
    [Header("多近開始追蹤?")]
    public float MaxSight;
    [Header("保持多少距離?")]
    public float KeepDis;
    public bool ToJump, GoLeft, GoRight,ToAttack;
    private int PlayerAxis,MyAxis;
    public bool Tofollow,GroundedAI;
    public float 距離;
    private float JumpCD;
  
    #endregion
    #region 
    //牆壁偵測功能 10/22
    [Header("偵測牆壁的射線起點")]
    public Transform 牆壁偵測器;
    [Header("地面圖層")]
    public LayerMask GroundLayer;
    public bool 被阻擋;
    [Header("感應牆壁的距離")]
    [Range(0, 3f)]
    public float 牆壁偵測器範圍;
    bool 牆壁偵測()
    {
        Vector2 start = 牆壁偵測器.position;
        Vector2 end = new Vector2(start.x- 牆壁偵測器範圍*MyAxis*-1, start.y);
        Debug.DrawLine(start, end, Color.blue);
        被阻擋 = Physics2D.Linecast(start, end, GroundLayer);
        return 被阻擋;

    }
    #endregion
    // Use this for initialization

    void Start () {

        #region 調用區塊

        #endregion
        
        Activable = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //===================  
        #region 來自其他腳本的變數
        GroundedAI = GetComponent<Enemy>().Grounded;
        Activable = GetComponent<Enemy>().Activable;
        Alife = !GetComponent<Enemy>().Die;
        #endregion
        //===================

        //判斷區 類別繼承 用來偵側狀態，即使在自身不可動的狀態 10/14
        #region
        牆壁偵測();
        PlayerSide();
        距離 = Vector2.Distance(Player.transform.position, transform.position);
        FaceToForAI();
        計時器();
        #endregion
        //執行區 類別繼承 用來執行、限制操作 必須Activable
        #region
        if (Activable)
        {
            Walking();
            Jump();
        }
        #endregion
       

    }
    //判斷區 類別
    private void PlayerSide()
    {
        if (Player.transform.position.x - transform.position.x > 0)
        {
            PlayerAxis = 1;
            
        }
        else
        {
            PlayerAxis = -1;
           
        }
        //正常執行
    }
    private void 計時器()
    {
        if (JumpCD > 0)
        {
            JumpCD -= Time.deltaTime;
        }
        if (JumpCD <= 0)
        {
            JumpCD = 0;
        }
    }
    //執行區 類別
    public void Walking()
    {
        var Distance = Player.transform.position - transform.position;
        var DistanceInX = Mathf.Abs(Distance.x);

        if (GroundedAI)//要在地板上才能追擊
        { 
        if (DistanceInX < MaxSight)
        {
            if (DistanceInX > KeepDis)
            {
                Tofollow = true;
            }

        }
        if (DistanceInX >= MaxSight || DistanceInX <= KeepDis)
        {
            Tofollow = false;
        }
        }

            if (Tofollow)//當開始追擊
        {
            if (PlayerAxis > 0)
            {
                GoRight=true;
                GoLeft = false;
            }
            if (PlayerAxis < 0)
            {
                GoLeft = true;
                GoRight = false;
            }
            
        }
            if (Tofollow == false)
            {
                GoLeft = false;
                GoRight = false;
            }

    }
    public void FaceToForAI()
    {
        var A = transform.localScale.x;
        if (A > 0)
        {
            MyAxis = 1;
        }
        if (A < 0)
        {
            MyAxis = -1;
        }
    }
    public void Jump()
    {

        if (被阻擋 && GroundedAI && JumpCD == 0)
        {
            //Debug.Log("AIJ");
            ToJump = true;       
        }
        else
        {
            ToJump = false;
        }
        if (!GroundedAI)
        {
            ToJump = false;
        }
        if (ToJump)
        {
            //Debug.Log(this.gameObject.name+"跳跳");
            JumpCD = 0.5f;
        }
      
    }
    
}
