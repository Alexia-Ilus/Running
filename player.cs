using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    private float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeight;
    public int maxLife = 3;
    private float minSpeed = 10f;
    private float maxSpeed = 30f;
    public float invincibleTime;
    public GameObject model;
    public GameObject fase1;
    public GameObject fase2;
    public GameObject fase3;
    public GameObject barreira1;
    public GameObject barreira2;
    public GameObject barreira3;
    public Text point;
    public Text YouWin;
    public Text perdeu;
    public GameObject opaco;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    public int currentLife;
    private bool invincible = false;
    public int pontos;
    //public Text point;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentLife = maxLife;

        speed = minSpeed;

        pontos = 0;
        fase2.SetActive(false);
        opaco.SetActive(false);
        fase3.SetActive(false);
        YouWin.enabled = false;
        perdeu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1f)
            {
                jumping = false;
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }


        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }

    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 2)
            return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            jumping = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("obstaculo"))
        {
            canMove = false;
            currentLife--;
            speed = 0;
                if (currentLife <= 0)
            {
                perdeu.enabled = true;
                transform.localPosition = new Vector3(0, 3, -47);
                fase1.SetActive(false);
                fase2.SetActive(false);
                fase3.SetActive(false);
                opaco.SetActive(true);

            }
            else
            {
                Invoke("CanMove", 0.75f);
                StartCoroutine(Blinking(invincibleTime));
            }
        }

        if (other.gameObject.CompareTag("item"))
        {
            //other.GetComponent<AudioSource>().Play();
            pontos++;
            other.GetComponent<Renderer>().enabled = false;
            other.GetComponent<BoxCollider>().enabled = false;
            Destroy(other.gameObject, 2);
        }

        if (other.CompareTag("fase1"))
        {
            if (fase1.active)
            {
                transform.localPosition = new Vector3(0, 3, -47);
                fase1.SetActive(false);
                fase2.SetActive(true);
                barreira2.SetActive(true);
            }
        }
        if (other.CompareTag("fase2"))
        {
            if (fase2.active)
            {
                transform.localPosition = new Vector3(0, 3, -47);
                fase2.SetActive(false);
                fase3.SetActive(true);
                barreira3.SetActive(true);
            }
        }

        if (other.CompareTag("fase3"))
        {
            if (fase3.active)
            {
                fase3.SetActive(false);
                YouWin.enabled = true;
                opaco.SetActive(true);
            }
        }

        point.text = "Score: " + pontos.ToString();
    }

    IEnumerator Blinking(float time)
        {
            invincible = true;
            float timer = 0;
            float currentBlink = 1f;
            float lastBlink = 0;
            float blinkPeriod = 0.1f;
            bool enabled = false;
            yield return new WaitForSeconds(1f);
            speed = minSpeed;
            while (timer < time && invincible)
            {
                model.SetActive(enabled);
                yield return null;
                timer += Time.deltaTime;
                lastBlink += Time.deltaTime;
                if (blinkPeriod < lastBlink)
                {
                    lastBlink = 0;
                    currentBlink = 1f - currentBlink;
                    enabled = !enabled;
                }
            }
        model.SetActive(true);
            invincible = false;
        }

    void CanMove()
    {
        canMove = true;
    }

    void GameOver()
    {

    }
}
