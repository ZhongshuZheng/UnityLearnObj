using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CommonUnit2D : MonoBehaviour
{

    // Common
    private Rigidbody2D rbody;
    private Animator animator;

    // Unit States
    enum UnitState {idle=1, move=2, died=3}
    UnitState unit_state;

    // Movement
    public float moving_speed;   // 4.5f as default
    [HideInInspector]
    public Vector2 moving_direction; // {private get; set;}

    // Health
    public int HPmax;
    private int hp_now;


    public AudioClip walk_audio = null;


    // Outer CallBacks //////////////////////////////////////////////////////////
    public Action CallBackDie = new Action(() => {});


    // Methods //////////////////////////////////////////////////////////////////
    void Awake()
    {
        // common
        rbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        
        // Movement
        moving_direction = Vector2.zero;
        // moving_speed = 4.5f;

        // Health
        hp_now = HPmax;

        // state
        unit_state = UnitState.idle;

    }
    
    void Update()
    {
    }
    
    void FixedUpdate() {
        if (unit_state == UnitState.move) Move(); // 如果Move出现掉帧问题，调整SmartCamera的更新方式为fixedUpdate
    }


    // Interface //////////////////////////////////////////////////////////
    // Movement
    public int MoveTowardDirection(Vector2 direction) {
        if (direction == Vector2.zero) return Stop();
        if (unit_state == UnitState.died) return -1;

        moving_direction = direction;
        unit_state = UnitState.move;
        animator.SetFloat("move_x", moving_direction.x);
        animator.SetFloat("move_y", moving_direction.y);
        animator.SetFloat("move_speed", moving_direction.SqrMagnitude());

        return 0;
    }

    // Stop
    public int StopTowardDirection(Vector2 direction) {
        if (direction == Vector2.zero) return Stop();
        if (unit_state == UnitState.died) return -1;
        unit_state = UnitState.idle;
        animator.SetFloat("move_speed", 0f);
        animator.SetFloat("move_x", direction.x);
        animator.SetFloat("move_y", direction.y);
        return 0;
    }

    public int Stop() {
        if (unit_state == UnitState.died) return -1;
        rbody.velocity = Vector2.zero;
        unit_state = UnitState.idle;
        animator.SetFloat("move_speed", 0f);
        return 0;
    }

    public int Die() {
        if (unit_state == UnitState.died) return -1;
        unit_state = UnitState.died;
        animator.SetFloat("move_speed", 0f);
        animator.SetTrigger("die");
        CallBackDie();
        return 0;
    }


    // Health Method
    public void makeHeal(int heal_value) {
        hp_now += heal_value;
        hp_now = Mathf.Min(hp_now, HPmax);
    }

    public void makeDamage(int damage_value) {
        if (unit_state == UnitState.died)  return;
        animator.SetTrigger("hurt");
        hp_now -= damage_value;
        if (hp_now <= 0)  Die();
    }


    // Inner Methods //////////////////////////////////////////////////////
    // Move
    void Move() {
        if (moving_direction == Vector2.zero) {
            Stop();
            return;
        }
        Vector2 circle_direction = Utils.GetCircleDirection2D(moving_direction.x, moving_direction.y);
        // rbody.velocity = Time.fixedDeltaTime * moving_speed * circle_direction;
        rbody.position += Time.fixedDeltaTime * moving_speed * circle_direction;
        if (walk_audio != null) {
            // GetComponent<AudioSource>()?.PlayOneShot(walk_audio);
        }
    }



}
