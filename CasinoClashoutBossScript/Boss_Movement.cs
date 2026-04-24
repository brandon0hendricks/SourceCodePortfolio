using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss_Movement : MonoBehaviour
{
    //componenetes for script
    private Boss_Manager manager;
    private Transform player;
    [SerializeField] private BoxCollider2D Boss_collider;
    private Transform late_player_postition;
    private Rigidbody2D rb;

    //position to check if is grounded
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //get distance from player
    private float Distance_from_player;
    //direction of player
    [HideInInspector] public Vector3 direction_player;


    //jump variables
    private int current_jumps = 0;
    private float Move_timer = 0;


    //variables for fliping the sprite
    private Vector2 currentPos;
    private Vector2 OldPos;

    //checks if move is finnished
    [HideInInspector] public bool finsihed;

    private Animator[] Legs_movement;

    void Start()
    {
        //gets needed components
        manager = gameObject.GetComponent<Boss_Manager>();
        player = manager.get_Player();
        rb = gameObject.GetComponent<Rigidbody2D>();

        Get_Animator();
    }
    private void Update()
    {
        //sets current pos to newest frame
        currentPos = transform.position;
        //scale timer with time
        Move_timer += Time.deltaTime;
        //gets player location.direction
        get_location_stats();
    }

    void Get_Animator()
    {
        Legs_movement = manager.Stats.leg_stats.legs_animation;
    }

    private void FixedUpdate()
    {
        //gets last frame position
        OldPos = currentPos;
    }
    private void LateUpdate()
    {
        late_player_postition = player.transform;
    }


    private void get_location_stats()
    {
        //gets distance of player
        Distance_from_player = Vector2.Distance(rb.position, player.position);
        //gets direction of player
        direction_player = (player.transform.position - transform.position).normalized;
    }

    public IEnumerator ShockWave()
    {
        
        yield return null;
    }



    public IEnumerator towards_Player(float Attack_Distance)
    {
        start_Legs_animation();
        while (Distance_from_player >= Attack_Distance)
        {
            rb.velocity = new Vector2(direction_player.x * manager.Stats.speed_stat, rb.velocity.y);
            finsihed = false;
            Flip();
            yield return new WaitForEndOfFrame();
        }
        if(Distance_from_player < Attack_Distance)
        {
            End_Legs_animation();
            finsihed = true;
        }
    }

    public IEnumerator Charge_Forward()
    {
        Disable_collision();
        Flip_to_player();
        Move_timer = 0f;
        Vector3 Direction_of_Charge = (player.transform.position - transform.position).normalized;
        start_Legs_animation();
        yield return new WaitForSeconds(.3f);
          
        while(Move_timer < 2f)
        {
            //gets direction to charge in, at double speed
            rb.velocity = new Vector2((Direction_of_Charge.x * manager.Stats.speed_stat) * 3.0f, rb.velocity.y);


            yield return new WaitForEndOfFrame();
        }
        finsihed = true;
        End_Legs_animation();
        Enable_collision();
    }

    public IEnumerator Jump()
    {
        Disable_collision();
        Flip_to_player();
        yield return null;
        if (IsGrounded() == true)
        {
            finsihed = false;

            //Jump location
            Vector2 jump_location = new Vector2(player.position.x - transform.position.x, transform.position.y + 35).normalized;
            rb.AddForce(jump_location * ((18f)*100), ForceMode2D.Impulse);
        }
        finsihed = true;
    }
    public IEnumerator Jump_away()
    {
        Disable_collision();
        if (IsGrounded() == true)
        {
            finsihed = false;

            //Jump location
            Vector2 jump_location = new Vector2((player.position.x - transform.position.x) * -1, transform.position.y + 35).normalized;
            rb.AddForce(jump_location * ((18f) * 100), ForceMode2D.Impulse);
        }
        while(IsGrounded() == false)
        {
            yield return new WaitForSeconds (.55f);
        }
        Enable_collision();
        finsihed = true;
    }

    public IEnumerator Jump_on_Player(int Jump_count)
    {
        current_jumps = 0;
        while(Jump_count + 1 > current_jumps)
        {
            if (IsGrounded() == true)
            {
                //Flip Direction of player
                Flip_to_player();
                //Jump to ontop of player
                Vector2 jump_location = new Vector2(player.position.x - transform.position.x, transform.position.y + 40).normalized;
                rb.AddForce(jump_location * 15f, ForceMode2D.Impulse);
                current_jumps += 1;
            }
            finsihed = false;
            yield return new WaitForEndOfFrame();
        }
        while(Jump_count + 1 == current_jumps && finsihed == false)
        {
            if(IsGrounded())
            {
                finsihed = true;
            }
        }
    }

    void start_Legs_animation()
    {
        if (Legs_movement != null)
        {
            for (int i = 0; i < Legs_movement.Length; i++)
            {
                Legs_movement[i].SetBool("legs_moving", true);
            }
        }
    }
    void End_Legs_animation()
    {
        if (Legs_movement != null)
        {
            for (int i = 0; i < Legs_movement.Length; i++)
            {
                Legs_movement[i].SetBool("legs_moving", false);
            }
        }
    }

    //checks if boss is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, .5f, groundLayer);
    }

    //flips based off direction of movement
    void Flip()
    {
            Vector2 localScale = transform.localScale;

            if (currentPos.x < OldPos.x)
            {
                localScale.x = 1f;
            }
            else if (currentPos.x > OldPos.x)
            {
                localScale.x = -1f;
            }
           transform.localScale = localScale;
    
    }

    public void Flip_to_player()
    {
            Vector2 localScale = transform.localScale;
            if (late_player_postition.position.x < transform.position.x)
            {
                localScale.x = 1f;
            }
            else if (late_player_postition.position.x > transform.position.x)
            {
                localScale.x = -1f;
            }
            transform.localScale = localScale;
        
    }

    void Flip_to_direction(float dir)
    {
            //converts direction of player to value of 1 or -1
            float whole_direction = Mathf.Sign(dir);

            Vector2 localScale = transform.localScale;
            if (whole_direction == -1)
            {
                localScale.x = 1f;
            }
            else if (whole_direction == 1)
            {
                localScale.x = -1f;
            }
            transform.localScale = localScale;
    }


    public void Disable_collision()
    {
        Boss_collider.enabled = false;
    }

    public void Enable_collision()
    {

        Boss_collider.enabled = true;
    }

}
