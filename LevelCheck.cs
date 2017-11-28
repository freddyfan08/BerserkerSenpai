using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCheck : MonoBehaviour {
    #region 變數宣告
    enum tags
    {
        Enemy,Player,CheckPoint
    }
    //static:靜態變數，當一個變數在整個遊戲當中只有一個值時可以使用，可以在其他腳本中直接運算不需另外偵測。
    [Header("場景中剩餘的敵人")]
    public static int EnemiesCount;//ok
    [Header("關卡敵人總數")]
    public static int TotalEnemies;
    [Header("殺敵數")]
    public static int Kill;//ok 已繫結
    [Header("經過的檢查點")]
    public static int CheckPointHaveBePass;

    #endregion
    #region 方法區
   
    public void ReloadThisScene()
    {
        Scene NowScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(NowScene.name);
    } //重新讀取場景
    public void HowManyEnemyHere()
    {
        EnemiesCount = GameObject.FindGameObjectsWithTag(tags.Enemy.ToString()).Length;
        
    }
    public static bool AllKilled
    {
        get
        {
            if (Kill >= TotalEnemies && EnemiesCount<=0)
            {
                
                return true;
            }
           
            return false;
           
        }
    }//是否全部殺光
    
    #endregion
    // Use this for initialization
    void Start ()
    {
       //取得遊戲物件:int=遊戲物件.尋找(陣列).數量
        
      
    }
	
	// Update is called once per frame
	void Update () {
        #region 調用區
        HowManyEnemyHere();
        #endregion
    }
}
