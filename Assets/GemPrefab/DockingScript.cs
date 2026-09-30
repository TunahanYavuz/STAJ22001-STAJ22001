//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class DockingScript : MonoBehaviour
//{
//    [Header("Dock Referansı")]
//    public Transform dockPoint;

//    [Header("Hareket Ayarları")]
//    [SerializeField] private float moveSpeed = 5f;

//    [Header("Dönüş Ayarları")]
//    [Tooltip("Bu mesafeye gelince dönmeye başlar.")]
//    [SerializeField] private float rotationStartDistance = 20f;

//    [Tooltip("Rotation'ın ne kadar hızlı değişeceği")]
//    [SerializeField] private float rotationSpeed = 3f;

//    [Header("Görsel Model Ayarları")]
//    [Tooltip("Görsel modelin Transform'u. Rigidbody'nin child'ı olmalı.")]
//    [SerializeField] private Transform visualModelTransform;
//    [Tooltip("Görsel modelin yerel rotasyonuna uygulanacak offset. Örneğin, dik duran bir modeli yatay yapmak için X: -90.")]
//    [SerializeField] private Vector3 visualRotationOffset = new Vector3(-90f, 0f, 0f);

//    private Rigidbody rb;

//    private enum DockingState
//    {
//        Moving,
//        Docked
//    }

//    private DockingState currentState = DockingState.Moving;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.interpolation = RigidbodyInterpolation.Interpolate;

//        // Başlangıçta görsel modeli offset ile ayarla
//        if (visualModelTransform != null)
//        {
//            visualModelTransform.localRotation = Quaternion.Euler(visualRotationOffset);
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (dockPoint == null)
//            return;

//        if (currentState == DockingState.Moving)
//        {
//            MoveToDock();
//        }
//    }

//    private void MoveToDock()
//    {
//        //----------------------------------------------------
//        // HEDEF POZİSYONU
//        //----------------------------------------------------

//        Vector3 currentPosition = rb.position;
//        Vector3 targetPosition = dockPoint.position;

//        // Geminin mevcut Y pozisyonunu koru.
//        // Sadece X ve Z ekseninde dock noktasına ilerlesin.
//        targetPosition.y = currentPosition.y;

//        //----------------------------------------------------
//        // MESAFE HESABI
//        //----------------------------------------------------

//        Vector3 toTarget = targetPosition - currentPosition;
//        float sqrDistance = toTarget.sqrMagnitude;

//        float rotationStartDistanceSqr = rotationStartDistance * rotationStartDistance;
//        float minimumRotationDistance = 1.8f;
//        float minimumRotationDistanceSqr = minimumRotationDistance * minimumRotationDistance;

//        //----------------------------------------------------
//        // HAREKET
//        //----------------------------------------------------

//        Vector3 targetPos = Vector3.MoveTowards(
//            currentPosition,
//            targetPosition,
//            moveSpeed * Time.fixedDeltaTime
//        );

//        rb.MovePosition(targetPos);

//        //----------------------------------------------------
//        // ROTASYON
//        //----------------------------------------------------

//        if (sqrDistance <= rotationStartDistanceSqr &&
//            sqrDistance >= minimumRotationDistanceSqr)
//        {
//            float distance = Mathf.Sqrt(sqrDistance);
//            float progress = 1f - (distance / rotationStartDistance);
//            progress = Mathf.Clamp01(progress);

//            // Rigidbody'nin sadece Y ekseninde dockPoint'e dönmesini sağla.
//            // X ve Z eksenleri Rigidbody'nin mevcut rotasyonunda kalır (yani gemi düz kalır).
//            Quaternion targetRotation = Quaternion.Euler(
//                rb.rotation.eulerAngles.x,
//                dockPoint.rotation.eulerAngles.y,
//                rb.rotation.eulerAngles.z
//            );

//            Quaternion newRotation = Quaternion.Slerp(
//                rb.rotation,
//                targetRotation,
//                progress * rotationSpeed * Time.fixedDeltaTime
//            );

//            rb.MoveRotation(newRotation);
//        }

//        // Görsel modelin yerel rotasyonunu her zaman koru
//        if (visualModelTransform != null)
//        {
//            visualModelTransform.localRotation = Quaternion.Euler(visualRotationOffset);
//        }
//    }
//}



// _?____________________ 2. HAL_______________________________________


//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class DockingScript : MonoBehaviour
//{
//    [Header("Dock Referansı")]
//    [Tooltip("Lp Container Ship objesinin child'ı olan DockPoint.")]
//    [SerializeField] private Transform dockPoint;

//    [Header("Hareket Ayarları")]
//    [SerializeField] private float moveSpeed = 5f;

//    [Header("Dönüş Ayarları")]
//    [Tooltip("Bu mesafeye gelince dönmeye başlar.")]
//    [SerializeField] private float rotationStartDistance = 20f;

//    [Tooltip("Rotation'ın ne kadar hızlı değişeceği.")]
//    [SerializeField] private float rotationSpeed = 3f;

//    [Header("Görsel Model Ayarları")]
//    [Tooltip("Görsel modelin Transform'u. Rigidbody'nin child'ı olmalı.")]
//    [SerializeField] private Transform visualModelTransform;

//    [Tooltip("Görsel modelin yerel rotasyon offset'i.")]
//    [SerializeField]
//    private Vector3 visualRotationOffset =
//        new Vector3(-90f, 0f, 0f);

//    private Rigidbody rb;

//    private enum DockingState
//    {
//        Moving,
//        Released
//    }

//    private DockingState currentState = DockingState.Moving;


//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.interpolation = RigidbodyInterpolation.Interpolate;

//        if (visualModelTransform != null)
//        {
//            visualModelTransform.localRotation =
//                Quaternion.Euler(visualRotationOffset);
//        }
//    }


//    private void FixedUpdate()
//    {
//        if (dockPoint == null)
//            return;

//        // Portal geçişi gerçekleşene kadar
//        // Sal Plane DockPoint'i takip eder.
//        if (currentState == DockingState.Moving)
//        {
//            MoveToDock();
//        }
//    }


//    private void MoveToDock()
//    {
//        //----------------------------------------------------
//        // POZİSYON
//        //----------------------------------------------------

//        Vector3 currentPosition = rb.position;

//        // DockPoint Lp Container Ship'in child'ıdır.
//        Vector3 targetPosition = dockPoint.position;

//        // Sal Plane'in mevcut Y'sini koru.
//        targetPosition.y = currentPosition.y;


//        //----------------------------------------------------
//        // MESAFE
//        //----------------------------------------------------

//        Vector3 toTarget =
//            targetPosition - currentPosition;

//        float sqrDistance =
//            toTarget.sqrMagnitude;

//        float rotationStartDistanceSqr =
//            rotationStartDistance *
//            rotationStartDistance;

//        float minimumRotationDistance = 1.8f;

//        float minimumRotationDistanceSqr =
//            minimumRotationDistance *
//            minimumRotationDistance;


//        //----------------------------------------------------
//        // HAREKET
//        //----------------------------------------------------

//        Vector3 targetPos =
//            Vector3.MoveTowards(
//                currentPosition,
//                targetPosition,
//                moveSpeed * Time.fixedDeltaTime
//            );

//        rb.MovePosition(targetPos);


//        //----------------------------------------------------
//        // ROTASYON
//        //----------------------------------------------------

//        if (sqrDistance <= rotationStartDistanceSqr &&
//            sqrDistance >= minimumRotationDistanceSqr)
//        {
//            float distance =
//                Mathf.Sqrt(sqrDistance);

//            float progress =
//                1f -
//                (distance / rotationStartDistance);

//            progress =
//                Mathf.Clamp01(progress);


//            Quaternion targetRotation =
//                Quaternion.Euler(
//                    rb.rotation.eulerAngles.x,
//                    dockPoint.rotation.eulerAngles.y,
//                    rb.rotation.eulerAngles.z
//                );


//            Quaternion newRotation =
//                Quaternion.Slerp(
//                    rb.rotation,
//                    targetRotation,
//                    progress *
//                    rotationSpeed *
//                    Time.fixedDeltaTime
//                );


//            rb.MoveRotation(newRotation);
//        }


//        //----------------------------------------------------
//        // GÖRSEL MODEL
//        //----------------------------------------------------

//        if (visualModelTransform != null)
//        {
//            visualModelTransform.localRotation =
//                Quaternion.Euler(visualRotationOffset);
//        }
//    }


//    //========================================================
//    // DOCKING BIRAK
//    //========================================================

//    public void ReleaseDocking()
//    {
//        if (currentState == DockingState.Released)
//            return;

//        currentState = DockingState.Released;

//        Debug.Log(
//            "Docking bırakıldı. " +
//            "Sal Plane artık DockPoint'i takip etmiyor."
//        );
//    }


//    //========================================================
//    // DURUM
//    //========================================================

//    public bool IsDocking()
//    {
//        return currentState == DockingState.Moving;
//    }
//}



//____________________3.HAL_________________


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DockingScript : MonoBehaviour
{
    [Header("Dock Referansı")]
    [Tooltip("Lp Container Ship objesinin child'ı olan DockPoint.")]
    [SerializeField] private Transform dockPoint;

    [Header("Hareket Ayarları")]
    [SerializeField] private float moveSpeed = 2.5f;

    [SerializeField] private float acceleration = 2.0f;

    [SerializeField] private float stoppingDistance = 0.5f;

    [Header("Dönüş Ayarları")]
    [SerializeField] private float rotationStartDistance = 20f;

    [SerializeField] private float rotationSpeed = 2f;

    [Header("Görsel Model Ayarları")]
    [Tooltip("Görsel modelin Transform'u.")]
    [SerializeField] private Transform visualModelTransform;

    [Tooltip("Görsel modelin yerel rotasyon offset'i.")]
    [SerializeField]
    private Vector3 visualRotationOffset =
        new Vector3(-90f, 0f, 0f);


    private Rigidbody rb;


    private enum DockingState
    {
        Moving,
        Released
    }

    private DockingState currentState =
        DockingState.Moving;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;
    }


    private void Start()
    {
        if (visualModelTransform != null)
        {
            visualModelTransform.localRotation =
                Quaternion.Euler(
                    visualRotationOffset
                );
        }
    }


    private void FixedUpdate()
    {
        if (dockPoint == null)
            return;

        if (currentState != DockingState.Moving)
            return;

        MoveToDock();
    }


    private void MoveToDock()
    {
        //----------------------------------------------------
        // MEVCUT POZİSYON
        //----------------------------------------------------

        Vector3 currentPosition =
            rb.position;


        //----------------------------------------------------
        // DOCKPOINT WORLD POSITION
        //----------------------------------------------------

        Vector3 targetPosition =
            dockPoint.position;


        //----------------------------------------------------
        // Y'YE DOKUNMA
        //----------------------------------------------------

        // Buoyancy (yuzdurme) sisteminin Y hareketini
        // yönetmesine izin veriyoruz.
        targetPosition.y =
            currentPosition.y;


        //----------------------------------------------------
        // XZ YÖNÜNDE HEDEF
        //----------------------------------------------------

        Vector3 horizontalDirection =
            targetPosition - currentPosition;

        horizontalDirection.y = 0f;


        float distance =
            horizontalDirection.magnitude;


        //----------------------------------------------------
        // HAREKET
        //----------------------------------------------------

        if (distance > stoppingDistance)
        {
            Vector3 desiredDirection =
                horizontalDirection.normalized;


            Vector3 desiredVelocity =
                desiredDirection *
                moveSpeed;


            // Mevcut X/Z hızını yumuşak şekilde
            // hedef hıza yaklaştır.
            Vector3 currentVelocity =
                rb.linearVelocity;


            Vector3 targetVelocity =
                Vector3.MoveTowards(
                    new Vector3(
                        currentVelocity.x,
                        0f,
                        currentVelocity.z
                    ),
                    desiredVelocity,
                    acceleration *
                    Time.fixedDeltaTime
                );


            // Y hızını değiştirmiyoruz.
            rb.linearVelocity =
                new Vector3(
                    targetVelocity.x,
                    currentVelocity.y,
                    targetVelocity.z
                );
        }
        else
        {
            //------------------------------------------------
            // HEDEFE YAKLAŞINCA XZ HIZINI YUMUŞAKÇA KES
            //------------------------------------------------

            Vector3 currentVelocity =
                rb.linearVelocity;


            Vector3 horizontalVelocity =
                new Vector3(
                    currentVelocity.x,
                    0f,
                    currentVelocity.z
                );


            horizontalVelocity =
                Vector3.MoveTowards(
                    horizontalVelocity,
                    Vector3.zero,
                    acceleration *
                    Time.fixedDeltaTime
                );


            rb.linearVelocity =
                new Vector3(
                    horizontalVelocity.x,
                    currentVelocity.y,
                    horizontalVelocity.z
                );
        }


        //----------------------------------------------------
        // ROTASYON
        //----------------------------------------------------

        if (distance <= rotationStartDistance)
        {
            Quaternion targetRotation =
                Quaternion.Euler(
                    rb.rotation.eulerAngles.x,
                    dockPoint.rotation.eulerAngles.y,
                    rb.rotation.eulerAngles.z
                );


            Quaternion newRotation =
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed *
                    100f *
                    Time.fixedDeltaTime
                );


            rb.MoveRotation(
                newRotation
            );
        }


        //----------------------------------------------------
        // GÖRSEL MODEL
        //----------------------------------------------------

        if (visualModelTransform != null)
        {
            visualModelTransform.localRotation =
                Quaternion.Euler(
                    visualRotationOffset
                );
        }
    }


    //========================================================
    // DOCKING BIRAK
    //========================================================

    public void ReleaseDocking()
    {
        if (currentState ==
            DockingState.Released)
            return;


        currentState =
            DockingState.Released;


        Debug.Log(
            "Docking bırakıldı. " +
            "Sal Plane artık DockPoint'i takip etmiyor."
        );
    }


    //========================================================
    // DURUM
    //========================================================

    public bool IsDocking()
    {
        return currentState ==
               DockingState.Moving;
    }
}