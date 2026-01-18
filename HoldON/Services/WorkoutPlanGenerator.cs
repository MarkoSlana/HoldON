using HoldON.Models;

namespace HoldON.Services;

public class WorkoutPlanGenerator
{
    private readonly ExerciseVideoService _videoService = new();

    public WorkoutPlan GeneratePlan(string goal, string equipment, string level, int daysPerWeek)
    {
        var plan = new WorkoutPlan
        {
            Goal = goal,
            EquipmentType = equipment,
            Level = level,
            DaysPerWeek = daysPerWeek,
            DurationWeeks = 8
        };

        // Generate plan based on goal
        if (goal == "Mišična masa")
        {
            plan.Name = "Pridobivanje mišične mase";
            plan.Description = "8-tedenski program za povečanje mišične mase z intenzivnim treningom";
            GenerateMuscleBuildingPlan(plan, equipment, level, daysPerWeek);
        }
        else if (goal == "Hujšanje")
        {
            plan.Name = "Hujšanje in definicija";
            plan.Description = "8-tedenski program za izgubo maščobe in definicijo mišic";
            GenerateWeightLossPlan(plan, equipment, level, daysPerWeek);
        }
        else if (goal == "Moč")
        {
            plan.Name = "Razvoj moči";
            plan.Description = "8-tedenski program za povečanje moči v osnovnih vajah";
            GenerateStrengthPlan(plan, equipment, level, daysPerWeek);
        }
        else if (goal == "Kondicija")
        {
            plan.Name = "Izboljšanje kondicije";
            plan.Description = "8-tedenski program za kardiovaskularno kondicijo";
            GenerateConditioningPlan(plan, equipment, level, daysPerWeek);
        }

        return plan;
    }

    private void GenerateMuscleBuildingPlan(WorkoutPlan plan, string equipment, string level, int daysPerWeek)
    {
        if (daysPerWeek >= 3)
        {
            // Ponedeljek - Potisni dan
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 1,
                DayName = "Ponedeljek",
                Focus = "Potisni dan (Prsi, Ramena, Triceps)",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Bench press", 4, "8-12", "90s"),
                    CreateExercise("Poševni bench press z giricami", 3, "10-12", "60s"),
                    CreateExercise("Vojaški pritisk", 3, "10-12", "60s"),
                    CreateExercise("Dviganje sveč bočno", 3, "12-15", "45s"),
                    CreateExercise("Iztegi za triceps", 3, "12-15", "45s")
                }
            });

            // Sreda - Vlečni dan
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "Vlečni dan (Hrbet, Biceps)",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Mrtvi dvig", 4, "6-10", "120s"),
                    CreateExercise("Veslanje s palico", 4, "8-12", "90s"),
                    CreateExercise("Lat pulldown", 3, "10-12", "60s"),
                    CreateExercise("Dvigovanje uteži za biceps", 3, "10-12", "60s"),
                    CreateExercise("Hammer curls", 3, "12-15", "45s")
                }
            });

            // Petek - Noge
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Dan nog (Kvadriceps, Zadnjice, Listi)",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Počep s palico", 4, "8-12", "120s"),
                    CreateExercise("Leg press", 4, "10-12", "90s"),
                    CreateExercise("Rumunski mrtvi dvig", 3, "10-12", "90s"),
                    CreateExercise("Leg curl", 3, "12-15", "60s"),
                    CreateExercise("Dvigovanje na prste", 4, "15-20", "45s")
                }
            });
        }

        // Add rest days
        AddRestDays(plan);
    }

    private void GenerateWeightLossPlan(WorkoutPlan plan, string equipment, string level, int daysPerWeek)
    {
        if (daysPerWeek >= 3)
        {
            // Ponedeljek - Full body + cardio
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 1,
                DayName = "Ponedeljek",
                Focus = "Celotno telo + kardio",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Počep s palico", 3, "12-15", "60s"),
                    CreateExercise("Bench press", 3, "12-15", "60s"),
                    CreateExercise("Veslanje", 3, "12-15", "60s"),
                    CreateExercise("Vojaški pritisk", 3, "12-15", "60s"),
                    CreateExercise("Kardio (tek/kolesarjenje)", 1, "20 min", "-")
                }
            });

            // Sreda - HIIT
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "HIIT trening",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Burpees", 4, "30s vklop / 30s izklop", "-"),
                    CreateExercise("Mountain climbers", 4, "30s vklop / 30s izklop", "-"),
                    CreateExercise("Skakanje z vrvico", 4, "30s vklop / 30s izklop", "-"),
                    CreateExercise("Kettlebell swing", 4, "30s vklop / 30s izklop", "-")
                }
            });

            // Petek - Full body circuit
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Celotno telo - krog",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Mrtvi dvig", 3, "12-15", "60s"),
                    CreateExercise("Potisk", 3, "12-15", "60s"),
                    CreateExercise("Dvigovanje uteži", 3, "12-15", "60s"),
                    CreateExercise("Planking", 3, "60s", "60s")
                }
            });
        }

        AddRestDays(plan);
    }

    private void GenerateStrengthPlan(WorkoutPlan plan, string equipment, string level, int daysPerWeek)
    {
        if (daysPerWeek >= 3)
        {
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 1,
                DayName = "Ponedeljek",
                Focus = "Bench press",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Bench press", 5, "5", "3-5 min", "85-90% 1RM"),
                    CreateExercise("Poševni bench press", 3, "8", "2 min"),
                    CreateExercise("Dips", 3, "8-10", "90s"),
                    CreateExercise("Triceps work", 3, "10-12", "60s")
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "Počep",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Počep s palico", 5, "5", "3-5 min", "85-90% 1RM"),
                    CreateExercise("Front squat", 3, "6-8", "2 min"),
                    CreateExercise("Leg press", 3, "10-12", "90s"),
                    CreateExercise("Bulgarian split squat", 3, "8-10", "60s")
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Mrtvi dvig",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Mrtvi dvig", 5, "5", "3-5 min", "85-90% 1RM"),
                    CreateExercise("Rumunski mrtvi dvig", 3, "8", "2 min"),
                    CreateExercise("Veslanje s palico", 4, "8-10", "90s"),
                    CreateExercise("Pull-ups", 3, "AMRAP", "2 min")
                }
            });
        }

        AddRestDays(plan);
    }

    private void GenerateConditioningPlan(WorkoutPlan plan, string equipment, string level, int daysPerWeek)
    {
        if (daysPerWeek >= 3)
        {
            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 1,
                DayName = "Ponedeljek",
                Focus = "Kardio - interval",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Ogrevanje", 1, "5 min", "-"),
                    CreateExercise("Sprint intervali", 8, "30s sprint / 90s počitek", "-"),
                    CreateExercise("Ohlajanje", 1, "5 min", "-")
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "Circuit training",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Burpees", 3, "15", "30s"),
                    CreateExercise("Kettlebell swing", 3, "20", "30s"),
                    CreateExercise("Box jumps", 3, "12", "30s"),
                    CreateExercise("Battle ropes", 3, "30s", "30s"),
                    CreateExercise("Rowing machine", 3, "500m", "2 min")
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Steady state cardio",
                Exercises = new List<PlannedExercise>
                {
                    CreateExercise("Tek/kolesarjenje", 1, "30-45 min", "-", "Vzdržljiv tempo")
                }
            });
        }

        AddRestDays(plan);
    }

    private PlannedExercise CreateExercise(string name, int sets, string reps, string restTime, string notes = "")
    {
        return new PlannedExercise
        {
            Name = name,
            Sets = sets,
            Reps = reps,
            RestTime = restTime,
            Notes = notes,
            YouTubeUrl = _videoService.GetVideoUrl(name)
        };
    }

    private void AddRestDays(WorkoutPlan plan)
    {
        var workoutDays = plan.WorkoutDays.Select(d => d.DayNumber).ToList();
        var daysOfWeek = new[] { "Ponedeljek", "Torek", "Sreda", "Četrtek", "Petek", "Sobota", "Nedelja" };

        for (int i = 1; i <= 7; i++)
        {
            if (!workoutDays.Contains(i))
            {
                plan.WorkoutDays.Add(new WorkoutDay
                {
                    DayNumber = i,
                    DayName = daysOfWeek[i - 1],
                    Focus = "Počitek",
                    IsRestDay = true,
                    Exercises = new List<PlannedExercise>()
                });
            }
        }

        // Sort by day number
        plan.WorkoutDays = plan.WorkoutDays.OrderBy(d => d.DayNumber).ToList();
    }

    public List<WorkoutPlan> GetAvailablePlans()
    {
        var plans = new List<WorkoutPlan>();

        // Generate default plans for all combinations
        var goals = new[] { "Mišična masa", "Hujšanje", "Moč", "Kondicija" };

        foreach (var goal in goals)
        {
            plans.Add(GeneratePlan(goal, "Gym", "Začetnik", 3));
        }

        return plans;
    }
}
