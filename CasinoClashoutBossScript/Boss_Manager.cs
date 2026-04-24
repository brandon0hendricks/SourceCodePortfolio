using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEditor;
using UnityEngine;

public class Boss_Manager : MonoBehaviour
{
    //Player Position
    private Transform Player;
    [HideInInspector] public Rigidbody2D rb;

    
    public Stats Stats;

    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject next_level;
    private CinemachineImpulseSource cinemachine_impulse;

    bool death_active;



    [HideInInspector]
    public float base_health;
    [HideInInspector]
    public float base_speed;
    [HideInInspector]
    public float base_dmg;


    //Damage Flash Variables
    private Flash_Damage Flash_Damage;
    private void Awake()
    {
        Next_level_Slot.Boss_Dead = false;
    }
    private void Start()
    {
        Stats.can_take_damage = true;
        death_active = false;

        Get_Components();
    }

    void Get_Components()
    {
        rb = GetComponent<Rigidbody2D>();
        Flash_Damage = GetComponent<Flash_Damage>();
        cinemachine_impulse = gameObject.GetComponent<CinemachineImpulseSource>();
    }


    private void FixedUpdate()
    {
        if(Stats.health_stat <= 0 & death_active == false)
        {
            StartCoroutine(Death());
        }
    }

    //Combining stats, replace with Limb stats.
    public void set_Stats(GameObject head, GameObject torso, GameObject legs)
    {
        //get script stats
        torso_Stats t_stats = torso.GetComponent<torso_Stats>();
        Leg_Stats l_stats = legs.GetComponent<Leg_Stats>();
        Head_stats h_stats = head.GetComponent<Head_stats>();

        Stats.Set_limbs(h_stats, t_stats, l_stats);

        base_stats();
    }

    void base_stats()
    {
        //set base values
        base_health = Stats.health_stat;
        base_speed = Stats.speed_stat;
        base_dmg = Stats.dmg_stat;
    }

    //Finds Player
    public Transform get_Player()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        return Player;
    }

    private IEnumerator Death()
    {
        Destroy(gameObject.GetComponent<Combo_system>());
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        death_active = true;
        float Speed_timer = .5f;
        for(int i = 0; i < 35; i++)
        {
            Camera_Shake.instance.camera_shake(cinemachine_impulse);
            float Spawn_adjustment_x = Random.Range(-2f, 2f);
            float Spawn_adjustment_y = Random.Range(-2f, 2f);

            GameObject Explosion_three = Instantiate(explosion, new Vector2(transform.position.x + Spawn_adjustment_x, transform.position.y + Spawn_adjustment_y), Quaternion.identity);
            yield return new WaitForSeconds(.05f);

            Spawn_adjustment_x = Random.Range(-2.5f, 2.5f);
            Spawn_adjustment_y = Random.Range(-2.5f, 2.5f);

            GameObject Explosion_two = Instantiate(explosion, new Vector2(transform.position.x - (Spawn_adjustment_x), transform.position.y + (Spawn_adjustment_y)), Quaternion.identity);
            yield return new WaitForSeconds(Speed_timer);
            Speed_timer /= 2f;
        }
        Next_level_Slot.Boss_Dead = true;
        Destroy(gameObject);
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && Stats.can_take_damage == true)
        {
            Stats.Take_Damage(2.5f);
            Camera_Shake.instance.camera_shake(cinemachine_impulse);
            StartCoroutine(Stats.Delay_Damage());
            Flash_Damage.Call_flash();
        }
    }



}

[System.Serializable]
public class Stats
{
    //base stats, shown in inspector for debugging
    public float health_stat;
    [HideInInspector]
    public float speed_stat;
    [HideInInspector]
    public float dmg_stat;

    public bool can_take_damage;
    private float Damage_delay = .35f;

    //gameobjects for boss being used
    [HideInInspector]
    public Head_stats head_stats;
    [HideInInspector]
    public torso_Stats torso_stats;
    [HideInInspector]
    public Leg_Stats leg_stats;


    //sets health,speed,dmg stat
    public void Set_Stats()
    {
        health_stat = torso_stats.max_health + leg_stats.max_health + head_stats.max_health;
        speed_stat = torso_stats.speed + leg_stats.speed + head_stats.speed;
        dmg_stat = torso_stats.base_dmg + leg_stats.base_dmg + head_stats.base_dmg;
    }

    //sets limbs for stats
    public void Set_limbs(Head_stats h_stats, torso_Stats t_stats, Leg_Stats l_stats)
    {
        head_stats = h_stats;
        torso_stats = t_stats;
        leg_stats = l_stats;
        Set_Stats();
    }

    //only us for current health stats
    public void Take_Damage(float dmg)
    {
        if(can_take_damage == true)
        {
            health_stat -= dmg;
        }
    }

    public IEnumerator Delay_Damage()
    {
        can_take_damage = false;
        Debug.Log(can_take_damage);
        yield return new WaitForSeconds(Damage_delay);
        can_take_damage = true;
    }

    public void change_speed(float change)
    {
        
        speed_stat = change;
    }


}
