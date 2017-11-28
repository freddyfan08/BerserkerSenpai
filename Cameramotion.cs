using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramotion : MonoBehaviour
{

    public GameObject PlayerObject;
    [Header("攝影機速率")]
    public float CameraSpeed;
    bool Dash;
    float DashCameraSP;

    // Use this for initialization
    void Start()
    {
        Dash = false;
        DashCameraSP = 1;

    }

    // Update is called once per frame
    void Update()
    {

      

        transform.position = Vector3.Lerp(transform.position, PlayerObject.transform.position - Vector3.forward * 10, Time.deltaTime * CameraSpeed * DashCameraSP);
        if (Dash == false)
        {
            DashCameraSP = 1;
        }
        if (Dash)
        {
            DashCameraSP = 1.5F;









        }
        //取得衝刺資訊

    }
}
