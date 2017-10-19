using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    public GameObject playerPrefab;
    
	void Awake () {
        GameObject player = Instantiate(playerPrefab,
            transform.position, transform.rotation);
        player.name = "Player";

        Destroy(gameObject);
	}
}
