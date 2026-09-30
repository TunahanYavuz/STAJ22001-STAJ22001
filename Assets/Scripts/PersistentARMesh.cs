using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PersistentARMesh : MonoBehaviour
{
    // Aynı GameObject üzerindeki ARMeshManager
    private ARMeshManager meshManager;

    // Kalıcı mesh'in oluşturulacağı parent
    [Header("Persistent Mesh Settings")]
    [SerializeField] private Transform persistentRoomParent;

    // Oluşturduğumuz kendi bağımsız GameObject
    private GameObject savedMeshObject;

    // Kendi kopyaladığımız Mesh
    private Mesh savedMesh;

    private void Awake()
    {
        // ARManager üzerindeki ARMeshManager'ı bul
        meshManager = GetComponent<ARMeshManager>();

        if (meshManager == null)
        {
            Debug.LogError(
                "[PERSISTENT MESH ERROR] ARMeshManager bulunamadı!"
            );
        }
    }

    private void OnEnable()
    {
        // Mesh değişikliklerini dinlemeye başla
        if (meshManager != null)
        {
            meshManager.meshInfosChanged.AddListener(OnMeshInfosChanged);
        }
    }

    private void OnDisable()
    {
        // Event dinlemeyi bırak
        if (meshManager != null)
        {
            meshManager.meshInfosChanged.RemoveListener(OnMeshInfosChanged);
        }
    }

    private void OnMeshInfosChanged(ARMeshInfosChangedEventArgs args)
    {
        // Aktif mesh yoksa işlem yapma
        if (meshManager.meshes.Count == 0)
        {
            return;
        }

        // ARMeshManager'ın tuttuğu ilk geçerli mesh'i bul
        MeshFilter sourceMeshFilter = null;

        foreach (MeshFilter meshFilter in meshManager.meshes)
        {
            if (meshFilter != null &&
                meshFilter.sharedMesh != null)
            {
                sourceMeshFilter = meshFilter;
                break;
            }
        }

        // Kaynak mesh bulunamadıysa çık
        if (sourceMeshFilter == null)
        {
            return;
        }

        // İlk kez çalışıyorsa kalıcı mesh objesini oluştur
        if (savedMeshObject == null)
        {
            CreatePersistentMeshObject();
        }

        // Kaynak mesh verisini kendi mesh'imize kopyala
        CopyMesh(sourceMeshFilter);
    }

    private void CreatePersistentMeshObject()
    {
        // Yeni bağımsız GameObject oluştur
        savedMeshObject = new GameObject("SavedRoomMesh");

        // PersistentRoom altına koy
        if (persistentRoomParent != null)
        {
            savedMeshObject.transform.SetParent(
                persistentRoomParent,
                false
            );
        }

        // MeshFilter ekle
        MeshFilter meshFilter =
            savedMeshObject.AddComponent<MeshFilter>();

        // MeshRenderer ekle
        MeshRenderer meshRenderer =
            savedMeshObject.AddComponent<MeshRenderer>();

        // Yeni bağımsız mesh oluştur
        savedMesh = new Mesh();

        // Büyük mesh desteği için
        savedMesh.indexFormat =
            UnityEngine.Rendering.IndexFormat.UInt32;

        // Oluşturduğumuz mesh'i MeshFilter'a bağla
        meshFilter.sharedMesh = savedMesh;

        Debug.Log(
            "[PERSISTENT MESH] Kalıcı mesh objesi oluşturuldu."
        );
    }

    private void CopyMesh(MeshFilter sourceMeshFilter)
    {
        // Kaynak mesh
        Mesh sourceMesh = sourceMeshFilter.sharedMesh;

        if (sourceMesh == null)
        {
            return;
        }

        // Kaynak transform bilgisini kendi objemize kopyala
        savedMeshObject.transform.position =
            sourceMeshFilter.transform.position;

        savedMeshObject.transform.rotation =
            sourceMeshFilter.transform.rotation;

        savedMeshObject.transform.localScale =
            sourceMeshFilter.transform.lossyScale;

        // Eski mesh verisini temizle
        savedMesh.Clear();

        // Vertex bilgilerini kopyala
        savedMesh.vertices = sourceMesh.vertices;

        // Normal bilgileri varsa kopyala
        if (sourceMesh.normals != null &&
            sourceMesh.normals.Length == sourceMesh.vertexCount)
        {
            savedMesh.normals = sourceMesh.normals;
        }

        // UV bilgilerini varsa kopyala
        if (sourceMesh.uv != null &&
            sourceMesh.uv.Length == sourceMesh.vertexCount)
        {
            savedMesh.uv = sourceMesh.uv;
        }

        // Üçgenleri kopyala
        savedMesh.triangles = sourceMesh.triangles;

        // Bounds yeniden hesapla
        savedMesh.RecalculateBounds();

        Debug.Log(
            $"[PERSISTENT MESH] Mesh güncellendi. " +
            $"Vertices: {savedMesh.vertexCount}"
        );
    }

    private void OnDestroy()
    {
        // Oluşturduğumuz mesh'i bellekten temizle
        if (savedMesh != null)
        {
            Destroy(savedMesh);
        }
    }
}