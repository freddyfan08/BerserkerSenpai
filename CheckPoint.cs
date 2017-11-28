using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour {
    public int 碰撞器編號;
    public bool 抵達;
	// Use this for initialization
	void Start () {
        抵達 = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            return;
        }
        
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            return;
        }
        if(抵達==false)
        {
            抵達 = true;
            Debug.Log("抵達 "+ 碰撞器編號+" 號檢查點");
        }
    }
}

