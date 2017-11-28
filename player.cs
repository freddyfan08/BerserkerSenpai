using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public class player : MonoBehaviour{
    public float Movespeed, DashForce;
    public float JumpForce,ComboDistance;
    public bool ActivateAble;//能操作的基本條件。
    public bool AttackAble;//僅用於能否攻擊的判斷，不干涉其他型為。
    public float 強制硬直;//專用於受擊、強制狀態等，非使用於攻擊。09/15
    public float 攻擊硬直;//用於攻擊的冷卻。 09/15
    float DashG,JumpCanDash, FirstATKTimer;
    public bool DashCheack, FirstAttack;
    private float 打下時間,打上時間,攻擊長押時間,UPT;
    private bool AtkAble,長押斷點,UPTB;
    public float DashCold;
    public bool FaceToRight;
    private float Ox, Oy, Oz,Nx;
    private Animator Ani;
    private int Ani_TypeA = Animator.StringToHash("Ani_TypeA");
    private int Ani_Jump = Animator.StringToHash("Jump");


   //attacking
   [Header("射出碰撞器的力道")]
    public float AttackForce;
    [Header("碰撞器的消失時間")]
    public float BulletLifeTime;
    public GameObject TypeA,TypeAEnd,AirEnd, AirStartJump, AirAtkStartPoint,Airstart,AirCombo;
    private int ComboCount;
    [Header("空中攻擊滯空")]
    public int ATKFloating;

    [Header("感應地板的距離")]
    [Range(0, 0.5f)]
    public float Distance;

    [Header("偵測地板的射線起點")]
    public Transform Groundcheck;
    [Header("地面圖層")]
    public LayerMask GroundLayer;
    public bool Grounded;
    bool JumpCheck {
     get{
            Vector2 start = Groundcheck.position;
            Vector2 end = new Vector2(start.x, start.y - Distance);
            Debug.DrawLine(start, end, Color.blue);
            Grounded = Physics2D.Linecast(start, end, GroundLayer);
            return Grounded;
        }
    }
    int XaixIDF;
    // Use this for initialization
    void Start()
    {
        //Ani=動畫控制器 11/14
        Ani = GetComponent<Animator>();
        
        FaceToRight = true;
        DashCheack = false;      
        ActivateAble = true;
        FirstAttack = true;
        FirstATKTimer = 0;
        ComboCount = 0;
        打下時間 = 0f;
        打上時間 = 0f;
        #region 調用方法區塊
        轉向設定START();
        #endregion

    }

    // Update is called once per frame

    void Update()
    {
        移動跳躍衝刺();
        打下();
        打上();
        攻擊設定();
        生命資訊();
        攻擊長押();
        轉向設定ING();// 10/19 add


        //已上調用

        //計時器相關變數在function Update 進行 09/15


        if (強制硬直 > 0)//如果硬直狀態>0秒
        {
            ActivateAble = false;
            強制硬直 = 強制硬直 - 1 * Time.deltaTime; //則每秒減去

        }
        if (強制硬直 <= 0)//如果硬直狀態<0秒，硬直歸零不為負數
        {
            強制硬直 = 0;
            ActivateAble = true;
        }

        if (攻擊硬直 > 0)
        {
            強制硬直 = 攻擊硬直;
            AttackAble = false;
            攻擊硬直 -= Time.deltaTime;
        }
        if (攻擊硬直 <= 0)
        {
            攻擊硬直 = 0;
            AttackAble = true;
        }
        

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
            transform.localScale = new Vector3(Nx,Oy,Oz);//向左乘以-1
            XaixIDF = -1;
        }
        if (FaceToRight == true)
        {
            transform.localScale = new Vector3(Ox, Oy, Oz);
            XaixIDF = 1;
        }
    }
    public void 移動跳躍衝刺()
    {

        if (ActivateAble)
        {
            var GoToRight = false;
            var GoToLeft = false;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(transform.right * Movespeed * Time.deltaTime);
                FaceToRight = true;//10/19 add

                // transform.eulerAngles = new Vector3(0, 0, 0);//跟著歐拉角0度 10/19 棄用
                if (Grounded)
                {
                    GoToRight = true;
                }
                
            }
            else
            {
                GoToRight = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(transform.right * Movespeed * -Time.deltaTime);

                //transform.eulerAngles = new Vector3(0, 180, 0);//跟著歐拉角180度 10/19 棄用
                FaceToRight = false;//10/19 add
                if (Grounded)
                {
                    GoToLeft = true;
                }
                
                
            }
            else
            {
                GoToLeft=false;
            }
            if (GoToLeft || GoToRight)
            {
                Ani.SetBool("running", true);
            }
            else
            {
                Ani.SetBool("running", false);
            }
            if (GoToLeft&&GoToRight)
            {
                Ani.SetBool("running", false);
            }

            //跳躍設置
            if (Input.GetKeyDown(KeyCode.Space) && JumpCheck == true && !Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * JumpForce);
                Grounded = false;//跳躍時無法在跳躍
                ComboCount = 0;
                強制硬直 += 0.03f;
                Ani.SetTrigger(Ani_Jump);

            }
            //
            if (Input.GetKeyDown(KeyCode.A) & DashCheack == false & JumpCheck == true && ActivateAble)//在壓住右鍵中按A衝刺
            {
                GetComponent<Rigidbody2D>().AddForce(transform.right * DashForce * XaixIDF);
                DashCold = 0.6f;

                強制硬直 = 0.6f;//06/21
            }

        }
       

        //衝刺冷卻時間設置
        if (DashCold>=0)
        {
            DashCheack = true;
            DashCold -= Time.deltaTime;

        }
        if (DashCold <= 0)
        {
            DashCheack = false;
            DashCold = 0;

        }
    }
    public void 攻擊設定()
    {


        //攻擊製作

        //地上
        if (ActivateAble && AttackAble)//ActivateAble 如果不在強制狀況、 AttackAble 非攻擊狀態(如攀爬)
        {

            if (Input.GetKey(KeyCode.D))//0828

            {

                if (Input.GetKeyDown(KeyCode.D))

                {
                    //連段A(DDDD)

                    if (Input.GetKeyDown(KeyCode.D) && ComboCount <= 3 && Grounded && !Input.GetKey(KeyCode.UpArrow))

                    {

                        if (ComboCount < 3)

                        {
                        var goA = Instantiate(TypeA, transform.position, Quaternion.identity) as GameObject;
                        goA.GetComponent<Rigidbody2D>().AddForce(transform.right * AttackForce * XaixIDF);
                        Destroy(goA, BulletLifeTime);
                        攻擊硬直 = 0.35f;//硬直時間
                        ComboCount++;
                            Ani.SetInteger("Ani_TypeA_Combo", 2);
                            Ani.SetTrigger(Ani_TypeA);
                        }
                    else
                    {
                            if (ComboCount==3)
                            {
                                Ani.SetInteger("Ani_TypeA_Combo",3);
                            }
                        var goAE = Instantiate(TypeAEnd, transform.position, Quaternion.identity) as GameObject;
                        goAE.GetComponent<Rigidbody2D>().AddForce(transform.right * AttackForce * XaixIDF);
                        Destroy(goAE, BulletLifeTime);
                        攻擊硬直 = 0.5f;//硬直時間
                        ComboCount = 0;
                            
                            Ani.SetTrigger(Ani_TypeA);
                        }
                        
                }
                
                //空中攻擊0727 0906加入打上、打下參考點AirAtkStartPoint
                

                    if (Input.GetKeyDown(KeyCode.D) && Grounded == false && ComboCount <= 3)
                    {
                        if (ComboCount < 3 && !Input.GetKey(KeyCode.DownArrow))
                        {
                            var goA = Instantiate(AirCombo, transform.position, Quaternion.identity) as GameObject;
                            goA.GetComponent<Rigidbody2D>().AddForce(transform.right * AttackForce*XaixIDF);
                            Destroy(goA, BulletLifeTime);
                            攻擊硬直 = 0.3f;//硬直時間
                            ComboCount++;
                            GetComponent<Rigidbody2D>().velocity = Vector2.up * ATKFloating;
                        }
                        else
                        {
                            打下時間 = (BulletLifeTime * 1.8f);
                            攻擊硬直 = 0.3f;//硬直時間
                            ComboCount = 0;
                            GetComponent<Rigidbody2D>().velocity = Vector2.up * -17;

                        }

                        if (Input.GetKey(KeyCode.DownArrow) && ComboCount < 3)

                        {
                            打下時間 = (BulletLifeTime * 1.8f);

                            攻擊硬直 = 0.7f;//硬直時間
                            ComboCount = 0;
                            GetComponent<Rigidbody2D>().velocity = Vector2.up * -17;

                        }

                    }


                

            }
                //打上攻擊
                if (Input.GetKey(KeyCode.UpArrow) && Grounded && DashCheack == false)
                {

                    if (攻擊長押時間 < 0.3f &&Input.GetKeyDown(KeyCode.D)&&UPTB==true)
                    {
                        var goAE = Instantiate(Airstart, AirAtkStartPoint.transform.position, Quaternion.identity) as GameObject;
                        goAE.GetComponent<Rigidbody2D>().AddForce(transform.up * AttackForce*0.5f);
                        Destroy(goAE, BulletLifeTime * 0.7f);
                        攻擊硬直 = 0.2f;
                        UPT = 0.7f;
                        ComboCount = 0;
                       
                    }
                    if (攻擊長押時間 >= 0.3f)
                    {

                        打上時間 = 打上時間 + (BulletLifeTime * 1.3f);
                        攻擊硬直 = 0.5f;//硬直時間
                        ComboCount = 0;
                        GetComponent<Rigidbody2D>().velocity = Vector2.up * 35;
                        長押斷點 = true;
                    }

                }

            }


            
        }
            //0828 衝刺攻擊
            if (AttackAble && Grounded)
            {
                if (Input.GetKeyDown(KeyCode.D) && DashCold>0 && DashCold<0.5f)
                {
                    var goAE = Instantiate(TypeAEnd, transform.position, Quaternion.identity) as GameObject;
                    goAE.GetComponent<Rigidbody2D>().AddForce(transform.right * AttackForce * XaixIDF);
                    Destroy(goAE, BulletLifeTime);
                    強制硬直 = 0.7f;//硬直時間
                    攻擊硬直 = 0.7f;
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * 20 * XaixIDF;    
                }
            }





        //連續技製作
        if (ComboCount == 0)//如果連擊數等於零
        {
            FirstAttack = true;//布靈變數"第一次攻擊"=是
            FirstATKTimer = 0;//計時器歸零
        }
        if (ComboCount > 0)//如果連擊數大於零
        {
            FirstATKTimer += Time.deltaTime;//計時器開始每秒累加
            FirstAttack = false;//布靈變數"第一次攻擊"=否
        }
        if (FirstATKTimer * 2 / ComboCount > ComboDistance)//時間內打出連擊
        {
            ComboCount = 0;
        }
        //Debug.Log("ComboCount" + ComboCount);
        //Debug.Log("MyComboDistant" + FirstATKTimer *2/ ComboCount);


    
}
    public void 打下()
    {
        if (打下時間 > 0)
        {
            AirEnd.SetActive(true);
            打下時間 -= Time.deltaTime;
        }
        if (打下時間 <= 0 || Grounded)
        {
            打下時間 = 0;
            AirEnd.SetActive(false);
        }
      
    }
    public void 打上()
    {
        if (打上時間 > 0)
        {
            AirStartJump.SetActive(true);
            打上時間 -= Time.deltaTime;
        }
        if (打上時間 <= 0 )
        {
            打上時間 = 0;
            AirStartJump.SetActive(false);
        }

    }
    public void 生命資訊()//0831 接收生命控制訊息包括HP生死、硬直
    {
        //攻擊硬直
       
        ActivateAble = GetComponent<Player_life>().ActivateAble;
        
    }
    public void 攻擊長押()
    {
        if (Input.GetKey(KeyCode.D)&&Grounded)
        {
            攻擊長押時間 += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            攻擊長押時間 = 0f;
        }
        if (長押斷點)
        {
            攻擊長押時間 = 0f;
        }
        if (Grounded)
        {
            長押斷點 = true;
            長押斷點 =false;
        }
        if (UPT > 0)//只決定打上(輕)的時間
        {
            UPTB = false;
            UPT -= Time.deltaTime;
        }
        if (UPT <= 0)
        {
            UPT = 0;
            UPTB = true;
        }
    }

class Timer
{   
    public float 時間,時限;
    public bool 變因;
    public void 加法浮點數計時器()
    {
        if (時間 < 時限)
        {
            變因 = false;
            時間 += Time.deltaTime;
        }
        if (時間 >= 時限)
        {
            變因 = true;
        }

            Debug.Log(時間);
    }
}
}

  