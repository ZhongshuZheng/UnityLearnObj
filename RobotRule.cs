using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RobotRule : MonoBehaviour
{

    // Common
    CommonUnit2D unit_move;
    Rigidbody2D rbody;

    // Child Part
    public GameObject vision_field;
    Transform vision_field_transform;

    public GameObject Smoke;

    // Vision Direction
    Vector2 vision_direction;


    // Robot State
    enum RobotState {
        random_walk = 0, is_fixed = 1, chasing_player = 2
    }
    // [SerializeField]
    RobotState robot_state;

    // move_rule
    [Tooltip("How many seconds before the robot will turn a round")]
    public int turn_timmer;
    Rigidbody2D player_rbody;

    // Start is called before the first frame update
    void Start()
    {
        unit_move = gameObject.GetComponent<CommonUnit2D>();
        rbody = gameObject.GetComponent<Rigidbody2D>();
        // vision_field = GameObject.FindGameObjectWithTag("EnemyCheckRegion");
        vision_field_transform = vision_field.GetComponent<Transform>();

        unit_move.CallBackDie += dieFunction;
        robot_state = RobotState.random_walk;

        vision_direction = Vector2.zero;

        StartCoroutine(randomMove(turn_timmer));
        StartCoroutine(adjustVisionDirection());
    }

    // Update is called once per frame
    void Update()
    {
        if (robot_state == RobotState.chasing_player) chasePlayer();
    }

    // State Change
    public void foundPlayer(Rigidbody2D player) {
        if (robot_state == RobotState.random_walk) {
            player_rbody = player;
            robot_state = RobotState.chasing_player;
        }
    }

    public void lostPlayer() {
        if (robot_state == RobotState.chasing_player) {
            robot_state = RobotState.random_walk;
        }
    }

    // Movement method
    void chasePlayer() {
        vision_direction = player_rbody.position - rbody.position;
        unit_move.MoveTowardDirection(vision_direction);
    }

    IEnumerator randomMove(int turn_timmer) {
        while (robot_state != RobotState.is_fixed) {
            if (robot_state == RobotState.random_walk) {
                vision_direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                unit_move.MoveTowardDirection(vision_direction);
            }
            yield return new WaitForSeconds(turn_timmer);
        }
    }

    IEnumerator adjustVisionDirection(){
        while (robot_state != RobotState.is_fixed) {
            float x = vision_direction.x;
            float y = vision_direction.y;
            float maxV;
            if (Mathf.Abs(y) > Mathf.Abs(x)) {
                maxV = y;
            } else {maxV = x;}
            if (maxV == y && maxV <= 0) {
            vision_field_transform.rotation = Quaternion.Euler(0, 0, 0); 
            } else if (maxV == y && maxV > 0) {
            vision_field_transform.rotation = Quaternion.Euler(0, 0, 180); 
            } else if (maxV == x && maxV >= 0) {
            vision_field_transform.rotation = Quaternion.Euler(0, 0, 90); 
            } else {
            vision_field_transform.rotation = Quaternion.Euler(0, 0, -90); 
            }
            yield return new WaitForSeconds(1f);
        }
    }


    // Got Damage
    void dieFunction() {
        robot_state = RobotState.is_fixed;
        Destroy(Smoke);
    }

    // Got State
    public bool isFixed() {
        return robot_state == RobotState.is_fixed;
    }

}
