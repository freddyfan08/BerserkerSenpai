using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int MaxHp;//最大血量
                     //public bool Pushed;//擊退判定
                     //public bool Floated;//浮空判定

    public float JumpForce;
    [Header("死亡演出時間")]
    public float DeathANIMEtime;
    public bool Activable, Die, FallDown;//可主動控制，死亡，無敵狀態
    public int GetATKinfo;//接收主角攻擊訊息
    public int Hp;
    float 無敵時間, FloatedInfo;
    public bool Untouchable, Onhit, Brustfloat;
    [Header("自身抗擊倒值上限")]
    public int MaxTK;
    private int TkInfo, NowTK, AttackAxial;
    private float Tkcd;
    private float HitSide,PushedInfo;

   



    //跳躍
    [Header("感應地板的距離")]
    [Range(0, 0.5f)]
    public float Distance;

    [Header("偵測地板的射線起點")]
    public Transform Groundcheck;
    [Header("地面圖層")]
    public LayerMask GroundLayer;
    public bool Grounded;
    bool JumpCheck()
    {


        Vector2 start = Groundcheck.position;
        Vector2 end = new Vector2(start.x, start.y - Distance);
        Debug.DrawLine(start, end, Color.red);
        Grounded = Physics2D.Linecast(start, end, GroundLayer);
        return Grounded;

    }


    // Use this for initialization
    void Start()
    {

      

        #region 調用區塊


        #endregion
        Die = false;
        Hp = MaxHp;
        
        無敵時間 = 0;
        Untouchable = false;
        NowTK = MaxTK;
        Brustfloat = false;
        Activable = true;
     
        //0823 攻擊接觸修正之調用(作廢)
        //InvokeRepeating("OnTriggerEnter2D", 0f, 2f);

    }
    
    // Update is called once per frame
    void Update()
    {
        //===================        
        #region 來自其他腳本的變數

        #endregion
        //===================

        //0818 調用方法
        #region 調用區塊
        JumpCheck();
        LifeInfo();
        受擊參數計算();
        無敵時間計算();
        #endregion
        if (Die)
        {
            Debug.Log(gameObject.name + "死亡");
            return;
        }






    

    }

    //生命控制
    public void 無敵時間計算()
    {
        //無敵狀態 PS:只能用於受擊
        if (無敵時間 > 0)
        {
            Onhit = false;
            Untouchable = true;
            無敵時間 = 無敵時間 - Time.deltaTime;
        }
        if (無敵時間 <= 0)
        {
            Untouchable = false;

        }
    }
    public void LifeInfo()
    {
        if (Hp <= 0)
        {
            Die = true;
        }
        if (Die)
        {
            Untouchable = true;
            if (Grounded && Die)
            {
                Activable=false;
                //不讓被打倒的敵人阻擋前進，鎖定位置、關閉碰撞器
                GetComponent<Rigidbody2D>().velocity = Vector2.down * 0;//修正會被主角打到地板以下的BUG
                GetComponent<Rigidbody2D>().velocity = Vector2.up * 0;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                GetComponent<CapsuleCollider2D>().enabled = false;
                Destroy(gameObject, DeathANIMEtime);
                LevelCheck.Kill++;
             
            }
        }
    }

    //ATKinfo取得資訊
    public void OnTriggerEnter2D(Collider2D col)
    {
       // LifeInfo();

        if (!col.CompareTag("MyAttack"))
        {
            return;
        }



        if (col.transform.tag == "MyAttack")
        {
            GetATKinfo = col.GetComponent<PlayerAttack>().ATK;//攻擊資訊
            FloatedInfo = col.GetComponent<PlayerAttack>().RisePower;//打上資訊
            TkInfo = col.GetComponent<PlayerAttack>().TkInfo;//擊倒值資訊 0911
            PushedInfo= col.GetComponent<PlayerAttack>().PushPoewr;//推推資訊0915
            HitSide = (Mathf.Sign(transform.position.x - col.transform.position.x));//10/21
            if (Untouchable == false && Onhit == false)//0825 Onhit=用來避免連續偵測碰撞器的布林開關
            {
                Onhit = true;
                
                無敵時間 += 0.2f;//Utabcd 無敵時間
 
            }

            if (col.transform.name == "AirStartJump")//0915
            {
              Brustfloat=true;
            }

        }
        //Brustfloat=打上跳躍時，為了突破空中受擊限制增加的新規則

    }
    private void 受擊參數計算()
    {
        var ATK = GetATKinfo;
        var Float = FloatedInfo;
        var Push = PushedInfo;
        var Side = HitSide;
        var TK = TkInfo;

        if (!(ATK == 0))
        {
            Hp = Hp - ATK;
            GetATKinfo = 0;
        }
        if (!(TK == 0))
        {
            NowTK = NowTK - TK;
            TkInfo = 0;
        }

        if (Grounded)
        {
       
            if (!(Float == 0))
            {
                Debug.Log("C");
                GetComponent<Rigidbody2D>().AddForce(transform.up * Float);
                FloatedInfo = 0;
            }
            if (!(Push == 0))
            {
                GetComponent<Rigidbody2D>().AddForce(transform.right * Push * Side);
                PushedInfo = 0;
                HitSide = 0;
            }

        }
        if(!Grounded)
        {
            if (!(Float == 0))
            {
                Debug.Log("A");
                if (Float > 0)
                   
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * 12;
                    if (Brustfloat)
                    {
                        GetComponent<Rigidbody2D>().velocity = Vector2.up * 20;
                        Brustfloat = false;
                    }
                    Debug.Log("B");
                    FloatedInfo = 0;
                }
                if (Float < 0)

                {

                    GetComponent<Rigidbody2D>().velocity = Vector2.up * -30;
                    FloatedInfo = 0;
                }
                if (!(Push == 0))
                {
                    GetComponent<Rigidbody2D>().AddForce(transform.right * Push * Side*0.5f);//0.5是調整
                    PushedInfo = 0;
                    HitSide = 0;
                }

            }
        }//假設騰空的情況 給與受限的力(此腳本自行調整)
        
           
        }





    public void FallDownFunction()   //擊倒設定、爬起
    {
        //擊倒判定
        if (NowTK >= MaxTK)
        {
            FallDown = true;
        }

        //被擊倒時
        if (FallDown)//如果被擊倒
        {
            Activable = false;//不可動
            Tkcd = 1.5f;//如果倒地，會躺多久?

            if (Grounded) //如果被擊倒且著地
            {
                Untouchable = true;//倒地不可被攻擊

            }

        }
        //起身時間
        if (Tkcd > 0 && Grounded)
        {
            Tkcd = Tkcd - Time.deltaTime;
        }
        if (Tkcd <= 0)
        {
            FallDown = false;//躺完站起

        }
    }




}

