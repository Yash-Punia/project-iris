using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;

public class ScrollItemScript : MonoBehaviour
{
    public PolyAsset asset { get; set; }
    public GameObject objectPlacedEffectsPrefab;
    private PolyImportOptions options;

    private void Start()
    {
        options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 1.0f;
        options.recenter = true;
    }

    // Update is called once per frame
    public void ImportPolyAsset()
    {
        this.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        PolyApi.Import(asset, options, ImportAssetCallback);
    }

    private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            return;
        }
        result.Value.gameObject.AddComponent<PolyObjectPlacer>();
        result.Value.gameObject.GetComponent<PolyObjectPlacer>().SetPlacedEffects(objectPlacedEffectsPrefab);
    }
}
