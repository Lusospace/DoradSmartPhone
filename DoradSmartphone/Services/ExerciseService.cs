using Android.Media;
using DoradSmartphone.Data;
using DoradSmartphone.Models;
using Newtonsoft.Json;
using ToastProject;
using Route = DoradSmartphone.Models.Route;

namespace DoradSmartphone.Services
{
    public class ExerciseService
    {
        IToast toast;
        DateTime today = DateTime.Now;
        public readonly IRepository _repository;
        public ExerciseService(IRepository repository, IToast toast)
        {
            _repository = repository;
            this.toast = toast;
        }

        public async Task<List<Exercise>> RecoverExerciseByIdAsync()
        {
            try
            {
                User user = UserSessionHelper.GetUserFromSessionJson();

                var exercises = await _repository.RecoverExerciseByIdAsync<Exercise>(null, user.Id);

                foreach (var exercise in exercises)
                {
                    if (exercise.Route != null && exercise.Route.Count > 0)
                    {
                        var firstRoute = exercise.Route[0];
                        exercise.StartingAddress = await GoogleMapsGeocoding.GetAddressName(firstRoute.Latitude, firstRoute.Longitude);
                        var lastRoute = exercise.Route[exercise.Route.Count - 1];
                        exercise.FinishingAddress = await GoogleMapsGeocoding.GetAddressName(lastRoute.Latitude, lastRoute.Longitude);
                    }
                }
                return exercises;
            } catch(Exception ex)
            {
                toast.MakeToast("Error when grabing Exercises information. " + ex);
                throw new Exception("Error retrieving exercises from database.");
            }            
        }

        public async void InsertExercises()
        {
            TimeSpan difference1 = today - today.AddMinutes(45);
            string differenceFormatted1 = difference1.ToString(@"hh\:mm\:ss");
            TimeSpan difference2 = today - today.AddMinutes(30);
            string differenceFormatted2 = difference2.ToString(@"hh\:mm\:ss");
            TimeSpan difference3 = today - today.AddMinutes(60);
            string differenceFormatted3 = difference3.ToString(@"hh\:mm\:ss");
            TimeSpan difference4 = today - today.AddMinutes(20);
            string differenceFormatted4 = difference4.ToString(@"hh\:mm\:ss");

            User user = UserSessionHelper.GetUserFromSessionJson();


            var speed1 = new Speed
            {
                Avg = 10.5f,
                Max = 15.2f,
            };

            var route1 = new List<Route>
            {
                new Route { Latitude = 35.6823324582143, Longitude = 139.7620853729577 },
                new Route { Latitude = 35.679263477092704, Longitude = 139.75773939496295 },
                new Route { Latitude = 35.68748054650018, Longitude = 139.761486207315 },
                new Route { Latitude = 35.690745005825136, Longitude = 139.7560362984393 },
                new Route { Latitude = 35.68966608916097, Longitude = 139.75147199952355 },
                new Route { Latitude = 35.68427128680411, Longitude = 139.7442168083328 }
            };

            var exercise1 = new Exercise
            {
                StartingDate = today,
                FinishingDate = today.AddMinutes(45),
                Time = differenceFormatted1,
                UserId = user.Id,
                Speed = speed1,
                Route = route1
            };

            await _repository.SaveItensAsync(exercise1);


            var speed2 = new Speed
            {
                Avg = 12.5f,
                Max = 16.2f,
            };

            var route2 = new List<Route>
            {
                new Route { Latitude = 38.70061856336034, Longitude = -8.957381918676203 },
                new Route { Latitude = 38.70671683905933, Longitude = -8.945225024701308 },
                new Route { Latitude = 38.701985630081595, Longitude = -8.944503277546072 },
                new Route { Latitude = 38.701872978433386, Longitude = -8.940750192338834 },
                new Route { Latitude = 38.71054663609023, Longitude = -8.939162348597312 },
                new Route { Latitude = 38.717755109243214, Longitude = -8.942193686649311 },
                new Route { Latitude = 38.7435419727561, Longitude = -8.928480490699792 },
                new Route { Latitude = 38.78327379379296, Longitude = -8.880556478454272 },
                new Route { Latitude = 38.925473761602376, Longitude = -8.881999972299806 },
                new Route { Latitude = 38.93692729913667, Longitude = -8.869585920414709 },
                new Route { Latitude = 38.93493556584553, Longitude = -8.86536198145887 }
            };

            var exercise2 = new Exercise
            {
                StartingDate = today,
                FinishingDate = today.AddMinutes(30),
                Time = differenceFormatted2,
                UserId = user.Id,
                Speed = speed2,
                Route = route2
            };

            await _repository.SaveItensAsync(exercise2);

            var speed3 = new Speed
            {
                Avg = 13.5f,
                Max = 14.8f,
            };

            var route3 = new List<Route>
            {
                new Route { Latitude = -1.4412474319742032, Longitude = -48.485914192075455 },
                new Route { Latitude = -1.4415280369316321, Longitude = -48.48548039261385 },
                new Route { Latitude = -1.438135265584208, Longitude = -48.47889684784361 },
                new Route { Latitude = -1.4519869242562538, Longitude = -48.47759544945879 },
                new Route { Latitude = -1.4515756786484433, Longitude = -48.47169012644734 }
            };

            var exercise3 = new Exercise
            {
                StartingDate = today,
                FinishingDate = today.AddMinutes(60),
                Time = differenceFormatted3,
                UserId = user.Id,
                Speed = speed3,
                Route = route3
            };

            await _repository.SaveItensAsync(exercise3);

            var speed4 = new Speed
            {
                Avg = 13.5f,
                Max = 14.8f,
            };

            var route4 = new List<Route>
            {
                new Route { Latitude = 38.70061856336034, Longitude = -8.957381918676203 },
                new Route { Latitude = 38.70671683905933, Longitude = -8.945225024701308 },
                new Route { Latitude = 38.701985630081595, Longitude = -8.944503277546072 },
                new Route { Latitude = 38.701872978433386, Longitude = -8.940750192338834 },
                new Route { Latitude = 38.71054663609023, Longitude = -8.939162348597312 },
                new Route { Latitude = 38.717755109243214, Longitude = -8.942193686649311 },
                new Route { Latitude = 38.7435419727561, Longitude = -8.928480490699792 },
                new Route { Latitude = 38.78327379379296, Longitude = -8.880556478454272 },
                new Route { Latitude = 38.925473761602376, Longitude = -8.881999972299806 },
                new Route { Latitude = 38.93692729913667, Longitude = -8.869585920414709 },
                new Route { Latitude = 38.93493556584553, Longitude = -8.86536198145887 }
            };

            var exercise4 = new Exercise
            {
                StartingDate = today,
                FinishingDate = today.AddMinutes(20),
                Time = differenceFormatted4,
                UserId = user.Id,
                Speed = speed4,
                Route = route4
            };

            await _repository.SaveItensAsync(exercise4);
        }

        public async void ClearAll()
        {
            await _repository.DeleteAllItemsAsync<Exercise>(); // Delete all Exercise items from the table
            await _repository.DeleteAllItemsAsync<Speed>();    // Delete all Speed items from the table
            await _repository.DeleteAllItemsAsync<Route>();   // Delete all Route items from the table
        }
    }
}
