using UnityEngine;

public static class DifficultyScaler
{
    
    public static int GetDepth() => StaticData.section;

    public static int GetEnemyCount()
    {
        return Mathf.Clamp(GetDepth(), 1, 6); // Depth 1 will spawn 1 enemy
    }

    public static float GetRamSpeed() {
        return 10f + GetDepth() * 5f;
    }

    public static float GetEmpCooldown() {
        return Mathf.Max(3f - GetDepth() * 0.3f, 0.5f);
    }

    public static float GetProjectileDamage() {
        return 1f + (GetDepth() - 1);
    }

    public static bool ShouldAllowRam() => GetDepth() >= 2;
    public static bool ShouldAllowEMP() => GetDepth() >= 1;
    public static bool ShouldAllowProjectile() => GetDepth() >= 1;

}
