using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator _animator; //animator 
    [HideInInspector] public Rigidbody2D _rb; //rigidbody for physics

    //Input Actions
    [SerializeField] private InputActionReference _moveAction;
    private Vector2 _moveInput;
    [SerializeField] private InputActionReference _moveObjectAction;
    private float _moveObjectVar;

    //Object Movement
    [HideInInspector] public bool _moveObject = false;
    [HideInInspector] public bool _pullingObject = false;
    [HideInInspector] public bool _pushingObject = false;
    private float startingPoint;
    private GameObject _object;
    [HideInInspector] public string _objectName;
    private Rigidbody2D _objectRb;

    //Checkpoint
    [HideInInspector] public bool canSave = false;
    [SerializeField] private GameObject saveTextObject;
    private TMP_Text saveText;
    private Vector2 saveLoc;
    private SceneSwitcher sceneSwitch;

    //horizontal movement
    private float _speed; //current player speed
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _baseSpeed;
    public bool _sprint; //if player is current sprinting

    //jumping variables
    [SerializeField] private float _jumpPower; 
    private float _coyoteTime = .2f; 
    private float _coyoteTimeCounter;


    [SerializeField] public Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    public bool _isFacingRight = true;

    [HideInInspector] public GameObject realm;

    [SerializeField] private SpriteRenderer sprite;

    [HideInInspector] public Collider2D col;
    [SerializeField] private PhysicsMaterial2D frictionless;
    [SerializeField] private PhysicsMaterial2D playerMat;

    private bool justStarting = true;

    //Sounds
    private AudioObject dragBoxRealm1;
    private AudioObject dragBoxRealm2;

    public CinemachineFollow Vcam;

    // Start is called before the first frame update
    void Start()
    {
        getComponents();
        _speed = _baseSpeed;
    }

    void FixedUpdate()
    {
        if(DialogueBox.inDialogue == false)
        {
            Jump();
            Moving();
        }
        else
        {
            _animator.SetBool("moving", false);
        }
        MoveObject();
        ObjectDirection();
        RealmDetector();
        CheckpointSystem();
        ChangeFriction();
    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log(realm.name);

        _moveInput = _moveAction.action.ReadValue<Vector2>(); //get input from the input system
        _moveObjectVar = _moveObjectAction.action.ReadValue<float>(); //same ^

        _animator.SetBool("Grounded", IsGrounded());
        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }


        if ((TransitionAnimEvents.loadGame && canSave) || justStarting)
        {
            //RunSave();
        }
        if (_objectRb != null)
            Debug.Log(_objectRb.linearVelocity.x);
    }
    void getComponents()
    {
        _rb = GetComponent<Rigidbody2D>();
        saveText = saveTextObject.GetComponent<TMP_Text>();
        sceneSwitch = gameObject.GetComponent<SceneSwitcher>();
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    void ChangeFriction()
    {
        if (IsGrounded())
        {
            col.sharedMaterial = playerMat;
        }
        else if(!IsGrounded())
        {
            col.sharedMaterial = frictionless;
        }
    }

    void Moving()
    {
         _speed = _baseSpeed;
        

        if (_moveInput.x != 0f)
        {
            _animator.SetBool("moving", true);
            UpdateFlip();
        }
        else
        {
            _animator.SetBool("moving", false);
        }

        if (_animator.GetBool("moving") == true)
        {
            //moves player
            _rb.linearVelocity = new Vector2(_moveInput.x * _speed, _rb.linearVelocity.y);
        }
    }
    void Jump()
    {
        _animator.SetFloat("Y_velocity", _rb.linearVelocity.y);
        //Jump Mechanic
        if (_moveInput.y >= .5f  && IsGrounded()) //jump is pressed/is grounded
        {
            if (_coyoteTimeCounter > 0f) //still has time to jump
            {
                Debug.Log("Jump initated");
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpPower); //jump action
            }
        }

        //Fast Falling
        float fastFall = 1.3f;
        if (!IsGrounded() && _rb.linearVelocity.y < 0f)
        {
            //Debug.Log("Fast Fall activated");
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fastFall) * Time.deltaTime;
        }
    }

    void MoveObject()
    {
        //Debug.Log(_pushingObject + "pull:" + _pullingObject + "obj: " + _objectName);
        if (_object != null && _moveObjectVar > 0 && _pushingObject)
        {
            if (realm.name == "Realm1Background" && dragBoxRealm1 == null && Mathf.Abs(_objectRb.linearVelocityX) >= 0.5f)
            {
                dragBoxRealm1 = SoundManager.PlayAudioObject(SoundType.boxDrag, _object.transform.position, 0.5f, true);
            }
            if (realm.name == "Realm2Background" && dragBoxRealm2 == null && Mathf.Abs(_objectRb.linearVelocityX) >= 0.5f)
            {
                dragBoxRealm2 = SoundManager.PlayAudioObject(SoundType.QboxDrag, _object.transform.position, 0.5f, true);
            }
            _moveObject = true;
            _objectRb.linearVelocity = new Vector2(_moveInput.x * (_speed * 0.9f), _objectRb.linearVelocity.y);
        }
        else if (_object != null && _moveObjectVar > 0 && _pullingObject)
        {
            if (realm.name == "Realm1Background" && dragBoxRealm1 == null && Mathf.Abs(_objectRb.linearVelocityX) >= 0.5f)
            {
                dragBoxRealm1 = SoundManager.PlayAudioObject(SoundType.boxDrag, _object.transform.position, 0.5f, true);
            }
            if (realm.name == "Realm2Background" && dragBoxRealm2 == null && Mathf.Abs(_objectRb.linearVelocityX) >= 0.5f)
            {
                dragBoxRealm2 = SoundManager.PlayAudioObject(SoundType.QboxDrag, _object.transform.position, 0.5f, true);
            }
            _moveObject = true;
            _objectRb.linearVelocity = new Vector2(_moveInput.x * (_speed * 1.1f), _objectRb.linearVelocity.y);
        }
        else if (_object == null)
        {
            _moveObject = false;
            SoundManager.KillSoundEarly(dragBoxRealm1);
            SoundManager.KillSoundEarly(dragBoxRealm2);
        }

        if (_object != null && _moveObjectVar == 0)
        {
            _objectRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            SoundManager.KillSoundEarly(dragBoxRealm1);
            SoundManager.KillSoundEarly(dragBoxRealm2);
        }
    }

    void CheckpointSystem()
    {
        if (canSave)
        {
            saveTextObject.SetActive(true);
            if (realm.name == "Realm1Background")
            {
                saveText.text = "Press C To Save";
            }
            else if (realm.name == "Realm2Background")
            {
                saveText.text = "Press C to load";
            }
        }
        else if (!canSave)
        {
            saveTextObject.SetActive(false);
        }
    }

    public void RunSave()
    {
        justStarting = false;
        Debug.Log("Save ran on Player!");
        if (realm.name == "Realm1Background")
        {
            saveLoc = transform.position;
        }
        else if (realm.name == "Realm2Background")
        {
            SceneSwitcher.realm = true;
            StartCoroutine(Camera());
            transform.position = saveLoc;
        }
    }

    void RealmDetector()
    {
        col = GetComponent<Collider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.size,
            0f);

        foreach (var hit in hits)
        {
            if (hit == hit.CompareTag("Realm1") || hit == hit.CompareTag("Realm2"))
                realm = hit.gameObject;
        }
    }

    void ObjectDirection()
    {
        if (_object != null)
        {
            if (_objectRb.linearVelocity.x == 0f)
            {
                startingPoint = _object.transform.position.x;
            }

            if (_object.transform.position.x >= transform.position.x && _object.transform.position.x >= startingPoint || 
                _object.transform.position.x <= transform.position.x && _object.transform.position.x <= startingPoint)
            {
                _pushingObject = true;
                _pullingObject = false;
                //Debug.Log("Pushing!");
            }
            else if (_object.transform.position.x < transform.position.x && _object.transform.position.x > startingPoint ||
                _object.transform.position.x > transform.position.x && _object.transform.position.x < startingPoint)
            {
                _pushingObject = false;
                _pullingObject = true;
                //Debug.Log("Pulling!");
            }
            //Debug.Log("Starting Point: " + startingPoint + " Object X: " + _object.transform.position.x + " player x: " + transform.position.x);
        }
        else
        {
            _pushingObject = false;
            _pullingObject = false;
        }
    }

    private void UpdateFlip()
    {
        if (_isFacingRight && _moveInput.x < 0f || !_isFacingRight && _moveInput.x > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = sprite.transform.localScale;
            localScale.x *= -1f;
            sprite.transform.localScale = localScale;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Object"))
        {
            _object = collision.gameObject;
            _objectName = _object.name;
            _objectRb = collision.collider.attachedRigidbody;
            Debug.Log("Touching object: " + _object.name);
        }
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Object"))
        {
            _object = null;
            _objectRb = null;
        }
    }

    public IEnumerator Camera()
    {
        Vector3 previousCamDamp = Vcam.TrackerSettings.PositionDamping;
        Vcam.TrackerSettings.PositionDamping = Vector3.zero;
        yield return new WaitForSeconds(.5f);
        Vcam.TrackerSettings.PositionDamping = previousCamDamp;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.05f, _groundLayer);
    }

    public bool IsGroundedButHigher()
    {
        return Physics2D.OverlapCircle(new Vector2(_groundCheck.position.x, (_groundCheck.position.y + 0.35f)), 0.05f, _groundLayer);
    }
}
