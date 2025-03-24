using UnityEngine;

#nullable enable

[CreateAssetMenu()]
public class HarvestableSO : ItemSO
{
    [SerializeField] private EHarvestableType harvestableType;
    [SerializeField] private HarvestedItem harvestedItem;
    public EHarvestableType HarvestableType => harvestableType;
}
