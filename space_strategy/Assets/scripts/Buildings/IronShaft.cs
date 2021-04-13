public class IronShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 2;

        base.ConstructBuilding(model);

        this.gameObject.name = "IS" + ISStaticData.ironShaft_counter;
        ISStaticData.ironShaft_counter++;

        ResourceManager.Instance.ironShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.ironShaftList.Add(this);
    }
}