using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [Header("攻擊力")]
    public int ATK;
    [Header("抬升力")]
    public float RisePower;
    [Header("推力")]
    public float PushPoewr;
    [Header("擊倒值")]
    public int TkInfo;
   
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}
    /*
    private void OnTriggerEnter2D(Collider2D col)
    {
        

        if (col.transform.tag == ("Enemy"))
        {
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(PushPoewr * Mathf.Sign(col.transform.position.x - transform.position.x), 0));
        }
    }
    */
}
