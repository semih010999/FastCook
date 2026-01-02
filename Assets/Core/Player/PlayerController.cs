using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public GameObject ÇiðKöfte1;
    public GameObject ÇiðKöfte2;
    public GameObject PiþmiþKöfte1;
    public GameObject PiþmiþKöfte2;

    public GameObject TezgahEkmek;
    public GameObject TezgahKöfte;
    public GameObject TezgahMarul;
    public GameObject TezgahPeynir;
    public GameObject TezgahDomates;

    public GameObject HazýrTabakDenemesi;

    public float moveSpeed = 1f;

    public bool EsyaTasýyorMu = false;
    public bool EtkilesimeGirilebilir = false;
    public bool EþyaSilebilir = false;
    public bool Piþebilir = false;
    public bool KöfteAlýnabilirMi = false;
    public bool EþyaKoyulabilir = false;

    public string MalzemeAdi = "";
    public string TasinanEsya = "";

    Vector2 movement;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.E))
        {
            EtkileþimKontrol();
        }

        YemekHazýrKontrol();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Etkilesilebilir"))
        {
            EtkilesimeGirilebilir = true;
            MalzemeAdi = other.gameObject.name;
            //Debug.Log("Etkileþime Girilebilir: " + MalzemeAdi);
        }

        if (other.CompareTag("Silinebilir"))
        {
            EþyaSilebilir = true;
            //Debug.Log("Eþya Silinebilir Alanýnda.");
        }

        if (other.CompareTag("Piþebilir"))
        {
            Piþebilir = true;
            //Debug.Log("Izgaranýn önündesin");
        }

        if (other.CompareTag("EþyaKoyulabilir"))
        {
            EþyaKoyulabilir = true;
            //Debug.Log("Tepsinin Önündesin");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Etkilesilebilir"))
        {
            EtkilesimeGirilebilir = false;
            MalzemeAdi = "";
            //Debug.Log("Etkileþim Alanýndan Çýkýldý.");
        }

        if (other.CompareTag("Silinebilir"))
        {
            EþyaSilebilir = false;
            //Debug.Log("Eþya Silinebilir Alanýndan Çýkýldý.");
        }

        if(other.CompareTag("Piþebilir"))
        {
            Piþebilir = false;
            //Debug.Log("Izgranýn önünden çýktýn");
        }

        if(other.CompareTag("EþyaKoyulabilir"))
        {
            EþyaKoyulabilir = false;
            //Debug.Log("Tepsinin Önündesin");
        }
    }

    private void EtkileþimKontrol()
    {
        if (EsyaTasýyorMu)
        {
            if (EþyaSilebilir)
            {
                EþyayýSil();
            }
            else if (EtkilesimeGirilebilir)
            {
                //Debug.Log("Taþýnan eþyayý býrak/kullan: " + TasinanEsya);
            }
        }
        else
        {
            if (EtkilesimeGirilebilir)
            {
                EþyayýAl(MalzemeAdi);
            }
        }

        if(EsyaTasýyorMu & TasinanEsya == "Köfte")
        {
            if(Piþebilir)
            {
                IzgaraKontrol();
                Debug.Log("Piþirebilirsin");
            }
        }
        else
        {
            if(Piþebilir)
            {
                Debug.Log("Elinde Köfte Yok");
            }
        }

        if(EsyaTasýyorMu == false & Piþebilir == true & PiþmiþKöfte1.activeInHierarchy || PiþmiþKöfte2.activeInHierarchy)
        {
            if(PiþmiþKöfte1.activeInHierarchy)
            {
                PiþmiþKöfte1.SetActive(false);
                TasinanEsya = "PiþmiþKöfte";
                EsyaTasýyorMu = true;
                Debug.Log("Piþmiþ Köfte Alýndý");
            }
            else if(PiþmiþKöfte2.activeInHierarchy)
            {
                PiþmiþKöfte2.SetActive(false);
                TasinanEsya = "PiþmiþKöfte";
                EsyaTasýyorMu = true;
                Debug.Log("Piþmiþ Köfte Alýndý");
            }
        }

        if(EsyaTasýyorMu == true & EþyaKoyulabilir == true)
        {
            if(TasinanEsya == "Ekmek")
            {
                TezgahEkmek.SetActive(true);
                TasinanEsya = "";
                EsyaTasýyorMu = false;
            }
            else if(TasinanEsya == "PiþmiþKöfte")
            {
                TezgahKöfte.SetActive(true);
                TasinanEsya = "";
                EsyaTasýyorMu = false;
            }
            else if(TasinanEsya == "Marul")
            {
                TezgahMarul.SetActive(true);
                TasinanEsya = "";
                EsyaTasýyorMu = false;
            }
            else if(TasinanEsya == "Peynir")
            {
                TezgahPeynir.SetActive(true);
                TasinanEsya = "";
                EsyaTasýyorMu = false;
            }
        }
    }

    private void EþyayýAl(string malzeme)
    {
        EsyaTasýyorMu = true;
        TasinanEsya = malzeme;

        Debug.Log(TasinanEsya + " alýndý.");
    }

    private void EþyayýSil()
    {
        Debug.Log(TasinanEsya + " çöpe atýldý.");

        EsyaTasýyorMu = false;
        TasinanEsya = "";
    }

    private void IzgaraKontrol()
    {
        if(ÇiðKöfte1.activeInHierarchy || PiþmiþKöfte1.activeInHierarchy)
        {
            StartCoroutine(KöftePiþirme02());
            EsyaTasýyorMu = false ;
            TasinanEsya = "";
        }
        else
        {
            StartCoroutine(KöftePiþirme01());
            EsyaTasýyorMu = false ;
            TasinanEsya = "";
        }
    }

    private void YemekHazýrKontrol()
    {
        //Hamburger
        if(TezgahEkmek.activeInHierarchy & TezgahKöfte.activeInHierarchy & TezgahMarul.activeInHierarchy & TezgahPeynir.activeInHierarchy)
        {
            HazýrTabakDenemesi.SetActive(true);
            TezgahEkmek.SetActive(false);
            TezgahKöfte.SetActive(false);
            TezgahMarul.SetActive(false);
            TezgahPeynir.SetActive(false);
        }
    }

    IEnumerator KöftePiþirme01()
    {
        ÇiðKöfte1.SetActive(true);

        yield return new WaitForSeconds(7);

        ÇiðKöfte1.SetActive(false);
        PiþmiþKöfte1.SetActive(true);
        KöfteAlýnabilirMi = true;
    }

    IEnumerator KöftePiþirme02()
    {
        ÇiðKöfte2.SetActive(true);

        yield return new WaitForSeconds(7);

        ÇiðKöfte2.SetActive(false);
        PiþmiþKöfte2.SetActive(true);
        KöfteAlýnabilirMi = true;
    }
}
