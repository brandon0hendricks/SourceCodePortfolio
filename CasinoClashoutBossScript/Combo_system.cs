using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_system : MonoBehaviour
{
    //Get Boss manager script
    private Boss_Manager boss_Manager;
    //Script to call attacks from
    private Boss_attack boss_Attack;
    //head attack
    //torso attack
    //leg attack

    //gets if head attack is finnished
    private bool head_finnished;
    //gets if torso attack is finnished
    private bool torso_finnished;
    //gets if legs attack is finnished
    private bool legs_finnished;

    private bool combo_finnished;

    //state variables
    private string current_state;
    private int current_move = 1;
    private int function_choice;


    //Timmer variables
    private float cooldown;
    private float current_time;
    //for moves
    private float Delay;
    private float Move_timer;

    //cooldown, dont move
    //In_Attack, doing move
    //choose_attack

    private void Start()
    {
        Get_Componenets();
        current_state = "cooldown";
    }
    public void Update()
    {
        //Run timmers
        timers();

        State_machine();
        
        
    }
    void timers()
    {
        //Timer variables
        current_time += Time.deltaTime;
        Delay += Time.deltaTime;
    }

    void Get_Componenets()
    {
        //Get needed componenets
        boss_Manager = GetComponent<Boss_Manager>();
        boss_Attack = GetComponent<Boss_attack>();
    }

    void State_machine()
    {
        switch (current_state)
        {
            case "cooldown":
                if(current_time >= cooldown)
                {
                    current_move = 1;
                    function_choice = Random.Range(1, 4);
                    current_state = "Choose_Attack";
                }
                break;
            case "Choose_Attack":
                switch(function_choice)
                {
                    case 1:
                        //start light combo
                        light_combo();
                        break;
                    case 2:
                        //start medium_combo
                        medium_combo();
                        break;
                    case 3:
                        //Start Heavy combo
                        Heavy_Combo();
                        break;
                }
                break;
        }
    }
    void light_combo()
    {
        switch (current_move)
        {
            case 1:
                Torso_attack();
                break;
            case 2:
                legs_attack();
                break;
            case 3:
                legs_attack();
                break;
            case 4:
                switch_to_cooldown();
                break;
        }
    }

    void medium_combo()
    {
        switch (current_move)
        {
            case 1:
                Head_attack();
                break;
            case 2:
                Torso_attack();
                break;
            case 3:
                switch_to_cooldown();
                break;
        }
    }

    void Heavy_Combo()
    {
        switch (current_move)
        {
            case 1:
                Head_attack();
                break;
            case 2:
                Head_attack();
                break;
            case 3:
                legs_attack();
                break;
            case 4:
                Torso_attack();
                break;
            case 5:
                switch_to_cooldown();
                break;
        }
    }

    void switch_to_cooldown()
    {
        cooldown = .25f;
        current_time = 0f;
        current_state = "cooldown";
    }
    
    void legs_attack()
    {
        boss_Attack.Legs_attack();
    }
    void Torso_attack()
    {
        boss_Attack.torso_attack();
    }
    void Head_attack()
    {
        boss_Attack.head_attack();
    }

    public void next_move()
    {
        current_move += 1;
    }
}

