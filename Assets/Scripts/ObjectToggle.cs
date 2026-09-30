using UnityEngine;

public class ObjectToggle : MonoBehaviour


{
    [Header("1 Tuşunda Açılacak / 2 Tuşunda Kapanacak Objeler")]
    public GameObject[] mode1Objects;

    [Header("1 Tuşunda Kapanacak / 2 Tuşunda Açılacak Objeler")]
    public GameObject[] mode2Objects;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetObjects(mode1Objects, true);
            SetObjects(mode2Objects, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetObjects(mode1Objects, false);
            SetObjects(mode2Objects, true);
        }
    }

    private void SetObjects(GameObject[] objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }
}