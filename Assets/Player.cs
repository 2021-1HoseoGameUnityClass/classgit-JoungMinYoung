using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float jumpForce = 300f;
    
    private bool isJump = false;

    [SerializeField]
    private GameObject bulletPos = null;
    
    [SerializeField]
    private GameObject bulletobj = null;

    private bool move = false;
    private float moveHorizonral = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move == true)
        {
            Move();
        }

        if(Input.GetButtonDown("Jump"))
        {
            PlayerJump();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Move()
    {
        //float h = Input.GetAxis("Horizontal");
        float h = moveHorizonral;
        float playerSpeed = h * moveSpeed * Time.deltaTime;
        Vector3 vector3 = new Vector3();
        vector3.x = playerSpeed;
        transform.Translate(vector3);

        if (h < 0)
        {
            GetComponent<Animator>().SetBool("Walk", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (h == 0)
        {
            GetComponent<Animator>().SetBool("Walk", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walk", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void PlayerJump()
    {
        if(isJump == false)
        {
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Animator>().SetBool("Jump", true);

            Vector2 vector2 = new Vector2(0, jumpForce);
            GetComponent<Rigidbody2D>().AddForce(vector2);
            isJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Platform")
        {
            GetComponent<Animator>().SetBool("Jump", false);
            isJump = false;
        }
    }

    private void Fire()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("RangedAttack") as AudioClip;
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();
        float direction = transform.localPosition.x;
        Quaternion quaternion = new Quaternion(0, 0, 0, 0);
        Instantiate(bulletobj, bulletPos.transform.position, quaternion).GetComponent<Bullet>().InstantiateBullet(direction);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            DataManager.instance.playerHP -= 1;
            if(DataManager.instance.playerHP < 0)
            {
                DataManager.instance.playerHP = 0;
            }
            UIManager.instance.PlayerHP();
        }
    }

    public void OnMove(bool _right)
    {
        if(_right)
        {
            moveHorizonral = 1;
        }
        else
        {
            moveHorizonral = -1;
        }

        move = true;
    }

    public void OffMove()
    {
        moveHorizonral = 0;
        move = false;
    }
}
