using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [Header("攻擊力")]
    public int ATK;
    [Header("上昇力")]
    public float RisePower;
    [Header("擊飛力")]
    public float PushPoewr;
    [Header("擊退力")]
    public int TK;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   /* private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            return;
        }

        if (col.transform.tag == ("Player"))
        {
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(PushPoewr * Mathf.Sign(col.transform.position.x - transform.position.x), 0));
        }
    }*/
}
