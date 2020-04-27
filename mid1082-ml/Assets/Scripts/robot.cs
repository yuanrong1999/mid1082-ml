using UnityEngine;
using MLAgents;
using MLAgents.Sensors;


public class robot : Agent
{
    public Collider colrobot;
    [Header("速度"), Range(1,50)]
    public float speed = 10;

    /// <summary>
    /// 機器人剛體
    /// </summary>
    private Rigidbody rigrobot;
    /// <summary>
    /// 雞剛體
    /// </summary>
    private Rigidbody rigchicken;

    private void Start()
    {
        rigrobot = GetComponent<Rigidbody>();
        rigchicken = GameObject.Find("雞").GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 事件開始時，重新設定機器人與足球之Position
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //重設加速度&角度加速度避免滑行
        rigrobot.velocity = Vector3.zero; //機器人加速度歸零
        rigrobot.angularVelocity = Vector3.zero; //機器人角度加速度歸零
        rigchicken.velocity = Vector3.zero; //雞加速度歸零
        rigchicken.angularVelocity = Vector3.zero; //雞角度加速度歸零

        Vector3 posrobot = new Vector3(Random.Range(-2f, 2f), 0.1f, Random.Range(-2f, 0f)); //隨機生成機器人座標
        transform.position = posrobot; //將隨機生成之座標指定給機器人

        Vector3 poschicken = new Vector3(Random.Range(-2f, 2f), 0.1f, Random.Range(-2f, 0f)); //隨機生成雞座標
        rigchicken.position = poschicken;

        chicken.catched = false; //事件開始時，雞尚未被抓到
    }

    /// <summary>
    /// 收集觀測資料
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        //加入觀測資料：機器人、雞的座標；機器人加速度：x、z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigchicken.position);
        sensor.AddObservation(rigrobot.velocity.x);
        sensor.AddObservation(rigrobot.velocity.z);
    }

    /// <summary>
    /// 動作：控制機器人與回饋
    /// </summary>
    public override void OnActionReceived(float[] vectorAction)
    {
        //使用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0]; //接收到的第一筆資料
        control.z = vectorAction[1]; //接收到的第二筆資料
        rigrobot.AddForce(control * speed);

        //雞被抓到，成功：加1分並結束
        if(chicken.catched )
        {
            SetReward(1);
            EndEpisode();
        }
        //機器人掉落，失敗：扣1分並結束
        if(transform.position.y<0 || rigchicken.position.y<0)
        {
            SetReward(1);
            EndEpisode();
        }

    }

    /// <summary>
    /// 探索：Unity官方提供開發者測試環境
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        //提供開發者控制方式：前後左右
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
