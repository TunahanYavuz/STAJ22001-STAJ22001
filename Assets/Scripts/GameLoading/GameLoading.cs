using UnityEngine;

public class GameLoading : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvasPrefab;

    public void Awake()
    {
        // Resources/Prefabs klasöründeki tüm prefab'ları çek
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Ana Menü/Prefabs");

        // Loading canvas'ı mevcut sahnede instantiate et
        GameObject canvasObj = Instantiate(loadingCanvasPrefab);
        Camera cam = Camera.main;
        canvasObj.transform.position = cam.transform.position + cam.transform.forward * 10f;
        canvasObj.transform.LookAt(cam.transform);
        LoadingCanvasController controller = canvasObj.GetComponentInChildren<LoadingCanvasController>();

        controller.StartLoading(0, prefabs);
    }
}
