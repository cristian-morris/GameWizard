using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float velocidad = 700f;
    // Start is called before the first frame update

    public float fuerzaSalto = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private Rigidbody2D rb; 

    public Animator animator;

     // Factor para controlar la velocidad de caída cuando no está en el suelo
    public float multiplicadorCaida = 4f; 
    public float multiplicadorSaltoBajo = 2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;

       animator.SetFloat("Run", velocidadX * velocidad);

       if (velocidadX < 0){
           transform.localScale = new Vector3(-3, 3, 1);
       }
       if (velocidadX > 0) {
           transform.localScale = new Vector3(3, 3, 1);
       }

       Vector3 posicion = transform.position;
       
       transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z); 

       RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
       enSuelo = hit.collider != null;

       if (enSuelo && Input.GetKeyDown(KeyCode.Space)) { 
           rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
       }
        // Controlar la velocidad de caída
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplicadorCaida - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplicadorSaltoBajo - 1) * Time.deltaTime;
        }
       animator.SetBool("Jump",enSuelo);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast); 
    }
}