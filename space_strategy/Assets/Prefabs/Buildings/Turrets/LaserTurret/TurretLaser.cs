public class TurretLaser : Turette
{
    public float barrelTurnSpeed = 200f;
    public bool isLasersEnabled = false; 

    public override void DestroyBuilding()
    {
        ResourceManager.Instance.laserTurretsList.Remove(this);

        base.DestroyBuilding();
    }
}