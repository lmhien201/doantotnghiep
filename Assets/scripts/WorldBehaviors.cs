using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehaviors : MonoBehaviour
{
    [SerializeField] agentmove agent_move;
    [SerializeField] police police_move;
    [SerializeField] public List<police> spawnPoliceList = new List<police>();
    [SerializeField] int police_count;

    [SerializeField] Transform environment_location;

    [SerializeField] int time_between_spawn =1;
    [SerializeField] Transform spawn_zone_1;
    [SerializeField] Transform spawn_zone_2;
    [SerializeField] Transform spawn_zone_3;
    [SerializeField] Transform spawn_zone_4;

    public void spawnAgent()
    {
        agent_move.transform.localPosition = new Vector3(0f, 0.5f, 0f);
    }
    public void callSpawnPolice()
    {
        StartCoroutine(spawnPolice());
    }
    IEnumerator spawnPolice()
    {
        Vector3 spawn_zone_1_coords = spawn_zone_1.transform.position;
        Vector3 spawn_zone_2_coords = spawn_zone_2.transform.position;
        Vector3 spawn_zone_3_coords = spawn_zone_3.transform.position;
        Vector3 spawn_zone_4_coords = spawn_zone_4.transform.position;


        if (spawnPoliceList.Count !=0)
        {
            removePolice(spawnPoliceList);
        }
        for (int i = 0; i < police_count; i++)
        {
            // (size/2)-1
            Vector3 random_spawn_zone_1_coords = spawn_zone_1_coords + new Vector3
                (Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            Vector3 random_spawn_zone_2_coords = spawn_zone_2_coords + new Vector3
                (Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            Vector3 random_spawn_zone_3_coords = spawn_zone_3_coords + new Vector3
                (Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            Vector3 random_spawn_zone_4_coords = spawn_zone_4_coords + new Vector3
                (Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));

            Vector3[] random_area =
            {
                random_spawn_zone_1_coords, random_spawn_zone_2_coords,
                random_spawn_zone_3_coords, random_spawn_zone_4_coords
            };
            int counter = 0;
            bool distanceGood;

            police new_police = Instantiate(police_move);
            new_police.target = agent_move.transform;
            new_police.world_behaviors = this;
            new_police.transform.parent = environment_location;

            Vector3 police_location = random_area[Random.Range(0, random_area.Length)];

            if (spawnPoliceList.Count == 0)
            {
                new_police.movePolice(police_location);
                spawnPoliceList.Add(new_police);
                yield return new WaitForSeconds(time_between_spawn);
            }
            else if(spawnPoliceList.Count !=0)
            {
                for(int k=0 ; k < spawnPoliceList.Count ; k++)
                {
                    if(counter <10)
                    {
                        distanceGood = CheckOverlap(police_location, spawnPoliceList[k].transform.localPosition, 1f);
                        if(distanceGood == false)
                        {
                            police_location = random_area[Random.Range(0, random_area.Length)];
                            k--;
                        }
                        counter++;
                    }
                    else
                    {
                        k = spawnPoliceList.Count;
                    }
                }
                new_police.movePolice(police_location);
                spawnPoliceList.Add(new_police);
                yield return new WaitForSeconds(time_between_spawn);

            }
        }     
    }
    public bool CheckOverlap(Vector3 objectWeWantToAvoidOverlapping, Vector3 alreadyExistingObject, float minDistanceWanted)
    {
        float distance_between_objects = Vector3.Distance(objectWeWantToAvoidOverlapping, alreadyExistingObject);
        if(minDistanceWanted <= distance_between_objects)
        {
            return true;
        }
        return false;
    }

    public int getPoliceCount()
    {
        return spawnPoliceList.Count;
    }
    private void removePolice(List<police> to_be_deleted_game_object_list)
    {
        foreach(police i in to_be_deleted_game_object_list)
        {
            Destroy(i.gameObject);
        }
        to_be_deleted_game_object_list.Clear();
    }
}
