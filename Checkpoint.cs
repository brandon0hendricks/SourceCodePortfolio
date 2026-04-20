using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Checkpoint : MonoBehaviour
{
    // This script handles the bools and visuals of the checkpoint system

    private PlayerMovement playerMovement;

    private bool eyeball = false;

    [SerializeField] private GameObject eyeballPrefab;

    private Animator anim;
    private Animator eyeballAnim;

    private GameObject eye;

    [HideInInspector] public bool cameraOn;
    [HideInInspector] public AudioObject clockNoise;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        if (!eyeball)
            SpawnPartner();
    }

    private void FixedUpdate()
    {
        if (!eyeball)
        CameraCheck();
    }

    private void Update()
    {
        if (playerMovement.canSave)
            cameraOn = true;
        else
            cameraOn = false;

        if (!eyeball)
        {
            if (eyeballAnim == null)
            {
                Debug.Log("Cannot find animator");
                eyeballAnim = eye.GetComponent<Animator>();
            }
        }
    }

    private void CameraCheck()
    {
        if (cameraOn)
            anim.SetBool("CameraOn", true);
        else if (!cameraOn)
            anim.SetBool("CameraOn", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !eyeball)
            anim.SetTrigger("WakeUp");
        playerMovement.canSave = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerMovement.canSave = false;
    }

    private void SpawnPartner()
    {
        if (!eyeball)
        {
            GameObject realm1 = GameObject.FindGameObjectWithTag("Realm1");
            GameObject realm2 = GameObject.FindGameObjectWithTag("Realm2");

            Vector2 relativePos = transform.position - realm1.transform.position;
            eye = Instantiate(eyeballPrefab, new Vector3(realm2.transform.position.x + relativePos.x, realm2.transform.position.y + relativePos.y, 0f), Quaternion.identity);
            Checkpoint eyeballScript = eye.GetComponent<Checkpoint>();
            eyeballScript.eyeball = true;
            eyeballAnim = eye.GetComponent<Animator>();
        }
    }
    public void CrowNoise()
    {
        SoundManager.PlaySound(SoundType.crow, 0.1f);
    }
}
