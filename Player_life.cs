using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_life : MonoBehaviour {
    public int Hp, MaxHp,GetATKinfo,TkInfo,MaxTK,NowTK;
    public bool ActivateAble, Untouchable,Die,被擊倒,倒地;
    public float 強制硬直, DeathAnimeTime,FloatedInfo,PushedInfo,無敵時間;
    private bool 被打,Grounded;
    private float HitSide;
    public  float 被擊倒的時間;
    public class 物件資訊
    {
        //屬性
        public string 名稱;
        public string 種類;
        public int 物品編號;
    

    }
    // Use this for initialization
    void Start () {
        物件資訊 玩家 = new 物件資訊();
        玩家.名稱 = "Senpai";
        玩家.種類 = "玩家";
        玩家.物品編號 = 001;
          
        Hp = MaxHp;
        NowTK = MaxTK;
        Die = false;
        被打 = false;
        ActivateAble=true;
        #region 調用區塊
        Death();
#endregion
    }
	// Update is called once per frame
	void Update () {
        #region 調用區塊
        if (Untouchable == false)//如果不是無敵才算被打到 10/20
        {
            受擊參數計算();
        }
        JumpCheck();
        時間計算();//10/21 因為跟倒地時間有相互關係，之後可能會在控制權上出錯。 包含無敵、硬直等時間計算
        擊倒();
        受擊硬直();

        #endregion
    }

    //狀態(跳躍)判斷
    public void JumpCheck()
    {
        Grounded = GetComponent<player>().Grounded;//我的變數Grounded=找到物件的類別為<player>的()變數Grounded。
    }
    
    //受擊
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("EnemyAttack"))
        {
            return;
        }
        GetATKinfo= col.GetComponent<EnemyAttack>().ATK;//攻擊資訊
        FloatedInfo = col.GetComponent<EnemyAttack>().RisePower;
        PushedInfo = col.GetComponent<EnemyAttack>().PushPoewr;
        TkInfo = col.GetComponent<EnemyAttack>().TK;
        HitSide = Mathf.Sign(transform.position.x - col.transform.position.x);
        強制硬直 += 0.2f; 
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

        if (Grounded)
        {
            if (!(Float == 0))
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * Float);
                FloatedInfo = 0;
            }
            if (!(Push == 0))
            {
                GetComponent<Rigidbody2D>().AddForce(transform.right * Push * Side);
                PushedInfo = 0;
                HitSide = 0;
            }
            if (!(TK == 0))
            {
                NowTK = NowTK - TK;
                TkInfo = 0;
            }
        }

    

    }//10/20
    public void 時間計算()
    {
        if (無敵時間 > 0)
        {
            Untouchable = true;
            無敵時間 -= Time.deltaTime;
        }
        if (無敵時間 <= 0)
        {
            無敵時間 = 0;
            Untouchable = false;  
        }
        if (強制硬直 > 0)
        {
            被打 = true;
            ActivateAble = false;
            強制硬直 -= Time.deltaTime;
        }
        if (強制硬直 <= 0)
        {
            被打 = false;
            ActivateAble = true;
            強制硬直 = 0;
           
        }
    }
    public void 擊倒()
    {
        if (NowTK >= MaxTK)
        {
            被擊倒 = true;
        }
        if (被擊倒)
        {
            ActivateAble = false;//無法操作
            if (Grounded)
            {
                倒地 = true;
            }
            if (倒地)
            {
                無敵時間=被擊倒的時間;
                if (被擊倒的時間 > 0)
                {
                    被擊倒的時間 -= Time.deltaTime;
                }
                if (被擊倒的時間 <= 0)//起身
                {
                    被擊倒的時間 = 0;//無敵時間跟著=0
                    NowTK = MaxTK;//擊倒值重置
                    倒地 = false;
                    被擊倒 = false;
                    ActivateAble = true;//操作重置
                }
                
            }
        }
    }
    public void 受擊硬直()
    {
        
    }
    private void Death()
    {
        if (Hp <= 0)
        {
            ActivateAble = false;
            Die=true;
        }
        if (Die && Grounded)
        {
            Destroy(gameObject, DeathAnimeTime);
        }
    }
}
