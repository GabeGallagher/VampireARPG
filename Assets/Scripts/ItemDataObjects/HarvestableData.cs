using UnityEngine;

public class HarvestableData : ItemData
{
    readonly EHarvestableType harvestableType;

    private int quantity;

    public HarvestableData(HarvestableSO harvestableSO, int count) : base(harvestableSO)
    {
        this.harvestableType = harvestableSO.HarvestableType;
        this.quantity = count;
    }

    public HarvestableSO HarvestableSO => (HarvestableSO)ItemSO;
    public EHarvestableType HarvestableType => harvestableType;
    public int Quantity { get => quantity; set => quantity = value; }
}
