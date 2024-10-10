using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class police : MonoBehaviour
{
    public Transform target;
    [SerializeField] float update_speed = 0.1f;

    private NavMeshAgent policee;

    public WorldBehaviors world_behaviors;

    private void Awake()
    {
        policee = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(update_speed);
        while(enabled)
        {
            policee.SetDestination(target.transform.position);

            yield return wait;
        }
    }

    public void movePolice(Vector3 pos)
    {
        policee.Warp(pos);
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        world_behaviors.spawnPoliceList.Remove(this);
    }
}
