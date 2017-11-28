using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalcheck : MonoBehaviour {
    [Header("抵達的檢查點數量")]
    public int Passed;
    public bool Checking;
	// Use this for initialization
	void Start () {
        Passed = 0;
        Checking = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GoalChecker()
    {

    }
    public void OnTriggerEnter2D(Collider2D col)
    {
       
        if (!col.CompareTag("CheckPoint"))
        {
            return;
        }
        Checking = col.GetComponent<CheckPoint>().抵達;
        if (Checking==false)
        {
            Passed++;
        }
    }
}
