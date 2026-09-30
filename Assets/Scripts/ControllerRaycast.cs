//------------------------ ver 1.0
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class ControllerRaycast : MonoBehaviour
//{
//    public float maxDistance = 20f;

//    public GameObject markerPrefab;

//    public InputActionProperty placeAction;

//    void Update()
//    {
//        Ray ray = new Ray(transform.position, transform.forward);

//        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
//        {
//            Debug.DrawLine(ray.origin, hit.point, Color.green);

//            if (placeAction.action.WasPressedThisFrame())
//            {
//                Instantiate(markerPrefab,
//                            hit.point,
//                            Quaternion.identity);
//            }
//        }
//    }
//}

//------------------------ ver 2.0
//using UnityEngine;
//using UnityEngine.XR;

//public class ControllerInputTest : MonoBehaviour
//{
//    private InputDevice rightHand;

//    void Start()
//    {
//        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//    }

//    void Update()
//    {
//        if (!rightHand.isValid)
//        {
//            rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//            return;
//        }

//        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
//        {
//            if (pressed)
//            {
//                Debug.Log("Trigger Basılı");
//            }
//        }
//    }
//}
//------------------------ ver 3.0
//using UnityEngine;
//using UnityEngine.XR;

//public class ControllerRaycast : MonoBehaviour
//{
//    [Header("Raycast")]
//    public float maxDistance = 20f;

//    [Header("Prefab")]
//    public GameObject markerPrefab;

//    private InputDevice rightHand;

//    private bool lastTriggerState = false;

//    void Start()
//    {
//        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//    }

//    void Update()
//    {
//        if (!rightHand.isValid)
//            rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

//        Ray ray = new Ray(transform.position, transform.forward);

//        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
//        {
//            Debug.DrawLine(ray.origin, hit.point, Color.green);

//            if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
//            {
//                // Trigger'a ilk basıldığı an
//                if (triggerPressed && !lastTriggerState)
//                {
//                    Instantiate(markerPrefab,
//                                hit.point,
//                                Quaternion.identity);

//                    Debug.Log("Obje oluşturuldu");
//                }

//                lastTriggerState = triggerPressed;
//            }
//        }
//    }
//}

//------------------------ ver 4.0
//using UnityEngine;
//using UnityEngine.XR;

//public class ControllerRaycast : MonoBehaviour
//{
//    [Header("Spawn")]
//    public GameObject markerPrefab;
//    public float maxDistance = 20f;

//    [Header("Controllers")]
//    public XRNode spawnController = XRNode.RightHand;
//    public XRNode grabController = XRNode.LeftHand;

//    [Header("Scale")]
//    public float scaleSpeed = 0.5f;
//    public float minScale = 0.05f;
//    public float maxScale = 5f;

//    private InputDevice spawnDevice;
//    private InputDevice grabDevice;

//    private bool lastSpawnTrigger = false;

//    private GameObject grabbedObject;

//    void Start()
//    {
//        spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);
//        grabDevice = InputDevices.GetDeviceAtXRNode(grabController);
//    }

//    void Update()
//    {
//        if (!spawnDevice.isValid)
//            spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);

//        if (!grabDevice.isValid)
//            grabDevice = InputDevices.GetDeviceAtXRNode(grabController);

//        SpawnSystem();

//        GrabSystem();
//    }

//    //-----------------------------------------------------
//    // SPAWN
//    //-----------------------------------------------------

//    void SpawnSystem()
//    {
//        Ray ray = new Ray(transform.position, transform.forward);

//        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
//        {
//            Debug.DrawLine(ray.origin, hit.point, Color.green);

//            if (spawnDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
//            {
//                if (pressed && !lastSpawnTrigger)
//                {
//                    Instantiate(markerPrefab, hit.point, Quaternion.identity);

//                    Debug.Log("Prefab Spawn");
//                }

//                lastSpawnTrigger = pressed;
//            }
//        }
//    }

//    //-----------------------------------------------------
//    // GRAB
//    //-----------------------------------------------------

//    void GrabSystem()
//    {
//        Ray ray = new Ray(transform.position, transform.forward);

//        grabDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger);

//        if (trigger)
//        {
//            if (grabbedObject == null)
//            {
//                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
//                {
//                    if (hit.collider.CompareTag("Spawnable"))
//                    {
//                        grabbedObject = hit.collider.gameObject;
//                    }
//                }
//            }

//            if (grabbedObject != null)
//            {
//                float distance = Vector3.Distance(transform.position,
//                                                  grabbedObject.transform.position);

//                grabbedObject.transform.position =
//                    transform.position + transform.forward * distance;

//                ScaleObject();
//            }
//        }
//        else
//        {
//            grabbedObject = null;
//        }
//    }

//    //-----------------------------------------------------
//    // SCALE
//    //-----------------------------------------------------

//    void ScaleObject()
//    {
//        if (grabbedObject == null)
//            return;

//        if (grabDevice.TryGetFeatureValue(CommonUsages.primary2DAxis,
//                                         out Vector2 stick))
//        {
//            float delta = stick.y * scaleSpeed * Time.deltaTime;

//            Vector3 scale = grabbedObject.transform.localScale;

//            scale += Vector3.one * delta;

//            scale.x = Mathf.Clamp(scale.x, minScale, maxScale);
//            scale.y = Mathf.Clamp(scale.y, minScale, maxScale);
//            scale.z = Mathf.Clamp(scale.z, minScale, maxScale);

//            grabbedObject.transform.localScale = scale;
//        }
//    }
//}
//------------------------ ver 5.0

using UnityEngine;
using UnityEngine.XR;

public class ControllerRaycast : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject markerPrefab;
    public float maxDistance = 20f;

    // Raycast bir şeye çarpmazsa bu mesafede oluşturur
    public float defaultSpawnDistance = 2.0f;

    [Header("Controllers")]
    public XRNode spawnController = XRNode.RightHand;
    public XRNode grabController = XRNode.LeftHand;

    [Header("Scale")]
    public float scaleSpeed = 0.5f;
    public float minScale = 0.05f;
    public float maxScale = 5f;

    private InputDevice spawnDevice;
    private InputDevice grabDevice;

    private bool lastSpawnTrigger = false;

    private GameObject grabbedObject;

    void Start()
    {
        spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);
        grabDevice = InputDevices.GetDeviceAtXRNode(grabController);
        //Debug.Log(gameObject.name);
        //Debug.Log(transform.parent.name);
    }

    void Update()
    {
        if (!spawnDevice.isValid)
            spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);

        if (!grabDevice.isValid)
            grabDevice = InputDevices.GetDeviceAtXRNode(grabController);

        //Debug.Log(transform.position);
        //Debug.Log(Camera.main.transform.position);
        Debug.Log("XR Origin : " + transform.root.position);
        Debug.Log("Camera Offset : " + transform.parent.position);
        Debug.Log("Right Controller : " + transform.position);

        SpawnSystem();
        GrabSystem();
    }

    //-------------------------------------------------
    // SPAWN
    //-------------------------------------------------

    void SpawnSystem()
    {
        if (!spawnDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
            return;

        if (pressed && !lastSpawnTrigger)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            Vector3 spawnPosition;

            // Önce colliderlara bak
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                spawnPosition = hit.point;
            }
            else
            {
                // Hiçbir collider yoksa kontrolcünün önünde oluştur
                spawnPosition = ray.origin + ray.direction * defaultSpawnDistance;
            }

            Instantiate(markerPrefab, spawnPosition, Quaternion.identity);

            Debug.Log("Prefab Spawned");
        }

        lastSpawnTrigger = pressed;
    }

    //-------------------------------------------------
    // GRAB
    //-------------------------------------------------

    void GrabSystem()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        grabDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger);

        if (trigger)
        {
            if (grabbedObject == null)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    if (hit.collider.CompareTag("Spawnable"))
                    {
                        grabbedObject = hit.collider.gameObject;
                    }
                }
            }

            if (grabbedObject != null)
            {
                float distance = Vector3.Distance(transform.position,
                                                  grabbedObject.transform.position);

                grabbedObject.transform.position =
                    transform.position + transform.forward * distance;

                ScaleObject();
            }
        }
        else
        {
            grabbedObject = null;
        }
    }

    //-------------------------------------------------
    // SCALE
    //-------------------------------------------------

    void ScaleObject()
    {
        if (grabbedObject == null)
            return;

        if (grabDevice.TryGetFeatureValue(CommonUsages.primary2DAxis,
                                          out Vector2 stick))
        {
            float delta = stick.y * scaleSpeed * Time.deltaTime;

            Vector3 scale = grabbedObject.transform.localScale;

            scale += Vector3.one * delta;

            scale.x = Mathf.Clamp(scale.x, minScale, maxScale);
            scale.y = Mathf.Clamp(scale.y, minScale, maxScale);
            scale.z = Mathf.Clamp(scale.z, minScale, maxScale);

            grabbedObject.transform.localScale = scale;
        }
    }
}

//using UnityEngine;
//using UnityEngine.XR;

//public class ControllerRaycast : MonoBehaviour
//{
//    [Header("Spawn")]
//    public GameObject markerPrefab;
//    public float maxDistance = 20f;

//    // Raycast hiçbir collider'a çarpmazsa
//    // kontrolcünün önünde bu mesafede oluşturulur.
//    public float defaultSpawnDistance = 2.0f;

//    [Header("Controllers")]
//    public XRNode spawnController = XRNode.RightHand;
//    public XRNode grabController = XRNode.LeftHand;

//    [Header("Scale")]
//    public float scaleSpeed = 0.5f;
//    public float minScale = 0.05f;
//    public float maxScale = 5f;

//    private InputDevice spawnDevice;
//    private InputDevice grabDevice;

//    private bool lastSpawnTrigger = false;

//    private GameObject grabbedObject;

//    void Start()
//    {
//        spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);
//        grabDevice = InputDevices.GetDeviceAtXRNode(grabController);
//    }

//    void Update()
//    {
//        // Controller cihazlarını tekrar bul
//        if (!spawnDevice.isValid)
//            spawnDevice = InputDevices.GetDeviceAtXRNode(spawnController);

//        if (!grabDevice.isValid)
//            grabDevice = InputDevices.GetDeviceAtXRNode(grabController);

//        SpawnSystem();
//        GrabSystem();
//    }

//    // =================================================
//    // SPAWN
//    // =================================================

//    void SpawnSystem()
//    {
//        if (!spawnDevice.TryGetFeatureValue(
//            CommonUsages.triggerButton,
//            out bool pressed))
//            return;

//        // Trigger'a ilk basıldığı an
//        if (pressed && !lastSpawnTrigger)
//        {
//            Ray ray = new Ray(
//                transform.position,
//                transform.forward
//            );

//            if (Physics.Raycast(
//                ray,
//                out RaycastHit hit,
//                maxDistance))
//            {
//                Debug.DrawLine(
//                    ray.origin,
//                    hit.point,
//                    Color.green,
//                    2f
//                );

//                Debug.Log(
//                    "Ray çarptı: " +
//                    hit.collider.gameObject.name
//                );

//                // -----------------------------------------
//                // PREFAB OLUŞTUR
//                // -----------------------------------------

//                GameObject obj = Instantiate(
//                    markerPrefab
//                );

//                // -----------------------------------------
//                // KÜPÜN COLLIDER BOYUTUNU AL
//                // -----------------------------------------

//                BoxCollider box =
//                    obj.GetComponentInChildren<BoxCollider>();

//                if (box != null)
//                {
//                    // Collider'ın dünya uzayındaki yarı yüksekliği
//                    float halfHeight =
//                        box.bounds.extents.y;

//                    // Zeminin normal yönünde yukarı taşı
//                    Vector3 spawnPosition =
//                        hit.point +
//                        hit.normal * halfHeight;

//                    obj.transform.position =
//                        spawnPosition;
//                }
//                else
//                {
//                    // Collider bulunamazsa doğrudan hit.point
//                    obj.transform.position =
//                        hit.point;
//                }

//                // Zeminin normaline göre yönlendir
//                obj.transform.rotation =
//                    Quaternion.FromToRotation(
//                        Vector3.up,
//                        hit.normal
//                    );

//                // Grab sistemi
//                obj.tag = "Spawnable";

//                Debug.Log(
//                    "Küp zemine yerleştirildi."
//                );
//            }
//            else
//            {
//                Debug.Log(
//                    "Ray hiçbir Collider'a çarpmadı."
//                );
//            }
//        }

//        lastSpawnTrigger = pressed;
//    }

//    // =================================================
//    // GRAB
//    // =================================================

//    void GrabSystem()
//    {
//        Ray ray = new Ray(
//            transform.position,
//            transform.forward
//        );

//        grabDevice.TryGetFeatureValue(
//            CommonUsages.triggerButton,
//            out bool trigger
//        );

//        if (trigger)
//        {
//            // Henüz obje tutulmuyorsa
//            if (grabbedObject == null)
//            {
//                if (Physics.Raycast(
//                    ray,
//                    out RaycastHit hit,
//                    maxDistance))
//                {
//                    if (hit.collider.CompareTag("Spawnable"))
//                    {
//                        grabbedObject =
//                            hit.collider.gameObject;
//                    }
//                }
//            }

//            // Obje tutuluyorsa controller'ın önünde taşı
//            if (grabbedObject != null)
//            {
//                float distance =
//                    Vector3.Distance(
//                        transform.position,
//                        grabbedObject.transform.position
//                    );

//                grabbedObject.transform.position =
//                    transform.position +
//                    transform.forward *
//                    distance;

//                ScaleObject();
//            }
//        }
//        else
//        {
//            grabbedObject = null;
//        }
//    }

//    // =================================================
//    // SCALE
//    // =================================================

//    void ScaleObject()
//    {
//        if (grabbedObject == null)
//            return;

//        if (grabDevice.TryGetFeatureValue(
//            CommonUsages.primary2DAxis,
//            out Vector2 stick))
//        {
//            float delta =
//                stick.y *
//                scaleSpeed *
//                Time.deltaTime;

//            Vector3 scale =
//                grabbedObject.transform.localScale;

//            scale += Vector3.one * delta;

//            scale.x =
//                Mathf.Clamp(
//                    scale.x,
//                    minScale,
//                    maxScale
//                );

//            scale.y =
//                Mathf.Clamp(
//                    scale.y,
//                    minScale,
//                    maxScale
//                );

//            scale.z =
//                Mathf.Clamp(
//                    scale.z,
//                    minScale,
//                    maxScale
//                );

//            grabbedObject.transform.localScale =
//                scale;
//        }
//    }
//}