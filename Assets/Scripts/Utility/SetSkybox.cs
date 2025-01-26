using UnityEngine;

public class SetSkybox : MonoBehaviour
{
    public Material skyboxMaterial;
    private void OnEnable()
    {     
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
    }
}
