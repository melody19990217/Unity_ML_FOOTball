using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Robot : Agent
{
    [Header("速度"), Range(1, 1000)]
    public float speed = 100;

    private Rigidbody rigRobot;

    private Rigidbody rigBall;

    private void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = GameObject.Find("足球").GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        Vector3 posRobot = new Vector3(Random.Range(-1f, 1f), 0.1f, Random.Range(-2f, 1.2f));
        transform.position = posRobot;

        Vector3 posBall = new Vector3(0, 0.1f, Random.Range(-1f, 2f));
        rigBall.position = posBall;

        Ball.complete = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 contorl = Vector3.zero;
        contorl.x = vectorAction[0];
        contorl.z = vectorAction[1];
        rigRobot.AddForce(contorl * speed);

        if(Ball.complete)
        {
            SetReward(1);
            EndEpisode();
        }

        if(transform.position.y<0 || rigBall.position.y<0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        //提供開發者控制的方式
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");

    }
}
