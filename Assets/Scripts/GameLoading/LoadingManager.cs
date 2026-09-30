using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCanvasController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text sceneNameText;

    private int targetSceneIndex;
    private GameObject[] prefabsToInstantiate;

    public void StartLoading(int sceneIndex, GameObject[] prefabs)
    {
        DontDestroyOnLoad(transform.root.gameObject);
        targetSceneIndex = sceneIndex;
        prefabsToInstantiate = prefabs;
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetSceneIndex, LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        sceneNameText.text += $" {targetSceneIndex}"; 
        // AŞAMA 1: Sahne yükleme (%0 - %60)
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f) * 0.6f;
            UpdateUI(progress);
            yield return null;
        }

        UpdateUI(0.6f);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
            yield return null;
        CleanupDuplicateEventSystems();
        Debug.Log("Sahne yüklendi ve EventSystem temizlendi.");
        // AŞAMA 2: Resources'tan prefab'ları instantiate etme (%60 - %100)
        if (prefabsToInstantiate != null && prefabsToInstantiate.Length > 0)
        {
            var sorted = prefabsToInstantiate.OrderByDescending(p => p.GetComponentInChildren<Camera>() != null).ToArray();
            int total = sorted.Length;
            for (int i = 0; i < total; i++)
            {
                try
                {
                    Instantiate(sorted[i]);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error instantiating {sorted[i]}: {ex.Message}");
                }
                
                Debug.Log(sorted[i]+" instantiated");
                float progress = 0.6f + ((float)(i + 1) / total) * 0.4f;
                UpdateUI(progress);
                yield return null;
            }
        }

        UpdateUI(1f);
        yield return new WaitForSeconds(0.2f);

        // Canvas'ı yok et, oyun başlasın
        Destroy(transform.root.gameObject);
    }
    private void CleanupDuplicateEventSystems()
    {
        EventSystem[] allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        if (allEventSystems.Length > 1)
        {
            // İlkini bırak, gerisini yok et
            for (int i = 1; i < allEventSystems.Length; i++)
            {
                Destroy(allEventSystems[i].gameObject);
            }
        }
    }
    private void UpdateUI(float value)
    {
        progressBar.value = value;
        if (progressText != null)
            progressText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}