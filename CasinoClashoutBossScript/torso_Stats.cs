using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torso_Stats : MonoBehaviour
{
    [Header("Snap Positions")]
    public Transform Head_Snap;
    public Transform Legs_Snap;

    [Header("Limb Stats")]
    public float base_dmg;
    public float max_health;
    public float speed;

    //gets animator for the torso
    [HideInInspector]
    public Animator animator;
    private Boss_Movement boss_Movement;
    private GameObject boss_origin;

    [Header("attack variables/components")]
    [SerializeField] private GameObject[] hitBox;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectile_offset;
    [Header("1) Insect Slash     2) Dragon Counter      3) Mech Shoot Or Mech Drill")]
    [SerializeField] private int Limb_attack;
    //1: insect slash
    //2: dragon counter
    //3: mech shoot


    private bool can_attack;
    private float Defense_Cooldown;
    private float Delay_Time;
    private Combo_system combo_system;

    private void Start()
    {
        Get_components();
        can_attack = true;
    }

    void Get_components()
    {
        //Gets needed Boss manager Scripts
        boss_origin = GameObject.FindGameObjectWithTag("Boss_Manger");
        boss_Movement = boss_origin.GetComponent<Boss_Movement>();
        combo_system = boss_origin.GetComponent<Combo_system>();

        //get animator for limb part
        animator = GetComponent<Animator>();

        hit_box_false();

    }
    private void Update()
    {
        Delay_Time += Time.deltaTime;
        Defense_Cooldown += Time.deltaTime;
    }

    public void limb_attack()
    {     
        if(can_attack == true && Delay_Time >= 1f)
        {
            switch (Limb_attack)
            {
                case 1:
                    StartCoroutine(Insect_slash());
                    break;
                case 2:
                    StartCoroutine(dragon_counter());
                    break;
                case 3:
                    StartCoroutine(mech_Attack());
                    break;
            }
            can_attack = false;
        }
    }

    private IEnumerator Insect_slash()
    {

        yield return StartCoroutine(boss_Movement.towards_Player(4f));
        //Start Attack
        if (animator.GetBool("Torso_attack") == false)
        {
            animator.SetBool("Torso_attack", true);
        }
    }
    private IEnumerator dragon_counter()
    {
        yield return null;
        if(GameObject.FindGameObjectWithTag("Sheild") == null && Defense_Cooldown >= 11f)
        {
            Defense_Cooldown = 0f;
            Create_sheild();
        }
        finnished_attack();

    }
    private IEnumerator mech_Attack()
    {
        int choose_Attack = Random.Range(1, 3);
        
        if (choose_Attack == 2)
        {
            //if mech attack is melee move to player first
            yield return StartCoroutine(boss_Movement.towards_Player(4f));
        }
        if (choose_Attack == 1)
        {
            //if mech attack is melee move to player first
            yield return StartCoroutine(boss_Movement.Jump_away());
        }
        //do attack chosen
        if (animator.GetBool("Torso_attack") == false)
        {
            animator.SetBool("Torso_attack", true);
            animator.SetInteger("Attack_Value", choose_Attack);
           
        }
    }

    void Create_Mech_Projectile()
    {
        boss_Movement.Flip_to_player();
        //create honne on projectile offset
        GameObject missle = Instantiate(projectile, projectile_offset.position, Quaternion.identity);
    }

    void Create_sheild()
    {
        GameObject sheild = Instantiate(projectile, transform.position, Quaternion.identity);
        sheild.GetComponent<Dragon_sheild>().boss = boss_origin;
    }

    //call in animation, sets attack hit boxes to true
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

    void Started_attack()
    {
        //The attack is Not Finnished
        animator.SetBool("Finnished", false);
    }

    //call at end of animation
    void finnished_attack()
    {
        //Restart Delay
        Delay_Time = 0f;

        //Animation Variables
        animator.SetBool("Finnished", true); 
        animator.SetBool("Torso_attack", false);

       
        can_attack = true;

        //goto next move 
        combo_system.next_move();
    }
}
