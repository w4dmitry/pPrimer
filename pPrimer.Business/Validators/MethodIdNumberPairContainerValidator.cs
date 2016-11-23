using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace pPrimer.Business.Validators
{
    public class MethodIdNumberPairContainerValidator : AbstractValidator<MethodIdNumberPairContainer>
    {
        public MethodIdNumberPairContainerValidator()
        {
            RuleFor(x => x.MethodIdNumberPairs).SetCollectionValidator(new MethodIdNumberPairValidator());
        }
    }

}
