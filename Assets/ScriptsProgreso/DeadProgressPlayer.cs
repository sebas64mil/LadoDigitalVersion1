using UnityEngine;

public static class DeadProgressPlayer
{
    private const string DeathsKey = "PlayerDeaths"; // Guarda total de muertes

    /// Suma una muerte al contador
    public static void AddDeath()
    {
        int current = PlayerPrefs.GetInt(DeathsKey, 0);
        current++;
        PlayerPrefs.SetInt(DeathsKey, current);
        PlayerPrefs.Save();
    }

    /// Obtiene total de muertes acumuladas
    public static int GetTotalDeaths()
    {
        return PlayerPrefs.GetInt(DeathsKey, 0);
    }

    /// Obtiene la parte "X" (0–100)
    public static int GetDeathsModulo()
    {
        int total = GetTotalDeaths();
        return total % 100;
    }

    /// Obtiene la parte "Y" (cuántas veces llegó a 100)
    public static int GetDeathsHundreds()
    {
        int total = GetTotalDeaths();
        return total / 100;
    }

    public static void ResetDeaths()
    {
        PlayerPrefs.SetInt(DeathsKey, 0);
        PlayerPrefs.Save();
    }

}
