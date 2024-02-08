using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerInRobotSight : MonoBehaviour
{

    public GameObject parentObject;
    RobotRule rule;

    // Start is called before the first frame update
    void Start()
    {
        rule = parentObject.GetComponent<RobotRule>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            rule.foundPlayer(col.GetComponent<Rigidbody2D>());
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Player") {
            rule.lostPlayer();
        }
    }
}
