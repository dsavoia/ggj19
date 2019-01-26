using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public List<GameObject> roofList;
    public List<GameObject> bodyList;
    public List<GameObject> doorList;
    public int minUpperDetailsQty = 2, maxUpperDetailsQty = 3;
    public int minBottomDetailsQty = 0, maxBottomDetailsQty = 2;
    public List<GameObject> upperDetailsList;
    public List<GameObject> bottomDetailsList;
    public GameObject buildingPrefab;
    public float distanceBetweenStructures = 10.0f;

    List<GameObject> buildings;

    void Start()
    {
        buildings = new List<GameObject>();    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject building = BuildStructure();
            if(buildings.Count > 0)
            {
                Vector2 previousBuildingPos = buildings[buildings.Count - 1].transform.position;
                building.transform.position = new Vector2(previousBuildingPos.x + distanceBetweenStructures, previousBuildingPos.y);
            }
            else
            {
                building.transform.position = Vector3.zero;
            }

            buildings.Add(building);
        }
    }

    public GameObject BuildStructure()
    {
        GameObject building = Instantiate(buildingPrefab) as GameObject;
        Building buildingScript = building.GetComponent<Building>();

        Instantiate(bodyList[Random.Range(0, bodyList.Count)], buildingScript.body.transform.position,
            Quaternion.identity, buildingScript.body.transform);
        Instantiate(roofList[Random.Range(0, roofList.Count)], buildingScript.roof.transform.position,
            Quaternion.identity, buildingScript.roof.transform);
        Instantiate(doorList[Random.Range(0, doorList.Count)], buildingScript.door.transform.position,
            Quaternion.identity, buildingScript.door.transform);

        int upperDetailsQty = Random.Range(minUpperDetailsQty, maxUpperDetailsQty + 1);

        //Making sure that there is at least 3 details
        if(upperDetailsQty + maxBottomDetailsQty < 3)
        {
            minBottomDetailsQty = 1;
        }

        int bottomDetailsQty = Random.Range(minBottomDetailsQty, maxBottomDetailsQty + 1);

        List<GameObject> upperDetailsListBucket = new List<GameObject>(upperDetailsList);
        List<GameObject> bottomDetailsListBucket = new List<GameObject>(bottomDetailsList);

        for (int i = 0; i < upperDetailsQty; i++)
        {
            int randomDetailIndex;
            int randomPositionIndex;
            bool placed = false;

            do
            {
                randomDetailIndex = Random.Range(0, upperDetailsListBucket.Count);
                randomPositionIndex = Random.Range(0, buildingScript.upperDetails.Count);

                if (buildingScript.upperDetails[randomPositionIndex].transform.childCount == 0)
                {
                    Instantiate(upperDetailsListBucket[randomDetailIndex], buildingScript.upperDetails[randomPositionIndex].transform.position, 
                        Quaternion.identity, buildingScript.upperDetails[randomPositionIndex].transform);
                    upperDetailsListBucket.RemoveAt(randomDetailIndex);
                    placed = true;
                }
            } while (!placed);
        }

        for (int i = 0; i < bottomDetailsQty; i++)
        {
            int randomDetailIndex;
            bool placed = false;
            int randomPositionIndex;

            do
            {
                randomDetailIndex = Random.Range(0, bottomDetailsListBucket.Count);
                randomPositionIndex = Random.Range(0, buildingScript.bottomDetails.Count);

                if (buildingScript.bottomDetails[randomPositionIndex].transform.childCount == 0)
                {                    
                    Instantiate(bottomDetailsListBucket[randomDetailIndex], buildingScript.bottomDetails[randomPositionIndex].transform.position,
                        Quaternion.identity, buildingScript.bottomDetails[randomPositionIndex].transform);
                    bottomDetailsListBucket.RemoveAt(randomDetailIndex);
                    placed = true;
                }
            } while (!placed);
        }

        return building;
    }
}
