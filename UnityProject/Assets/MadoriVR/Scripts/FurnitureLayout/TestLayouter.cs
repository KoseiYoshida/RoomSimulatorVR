using MadoriVR.Scripts.FurnitureLayout;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public sealed class TestLayouter : MonoBehaviour
{
    [SerializeField] private GameObject parentPrefab = default;
    
    
    [SerializeField] private Button chairButton = default;
    [SerializeField] private Button tableButton = default;
    [SerializeField] private Button couchButton = default;

    [SerializeField] private GameObject chairPrefab = default;
    [SerializeField] private GameObject tablePrefab = default;
    [SerializeField] private GameObject couchPrefab = default;

    private void Start()
    {
        chairButton.OnClickAsObservable()
            .Subscribe(_ => GenerateModel(chairPrefab))
            .AddTo(this);
        
        tableButton.OnClickAsObservable()
            .Subscribe(_ => GenerateModel(tablePrefab))
            .AddTo(this);
        
        couchButton.OnClickAsObservable()
            .Subscribe(_ => GenerateModel(couchPrefab))
            .AddTo(this);
    }

    private void GenerateModel(GameObject prefab)
    {
        var parent = Instantiate(parentPrefab);
        // TODO: 疎結合にする
        parent.GetComponent<Furniture>().Initialize(prefab);
    }
}
