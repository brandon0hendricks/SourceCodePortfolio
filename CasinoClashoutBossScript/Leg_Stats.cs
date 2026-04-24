using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Leg_Stats : MonoBehaviour
{
    //base stats for legs
    public float base_dmg;
    public float max_health;
    public float speed;

    private Boss_Movement boss_Movement;
    private Transform player;
    [HideInInspector] public Animator animator;

    [Header("attack variables/components")]
    [SerializeField] private GameObject[] hitBox;
    [SerializeField] private Transform projectile_offset;
    [SerializeField] private GameObject projectile;
    [SerializeField] private int Limb_attack;


    private bool can_attack;
    private float delay_time;
    private Combo_system combo_system;

    [Header("components for animation")]
    public Animator[] legs_animation;
    

    private void Start()
    {
        get_components();
        can_attack = true;
    }
    private void Update()
    {
        animator.SetBool("Grounded", boss_Movement.IsGrounded());
        delay_time += Time.deltaTime;
    }

    void get_components()
    {
        //Get animator for the head
        animator = gameObject.GetComponent<Animator>();

        //Gets needed Boss manager Scripts
        GameObject BossManager = GameObject.FindGameObjectWithTag("Boss_Manger");
        boss_Movement = BossManager.GetComponent<Boss_Movement>();
        combo_system = BossManager.GetComponent<Combo_system>();
    }


    public void limb_attack()
    {
        if (can_attack == true && delay_time >= 1f)
        {
            switch (Limb_attack)
            {
                case 1:
                    StartCoroutine(Insect_stinger());
                    break;
                case 2:
                    StartCoroutine(Dragon_Jump());
                    break;
                case 3:
                    StartCoroutine(Mech_charge());
                    break;
            }
            can_attack = false;
        }
    }
    private IEnumerator Insect_stinger()
    {
        int choose_Attack = Random.Range(1, 3);

        if (choose_Attack == 2)
        {
            yield return StartCoroutine(boss_Movement.towards_Player(4f));
        }
        if (animator.GetBool("Legs_attack") == false && boss_Movement.IsGrounded())
        {
            animator.SetBool("Legs_attack", true);
        }

    }
    private IEnumerator Dragon_Jump()
    {       
        yield return null;
        if (animator.GetBool("Legs_attack") == false && boss_Movement.IsGrounded())
        {
            animator.SetBool("Legs_attack", true);
        }
        else if(boss_Movement.IsGrounded() == false)
        {
            finnished_attack();
        }
    }

    private IEnumerator Mech_charge()
    {
        //start speed up animation
        if (animator.GetBool("Legs_attack") == false && boss_Movement.IsGrounded())
        {
            animator.SetBool("Legs_attack", true);
            //can deal damage
            hit_box_true();
            //charge forward
            yield return StartCoroutine(boss_Movement.Charge_Forward());
            //cannot deal damage
            hit_box_false();
            finnished_attack();
        }
    }

    public void jump()
    {
        StartCoroutine(boss_Movement.Jump());
    }

    void Started_attack()
    {
        animator.SetBool("Legs_attack", true);
        //The attack is Not Finnished
        animator.SetBool("Finnished", false);
    }

    void finnished_attack()
    {
        delay_time = 0f;

        //Attack is finnished
        animator.SetBool("Finnished", true);
        animator.SetBool("Legs_attack", false);

        can_attack = true;

        combo_system.next_move();
    }

    void hit_box_true()
    {
        //sets all hitboxes to true
        for (int i = 0; i < hitBox.Length; i++)
        {
            hitBox[i].SetActive(true);
        }
    }
    //call in animation, sets attack hit boxes to false
    void hit_box_false()
    {
        //sets all hitboxes to false
        for (int i = 0; i < hitBox.Length; i++)
        {
            hitBox[i].SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
    void Create_Insect_Stinger()
    {
        boss_Movement.Flip_to_player();

        GameObject Stinger = Instantiate(projectile, projectile_offset.position, Quaternion.identity);
        Stinger.GetComponent<Pincer_projectile>().projectile_offset = projectile_offset;
        Stinger.GetComponent<Pincer_projectile>().boss = gameObject.transform;
    }

    void enable_collision()
    {
        boss_Movement.Enable_collision();
    }
    void Disable_collision()
    {
        boss_Movement.Disable_collision();
    }

    void Start_Spikes()
    {
        //StartCoroutine(Dragon_Shockwave());
    }
    IEnumerator Dragon_Shockwave()
    {
        Vector2 Start_Position = projectile_offset.localPosition;
        float Add_position = .0f;
        float sub_postion = -0f;


        Vector2 Spawn_right = new Vector2(Start_Position.x + Add_position, Start_Position.y);
        Vector2 Spawn_left = new Vector2(Start_Position.x + sub_postion, Start_Position.y);

        for (int i = 0; i < 10; i++)
        {
            Spawn_right = new Vector2(Start_Position.x + Add_position, Start_Position.y);
            Spawn_left = new Vector2(Start_Position.x + sub_postion, Start_Position.y);

            GameObject Left_Spike = Instantiate(projectile, Spawn_left, Quaternion.identity);

            GameObject Right_Spike = Instantiate(projectile, Spawn_right, Quaternion.identity);
            Add_position += .6f;
            sub_postion += -.6f;
            yield return new WaitForSeconds(.06f);
        }


    }
}
