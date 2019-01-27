using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public int minUpperDetailsQty = 2, maxUpperDetailsQty = 3;
    public int minBottomDetailsQty = 0, maxBottomDetailsQty = 2;
    public int minStructuresPerBlock = 6, maxStructuresPerBlock = 8;
    public int streetQty = 5, blocksPerStreet = 5;
    public float distanceBetweenStructures = 13.0f;
    public List<GameObject> roofList;
    public List<GameObject> bodyList;
    public List<GameObject> doorList;
    public List<GameObject> intersectionList;
    public List<GameObject> upperDetailsList;
    public List<GameObject> bottomDetailsList;
    public GameObject buildingPrefab;

    public GameObject catHouse;
    public Building catHouseScript;
    public Vector3 catHousePos;

    public GameObject[,][] BuildNeighbourhood()
    {
        GameObject[,][] neighbourhood = new GameObject[streetQty, blocksPerStreet][];

        int randStreet = Random.Range(3, streetQty + 1);
        int randBlock = Random.Range(1, blocksPerStreet + 1);
        int randStructure = Random.Range(2, minStructuresPerBlock + 1);

        catHousePos = new Vector3(randStreet, randBlock, randStructure);
        catHouse = null;
        catHouse = BuildStructure();
        catHouseScript = catHouse.GetComponent<Building>();

        for (int i = 0; i < neighbourhood.GetLength(0); i++)
        {
            for (int j = 0; j < neighbourhood.GetLength(1); j++)
            {
                int structureQty = Random.Range(minStructuresPerBlock, maxStructuresPerBlock + 1);
                //Adding space to intersections
                structureQty += 2;
                neighbourhood[i, j] = new GameObject[structureQty];
                neighbourhood[i, j] = BuildStreetBlock(i, j);
            }
        }

        Vector2 pos;
        
        GameObject streetParent;
        GameObject blockParent;

        for (int i = 0; i < neighbourhood.GetLength(0); i++)
        {
            streetParent = new GameObject("StreetParent" + i);
            streetParent.transform.parent = GameManager.Instance.neighbourhoodParent.transform;
            pos = Vector2.zero;

            for (int j = 0; j < neighbourhood.GetLength(1); j++)
            {
                blockParent = new GameObject("BlockParent" + j);
                blockParent.transform.parent = streetParent.transform;

                for (int k = 0; k < neighbourhood[i, j].GetLength(0); k++)
                {
                    neighbourhood[i, j][k].transform.position = new Vector2(pos.x + (k * distanceBetweenStructures), pos.y);
                    neighbourhood[i, j][k].transform.parent = blockParent.transform;
                }

                blockParent.SetActive(false);
            }
        }

        AssignNeighbours(neighbourhood);
        GameManager.Instance.neighbourhoodParent.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);

        return neighbourhood;
    }

    GameObject[] BuildStreetBlock(int blockX, int blockY)
    {
        int structureQty = Random.Range(minStructuresPerBlock, maxStructuresPerBlock + 1);
        //Adding space to intersections
        structureQty += 2;
        GameObject[] streetBlock = new GameObject[structureQty];
        
        for (int i = 1; i < structureQty-1; i++)
        {
            streetBlock[i] = BuildStructure();
        }

        streetBlock[0] = Instantiate(intersectionList[Random.Range(0, intersectionList.Count)]) as GameObject;
        streetBlock[0].transform.localScale *= -1;
        streetBlock[structureQty-1] = Instantiate(intersectionList[Random.Range(0, intersectionList.Count)]) as GameObject;           

        return streetBlock;
    }

    void AssignNeighbours(GameObject[,][] neighbourhood)
    {
        Vector2 left, right, up, down;

        int xPos, yPos;

        for (int i = 0; i < neighbourhood.GetLength(0); i++)
        {
            for (int j = 0; j < neighbourhood.GetLength(1); j++)
            {                
                //Left
                if (i == 0)
                {
                    xPos = neighbourhood.GetLength(0) - 1;
                }
                else
                {
                    xPos = i - 1;
                }

                left = new Vector2(xPos, j);

                //Right
                if (i == neighbourhood.GetLength(0) - 1)
                {
                    xPos = 0;
                }
                else
                {
                    xPos = i + 1;
                }

                right = new Vector2(xPos, j);

                //Down
                if (j == 0)
                {
                    yPos = neighbourhood.GetLength(1) - 1;
                }
                else
                {
                    yPos = j - 1;
                }

                down = new Vector2(i, yPos);

                //Up
                if (j == neighbourhood.GetLength(1) - 1)
                {
                    yPos = 0;
                }
                else
                {
                    yPos = j + 1;
                }

                up = new Vector2(i, yPos);

                Intersection intersectionScript = neighbourhood[i,j][0].GetComponent<Intersection>();
                intersectionScript.sideBlock = left;
                intersectionScript.upBlock = up;
                intersectionScript.downBlock = down;
                intersectionScript.right = false;

                intersectionScript = neighbourhood[i, j][neighbourhood[i, j].GetLength(0)- 1].GetComponent<Intersection>();
                intersectionScript.sideBlock = right;
                intersectionScript.upBlock = up;
                intersectionScript.downBlock = down;
                intersectionScript.right = true;                
            }
        }
    }

    GameObject BuildStructure()
    {
         
        GameObject building = Instantiate(buildingPrefab) as GameObject;
        Building buildingScript = building.GetComponent<Building>();

        Instantiate(bodyList[Random.Range(0, bodyList.Count)], buildingScript.body.transform.position,
            Quaternion.identity, buildingScript.body.transform);
        Instantiate(roofList[Random.Range(0, roofList.Count)], buildingScript.roof.transform.position,
            Quaternion.identity, buildingScript.roof.transform);
        Instantiate(doorList[Random.Range(0, doorList.Count)], buildingScript.door.transform.position,
            Quaternion.identity, buildingScript.door.transform);
        
        int upperDetailsQty = 0;
        int bottomDetailsQty = 0;

        do
        {
            upperDetailsQty = Random.Range(minUpperDetailsQty, maxUpperDetailsQty + 1);
            bottomDetailsQty = Random.Range(minBottomDetailsQty, maxBottomDetailsQty + 1);
        } while ((upperDetailsQty + bottomDetailsQty) < 3);

        bool differentFromCatHouse;

        do
        {
            differentFromCatHouse = true;
            //bool results =  query2.All(i=>query1.Contains(i));

            List<GameObject> upperDetailsListBucket = new List<GameObject>(upperDetailsList);
            List<GameObject> bottomDetailsListBucket = new List<GameObject>(bottomDetailsList);
            buildingScript.detailsCodeList = new List<int>();

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
                        GameObject detail = Instantiate(upperDetailsListBucket[randomDetailIndex], buildingScript.upperDetails[randomPositionIndex].transform.position,
                            Quaternion.identity, buildingScript.upperDetails[randomPositionIndex].transform) as GameObject;
                        placed = true;
                        upperDetailsListBucket.RemoveAt(randomDetailIndex);
                        buildingScript.detailsCodeList.Add(detail.GetComponent<Details>().code);
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
                        GameObject detail = Instantiate(bottomDetailsListBucket[randomDetailIndex], buildingScript.bottomDetails[randomPositionIndex].transform.position,
                            Quaternion.identity, buildingScript.bottomDetails[randomPositionIndex].transform) as GameObject;
                        bottomDetailsListBucket.RemoveAt(randomDetailIndex);
                        placed = true;
                        buildingScript.detailsCodeList.Add(detail.GetComponent<Details>().code);
                    }
                } while (!placed);
            }

            if (catHouseScript)
            {
                if (catHouseScript.detailsCodeList.All(i => buildingScript.detailsCodeList.Contains(i)))
                {
                    print("cat: " + catHouseScript.detailsCodeList + " || created: " + buildingScript.detailsCodeList);
                    differentFromCatHouse = false;
                }
            }

        } while (!differentFromCatHouse);

        return building;
    }
}
