using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    public int Counter = 0;
    public int count;
    public static Enemy Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;
        agent.speed = 5f;
        agent.stoppingDistance = 5f;
        count = Counter;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&transform.CompareTag("Enemy2"))
        {
            Counter++;
        }
        if (Counter>1)
        {
            Destroy(gameObject);
        }


    }



}
