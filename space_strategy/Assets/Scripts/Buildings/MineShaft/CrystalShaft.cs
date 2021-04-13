public class CrystalShaft : MineShaft
{
    public override void ConstructBuilding(Model model)
    {
        type = 1;

        base.ConstructBuilding(model);

        this.gameObject.name = "CS" + CSStaticData.crystalShaft_counter;
        CSStaticData.crystalShaft_counter++;

        ResourceManager.Instance.crystalShaftList.Add(this);
    }

    public void ConstructShaftFromFile()
    {
        ResourceManager.Instance.crystalShaftList.Add(this);
    }
}