using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public GameObject CigKofte1;
    public GameObject CigKofte2;
    public GameObject PismisKofte1;
    public GameObject PismisKofte2;
    public bool isGrill1Empty = true;
    public bool isGrill2Empty = true;

    public GameObject TezgahEkmek;
    bool isBreadEmptyStand = true;
    public GameObject TezgahKofte;
    bool isMeatballEmptyStand = true;
    public GameObject TezgahMarul;
    bool isLettuceEmptyStand = true;
    public GameObject TezgahPeynir;
    bool isCheeseEmptyStand = true;
    public GameObject TezgahDomates;
    bool isTomatoEmptyStand = true;

    public GameObject HazirTabakDenemesi;

    public float moveSpeed = 1f;

    public bool EsyaTasiyorMu = false;
    public bool EtkilesimeGirilebilir = false;
    public bool EsyaSilinebilir = false;
    public bool Pisebilir = false;
    public bool KofteAlinabilirMi = false;
    public bool EsyaKoyulabilir = false;

    public string MalzemeAdi = "";
    public string TasinanEsya = "";

    [Header("Taşınabilir Eşya Özellikleri")]
    public GameObject portableObjectChild;
    public SpriteRenderer portableObjectSprite;
    public Sprite[] portableObjects;
    public Vector3[] carryPoses;

    [Header("GameObjects")]
    public SpriteRenderer cashRegisterSpriteRenderer;

    Vector2 movement;
    Vector2 lastMoveDir = Vector2.down;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0.01f)
        {
            lastMoveDir = movement;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EtkilesimKontrol();
        }

        YemekHazirKontrol();
        UpdatePortableObjectPosition();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdatePortableObjectPosition()
    {
        if (!portableObjectChild.activeSelf) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
        {
            portableObjectChild.transform.localPosition = carryPoses[1]; // Down
            portableObjectSprite.sortingOrder = 2;
            return;
        }

        if (v > 0)
        {
            portableObjectChild.transform.localPosition = carryPoses[0];
            portableObjectSprite.sortingOrder = -1;
        }

        else if (v < 0)
        {
            portableObjectChild.transform.localPosition = carryPoses[1];
            portableObjectSprite.sortingOrder = 2;
        }

        else if (h != 0)
        {
            portableObjectSprite.sortingOrder = 2;
            portableObjectChild.transform.localPosition = new Vector3(
                Mathf.Sign(h) * Mathf.Abs(carryPoses[2].x),
                carryPoses[2].y,
                0
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Etkilesilebilir"))
        {
            EtkilesimeGirilebilir = true;
            MalzemeAdi = other.gameObject.name;
        }

        if (other.CompareTag("Silinebilir"))
        {
            EsyaSilinebilir = true;
        }

        if (other.CompareTag("Pişebilir"))
        {
            Pisebilir = true;
        }

        if (other.CompareTag("EşyaKoyulabilir"))
        {
            EsyaKoyulabilir = true;
        }

        if (other.CompareTag("CashRegister"))
        {
            cashRegisterSpriteRenderer.sortingLayerName = "Sprite player ön";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Etkilesilebilir"))
        {
            EtkilesimeGirilebilir = false;
            MalzemeAdi = "";
        }

        if (other.CompareTag("Silinebilir"))
        {
            EsyaSilinebilir = false;
        }

        if (other.CompareTag("Pişebilir"))
        {
            Pisebilir = false;
        }

        if (other.CompareTag("EşyaKoyulabilir"))
        {
            EsyaKoyulabilir = false;
        }

        if (other.CompareTag("CashRegister"))
        {
            cashRegisterSpriteRenderer.sortingLayerName = "Sprite player arka";
        }
    }

    private void EtkilesimKontrol()
    {
        if (EsyaTasiyorMu)
        {
            if (EsyaSilinebilir)
            {
                EsyayiSil();
            }
        }
        else
        {
            if (EtkilesimeGirilebilir)
            {
                EsyayiAl(MalzemeAdi);
                animator.SetBool("isHolding", true);
                portableObjectChild.SetActive(true);
                if (TasinanEsya == "Ekmek") portableObjectSprite.sprite = portableObjects[0];
                else if (TasinanEsya == "Köfte") portableObjectSprite.sprite = portableObjects[1];
                else if (TasinanEsya == "Marul") portableObjectSprite.sprite = portableObjects[2];
                else if (TasinanEsya == "Peynir") portableObjectSprite.sprite = portableObjects[3];
                else if (TasinanEsya == "Domates") portableObjectSprite.sprite = portableObjects[4];
            }
        }

        if (EsyaTasiyorMu && TasinanEsya == "Köfte" && Pisebilir)
        {
            if(!isGrill1Empty && !isGrill2Empty) return;

            IzgaraKontrol();
        }

        if (!EsyaTasiyorMu && Pisebilir && (PismisKofte1.activeInHierarchy || PismisKofte2.activeInHierarchy))
        {
            if (PismisKofte1.activeInHierarchy)
            {
                PismisKofte1.SetActive(false);
                isGrill1Empty = true;
            }
            else
            {
                PismisKofte2.SetActive(false);
                isGrill2Empty = true;
            }

            TasinanEsya = "PişmişKöfte";
            EsyaTasiyorMu = true;

            portableObjectChild.SetActive(true);
            animator.SetBool("isHolding", true);
            if (TasinanEsya == "PişmişKöfte") portableObjectSprite.sprite = portableObjects[5];
        }

        if (EsyaTasiyorMu && EsyaKoyulabilir)
        {
            if (TasinanEsya == "Köfte") return;
            else if (TasinanEsya == "Ekmek")
            {
                if (!isBreadEmptyStand) return;

                TezgahEkmek.SetActive(true);
            } 
            else if (TasinanEsya == "PişmişKöfte")
            {
                if (!isMeatballEmptyStand) return;
                isMeatballEmptyStand = false;
                TezgahKofte.SetActive(true);
            } 
            else if (TasinanEsya == "Marul")
            {
                if (!isLettuceEmptyStand) return;
                isLettuceEmptyStand = false;
                TezgahMarul.SetActive(true);
            } 
            else if (TasinanEsya == "Peynir")
            {
                if (!isCheeseEmptyStand) return;
                isCheeseEmptyStand = false;
                TezgahPeynir.SetActive(true);
            } 
            else if (TasinanEsya == "Domates")
            {
                if (!isTomatoEmptyStand) return;
                isTomatoEmptyStand = false;
                TezgahDomates.SetActive(true);
            } 

            TasinanEsya = "";
            EsyaTasiyorMu = false;
            portableObjectChild.SetActive(false);
            animator.SetBool("isHolding", false);
        }
    }

    private void EsyayiAl(string malzeme)
    {
        EsyaTasiyorMu = true;
        TasinanEsya = malzeme;
    }

    private void EsyayiSil()
    {
        EsyaTasiyorMu = false;
        TasinanEsya = "";
        portableObjectChild.SetActive(false);
        animator.SetBool("isHolding", false);
    }

    private void IzgaraKontrol()
    {
        portableObjectChild.SetActive(false);
        animator.SetBool("isHolding", false);

        if (CigKofte1.activeInHierarchy || PismisKofte1.activeInHierarchy)
            StartCoroutine(KoftePisir02());
        else
            StartCoroutine(KoftePisir01());

        EsyaTasiyorMu = false;
        TasinanEsya = "";
    }

    private void YemekHazirKontrol()
    {
        if (TezgahEkmek.activeInHierarchy &&
            TezgahKofte.activeInHierarchy &&
            TezgahMarul.activeInHierarchy &&
            TezgahPeynir.activeInHierarchy)
        {
            HazirTabakDenemesi.SetActive(true);

            TezgahEkmek.SetActive(false);
            TezgahKofte.SetActive(false);
            TezgahMarul.SetActive(false);
            TezgahPeynir.SetActive(false);
            TezgahDomates.SetActive(false);

            isBreadEmptyStand = true;
            isMeatballEmptyStand = true;
            isLettuceEmptyStand = true;
            isCheeseEmptyStand = true;
            isTomatoEmptyStand = true;
        }
    }

    IEnumerator KoftePisir01()
    {
        CigKofte1.SetActive(true);
        isGrill1Empty = false;
        yield return new WaitForSeconds(7);
        CigKofte1.SetActive(false);
        PismisKofte1.SetActive(true);
    }

    IEnumerator KoftePisir02()
    {
        CigKofte2.SetActive(true);
        isGrill2Empty = false;
        yield return new WaitForSeconds(7);
        CigKofte2.SetActive(false);
        PismisKofte2.SetActive(true);
    }
}
