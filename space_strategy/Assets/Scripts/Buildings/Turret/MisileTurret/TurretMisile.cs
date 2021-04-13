public class TurretMisile : Turette
{
    public bool isFired = false;
    public float coolDownTimer = 1f;

    public override void ResetCombatMode()
    {
        isFacingEnemy = false;
    }

    public override void DestroyBuilding()
    {
        ResourceManager.Instance.misileTurretsList.Remove(this);

        base.DestroyBuilding();
    }
}