using HoldON.Models;

namespace HoldON.Services;

public class WorkoutPlanGenerator
{
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
                    new PlannedExercise { Name = "Bench press", Sets = 4, Reps = "8-12", RestTime = "90s" },
                    new PlannedExercise { Name = "Poševni bench press z giricami", Sets = 3, Reps = "10-12", RestTime = "60s" },
                    new PlannedExercise { Name = "Vojaški pritisk", Sets = 3, Reps = "10-12", RestTime = "60s" },
                    new PlannedExercise { Name = "Dviganje sveč bočno", Sets = 3, Reps = "12-15", RestTime = "45s" },
                    new PlannedExercise { Name = "Iztegi za triceps", Sets = 3, Reps = "12-15", RestTime = "45s" }
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
                    new PlannedExercise { Name = "Mrtvi dvig", Sets = 4, Reps = "6-10", RestTime = "120s" },
                    new PlannedExercise { Name = "Veslanje s palico", Sets = 4, Reps = "8-12", RestTime = "90s" },
                    new PlannedExercise { Name = "Lat pulldown", Sets = 3, Reps = "10-12", RestTime = "60s" },
                    new PlannedExercise { Name = "Dvigovanje uteži za biceps", Sets = 3, Reps = "10-12", RestTime = "60s" },
                    new PlannedExercise { Name = "Hammer curls", Sets = 3, Reps = "12-15", RestTime = "45s" }
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
                    new PlannedExercise { Name = "Počep s palico", Sets = 4, Reps = "8-12", RestTime = "120s" },
                    new PlannedExercise { Name = "Leg press", Sets = 4, Reps = "10-12", RestTime = "90s" },
                    new PlannedExercise { Name = "Rumunski mrtvi dvig", Sets = 3, Reps = "10-12", RestTime = "90s" },
                    new PlannedExercise { Name = "Leg curl", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Dvigovanje na prste", Sets = 4, Reps = "15-20", RestTime = "45s" }
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
                    new PlannedExercise { Name = "Počep s palico", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Bench press", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Veslanje", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Vojaški pritisk", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Kardio (tek/kolesarjenje)", Sets = 1, Reps = "20 min", RestTime = "-" }
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
                    new PlannedExercise { Name = "Burpees", Sets = 4, Reps = "30s vklop / 30s izklop", RestTime = "-" },
                    new PlannedExercise { Name = "Mountain climbers", Sets = 4, Reps = "30s vklop / 30s izklop", RestTime = "-" },
                    new PlannedExercise { Name = "Skakanje z vrvico", Sets = 4, Reps = "30s vklop / 30s izklop", RestTime = "-" },
                    new PlannedExercise { Name = "Kettlebell swing", Sets = 4, Reps = "30s vklop / 30s izklop", RestTime = "-" }
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
                    new PlannedExercise { Name = "Mrtvi dvig", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Potisk", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Dvigovanje uteži", Sets = 3, Reps = "12-15", RestTime = "60s" },
                    new PlannedExercise { Name = "Planking", Sets = 3, Reps = "60s", RestTime = "60s" }
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
                    new PlannedExercise { Name = "Bench press", Sets = 5, Reps = "5", RestTime = "3-5 min", Notes = "85-90% 1RM" },
                    new PlannedExercise { Name = "Poševni bench press", Sets = 3, Reps = "8", RestTime = "2 min" },
                    new PlannedExercise { Name = "Dips", Sets = 3, Reps = "8-10", RestTime = "90s" },
                    new PlannedExercise { Name = "Triceps work", Sets = 3, Reps = "10-12", RestTime = "60s" }
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "Počep",
                Exercises = new List<PlannedExercise>
                {
                    new PlannedExercise { Name = "Počep s palico", Sets = 5, Reps = "5", RestTime = "3-5 min", Notes = "85-90% 1RM" },
                    new PlannedExercise { Name = "Front squat", Sets = 3, Reps = "6-8", RestTime = "2 min" },
                    new PlannedExercise { Name = "Leg press", Sets = 3, Reps = "10-12", RestTime = "90s" },
                    new PlannedExercise { Name = "Bulgarian split squat", Sets = 3, Reps = "8-10", RestTime = "60s" }
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Mrtvi dvig",
                Exercises = new List<PlannedExercise>
                {
                    new PlannedExercise { Name = "Mrtvi dvig", Sets = 5, Reps = "5", RestTime = "3-5 min", Notes = "85-90% 1RM" },
                    new PlannedExercise { Name = "Rumunski mrtvi dvig", Sets = 3, Reps = "8", RestTime = "2 min" },
                    new PlannedExercise { Name = "Veslanje s palico", Sets = 4, Reps = "8-10", RestTime = "90s" },
                    new PlannedExercise { Name = "Pull-ups", Sets = 3, Reps = "AMRAP", RestTime = "2 min" }
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
                    new PlannedExercise { Name = "Ogrevanje", Sets = 1, Reps = "5 min", RestTime = "-" },
                    new PlannedExercise { Name = "Sprint intervali", Sets = 8, Reps = "30s sprint / 90s počitek", RestTime = "-" },
                    new PlannedExercise { Name = "Ohlajanje", Sets = 1, Reps = "5 min", RestTime = "-" }
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 3,
                DayName = "Sreda",
                Focus = "Circuit training",
                Exercises = new List<PlannedExercise>
                {
                    new PlannedExercise { Name = "Burpees", Sets = 3, Reps = "15", RestTime = "30s" },
                    new PlannedExercise { Name = "Kettlebell swing", Sets = 3, Reps = "20", RestTime = "30s" },
                    new PlannedExercise { Name = "Box jumps", Sets = 3, Reps = "12", RestTime = "30s" },
                    new PlannedExercise { Name = "Battle ropes", Sets = 3, Reps = "30s", RestTime = "30s" },
                    new PlannedExercise { Name = "Rowing machine", Sets = 3, Reps = "500m", RestTime = "2 min" }
                }
            });

            plan.WorkoutDays.Add(new WorkoutDay
            {
                DayNumber = 5,
                DayName = "Petek",
                Focus = "Steady state cardio",
                Exercises = new List<PlannedExercise>
                {
                    new PlannedExercise { Name = "Tek/kolesarjenje", Sets = 1, Reps = "30-45 min", RestTime = "-", Notes = "Vzdržljiv tempo" }
                }
            });
        }

        AddRestDays(plan);
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
