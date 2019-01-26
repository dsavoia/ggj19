using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour {

    Player player;
        	
	void Start () {

        player = GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update () {

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //@TODO: up arrow input
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            //@TODO: down arrow input
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //@TODO: LeftShift input
        }
    }
}
