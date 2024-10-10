using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class agentmove : Agent
{
    [SerializeField] float move_speed = 2f;
    Rigidbody rb;

    [SerializeField] gun gunObject;
    bool can_shoot, hit_target, has_shot = false;
    int time_until_next_bullet = 0;
    int min_time_until_next_bullet = 30;
    private Vector3 lastPosition;
    private float moveThreshold = 1.0f;

    [SerializeField] WorldBehaviors world_behaviors;
   

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        world_behaviors.spawnAgent();
        world_behaviors.callSpawnPolice();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        can_shoot = false;
        float move_rotate = actions.ContinuousActions[0];
        float move_forward = actions.ContinuousActions[1];
        bool shoot = actions.DiscreteActions[0] > 0;
        rb.MovePosition(transform.position + transform.forward * move_forward * move_speed * Time.deltaTime);
        transform.Rotate(0f, move_rotate * move_speed, 0f, Space.Self);


        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        if (distanceMoved > moveThreshold)
        {
            AddReward(3f); // Thưởng nhỏ cho việc di chuyển
            lastPosition = transform.position; // Cập nhật vị trí cuối cùng
        }

        // Phạt nếu agent không di chuyển đáng kể
        else
        {
            AddReward(-0.5f); // Phạt nhỏ nếu agent đứng yên hoặc chỉ di chuyển rất ít
        }


        if (shoot && !has_shot)
        {
            can_shoot = true;
        }
        if(can_shoot)
        {
            hit_target = gunObject.shootGun();
            time_until_next_bullet = min_time_until_next_bullet;
            has_shot = true;
            if(hit_target)
            {
                AddReward(30);
                if(world_behaviors.getPoliceCount() <=0 )
                   EndEpisode();
            }
            else
            {
                AddReward(-1);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreateActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

        discreateActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; 
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "wall")
        {
            AddReward(-15f);
            EndEpisode();
        }
        
        
        else if(other.gameObject.tag == "police")
        {
            AddReward(-15f);
            EndEpisode();
        }
    }
    private void FixedUpdate()
    {
        if(has_shot==true)
        {
            time_until_next_bullet--;
            if(time_until_next_bullet <=0)
            {
                has_shot = false;
            }
        }    
    }
}

