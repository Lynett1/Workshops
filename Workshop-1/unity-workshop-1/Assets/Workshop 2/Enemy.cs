using System.Numerics;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D player;
    private Rigidbody2D rb;
    [SerializeField] private bool Scared;
    public GameObject bucketPrefab;
    private float distance;
    [SerializeField] private GameObject attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);
        if (distance < 1f)
        {

            StartCoroutine(Attacking());
        }
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
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
            
    }

    private IEnumerator TeammateDied()
    {
        yield return new WaitForSeconds(1);
        CustomEvent.Trigger(this.gameObject, "TeammateDied");
        Debug.Log("TeammateDied");
    }

    void Die()
    {
        Destroy(this.gameObject);
        Instantiate(bucketPrefab, transform.position, transform.rotation);
    }

    private IEnumerable chase()
    {
        while(true)
        {
            Vector2 direction = (player.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
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
            Vector2 direction = (player.position - rb.position).normalized;
            rb.MovePosition(rb.position - direction * (speed/2) * Time.deltaTime);            
        }

    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        CustomEvent.Trigger(this.gameObject, "CountedDown");
        Debug.Log("CountedDown");
        Scared = false;
    }

}
