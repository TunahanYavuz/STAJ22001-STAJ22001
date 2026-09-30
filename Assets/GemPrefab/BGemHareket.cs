//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class BGemHareket : MonoBehaviour
//{
//    [Header("Hedef")]
//    [SerializeField] private Transform hedefNokta;

//    [Header("Motor Ayarları")]
//    [SerializeField] private float maksimumHiz = 8f;
//    [SerializeField] private float ivmelenme = 3f;

//    [Header("Varış Ayarı")]
//    [SerializeField] private float durmaMesafesi = 3f;

//    [Header("Kontrol")]
//    [SerializeField] private bool hareketEt = true;

//    private Rigidbody rb;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    private void FixedUpdate()
//    {
//        if (!hareketEt)
//            return;

//        if (hedefNokta == null)
//            return;

//        // Hedefe olan mesafe
//        float mesafe = Vector3.Distance(transform.position, hedefNokta.position);

//        // Hedefe ulaştıysa dur
//        if (mesafe <= durmaMesafesi)
//        {
//            rb.linearVelocity = Vector3.zero;
//            return;
//        }

//        // Hedef yönü (Y eksenini yok sayıyoruz)
//        Vector3 yon = hedefNokta.position - transform.position;
//        yon.y = 0f;
//        yon.Normalize();

//        // İleri hız
//        float ileriHiz = Vector3.Dot(rb.linearVelocity, transform.forward);

//        // Maksimum hıza ulaşmadıysa kuvvet uygula
//        if (ileriHiz < maksimumHiz)
//        {
//            rb.AddForce(transform.forward * ivmelenme, ForceMode.Acceleration);
//        }
//    }

//    public void HareketBaslat()
//    {
//        hareketEt = true;
//    }

//    public void HareketDurdur()
//    {
//        hareketEt = false;
//    }
//}


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BGemHareket : MonoBehaviour
{
    [Header("Hedef")]
    [SerializeField] private Transform hedefNokta;

    [Header("Motor Ayarları")]
    [SerializeField] private float maksimumHiz = 8f;
    [SerializeField] private float ivmelenme = 3f;

    [Header("Varış Ayarı")]
    [SerializeField] private float durmaMesafesi = 3f;

    [Header("Kontrol")]
    [SerializeField] private bool hareketEt = true;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!hareketEt)
            return;

        if (hedefNokta == null)
            return;

        // Hedefe olan mesafe
        float mesafe = Vector3.Distance(transform.position, hedefNokta.position);

        // Hedefe ulaştıysa dur
        if (mesafe <= durmaMesafesi)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Hedef yönü (X eksenini yok sayıyoruz)
        Vector3 yon = hedefNokta.position - transform.position;
        yon.x = 0f; // <--- DEĞİŞTİ: y yerine x sıfırlandı
        yon.Normalize();

        // İleri hız (X ekseni = transform.right)
        float ileriHiz = Vector3.Dot(rb.linearVelocity, transform.right); // <--- DEĞİŞTİ: forward yerine right

        // Maksimum hıza ulaşmadıysa kuvvet uygula
        if (ileriHiz < maksimumHiz)
        {
            rb.AddForce(transform.right * ivmelenme, ForceMode.Acceleration); // <--- DEĞİŞTİ: forward yerine right
        }
    }

    public void HareketBaslat()
    {
        hareketEt = true;
    }

    public void HareketDurdur()
    {
        hareketEt = false;
    }
}
