using UnityEngine;

public class chicken : MonoBehaviour
{
    /// <summary>
    /// 雞是否被抓到
    /// </summary>
    public static bool catched;
    private Rigidbody rigrobot;
    private void Start()
    {
        
        rigrobot = GameObject.Find("機器人").GetComponent<Rigidbody>();
    }


    /// <summary>
    /// 觸發開始物件：碰到機器人(is trigger)
    /// </summary>
    /// <param name="other"></param>
    /*private void OnTriggerEnter(Collider other)
    {
        if(other.name == "機器人")
        {
            catched = true;
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == rigrobot)
        {
            catched = true;
        }
    }

}
