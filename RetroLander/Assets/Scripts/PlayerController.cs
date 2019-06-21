using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float velocidade = 50f;
    public float combustivel = 1000f;
    public float consumoCombustivel = 1f;
    public float velocidadeLimitePouso = 50f;

    public LayerMask raycastLayerMask;

    public SpriteRenderer propulsorFX;

    public GameObject explosao;

    public Text hudAltitude;
    public Text hudCombustivel;
    public Text hudVelocidadeVertical;
    public Text hudVelocidadeHorizontal;

    private Rigidbody2D rb;

    private float velocidadeVertical;
    private float velocidadeHorizontal;
    private float altitude;

    private string setaEsquerda = "\u2190";
    private string setaDireita = "\u2192";
    private string setaCima = "\u2191";
    private string setaBaixo = "\u2193";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * 100f);
    }

    private void FixedUpdate()
    {
        if (combustivel > 0)
        {
            Propulsor();
        }

        Rotaciona();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, raycastLayerMask);

        if (hit.collider == null)
        {
            altitude = hit.point.y + transform.position.y;
        }
        
    }

    private void Update()
    {
        AtualizaHUD();
    }

    public void Propulsor()
    {
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            propulsorFX.enabled = true;
            combustivel -= consumoCombustivel;
        }
        else
        {
            propulsorFX.enabled = false;
        }

        rb.AddForce(transform.up * v);

        velocidadeVertical = Mathf.FloorToInt(rb.velocity.y * 100f);

        if(Mathf.Abs(rb.position.x) > 27)
        {
            rb.position = new Vector2(rb.position.x * -1, rb.position.y);
        }
    }

    public void Rotaciona()
    {
        float h = Input.GetAxis("Horizontal");
        rb.MoveRotation(rb.rotation + (h * velocidade * Time.fixedDeltaTime) * -1);

        velocidadeHorizontal = Mathf.FloorToInt(rb.velocity.x * 100f);
    }

    public void AtualizaHUD()
    {
        if(velocidadeVertical < 0)
        {
            hudVelocidadeVertical.text = string.Format("VELOC V.:{0} {1}", Mathf.Abs(velocidadeVertical), setaBaixo);
        }
        else
        {
            hudVelocidadeVertical.text = string.Format("VELOC V.:{0} {1}", Mathf.Abs(velocidadeVertical), setaCima);
        }

        if (velocidadeVertical < 0)
        {
            hudVelocidadeHorizontal.text = string.Format("VELOC H.:{0} {1}", Mathf.Abs(velocidadeHorizontal), setaEsquerda);
        }
        else
        {
            hudVelocidadeHorizontal.text = string.Format("VELOC H.:{0} {1}", Mathf.Abs(velocidadeHorizontal), setaDireita);
        }

        hudCombustivel.text = string.Format("COMBUSTIVEL.: {0}", Mathf.Floor(combustivel));
        hudAltitude.text = string.Format("ALTITUDE.: {0}", Mathf.Floor(altitude + 12f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Montanha": Instantiate(explosao, collision.contacts[0].point, Quaternion.identity);
                Destroy(gameObject);
                break;

            case "AreaSegura":
                if(Mathf.Abs(velocidadeVertical) > velocidadeLimitePouso)
                {
                    Instantiate(explosao, collision.contacts[0].point, Quaternion.identity);
                    Destroy(gameObject);
                }
                else
                {
                    GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
                    this.enabled = false;
                    propulsorFX.enabled = false;
                    gc.OnGameWin();
                }
                break;
            default:
                break;
                
                
        }   
    }


}
