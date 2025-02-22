using System.Collections;
using UnityEngine;

public class Tears : MonoBehaviour
{
    private Rigidbody2D playerRB;
    public int damage;
    [SerializeField] private bool isRight;
    [SerializeField] private bool isUp;
    [SerializeField] private int speed;
    [SerializeField] private float collisionSize;
    [SerializeField] private float offset;
    [SerializeField] private float maxDistance;
    [SerializeField] private float curDistance = 0;
    private bool stop = false;

    public float forceOfOutput;
    private void Awake()
    {
        playerRB = MovementController.instance.GetComponent<Rigidbody2D>();
        damage = GameManager.instance.damageDist;
    }
    void FixedUpdate()
    {
        Collider2D touch = null;
        //�����������
        var realspeed = speed * Time.fixedDeltaTime;
        if (!stop)
        {
            if (!isUp)
            {
                if (isRight)
                {
                    transform.position = transform.position = new Vector2(transform.position.x + realspeed, transform.position.y); ;
                    touch = Physics2D.OverlapCircle(transform.position + new Vector3(offset, 0f, 0f), collisionSize);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x - realspeed, transform.position.y);
                    touch = Physics2D.OverlapCircle(transform.position - new Vector3(offset, 0f, 0f), collisionSize);
                }
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + realspeed);
                touch = Physics2D.OverlapCircle(transform.position + new Vector3(0f, offset, 0f), collisionSize);
            }
            curDistance += realspeed;
        }

        //������������
        if (touch && !stop)
        {
            if (touch.CompareTag("Enemy") || touch.CompareTag("Ground") || touch.CompareTag("Spikes") || touch.CompareTag("Door") || curDistance >= maxDistance)
            {
                stop = true;
                GetComponent<Animator>().SetTrigger("crash");
                if (touch.CompareTag("Enemy"))
                {
                    //����
                    touch.GetComponent<Enemy>().TakeDamage(damage);
                    //������
                    transform.SetParent(touch.transform);
                    recoil(touch);
                }
            }
        }
        else
        {
            stop = true;
            GetComponent<Animator>().SetTrigger("crash");
        }
    }

    //������ �� ������������
    private void recoil(Collider2D collision)
    {
        if (!collision.GetComponent<Enemy>().isHeavy)      
        {
            if (isRight)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.right * forceOfOutput);
            }
            else if (isUp)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * forceOfOutput);
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.right * -1 * forceOfOutput);
            }
        }
    }

    //event started in animator
    public void OnCrash()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (!isUp)
        {
            if(isRight)
            {
                Gizmos.DrawWireSphere(transform.position + new Vector3(offset, 0f, 0f), collisionSize);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position - new Vector3(offset, 0f, 0f), collisionSize);
            }
        }
        else 
        {
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, offset, 0f), collisionSize);
        }
    }
}
