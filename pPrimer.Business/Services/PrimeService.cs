using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using log4net;

using pPrimer.Business.Tools;
using pPrimer.Business.TypeResolver;
using pPrimer.Business.Properties;

namespace pPrimer.Business.Services
{
    using FluentValidation;

    public class PrimeService : IPrimeService, IDisposable
    {
        private ConcurrentDictionary<string, List<PrimeCalculationTask>> _tasks = new ConcurrentDictionary<string, List<PrimeCalculationTask>>();

        private readonly object _taskCheckLocker = new object();

        private readonly IPrimeFacilitiesResolver _resolver;

        private readonly Task _cleanerTask;

        private readonly TimeSpan _cleanerTaskPeriod = TimeSpan.FromMinutes(1);

        private readonly double TASK_TIMEOUT = 5.0;

        private readonly TaskCompletionSource<bool> _cleanerTcs;

        private readonly ILog _logger;

        private readonly IValidator<MethodIdNumberPairContainer> _methodIdNumberPairContainerValidator;

        public PrimeService(IPrimeFacilitiesResolver resolver, ILog logger, IValidator<MethodIdNumberPairContainer> methodIdNumberPairContainerValidator)
        {
            if (resolver == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "resolver"));
            _resolver = resolver;

            if (logger == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "logger"));
            _logger = logger;

            if (methodIdNumberPairContainerValidator == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "methodIdNumberPairContainerValidator"));
            _methodIdNumberPairContainerValidator = methodIdNumberPairContainerValidator;

            _cleanerTcs = ((Action)CleanUpTasks).ToPeriodicTask<bool>(_cleanerTaskPeriod, _cleanerTaskPeriod);
        }

        private IList<PrimeCalculationTask> RemoveTimedOutTasks(List<PrimeCalculationTask> tasks, out bool hasTasksLeft)
        {
            var timedOutTasks = new List<PrimeCalculationTask>();

            // Breif lock to get completed tasks
            // Some contention is possible due to cleaning/status operations, low probability
            lock (_taskCheckLocker)
            {
                var timedOut = tasks.Where(task => (DateTime.UtcNow - task.StartTime).TotalMinutes > TASK_TIMEOUT).ToList();
                timedOutTasks.AddRange(timedOut);
                timedOut.ForEach(t => tasks.Remove(t));
                hasTasksLeft = tasks.Any();
            }

            return timedOutTasks;
        }
        private void CleanUpTasks()
        {
            foreach (var task in _tasks)
            {
                bool hasTasksLeft;
                var timedOutTasks = RemoveTimedOutTasks(task.Value, out hasTasksLeft);

                if (!hasTasksLeft)
                {
                    List<PrimeCalculationTask> value;
                    _tasks.TryRemove(task.Key, out value);
                }

                foreach (var pct in timedOutTasks)
                    _logger.Warn($"Task '{pct.MethodSet.DisplayName}' has timed out with status '{pct.Task.Status}'.");
            }
        }

        public IEnumerable<PrimeMethodSet> MethodsSets => _resolver.MethodSets;

        public string StartCalculation(MethodIdNumberPairContainer methodIdNumberPairContainer)
        {
            if (methodIdNumberPairContainer == null ||
                methodIdNumberPairContainer.MethodIdNumberPairs == null ||
                methodIdNumberPairContainer.MethodIdNumberPairs.Count() == 0)
                return null;

            var validationResult = _methodIdNumberPairContainerValidator.Validate(methodIdNumberPairContainer);
            if (!validationResult.IsValid)
                throw new ArgumentException(string.Join("\n", validationResult.Errors));

            var taskId = Guid.NewGuid().ToString("N");

            var selectedMethods = (from method in _resolver.MethodSets
                                  join pair in methodIdNumberPairContainer.MethodIdNumberPairs on method.Id equals pair.Id
                                  select new MethodSetTopNumberPair { MethodSet = method, TopNumber = pair.TopNumber })
                                  .ToList();

            if (selectedMethods.Count == 0)
                return null;

            var tasksToRun = selectedMethods.Select(item =>
                    {
                        return new PrimeCalculationTask
                                   {
                                       Task = Task.Run(() =>
                                               {
                                                   var performanceRunner = _resolver.GetPerformanceRunner(item.MethodSet.MethodType, item.MethodSet.RunnerType);
                                                   performanceRunner.GetAllNumbers(item.TopNumber);
                                                   return performanceRunner.Result;
                                               }),
                                       MethodSet = item.MethodSet,
                                       StartTime = DateTime.UtcNow
                                   };
                    }).ToArray();

            if (!_tasks.TryAdd(taskId, new List<PrimeCalculationTask>(tasksToRun)))
                _logger.Warn($"Coudn't add the task with id:'{taskId}'.");

            return taskId;
        }

        private IList<PrimeCalculationTask> GetCompletedTasks(List<PrimeCalculationTask> tasks, out bool hasTasksLeft)
        {
            var completedTasks = new List<PrimeCalculationTask>();

            // Breif lock to get completed tasks
            // Some contention is possible due to cleaning/status operations, low probability
            lock (_taskCheckLocker)
            {
                var completed = tasks.Where(task => task.Task.IsCompleted).ToList();
                completedTasks.AddRange(completed);
                completed.ForEach(t => tasks.Remove(t));
                hasTasksLeft = tasks.Any();
            }

            return completedTasks;
        }

        public PrimeCalculationStatus GetStatus(string sid)
        {
            List<PrimeCalculationTask> tasks;
            if (_tasks.TryGetValue(sid, out tasks))
            {
                bool hasTasksLeft;
                var completedTasks = GetCompletedTasks(tasks, out hasTasksLeft);

                if (!hasTasksLeft)
                {
                    List<PrimeCalculationTask> value;
                    _tasks.TryRemove(sid, out value);
                }

                // Check tasks
                foreach (var task in completedTasks)
                {
                    try
                    {
                        // Handle Exceptions if any
                        var result = task.Result;
                    }
                    catch (AggregateException aex)
                    {
                        task.HasErrors = true;
                        task.Errors = new List<string>(new[] { string.Format(Strings.TaskHasFailed, task.MethodSet.DisplayName) });

                        _logger.Error($"Exception while executing task '{task.MethodSet.DisplayName}': {string.Join("\n", aex.Flatten().InnerExceptions.Select(ex => ex.ToString()))}");
                    }
                }

                return new PrimeCalculationStatus { IsCompleted = !hasTasksLeft, Tasks = completedTasks };
            }

            return null;
        }

        public void Dispose()
        {
            // Dispose called by Unity for ContainerControlledLifetimeManager
            _cleanerTcs.TrySetCanceled();

            if (_cleanerTcs.Task.IsFaulted)
            {
                _logger.Error("Cleaner task faulted!");
            }
        }
    }
}
