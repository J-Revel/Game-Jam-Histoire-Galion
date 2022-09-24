using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public const string IDENTITY_ID = "I";
    public const string TURC_NATIONALISM_ID = "NT";
    public const string GREC_NATIONALISM_ID = "NG";
    public const string GLOBAL_NATIONALISM_ID = "N";
    public const string MONEY_ID = "M";
    public static int PUBLISHING_COST = 2000;

    public int Identity = 0;
    public int TurcNationalism = 0;
    public int GrecNationalism = 0;
    public int GlobalNationalism = 0;
    public int Money = 5000;

    public GameState()
    {

    }

    public void ApplyEffects(List<GameEffects> effects)
    {
        foreach(GameEffects effect in effects)
        {
            this.ApplyEffect(effect);
        }
    }

    private void ApplyEffect(GameEffects effect)
    {
        switch (effect.ID)
        {
            case IDENTITY_ID:
                this.Identity += effect.Variation;
                break;
            case TURC_NATIONALISM_ID:
                this.TurcNationalism += effect.Variation;
                break;
            case GREC_NATIONALISM_ID:
                this.GrecNationalism += effect.Variation;
                break;
            case GLOBAL_NATIONALISM_ID:
                this.Identity += effect.Variation;
                break;
            case MONEY_ID:
                this.Money += effect.Variation;
                break;
            default:
                Debug.LogError($"Effect with ID '{effect.ID}' does not correspond with any game variables");
                break;
        }
    }
}

public class GameEffects
{
    public string ID;
    public int Variation;

    public GameEffects(string iD, int variation)
    {
        this.ID = iD;
        this.Variation = variation;
    }
}