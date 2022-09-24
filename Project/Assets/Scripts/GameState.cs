using System;
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

    public void ApplyEffects(List<GaugeEffect> effects)
    {
        foreach(GaugeEffect effect in effects)
        {
            this.ApplyEffect(effect);
        }
    }

    public int GetGaugeValue(string gaugeID)
    {
        switch (gaugeID)
        {
            case IDENTITY_ID:
                return this.Identity;
            case TURC_NATIONALISM_ID:
                return this.TurcNationalism;
            case GREC_NATIONALISM_ID:
                return this.GrecNationalism;
            case GLOBAL_NATIONALISM_ID:
                return this.GlobalNationalism;
            case MONEY_ID:
                return this.Money;
            default:
                Debug.LogError($"Effect with ID '{gaugeID}' does not correspond with any game variables");
                return 0;
        }
    }

    private void ApplyEffect(GaugeEffect effect)
    {
        switch (effect.gauge)
        {
            case IDENTITY_ID:
                this.Identity += effect.effect;
                break;
            case TURC_NATIONALISM_ID:
                this.TurcNationalism += effect.effect;
                break;
            case GREC_NATIONALISM_ID:
                this.GrecNationalism += effect.effect;
                break;
            case GLOBAL_NATIONALISM_ID:
                this.GlobalNationalism += effect.effect;
                break;
            case MONEY_ID:
                this.Money += effect.effect;
                break;
            default:
                Debug.LogError($"Effect with ID '{effect.gauge}' does not correspond with any game variables");
                break;
        }
    }
}