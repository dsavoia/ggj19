using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Builder builder;
    // Start is called before the first frame update
    void Start()
    {
        builder = GetComponent<Builder>();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void InitGame()
    {

    }
}
