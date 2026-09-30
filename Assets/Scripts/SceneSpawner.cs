using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class SceneSpawner : MonoBehaviour
{
    [Header("Spawned Cube")]
    public GameObject markerPrefab;

    [Header("Scene 3")]
    [Tooltip("Geçiş yapılacak sahnenin Build Index'i")]
    public int targetSceneBuildIndex = 1;

    [Tooltip("Scene 3'te küpün yerine yerleştirilecek prefab")]
    public GameObject targetPrefab;

    [Header("Spawn")]
    public float maxDistance = 20f;

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
    private bool lastAButton = false;

    private GameObject grabbedObject;
    private GameObject lastSpawnedCube;

    //=================================================
    // START
    //=================================================

    void Start()
    {
        spawnDevice =
            InputDevices.GetDeviceAtXRNode(spawnController);

        grabDevice =
            InputDevices.GetDeviceAtXRNode(grabController);
    }

    //=================================================
    // UPDATE
    //=================================================

    void Update()
    {
        if (!spawnDevice.isValid)
        {
            spawnDevice =
                InputDevices.GetDeviceAtXRNode(spawnController);
        }

        if (!grabDevice.isValid)
        {
            grabDevice =
                InputDevices.GetDeviceAtXRNode(grabController);
        }

        SpawnSystem();
        GrabSystem();
        SceneChangeSystem();
    }

    //=================================================
    // KÜP OLUŞTURMA
    //=================================================

    void SpawnSystem()
    {
        if (!spawnDevice.TryGetFeatureValue(
                CommonUsages.triggerButton,
                out bool pressed))
        {
            return;
        }

        if (pressed && !lastSpawnTrigger)
        {
            SpawnCube();
        }

        lastSpawnTrigger = pressed;
    }

    void SpawnCube()
    {
        if (markerPrefab == null)
        {
            Debug.LogError(
                "Marker Prefab atanmadı!"
            );

            return;
        }

        Ray ray = new Ray(
            transform.position,
            transform.forward
        );

        Vector3 spawnPosition;

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                maxDistance))
        {
            spawnPosition = hit.point;
        }
        else
        {
            spawnPosition =
                ray.origin +
                ray.direction *
                defaultSpawnDistance;
        }

        lastSpawnedCube = Instantiate(
            markerPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Debug.Log(
            "Küp oluşturuldu: " +
            lastSpawnedCube.name
        );
    }

    //=================================================
    // A BUTONU
    //=================================================

    void SceneChangeSystem()
    {
        if (!spawnDevice.TryGetFeatureValue(
                CommonUsages.secondaryButton,
                out bool aButton))
        {
            return;
        }

        if (aButton && !lastAButton)
        {
            ChangeToTargetScene();
        }

        lastAButton = aButton;
    }

    //=================================================
    // SAHNE DEĞİŞTİR
    //=================================================

    void ChangeToTargetScene()
    {
        if (lastSpawnedCube == null)
        {
            Debug.LogError(
                "Önce bir küp oluşturmalısın!"
            );

            return;
        }

        if (targetPrefab == null)
        {
            Debug.LogError(
                "Target Prefab atanmadı!"
            );

            return;
        }

        // Küpün pozisyonunu kaydet
        SceneTransitionData.spawnPosition =
            lastSpawnedCube.transform.position;

        // Küpün rotasyonunu kaydet
        SceneTransitionData.spawnRotation =
            lastSpawnedCube.transform.rotation;

        // Hedef prefabı kaydet
        SceneTransitionData.targetPrefab =
            targetPrefab;

        // Veri hazır
        SceneTransitionData.hasSpawnData = true;

        Debug.Log(
            "Küp pozisyonu kaydedildi: " +
            SceneTransitionData.spawnPosition
        );

        Debug.Log(
            "Scene 3 yükleniyor..."
        );

        // SceneSpawner artık burada yok olacak.
        // Bu problem değil.
        SceneManager.LoadScene(
            targetSceneBuildIndex
        );
    }

    //=================================================
    // GRAB
    //=================================================

    void GrabSystem()
    {
        Ray ray = new Ray(
            transform.position,
            transform.forward
        );

        grabDevice.TryGetFeatureValue(
            CommonUsages.triggerButton,
            out bool trigger
        );

        if (trigger)
        {
            if (grabbedObject == null)
            {
                if (Physics.Raycast(
                        ray,
                        out RaycastHit hit,
                        maxDistance))
                {
                    if (hit.collider.CompareTag(
                            "Spawnable"))
                    {
                        grabbedObject =
                            hit.collider.gameObject;
                    }
                }
            }

            if (grabbedObject != null)
            {
                float distance =
                    Vector3.Distance(
                        transform.position,
                        grabbedObject.transform.position
                    );

                grabbedObject.transform.position =
                    transform.position +
                    transform.forward * distance;

                ScaleObject();
            }
        }
        else
        {
            grabbedObject = null;
        }
    }

    //=================================================
    // SCALE
    //=================================================

    void ScaleObject()
    {
        if (grabbedObject == null)
        {
            return;
        }

        if (grabDevice.TryGetFeatureValue(
                CommonUsages.primary2DAxis,
                out Vector2 stick))
        {
            float delta =
                stick.y *
                scaleSpeed *
                Time.deltaTime;

            Vector3 scale =
                grabbedObject.transform.localScale;

            scale += Vector3.one * delta;

            scale.x = Mathf.Clamp(
                scale.x,
                minScale,
                maxScale
            );

            scale.y = Mathf.Clamp(
                scale.y,
                minScale,
                maxScale
            );

            scale.z = Mathf.Clamp(
                scale.z,
                minScale,
                maxScale
            );

            grabbedObject.transform.localScale =
                scale;
        }
    }
}