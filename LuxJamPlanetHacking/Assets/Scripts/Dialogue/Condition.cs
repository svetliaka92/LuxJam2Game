using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField] Disjunction[] and;

    public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
    {
        foreach (Disjunction disjunction in and)
            if (!disjunction.Check(evaluators))
                return false;

        return true;
    }

    [System.Serializable]
    class Disjunction
    {
        [SerializeField] Predicate[] or;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Predicate predicate in or)
                if (predicate.Check(evaluators))
                    return true;

            return false;
        }
    }

    [System.Serializable]
    class Predicate
    {
        [SerializeField] string predicate;
        [SerializeField] bool isNegated = false;
        [SerializeField] string[] parameters;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (IPredicateEvaluator evaluator in evaluators)
            {
                bool? result = evaluator.Evaluate(predicate, parameters);
                if (result == null)
                    continue;

                if (result == isNegated)
                    return false;
            }

            return true;
        }
    }
}
