using UnityEngine;

public class PortalFollowPointFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField] private GameObject obj;


    private float fixedRotationX;
    private float fixedRotationZ;

    private void Start()
    {
        // Prism'in başlangıç X ve Z rotasyonlarını kaydet
        fixedRotationX = transform.eulerAngles.x;
        fixedRotationZ = transform.eulerAngles.z;
    }

    //private void LateUpdate()
    //{
    //    if (target == null)
    //        return;

    //    // Position tamamen PortalFollowPoint'i takip eder
    //    transform.position = target.position;

    //    // Sadece Y rotasyonu takip edilir
    //    Vector3 newRotation = transform.eulerAngles;

    //    newRotation.x = fixedRotationX; // 0 a sabitlicez 
    //    newRotation.y = target.eulerAngles.y;
    //    newRotation.z = fixedRotationZ; 

    //    transform.eulerAngles = newRotation;
    //}



    private void LateUpdate()
    {
        if (target == null)
            return;
        if (!obj.activeSelf)
        {
            transform.position = target.position;
        }
        else
        {
            var z = transform.position.z - target.position.z <= 0.7f ? transform.position.z : target.position.z;
            var y = transform.position.y - target.position.y <= 0.7f ? transform.position.y : target.position.y;
            transform.position = new Vector3(target.position.x, y, z);
        }

        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
    }


}