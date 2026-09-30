using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{

    public void GoToSceneByIndex(int sceneIndex) // Bu fonksiyon, sahne indeksine göre sahne değişimini gerçekleştirir.
    {
        Debug.Log("[XR] Butona Basıldı. Şu anki Sahne: " + SceneManager.GetActiveScene().name + " -> Hedef İndeks: " + sceneIndex); // Butona basıldığında sahne değişimi yapılacak ve log mesajı yazdırılacak.
        SceneManager.LoadScene(sceneIndex); // Sahne değişimi yapılır.
    }

}