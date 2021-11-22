using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject
{
    public class ScoreCalculator
    {
        public int AddScores(IEnumerable<int> scores) => scores.Sum();
        //{
        //    var sum = 0;
        //    foreach ( var score in scores)
        //    {
        //        sum += score;
                
        //    }
        //    return sum;
        //}
    }
}
