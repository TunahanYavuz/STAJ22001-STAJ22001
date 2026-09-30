using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARMeshDebugLogger : MonoBehaviour
{
    // Aynı GameObject üzerindeki ARMeshManager referansı
    private ARMeshManager meshManager;

    private void Awake()
    {
        // ARMeshManager bileşenini alıyoruz
        meshManager = GetComponent<ARMeshManager>();

        // ARMeshManager bulunamazsa hata ver
        if (meshManager == null)
        {
            Debug.LogError("[AR MESH ERROR] ARMeshManager bulunamadı!");
        }
    }

    private void OnEnable()
    {
        // Event'i dinlemeye başla
        if (meshManager != null)
        {
            meshManager.meshInfosChanged.AddListener(OnMeshInfosChanged);

            Debug.Log("[AR MESH] Event listener eklendi.");
        }
    }

    private void OnDisable()
    {
        // Event'i dinlemeyi bırak
        if (meshManager != null)
        {
            meshManager.meshInfosChanged.RemoveListener(OnMeshInfosChanged);

            Debug.Log("[AR MESH] Event listener kaldırıldı.");
        }
    }

    // Mesh eklendiğinde, güncellendiğinde veya kaldırıldığında çalışır
    private void OnMeshInfosChanged(ARMeshInfosChangedEventArgs args)
    {
        // Bu event anındaki değişiklikleri yazdır
        Debug.Log(
            $"[AR MESH EVENT] " +
            $"Added: {args.added.Count} | " +
            $"Updated: {args.updated.Count} | " +
            $"Removed: {args.removed.Count}"
        );

        // Şu anda ARMeshManager tarafından tutulan aktif mesh sayısı
        Debug.Log(
            $"[AR MESH STATUS] " +
            $"Aktif Mesh Sayısı: {meshManager.meshes.Count}"
        );

        // Hiç aktif mesh kalmadıysa uyarı ver
        if (meshManager.meshes.Count == 0)
        {
            Debug.LogWarning(
                "[AR MESH WARNING] Şu anda aktif mesh kalmadı!"
            );

            return;
        }

        // Aktif meshler içerisinden ilk mesh'i bul
        MeshFilter firstMesh = null;

        foreach (MeshFilter mesh in meshManager.meshes)
        {
            if (mesh != null)
            {
                firstMesh = mesh;
                break;
            }
        }

        // Mesh bulunamadıysa işlemi bitir
        if (firstMesh == null)
        {
            Debug.LogWarning("[AR MESH WARNING] MeshFilter bulunamadı!");
            return;
        }

        // Mesh GameObject'inin Transform bilgileri
        Transform meshTransform = firstMesh.transform;

        Debug.Log(
            $"[AR MESH TRANSFORM] " +
            $"Position: {meshTransform.position} | " +
            $"Rotation: {meshTransform.rotation.eulerAngles} | " +
            $"Local Rotation: {meshTransform.localRotation.eulerAngles} | " +
            $"Scale: {meshTransform.lossyScale}"
        );

        // Gerçek mesh verisi varsa Bounds ve vertex bilgilerini yazdır
        if (firstMesh.sharedMesh != null)
        {
            Bounds bounds = firstMesh.sharedMesh.bounds;

            Debug.Log(
                $"[AR MESH BOUNDS] " +
                $"Center: {bounds.center} | " +
                $"Size: {bounds.size} | " +
                $"Vertices: {firstMesh.sharedMesh.vertexCount}"
            );
        }
        else
        {
            Debug.LogWarning(
                "[AR MESH WARNING] MeshFilter üzerinde SharedMesh bulunamadı!"
            );
        }
    }
}