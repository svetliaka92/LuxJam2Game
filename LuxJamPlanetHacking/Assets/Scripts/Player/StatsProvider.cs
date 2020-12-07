using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsProvider : MonoBehaviour, IPredicateEvaluator
{
    [System.Serializable]
    class PlayerStat
    {
        public string statName;
        public Stat stat;
        public int value;
    }

    [SerializeField] private PlayerStat[] playerStats;

    private Dictionary<string, PlayerStat> statsLookup;

    private void Awake()
    {
        BuildStatsLookup();
    }

    private void BuildStatsLookup()
    {
        foreach (PlayerStat stat in playerStats)
        {
            statsLookup[stat.statName] = stat;
        }
    }

    public bool? Evaluate(string predicate, string[] paremeters)
    {
        if (statsLookup.ContainsKey(predicate))
        {
            int requiredValue = int.Parse(paremeters[0]);

            return statsLookup[predicate].value >= requiredValue;
        }

        return null;
    }
}
