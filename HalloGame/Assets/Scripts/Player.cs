using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;
    float Horizontal;
    float Vertical;
    float turnSmoothVelocity;
    [SerializeField] float smoothTurnTime = 0.1f;
    Vector3 direction;
    [SerializeField] float MovementSpeed = 10;
    Rigidbody rb;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform InstantinPos;
    [SerializeField] Transform gizmosPos;
    float Rayrange = 500f;
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject mermi;
    [SerializeField] Quaternion StayAtRotation;
    [SerializeField] List<GameObject> MyPrefabs;
    GameObject activePrefab;
    [SerializeField] Collider[] Candies;
    float time;
    [SerializeField] LayerMask layer;
    [SerializeField] Transform CandyPos;
    [SerializeField] TextMeshProUGUI candyCountText;
    public TextMeshProUGUI playerHealth;
    public GameObject StartMenu;
    public int Health = 100;
    int candyCount = 0;
    float nextTime=0;
    Vector3 distance;
    Vector3 currentPos;
    float nextCreateTime=0;
    float tim=5;
    Vector3 random;
    Animator anim;
    [SerializeField] GameObject RestartMenu;
    GameObject[] Enemies;
    [SerializeField] TextMeshProUGUI Score;
    bool isLive=true;
    public Material mat;
    public Color defoultColor;
    public Color damageColor;
    private void Awake()
    {
        Time.timeScale = 0f;
        if (Instance==null)
        {
            Instance = this;

        }
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        currentPos = transform.position;
        rb = GetComponent<Rigidbody>();
        //InvokeRepeating("Create", 1f, 0.5f);
        activePrefab = MyPrefabs[0];
        distance=new Vector3(2,0,2);

    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(gizmosPos.position, 2);
    }
    void Update()
    {
        
        
            random = Create();
        
        Candies = Physics.OverlapSphere(gizmosPos.position, 2f, layer);
        foreach (var item in Candies)
        {
            if (item.CompareTag("Candy1"))
            {
                item.transform.DOJump(CandyPos.position, 2, 1, 0.2f).OnComplete(() =>
                {
                    candyCount++;
                    candyCountText.text = candyCount.ToString();
                    Destroy(item.gameObject);
                });
            }
        }
        if (Time.time>nextCreateTime)
        {
            Instantiate(activePrefab,random , transform.rotation);
            nextCreateTime = Time.time+0.9f;
        }
        //RaycastHit hit;
        //if (Physics.Raycast(FirePos.position, transform.forward, out hit, Rayrange))
        //{
        //    Debug.DrawRay(FirePos.posSition, transform.forward * Rayrange, Color.green);
        //    if (hit.transform.CompareTag("Enemy"))
        //    {
        //        Instantiate(mermi,FirePos.position,Quaternion.identity);
        //    }
        //}

        time = Time.time;

        if (time > 45)
        {
            activePrefab = MyPrefabs[1];
        }
        Horizontal = joystick.Horizontal;
        Vertical = joystick.Vertical;
        direction = new Vector3(Horizontal, 0, Vertical);
        if (isLive)
        {
            if (direction.magnitude > 0.01f)
            {
                anim.SetTrigger("Run");
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
                transform.rotation = Quaternion.Euler(0, Angle, 0);
                StayAtRotation = Quaternion.Euler(0, Angle, 0);
                rb.MovePosition(transform.position + (direction * MovementSpeed * Time.fixedDeltaTime));
            }
            else
            {
                anim.SetTrigger("Idle");
                transform.rotation = StayAtRotation;
            }
        }

    }
    Vector3 Create()
    {
       Vector3 inst= new Vector3(Random.Range((transform.position.x-80) - 11, (transform.position.x + 80) + 11), 0,
                Random.Range((transform.position.z -80) - 11, (transform.position.z + 80) + 11));
        return inst;
    }
    private void FixedUpdate()
    {

    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Enemy2"))
        {
            if (Time.time >= nextTime)
            {
                Health -= 5;
                var seq = DOTween.Sequence();
                seq.Append(mat.DOColor(damageColor, "_EmissionColor", 0.15f));
                seq.Append(mat.DOColor(defoultColor, "_EmissionColor", 0.15f));
                
                playerHealth.text = Health.ToString();
                nextTime = Time.time + 0.3f;
                if (Health<=0)
                {
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (var item in Enemies)
                    {
                        item.transform.tag = "Enemy2";
                    }
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy2");
                    foreach (var item in Enemies)
                    {
                        Destroy(item);
                    }
                    nextCreateTime = Time.time+100f;
                    isLive = false;
                    anim.SetTrigger("Dead");
                    
                    StartCoroutine(Dead());
                }
            }
        }
    }
    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(3);
        Score.text=candyCount.ToString();
        candyCountText.transform.parent.gameObject.SetActive(false);
        RestartMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        candyCountText.transform.parent.gameObject.SetActive(true);

        Time.timeScale = 1;
        
    }
   public void startGame()
    {
        StartMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
