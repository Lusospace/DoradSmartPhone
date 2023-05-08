using DoradSmartphone;
using DoradSmartphone.Models;

namespace DoradSmartphone.Services
{
    public class ExerciseService

    {
        DateTime today = DateTime.Now;
        public ExerciseService() { }

        public List<Exercise> GetExercises() => new List<Exercise> {
            new Exercise {
                    Id= 1, Date= today.AddDays(-2)
            },
            new Exercise {
                    Id= 2, Date= today.AddDays(-1)
            },
            new Exercise {
                    Id= 3, Date= today
            }
        };

    }
}
