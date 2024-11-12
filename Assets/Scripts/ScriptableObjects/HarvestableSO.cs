using UnityEngine;

[CreateAssetMenu()]
public class HarvestableSO : ItemSO
{
    public enum EHarvestableType
    {
        Wood,
        Copper,
    }

    [SerializeField] private EHarvestableType harvestableType;

    public EHarvestableType HarvestableType { get => harvestableType; }
}
