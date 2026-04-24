using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Head_stats : MonoBehaviour
{
    [Header("Limb Stats")]
    //stats for limb
    public float base_dmg;
    public float max_health;
    public float speed;

    [HideInInspector]
    public Animator animator;
    private Transform player;
    private Boss_Movement boss_Movement;
    [SerializeField] private int Limb_attack;


    [Header("projectile Stats")]
    //projectile offset
    [SerializeField]
    private Transform projectile_offset;
    //projectile prefab
    [SerializeField]
    private GameObject projectile;


    private bool can_attack;
    private float delay_time;
    private Combo_system combo_system;


    private void Start()
    {
        get_components();
        can_attack = true;
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
    private void Update()
    {
        //delay before resetting attacks
        delay_time += Time.deltaTime;
    }

    public void limb_attack()
    {
        if (can_attack == true && delay_time >= 1f)
        {
            switch (Limb_attack)
            {
                case 1:
                    StartCoroutine(Insect_honey());
                    break;
                case 2:
                    StartCoroutine(dragon_blast());
                    break;
                case 3:
                    StartCoroutine(mech_laser());
                    break;
            }
            can_attack = false;
        }
    }

    private IEnumerator Insect_honey()
    {
        yield return null;
        //Start Attack
        if (animator.GetBool("Head_attack") == false)
        {
            animator.SetBool("Head_attack", true);
        }
    }
    private IEnumerator mech_laser()
    {
        yield return null;
        //Start Attack
        if (animator.GetBool("Head_attack") == false)
        {
            StartCoroutine(mech_laser_timer());
            animator.SetBool("Head_attack", true);
        }
    }
    private IEnumerator dragon_blast()
    {
        yield return null;
        //Start Attack
        if(animator.GetBool("Head_attack") == false)
        {
            animator.SetBool("Head_attack", true);
        }
    }

    void Started_attack()
    {
        //The attack is Not Finnished
        animator.SetBool("Finnished", false);
    }
    void finnished_attack()
    {
        delay_time = 0f;

        //Attack is finnished
        animator.SetBool("Finnished", true);
        animator.SetBool("Head_attack", false);

        can_attack = true;

        combo_system.next_move();
    }

    private IEnumerator mech_laser_timer()
    {
        Started_attack();
        boss_Movement.Flip_to_player();

        GameObject Laser = Instantiate(projectile, projectile_offset.position, Quaternion.identity);
        Laser.transform.parent = gameObject.transform;
        Laser.GetComponent<Mech_laser>().projectile_offset = projectile_offset;

        while(GameObject.FindGameObjectWithTag("mech_laser") != null)
        {
            yield return null;
        }
        if(GameObject.FindGameObjectWithTag("mech_laser") == null)
        {
            finnished_attack();
        }
    } 

    //Spawn Dragon blast call in animation
    void Create_Dragon_Blast()
    {
        boss_Movement.Flip_to_player();

        GameObject Energy = Instantiate(projectile, projectile_offset.position, Quaternion.identity);
        //Set Energy offset to outside the mouth
        Energy.GetComponent<Dragon_projectile>().projectile_offset = projectile_offset;
    }
    //Spawn Honey Blast Call in animation
    void Create_Honey_Blast()
    {
        boss_Movement.Flip_to_player();

        //create honne on projectile offset
        GameObject honey = Instantiate(projectile, projectile_offset.position, Quaternion.identity);
        honey.GetComponent<Honey_Projectile>().projectile_offset = projectile_offset;
    }
}


