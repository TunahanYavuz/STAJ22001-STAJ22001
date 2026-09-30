using UnityEngine;

public class Scene3PrefabLoader : MonoBehaviour
{
    void Start()
    {
        if (!SceneTransitionData.hasSpawnData)
        {
            Debug.LogWarning(
                "Scene 3'e aktarılmış spawn bilgisi bulunamadı."
            );

            return;
        }

        if (SceneTransitionData.targetPrefab == null)
        {
            Debug.LogError(
                "Scene 3 için Target Prefab bulunamadı."
            );

            return;
        }

        GameObject spawnedPrefab = Instantiate(
            SceneTransitionData.targetPrefab,
            SceneTransitionData.spawnPosition,
            SceneTransitionData.spawnRotation
        );

        Debug.Log(
            "Scene 3 prefab oluşturuldu: " +
            spawnedPrefab.name
        );

        Debug.Log(
            "Prefab pozisyonu: " +
            spawnedPrefab.transform.position
        );

        // Veri kullanıldı.
        SceneTransitionData.hasSpawnData = false;
    }
}