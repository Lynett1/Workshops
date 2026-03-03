using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody player;
    private Rigidbody rb; 
    //[SerializeField] private bool Scared;
    public GameObject bucketPrefab;
    private float distance;
    [SerializeField] private GameObject attack;
    [SerializeField] private int health = 2;

    private Coroutine chaseCoroutine;
    private Coroutine scaredCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        chaseCoroutine = StartCoroutine(chase());
    }

    void LateUpdate()
    {/*
        if (Scared)
        {
            StartCoroutine(Scared());
            StartCoroutine(CountDown());
        }
        else if (!Scared)
        {
            StopCoroutine(Scared());
            StopCoroutine(CountDown());
            StartCoroutine(chase());
        }
            */
        if (health <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Attacking());
        }
        else if (other.gameObject.CompareTag("PlayerAttack"))
        {
            health--;
        }
    }

    public void TriggerTeammateDied()
    {
        StartCoroutine(TeammateDiedRoutine());
    }

    private IEnumerator TeammateDiedRoutine()
    {
        CustomEvent.Trigger(this.gameObject, "TeammateDied");
        Debug.Log("TeammateDied");
        
        if (chaseCoroutine != null) StopCoroutine(chaseCoroutine);
        scaredCoroutine = StartCoroutine(Scared());
        
        yield return new WaitForSeconds(1);
        
        if (scaredCoroutine != null) StopCoroutine(scaredCoroutine);
        chaseCoroutine = StartCoroutine(chase());
    }

    void Die()
    {
        Destroy(this.gameObject);
        Instantiate(bucketPrefab, transform.position, transform.rotation);
    }

    private IEnumerator chase()
    {
        while(true)
        {
            Vector3 direction = (player.position - rb.position).normalized;
            rb.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1);
        attack.SetActive(true);
        yield return new WaitForSeconds(1);
        attack.SetActive(false);
    }

    private IEnumerator Scared()
    {
        while(true)
        {
            Vector3 direction = (player.position - rb.position).normalized;
            rb.AddForce(-direction * (speed/2) * Time.deltaTime, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }

    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        CustomEvent.Trigger(this.gameObject, "CountedDown");
        Debug.Log("CountedDown");
    }

}
