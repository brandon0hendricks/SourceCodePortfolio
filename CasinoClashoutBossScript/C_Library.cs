using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C_Library : MonoBehaviour
{
    public List<Creature_ID_List> Creature_Data = new List<Creature_ID_List>();
}

[System.Serializable]
public class Creature_ID_List
{
    public GameObject Head;
    public GameObject Torso;
    public GameObject Legs;
}
