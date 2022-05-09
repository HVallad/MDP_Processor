using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDPProcessor.MVVM.Model
{
    public class FiniteHorizon
    {
        public float DiscountFactor { get; set; } = 1;

        public int Iteration { get; set; } = 3;
    }
}
