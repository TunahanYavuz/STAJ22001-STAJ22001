//using Unity.VisualScripting;
//using UnityEngine;

//public class xrcabin : MonoBehaviour
//{
//    [SerializeField] private Transform parentToSet;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.transform.parent.parent.CompareTag("XRCollider"))
//        {
//            other.transform.parent.parent.SetParent(parentToSet, true);
//            Debug.Log("Xr Collider Portala Giriş Yapıldı");

//        }

//    }
//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("XRCollider"))
//        {
//                Debug.Log("Xr  Collider Nesnesi portal collider çıkış yapıldı");
//        }
//    }
//}


using UnityEngine;

public class xrcabin : MonoBehaviour
{
    [SerializeField] private Transform parentToSet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null &&
            other.transform.parent.parent != null &&
            other.transform.parent.parent.CompareTag("XRCollider"))
        {
            Transform xrCollider = other.transform.parent.parent;

            // Parent değiştir
            xrCollider.SetParent(parentToSet, true);

            // Parent değiştikten sonra X ve Z rotation'ı bir kere sıfırla
            Vector3 rotation = xrCollider.localEulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;
            xrCollider.localEulerAngles = rotation;

            Debug.Log("Xr Collider Portala Giriş Yapıldı");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("XRCollider"))
        {
            Debug.Log("Xr Collider Nesnesi portal collider çıkış yapıldı");
        }
    }
}