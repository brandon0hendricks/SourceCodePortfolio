using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_attack : MonoBehaviour
{
    private Boss_Manager boss_Manager;
    private Boss_Movement boss_Movement;

    private Stats Stats;
    //bool to check is movement is finnished
    private bool movement_finnished;


    private Transform player;

    
    public bool torso_attack_finnished;
      
    public bool head_attack_finnished;
    
    public bool legs_attack_finnished;
    // Start is called before the first frame update
    void Start()
    {
        Get_componenets();
    }

    private void Update()
    {
        Get_Finnished_variables();
    }

    void Get_Finnished_variables()
    {
        //checks if head Attack is finnished
        head_attack_finnished = Stats.head_stats.animator.GetBool("Finnished");
        //Checks if torso attack is finnished
        torso_attack_finnished = Stats.torso_stats.animator.GetBool("Finnished");
        //Checks if legs are finnished
        legs_attack_finnished = Stats.torso_stats.animator.GetBool("Finnished");
    }

    void Get_componenets()
    {
        //set get boss manager script
        boss_Manager = GetComponent<Boss_Manager>();
        boss_Movement = GetComponent<Boss_Movement>();

        //get stats from boss manager
        Stats = boss_Manager.Stats;

        //find player position
        player = boss_Manager.get_Player();
    }


    public void torso_attack()
    {
        torso_Stats Torso_sats = Stats.torso_stats;
        Torso_sats.limb_attack();    
    }
    //use head attack
    public void head_attack()
    {
        Head_stats Head_stats = Stats.head_stats;
        Head_stats.limb_attack();
    }
    //Use Legs attack
    public void Legs_attack()
    {
        Leg_Stats Leg_stats = Stats.leg_stats;
        Leg_stats.limb_attack();
    }
}
