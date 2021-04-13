public class GelShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 3;

        base.ConstructBuilding(model);

        this.gameObject.name = "GS" + GSStaticData.gelShaft_counter;
        GSStaticData.gelShaft_counter++;

        ResourceManager.Instance.gelShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.gelShaftList.Add(this);
    }
}