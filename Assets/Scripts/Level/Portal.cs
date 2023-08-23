using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    SceneChange sceneChanger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            sceneChanger.Scene3();
    }
}
