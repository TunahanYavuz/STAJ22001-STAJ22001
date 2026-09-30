//////////////using UnityEngine;

//////////////public class PassTrigger : MonoBehaviour
//////////////{
//////////////    [SerializeField] private GameObject[] obj;

//////////////    private void Awake()
//////////////    {
//////////////        foreach (var o in obj)
//////////////        {
//////////////            o.SetActive(false);
//////////////        }
//////////////    }

//////////////    private void OnTriggerEnter(Collider other)
//////////////    {
//////////////        if (other.CompareTag("Player"))
//////////////        {
//////////////            foreach (var o in obj)
//////////////            {
//////////////                o.SetActive(true);
//////////////            }
//////////////        }
//////////////    }

//////////////    private void OnTriggerExit(Collider other)
//////////////    {
//////////////        if (other.CompareTag("Player"))
//////////////        {
//////////////            foreach (var o in obj)
//////////////            {
//////////////                o.SetActive(false);
//////////////            }
//////////////        }
//////////////    }
//////////////}


using Unity.XR.CoreUtils;
using UnityEngine;

public class passtrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] obj;
    [SerializeField] private Transform xr;

    private void Awake()
    {
        foreach (var o in obj)
        {
            o.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(new Vector3(obj[0].transform.position.x, 0f, obj[0].transform.position.z), new Vector3(xr.position.x, 0f, xr.position.z));
            Debug.Log("Distance: " + distance);
            Debug.Log(" Bot Portala Giriş Yapıldı");
            // portal objelerini aktif et
            foreach (var o in obj)
            {
                o.SetActive(true);
            }

            // player = camera offset olduğu için
            // yukarıdaki xr origin'i buluyoruz.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var o in obj)
            {
                Debug.Log("Bot portal collider çıkış yapıldı");
                //o.SetActive(false);
            }
        }
    }
}

