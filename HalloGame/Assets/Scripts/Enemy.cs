using DG.Tweening;
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
        agent.speed = 4f;
        //agent.stoppingDistance = 5f;
        count = Counter;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Qilinc") && transform.CompareTag("Enemy"))
        {
            Transform candy = gameObject.transform.GetChild(1);
            candy.parent = null;
            candy.transform.DOScale(1.4f, 0.2f);
            candy.gameObject.layer = 6;
            candy.GetComponent<Rigidbody>().isKinematic = false;

            transform.DOShakeScale(0.2f, 1.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });

            
        }
        if (other.CompareTag("Qilinc")&&transform.CompareTag("Enemy2"))
        {
            transform.DOShakeScale(0.2f, 1.3f).OnComplete(() =>
            {
                Counter++;
                transform.DOScale(1,0.2f);
                
            });
            
        }
        if (Counter>1)
        {
            Transform candy = gameObject.transform.GetChild(1);
            candy.GetComponent<Rigidbody>().isKinematic = false;
            candy.parent = null;
            candy.transform.DOScale(1.4f, 0.2f);
            candy.gameObject.layer = 6;
            
            transform.DOShakeScale(0.2f, 1.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
            
        }


    }



}
