using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CommonThrownObj2D : MonoBehaviour
{
    
    Rigidbody2D rbody;
    Animator animator;
    public float speed;
    public float life_time;

    // Target
    Rigidbody2D chasing_target;
    Vector2 target_point;

    public string[] collidable_tags;

    // MoveParameters
    Vector2 moving_direction;

    // Status
    bool live;


    // Callback Functions ///////////////////////////////////////////////////////
    public Action<Collider2D> CallBackGotTarget = new Action<Collider2D>((x) => {});
    public Action<Vector2> CallBackThrownDied = new Action<Vector2>((x) => {});


    // Methods //////////////////////////////////////////////////////////////////
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        live = true;
    }

    void Update()
    {
        CheckLifeTime();
        if (chasing_target != null) {AdjustChasingPosition();}
        Move();
    }

    // Interface //////////////////////////////////////////////////////////////////
    public int MoveToPosition(Vector2 position) {
        moving_direction = position;
        return 0;
    }
    public int MoveToTarget(Rigidbody2D target) {
        chasing_target = target;
        return 0;
    }

    // Inner Methods //////////////////////////////////////////////////////////////
    // Move
    void AdjustChasingPosition() {
        MoveToPosition(chasing_target.position);
    }

    void Move() {
        if (moving_direction == Vector2.zero) return;
        Vector2 circle_direction = Utils.GetCircleDirection2D(moving_direction.x, moving_direction.y);
        rbody.position += Time.deltaTime * speed * circle_direction;
        // rbody.velocity = Time.deltaTime * speed * circle_direction;
    }

    void KillSelf() {
        moving_direction = Vector2.zero;
        rbody.velocity = Vector2.zero;
        CallBackThrownDied(rbody.position);
        animator.SetTrigger("died");
        live = false;
        TimerController.Instance.do_after_seconds(1f, () => {
            Destroy(this.gameObject);
        });
    }

    // Collision
    void OnTriggerEnter2D(Collider2D col) {
        for (int i = 0; i < collidable_tags.Length; i++) {
            if (col.tag == collidable_tags[i] && live == true) {
                CallBackGotTarget(col); 
                KillSelf();
            }
        }
    }

    // LifeTimeUp
    void CheckLifeTime() {
        life_time -= Time.deltaTime;
        if(life_time < 0 && live == true)  KillSelf();
    }
}

