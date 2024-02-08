using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
// using UnityEngine.Assertions;
// using UnityEngine.WSA;

public class playerController : MonoBehaviour
{   

    private CommonUnit2D unit_move;
    private Animator animator;

    public GameObject throwArrow;
    public SpriteRenderer arrow_renderer;

    // Player state
    enum PlayerState {
        normal = 0, outofControl = 1, attack = 2
    }
    [SerializeField]
    PlayerState player_state;


    // Player Input
    public float x, y, x_smooth, y_smooth;
    int is_j;  // 0 is empty, 1 is on, -1 is off


    // Player Attack
    public float battle_backswing;
    float battle_backswing_timmer;
    public float hurt_stiffness;
    float hurt_stiffness_timmer;

    // Thrown object
    public float default_throw_power;
    public float max_power_time;
    public float max_power_speed;
    float throw_power;

    float arrow_range_less = 3f;
    float arrow_range_max = 10f;
    float arrow_range;

    public AssetReference bullet_ref;
    private GameObject bullet;
    public GameObject bulletExplosion;

    
    // Start is called before the first frame update
    void Start()
    {
        unit_move = gameObject.GetComponent<CommonUnit2D>();
        animator = gameObject.GetComponent<Animator>();
        player_state = PlayerState.normal;
        throwArrow.SetActive(false);
        arrow_renderer = throwArrow.GetComponent<SpriteRenderer>();

        Addressables.LoadAssetAsync<GameObject>(bullet_ref).Completed += (handle) => {
            bullet = handle.Result;
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput(); 
        if (player_state == PlayerState.normal) prepareLaunch();
        if (player_state == PlayerState.attack || player_state == PlayerState.outofControl) Launch();
        if (player_state == PlayerState.normal) unit_move.MoveTowardDirection(new Vector2(x, y));
        if (player_state == PlayerState.attack) {
            unit_move.StopTowardDirection(new Vector2(x_smooth, y_smooth));
            adjustArrow();
            gatherPower();
        }
     
    }

    // Check player input
    void CheckPlayerInput() {
        // Check WASD movement
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        x_smooth = Input.GetAxis("Horizontal");
        y_smooth = Input.GetAxis("Vertical");

        // Check Attack
        if (Input.GetKeyDown(KeyCode.J) && player_state == PlayerState.normal) {
            is_j = 1;
        }
        if (Input.GetKeyUp(KeyCode.J) && player_state == PlayerState.attack) {
            is_j = -1;
        }
    }

    // Launch Funs
    void prepareLaunch() {
        if (is_j == 1) {
            player_state = PlayerState.attack;
            unit_move.Stop();
            throwArrow.SetActive(true);
            arrow_range = arrow_range_less;
            throw_power = default_throw_power;
        }
    }

    void Launch() {
        if (is_j == -1) {
            if (player_state == PlayerState.attack) {
                player_state = PlayerState.outofControl;
                animator.SetTrigger("Launch");
                throwArrow.SetActive(false);
                battle_backswing_timmer = Time.time;
                launchBullet();
            }
            if (player_state == PlayerState.outofControl && battle_backswing_timmer +  battle_backswing < Time.time) {
                player_state = PlayerState.normal;
                is_j = 0;
            }
        }
    }

    void adjustArrow() {
        float face_x = animator.GetFloat("move_x");
        float face_y = animator.GetFloat("move_y");
        float angle = Utils.GetDegreeFromVector2D(face_x, face_y);
        throwArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        arrow_range += Time.deltaTime / max_power_time * (arrow_range_max - arrow_range_less);
        arrow_renderer.size = new Vector2(MathF.Min(arrow_range_max, arrow_range), 5f);
    }

    void gatherPower() {
        throw_power += Time.deltaTime / max_power_time * (1 - default_throw_power);
    }

    void launchBullet() {
        float face_x = animator.GetFloat("move_x");
        float face_y = animator.GetFloat("move_y");
        float angle = Utils.GetDegreeFromVector2D(face_x, face_y);
        if (angle > 90) angle -= 180;
        else if (angle < -90) angle += 180;
        CommonThrownObj2D ibullet = GameObject.Instantiate(bullet, 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 
            Quaternion.Euler(new Vector3(0, 0, angle))
        ).GetComponent<CommonThrownObj2D>();

        ibullet.speed = max_power_speed * Mathf.Min(1, throw_power);
        ibullet.CallBackGotTarget += hitEmemy;
        ibullet.CallBackThrownDied += dropAsBullet;
        ibullet.MoveToPosition(new Vector2(face_x, face_y));
    }

    // Bullet Functions
    void hitEmemy(Collider2D col) {
        if (col.tag == "Enemy") {
            col.GetComponent<CommonUnit2D>().makeDamage(100);
        }
    }
    void dropAsBullet(Vector2 position) {
    }

    // Health Functions
    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy" && !col.gameObject.GetComponent<RobotRule>().isFixed()) {
            Debug.Log("Robat got" + col.gameObject.name);
        } 
    } 

}
