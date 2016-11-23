using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using FluentValidation;

namespace pPrimer.Business.Validators
{
    public class MethodIdNumberPairValidator : AbstractValidator<MethodIdNumberPair>
    {
        public MethodIdNumberPairValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(0, byte.MaxValue);

            RuleFor(x => x.TopNumber).InclusiveBetween(PrimeNumber.FIRST_PRIME_NUMBER, int.MaxValue);
        }
    }
}
