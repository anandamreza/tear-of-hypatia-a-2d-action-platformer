using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    private Animator anim;
    private bool enemyGo;
    private bool enemyChase;
    private int destination;

    private float size_x = 3;
    private float size_y = 3;
    private float size_z = 3;

    [Header("Enemey Movement")]
    public Transform[] patrolPoint;
    public float moveSpeed;
    public int destinationWhere;
    public float idleTime;

    [Header("Enemy Chasing")]
    private Transform playerTransform;
    public float chaseSpeed;
    public float chaseDistance;

    [Header("Enemy Attack")]
    public int damage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void Start()
    {
        destination = destinationWhere;
        enemyGo = true;
        enemyChase = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance && enemyChase == true)
        {
            anim.SetBool("IsEnemyChasing?", true);
            //Debug.Log("chaseMode activated\n");

            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(size_x, size_y, size_z);
                transform.position += Vector3.left * chaseSpeed * Time.deltaTime;
                //Debug.Log("Enemy is chasing to the left...\n");

                StartCoroutine(enemyAttack());
            }
            else if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-size_x, size_y, size_z);
                transform.position += Vector3.right * chaseSpeed * Time.deltaTime;
                //Debug.Log("Enemy is chasing to the right...\n");

                StartCoroutine(enemyAttack());
            }

        }
        else
        {
            if(destination == 3)
            {
                enemyGo = false;
            }

            anim.SetBool("IsEnemyChasing?", false);
            if (destination == 0 && enemyGo == true)
            {
                anim.SetBool("IsEnemyMoving?", true);
                transform.localScale = new Vector3(size_x, size_y, size_z);
                transform.position = Vector2.MoveTowards(transform.position, patrolPoint[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoint[0].position) < 0.2f)
                {
                    StartCoroutine(reachPoint());
                }
            }
            else if (destination == 1 && enemyGo == true)
            {
                transform.localScale = new Vector3(-size_x, size_y, size_z);
                transform.position = Vector2.MoveTowards(transform.position, patrolPoint[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoint[1].position) < 0.2f)
                {
                    StartCoroutine(reachPoint());
                }
            }
        }
    }
    private IEnumerator reachPoint()
    {
        //Debug.Log("test : ");
        anim.SetBool("IsEnemyMoving?", false);
        enemyGo = false;
        yield return new WaitForSeconds(idleTime);
        destination = destination == 0 ? 1 : 0;

        enemyGo = true;
        anim.SetBool("IsEnemyMoving?", true);
    }

    private IEnumerator enemyAttack()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < 1f)
        {
            anim.SetBool("IsEnemyAttacking?", true);
            enemyGo = false;
            enemyChase = false;
            yield return new WaitForSeconds(0.01f);
        }
        else
        {
            anim.SetBool("IsEnemyAttacking?", false);
        }
        
        enemyGo = true;
        enemyChase = true;
    }
}