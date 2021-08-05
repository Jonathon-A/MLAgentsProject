using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class MoveToGoalAgentPositions : Agent
{
    private Rigidbody RB;
    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(5 + Random.Range(-10,10), 0.5f, 2 + Random.Range(-1, 10)); 
        TargetTransform.localPosition = new Vector3(5 + Random.Range(-10, 10), 0.5f, 24 + Random.Range(-10, 1));
        RB.velocity = Vector3.zero;
    }

    public Transform TargetTransform;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(TargetTransform.localPosition);
    }
    public float MoveSpeed;
    public override void OnActionReceived(ActionBuffers actions)
    {
        float MoveX = actions.ContinuousActions[0];
      
        float MoveZ = actions.ContinuousActions[1];
     //   Debug.Log("c1 " + actions.ContinuousActions[0]);
     //  Debug.Log("c2 " + actions.ContinuousActions[1]);
      
        transform.localPosition += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * MoveSpeed;
      
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> ContinuousActions = actionsOut.ContinuousActions;
        ContinuousActions[0] = Input.GetAxisRaw("Horizontal");
       
          //  ContinuousActions[1] = 1;

            ContinuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public Material WinMat;
    public Material LossMat;
    public MeshRenderer Floor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {

            SetReward(10000f);
            
            Floor.material = WinMat;
            EndEpisode();
        }
        if (other.CompareTag("Border"))
        {
            SetReward(-1000f);
           
            Floor.material = LossMat;
            
            EndEpisode();
        }

    }

    
}