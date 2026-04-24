using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Creature : MonoBehaviour
{
    //ID used for limbs

    [SerializeField] static public int HeadID = 1;
    [SerializeField] static public int TorsoID= 1;
    [SerializeField] static public int LegsID = 1;

    private Transform head_s;
    private Transform legs_s;


    //refrence creature management Data
    public C_Library ID_data;
    private Boss_Manager Boss_Manager;

    void Awake()
    {
        Boss_Manager = gameObject.GetComponent<Boss_Manager>();
        //create limbs
        Create_Limbs();
    }

    public void Create_Limbs()
    {
        //create torso, grab torso data from C_Library, make child of boss manager
        GameObject Torso = Instantiate(ID_data.Creature_Data[TorsoID].Torso, transform.position, Quaternion.identity);
        Torso.transform.parent = gameObject.transform;

        //get torso stats
        torso_Stats t_stats = Torso.GetComponent<torso_Stats>();


        //Get spawn location for head and legs
        head_s = t_stats.Head_Snap;
        legs_s = t_stats.Legs_Snap;

        //create Head, grab Head data from C_Library, make child of boss manager, connects head to torso
        GameObject Head = Instantiate(ID_data.Creature_Data[HeadID].Head, head_s.position, Quaternion.identity);
        Head.transform.parent = Torso.transform;

        //create Legs, grab leg data from C_Library, make child of boss manager, connects head to torso
        GameObject Legs = Instantiate(ID_data.Creature_Data[LegsID].Legs, legs_s.position, Quaternion.identity);
        Legs.transform.parent = Torso.transform;

        //gets stats created
        Head_stats h_stats = Head.GetComponent<Head_stats>();
        Leg_Stats l_stats = Legs.GetComponent<Leg_Stats>();

        Boss_Manager.set_Stats(Head,Torso,Legs);
    }

   
}
