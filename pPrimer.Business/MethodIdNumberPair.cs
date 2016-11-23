using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public class MethodIdNumberPair
    {
        public MethodIdNumberPair(int id, int topNumber)
        {
            this.Id = id;
            this.TopNumber = topNumber;
        }

        public int Id { get; }

        public int TopNumber { get; }
    }
}
