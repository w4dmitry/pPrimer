using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pPrimer.Web.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using pPrimer.Business;

    public class PrimeMethodViewModel
    {
        public bool Selected { get; set; }

        [Required]
        public int Value { get; set; }
        public string Text { get; set; }

        [Required]
        [DisplayName("Number")]
        [Range(2, int.MaxValue)]
        public int TopNumber { get; set; }
    }

    public class PrimeMethodsViewModel
    {
        public PrimeMethodsViewModel()
        {
        }

        public PrimeMethodsViewModel(IList<PrimeMethodViewModel> items)
        {
            Methods = items;
        }

        public IList<PrimeMethodViewModel> Methods { get; set; }
    }

    public class StatusModel
    {
        public IEnumerable<string> StatusList { get; }

        public bool IsSuccess { get; }

        public bool IsCompleted { get; }

        public StatusModel(PrimeCalculationStatus status)
        {
            var statusList = new List<string>();

            if (status == null)
            {
                statusList.Add(Strings.UnknownTaskSid);
                IsSuccess = false;
            }
            else
            {
                foreach (var primeTask in status.Tasks)
                {
                    if (primeTask.HasErrors)
                    {
                        statusList.Add(string.Format(Strings.ErrorOccuredDuringExecutionOf, primeTask.MethodSet.DisplayName));
                    }
                    else
                    {
                        statusList.Add(string.Format(Strings.MethodCompletedIn, primeTask.MethodSet.DisplayName, (primeTask.Task.Result.EndTime - primeTask.Task.Result.StartTime).TotalMilliseconds));
                        statusList.Add(string.Format(Strings.BiggestPrimeNumberAndTotal, primeTask.Task.Result.Primes.Max(), primeTask.Task.Result.Primes.Count()));
                    }
                }

                if (status.IsCompleted)
                {
                    IsCompleted = true;
                    statusList.Add(Strings.AllCalculationsCompleted);
                }

                IsSuccess = true;
            }

            StatusList = statusList;
        }
    }
}