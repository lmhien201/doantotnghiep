using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    [SerializeField] Transform spawn_point;
    [SerializeField] LineRenderer laser_line;
    [SerializeField] float laser_duration = 0.05f;
    [SerializeField] float laser_range = 600f;

    [SerializeField] WorldBehaviors world_behaviors;
   public bool shootGun()
    {
        if( Physics.Raycast(spawn_point.position, transform.forward, out RaycastHit hit, laser_range))
        {
            laser_line.enabled = true;
            laser_line.SetPosition(0, spawn_point.position);
            laser_line.SetPosition(1, hit.point);

            if (hit.transform.gameObject.tag == "police")
            {
                StartCoroutine(ShootLaszer());
                world_behaviors.spawnPoliceList.Remove(hit.transform.GetComponent<police>());
                Destroy(hit.transform.gameObject);
                return true;
            }
            else if (hit.collider.gameObject.tag == "wall")
            {
                StartCoroutine(ShootLaszer());
                return false;
            }
        }
        StartCoroutine(ShootLaszer());
        return false;
    }
    private IEnumerator ShootLaszer()
    {
        yield return new WaitForSeconds(laser_duration);
        laser_line.enabled = false;
    }
}
