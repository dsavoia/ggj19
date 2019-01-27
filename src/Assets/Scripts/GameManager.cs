using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject player;
    public Player playerScript;
    public GameObject mainCamera;
    public GameObject[,][] neighbourhood;
    public GameObject neighbourhoodParent;

    Builder builder;
    Vector2 currentBlock;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }    
    // Start is called before the first frame update
    void Start()
    {
        builder = GetComponent<Builder>();
        neighbourhoodParent = new GameObject("NeighbourhoodParent");
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void InitGame()
    {
        playerScript = player.GetComponent<Player>();
        neighbourhood = builder.BuildNeighbourhood();
        currentBlock = new Vector2(0, 0);
    }

    public void NextBlock(Vector2 nextBlock, bool right, bool isSideBlock)
    {
        //neighbourhood[(int)currentBlock.x, (int)currentBlock.y];
        Vector2 placementPos = Vector2.zero;
        print("next block" + nextBlock);

        Transform block = neighbourhoodParent.transform.GetChild((int)nextBlock.x).transform.GetChild((int)nextBlock.y);
        int rightChildPos = isSideBlock ? 0 : block.childCount - 1;
        int leftChildPos = isSideBlock ? block.childCount - 1 : 0;

        if (right)
        {            
            Intersection blockIntersectionScript = block.GetChild(rightChildPos).GetComponent<Intersection>();
            placementPos = blockIntersectionScript.startPosition.position;
        }
        else
        {            
            Intersection blockIntersectionScript = block.GetChild(leftChildPos).GetComponent<Intersection>();
            placementPos = blockIntersectionScript.startPosition.position;
        }
        

        neighbourhoodParent.transform.GetChild((int)currentBlock.x).transform.GetChild((int)currentBlock.y).gameObject.SetActive(false);
        neighbourhoodParent.transform.GetChild((int)nextBlock.x).transform.GetChild((int)nextBlock.y).gameObject.SetActive(true);
        currentBlock = nextBlock;
        player.transform.position = new Vector2(placementPos.x, placementPos.y);
        mainCamera.transform.position = new Vector3(player.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        playerScript.currentIntersection = null;
        playerScript.goToSideBlock = false;

    }
}
