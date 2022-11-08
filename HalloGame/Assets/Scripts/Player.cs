using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] Transform FirePos;
    float Rayrange = 500f;
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject mermi;
    [SerializeField] Quaternion StayAtRotation;
    [SerializeField] List<GameObject> MyPrefabs;
    GameObject activePrefab;
    float time;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("Create", 1f, 5f);
        activePrefab = MyPrefabs[0];

    }

    // Update is called once per frame
    void Update()
    {

        //RaycastHit hit;
        //if (Physics.Raycast(FirePos.position, transform.forward, out hit, Rayrange))
        //{
        //    Debug.DrawRay(FirePos.position, transform.forward * Rayrange, Color.green);
        //    if (hit.transform.CompareTag("Enemy"))
        //    {
        //        Instantiate(mermi,FirePos.position,Quaternion.identity);
        //    }
        //}

        time = Time.time;

        if (time > 15)
        {
            activePrefab = MyPrefabs[1];
        }
        Horizontal = joystick.Horizontal;
        Vertical = joystick.Vertical;
        direction = new Vector3(Horizontal, 0, Vertical);
        if (direction.magnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0, Angle, 0);
            StayAtRotation = Quaternion.Euler(0, Angle, 0);
            rb.MovePosition(transform.position + (direction * MovementSpeed * Time.fixedDeltaTime));
        }
        else
        {
            transform.rotation = StayAtRotation;
        }

    }
    void Create()
    {
        Instantiate(activePrefab, new Vector3(Random.Range(-100f, 100f), 1, Random.Range(-100f, 100f)), transform.rotation);
    }
    private void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }

        
    }
    
}
