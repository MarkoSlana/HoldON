using SQLite;
using HoldON.Models;

namespace HoldON.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        // Database path setup
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "holdon.db");
    }

    // Initialize database connection and create tables
    private async Task InitAsync()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);

        // Create all tables based on schema
        await _database.CreateTableAsync<User>();
        await _database.CreateTableAsync<UserProfileDb>();
        await _database.CreateTableAsync<ExerciseDb>();
        await _database.CreateTableAsync<ExerciseMedia>();
        await _database.CreateTableAsync<ExerciseInstruction>();
        await _database.CreateTableAsync<WorkoutProgramDb>();
        await _database.CreateTableAsync<ProgramWorkout>();
        await _database.CreateTableAsync<ProgramExercise>();
        await _database.CreateTableAsync<WorkoutSession>();
        await _database.CreateTableAsync<WorkoutSetDb>();
        await _database.CreateTableAsync<PersonalRecord>();
        await _database.CreateTableAsync<StrengthStandard>();
        await _database.CreateTableAsync<Badge>();
        await _database.CreateTableAsync<UserBadge>();
        await _database.CreateTableAsync<Challenge>();
        await _database.CreateTableAsync<UserChallenge>();
        await _database.CreateTableAsync<Friendship>();
        await _database.CreateTableAsync<Leaderboard>();
        await _database.CreateTableAsync<LeaderboardEntry>();
        await _database.CreateTableAsync<ActivityFeed>();
        await _database.CreateTableAsync<ActivityLike>();
        await _database.CreateTableAsync<ActivityComment>();
        await _database.CreateTableAsync<RestTimer>();
        await _database.CreateTableAsync<SleepLog>();
        await _database.CreateTableAsync<RecoveryLog>();
        await _database.CreateTableAsync<FoodDatabase>();
        await _database.CreateTableAsync<NutritionLog>();
        await _database.CreateTableAsync<DailyNutritionGoal>();
        await _database.CreateTableAsync<UserGoal>();
        await _database.CreateTableAsync<Reminder>();
        await _database.CreateTableAsync<MotivationalMessage>();
        await _database.CreateTableAsync<BodyMeasurement>();
    }

    // ============================================================================
    // USER OPERATIONS
    // ============================================================================

    public async Task<User?> GetUserAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<User>().Where(u => u.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        await InitAsync();
        return await _database!.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await InitAsync();
        if (user.UserId != 0)
            return await _database!.UpdateAsync(user);
        else
            return await _database!.InsertAsync(user);
    }

    public async Task<UserProfileDb?> GetUserProfileAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<UserProfileDb>().Where(p => p.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserProfileAsync(UserProfileDb profile)
    {
        await InitAsync();
        profile.UpdatedAt = DateTime.Now;

        var existing = await GetUserProfileAsync(profile.UserId);
        if (existing != null)
            return await _database!.UpdateAsync(profile);
        else
            return await _database!.InsertAsync(profile);
    }

    // ============================================================================
    // EXERCISE OPERATIONS
    // ============================================================================

    public async Task<List<ExerciseDb>> GetAllExercisesAsync()
    {
        await InitAsync();
        return await _database!.Table<ExerciseDb>().ToListAsync();
    }

    public async Task<ExerciseDb?> GetExerciseAsync(int exerciseId)
    {
        await InitAsync();
        return await _database!.Table<ExerciseDb>().Where(e => e.ExerciseId == exerciseId).FirstOrDefaultAsync();
    }

    public async Task<List<ExerciseDb>> GetExercisesByCategoryAsync(string category)
    {
        await InitAsync();
        return await _database!.Table<ExerciseDb>().Where(e => e.Category == category).ToListAsync();
    }

    public async Task<int> SaveExerciseAsync(ExerciseDb exercise)
    {
        await InitAsync();
        if (exercise.ExerciseId != 0)
            return await _database!.UpdateAsync(exercise);
        else
            return await _database!.InsertAsync(exercise);
    }

    public async Task<List<ExerciseMedia>> GetExerciseMediaAsync(int exerciseId)
    {
        await InitAsync();
        return await _database!.Table<ExerciseMedia>()
            .Where(m => m.ExerciseId == exerciseId)
            .OrderBy(m => m.MediaOrder)
            .ToListAsync();
    }

    public async Task<List<ExerciseInstruction>> GetExerciseInstructionsAsync(int exerciseId, string language)
    {
        await InitAsync();
        return await _database!.Table<ExerciseInstruction>()
            .Where(i => i.ExerciseId == exerciseId && i.Language == language)
            .OrderBy(i => i.StepNumber)
            .ToListAsync();
    }

    // ============================================================================
    // WORKOUT SESSION OPERATIONS
    // ============================================================================

    public async Task<List<WorkoutSession>> GetUserWorkoutSessionsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<WorkoutSession>()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync();
    }

    public async Task<WorkoutSession?> GetWorkoutSessionAsync(int sessionId)
    {
        await InitAsync();
        return await _database!.Table<WorkoutSession>().Where(s => s.SessionId == sessionId).FirstOrDefaultAsync();
    }

    public async Task<int> SaveWorkoutSessionAsync(WorkoutSession session)
    {
        await InitAsync();
        if (session.SessionId != 0)
            return await _database!.UpdateAsync(session);
        else
            return await _database!.InsertAsync(session);
    }

    public async Task<List<WorkoutSetDb>> GetSessionSetsAsync(int sessionId)
    {
        await InitAsync();
        return await _database!.Table<WorkoutSetDb>()
            .Where(s => s.SessionId == sessionId)
            .OrderBy(s => s.ExerciseId)
            .ThenBy(s => s.SetNumber)
            .ToListAsync();
    }

    public async Task<int> SaveWorkoutSetAsync(WorkoutSetDb set)
    {
        await InitAsync();
        if (set.SetId != 0)
            return await _database!.UpdateAsync(set);
        else
            return await _database!.InsertAsync(set);
    }

    // ============================================================================
    // PERSONAL RECORDS OPERATIONS
    // ============================================================================

    public async Task<List<PersonalRecord>> GetUserPersonalRecordsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<PersonalRecord>()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.AchievedDate)
            .ToListAsync();
    }

    public async Task<PersonalRecord?> GetExerciseRecordAsync(int userId, int exerciseId, string recordType)
    {
        await InitAsync();
        return await _database!.Table<PersonalRecord>()
            .Where(r => r.UserId == userId && r.ExerciseId == exerciseId && r.RecordType == recordType)
            .OrderByDescending(r => r.Value)
            .FirstOrDefaultAsync();
    }

    public async Task<int> SavePersonalRecordAsync(PersonalRecord record)
    {
        await InitAsync();
        return await _database!.InsertAsync(record);
    }

    public async Task<bool> UpdatePersonalRecordIfBetterAsync(int userId, int exerciseId, double weightKg, int sessionId)
    {
        await InitAsync();

        // Get current record for this exercise (1RM)
        var currentRecord = await GetExerciseRecordAsync(userId, exerciseId, "1RM");

        // If no record exists or new weight is higher, create/update record
        if (currentRecord == null || weightKg > currentRecord.Value)
        {
            var newRecord = new PersonalRecord
            {
                UserId = userId,
                ExerciseId = exerciseId,
                RecordType = "1RM",
                Value = weightKg,
                Unit = "kg",
                AchievedDate = DateTime.Now,
                SessionId = sessionId,
                Notes = "Avtomatsko zabeleženo"
            };

            await SavePersonalRecordAsync(newRecord);
            return true; // New record achieved
        }

        return false; // No new record
    }

    // ============================================================================
    // WORKOUT PROGRAM OPERATIONS
    // ============================================================================

    public async Task<List<WorkoutProgramDb>> GetUserWorkoutProgramsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<WorkoutProgramDb>()
            .Where(p => p.UserId == userId || p.IsTemplate)
            .ToListAsync();
    }

    public async Task<WorkoutProgramDb?> GetWorkoutProgramAsync(int programId)
    {
        await InitAsync();
        return await _database!.Table<WorkoutProgramDb>().Where(p => p.ProgramId == programId).FirstOrDefaultAsync();
    }

    public async Task<int> SaveWorkoutProgramAsync(WorkoutProgramDb program)
    {
        await InitAsync();
        if (program.ProgramId != 0)
            return await _database!.UpdateAsync(program);
        else
            return await _database!.InsertAsync(program);
    }

    public async Task<List<ProgramWorkout>> GetProgramWorkoutsAsync(int programId)
    {
        await InitAsync();
        return await _database!.Table<ProgramWorkout>()
            .Where(w => w.ProgramId == programId)
            .OrderBy(w => w.WeekNumber)
            .ThenBy(w => w.DayOfWeek)
            .ToListAsync();
    }

    public async Task<List<ProgramExercise>> GetWorkoutExercisesAsync(int programWorkoutId)
    {
        await InitAsync();
        return await _database!.Table<ProgramExercise>()
            .Where(e => e.ProgramWorkoutId == programWorkoutId)
            .OrderBy(e => e.ExerciseOrder)
            .ToListAsync();
    }

    // ============================================================================
    // BADGES & ACHIEVEMENTS OPERATIONS
    // ============================================================================

    public async Task<List<UserBadge>> GetUserBadgesAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<UserBadge>()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.EarnedDate)
            .ToListAsync();
    }

    public async Task<int> SaveUserBadgeAsync(UserBadge userBadge)
    {
        await InitAsync();
        return await _database!.InsertAsync(userBadge);
    }

    public async Task<List<Badge>> GetAllBadgesAsync()
    {
        await InitAsync();
        return await _database!.Table<Badge>().ToListAsync();
    }

    // ============================================================================
    // CHALLENGES OPERATIONS
    // ============================================================================

    public async Task<List<UserChallenge>> GetUserChallengesAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<UserChallenge>()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();
    }

    public async Task<int> SaveUserChallengeAsync(UserChallenge challenge)
    {
        await InitAsync();
        if (challenge.UserChallengeId != 0)
            return await _database!.UpdateAsync(challenge);
        else
            return await _database!.InsertAsync(challenge);
    }

    public async Task<List<Challenge>> GetAllChallengesAsync()
    {
        await InitAsync();
        return await _database!.Table<Challenge>().ToListAsync();
    }

    // ============================================================================
    // SOCIAL OPERATIONS
    // ============================================================================

    public async Task<List<Friendship>> GetUserFriendsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<Friendship>()
            .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == "accepted")
            .ToListAsync();
    }

    public async Task<int> SaveFriendshipAsync(Friendship friendship)
    {
        await InitAsync();
        if (friendship.FriendshipId != 0)
            return await _database!.UpdateAsync(friendship);
        else
            return await _database!.InsertAsync(friendship);
    }

    public async Task<List<ActivityFeed>> GetActivityFeedAsync(int userId, int limit = 50)
    {
        await InitAsync();
        return await _database!.Table<ActivityFeed>()
            .Where(a => a.UserId == userId && a.IsPublic)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> SaveActivityAsync(ActivityFeed activity)
    {
        await InitAsync();
        return await _database!.InsertAsync(activity);
    }

    // ============================================================================
    // NUTRITION OPERATIONS
    // ============================================================================

    public async Task<List<NutritionLog>> GetNutritionLogsAsync(int userId, DateTime date)
    {
        await InitAsync();
        return await _database!.Table<NutritionLog>()
            .Where(n => n.UserId == userId && n.LogDate == date.Date)
            .ToListAsync();
    }

    public async Task<int> SaveNutritionLogAsync(NutritionLog log)
    {
        await InitAsync();
        if (log.NutritionId != 0)
            return await _database!.UpdateAsync(log);
        else
            return await _database!.InsertAsync(log);
    }

    public async Task<DailyNutritionGoal?> GetUserNutritionGoalsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<DailyNutritionGoal>().Where(g => g.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<List<FoodDatabase>> SearchFoodAsync(string searchTerm)
    {
        await InitAsync();
        return await _database!.Table<FoodDatabase>()
            .Where(f => f.FoodNameEn.Contains(searchTerm) || f.FoodNameSl.Contains(searchTerm))
            .ToListAsync();
    }

    // ============================================================================
    // RECOVERY & WELLNESS OPERATIONS
    // ============================================================================

    public async Task<List<SleepLog>> GetSleepLogsAsync(int userId, int days = 30)
    {
        await InitAsync();
        var startDate = DateTime.Now.AddDays(-days).Date;
        return await _database!.Table<SleepLog>()
            .Where(s => s.UserId == userId && s.SleepDate >= startDate)
            .OrderByDescending(s => s.SleepDate)
            .ToListAsync();
    }

    public async Task<int> SaveSleepLogAsync(SleepLog log)
    {
        await InitAsync();
        if (log.SleepId != 0)
            return await _database!.UpdateAsync(log);
        else
            return await _database!.InsertAsync(log);
    }

    public async Task<List<RecoveryLog>> GetRecoveryLogsAsync(int userId, int days = 30)
    {
        await InitAsync();
        var startDate = DateTime.Now.AddDays(-days).Date;
        return await _database!.Table<RecoveryLog>()
            .Where(r => r.UserId == userId && r.LogDate >= startDate)
            .OrderByDescending(r => r.LogDate)
            .ToListAsync();
    }

    public async Task<int> SaveRecoveryLogAsync(RecoveryLog log)
    {
        await InitAsync();
        if (log.RecoveryId != 0)
            return await _database!.UpdateAsync(log);
        else
            return await _database!.InsertAsync(log);
    }

    // ============================================================================
    // GOALS & REMINDERS OPERATIONS
    // ============================================================================

    public async Task<List<UserGoal>> GetUserGoalsAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<UserGoal>()
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.IsCompleted)
            .ThenBy(g => g.TargetDate)
            .ToListAsync();
    }

    public async Task<int> SaveUserGoalAsync(UserGoal goal)
    {
        await InitAsync();
        if (goal.GoalId != 0)
            return await _database!.UpdateAsync(goal);
        else
            return await _database!.InsertAsync(goal);
    }

    public async Task<List<Reminder>> GetActiveRemindersAsync(int userId)
    {
        await InitAsync();
        return await _database!.Table<Reminder>()
            .Where(r => r.UserId == userId && r.IsActive)
            .ToListAsync();
    }

    public async Task<int> SaveReminderAsync(Reminder reminder)
    {
        await InitAsync();
        if (reminder.ReminderId != 0)
            return await _database!.UpdateAsync(reminder);
        else
            return await _database!.InsertAsync(reminder);
    }

    // ============================================================================
    // BODY MEASUREMENTS OPERATIONS
    // ============================================================================

    public async Task<List<BodyMeasurement>> GetBodyMeasurementsAsync(int userId, int days = 90)
    {
        await InitAsync();
        var startDate = DateTime.Now.AddDays(-days).Date;
        return await _database!.Table<BodyMeasurement>()
            .Where(m => m.UserId == userId && m.MeasurementDate >= startDate)
            .OrderByDescending(m => m.MeasurementDate)
            .ToListAsync();
    }

    public async Task<int> SaveBodyMeasurementAsync(BodyMeasurement measurement)
    {
        await InitAsync();
        if (measurement.MeasurementId != 0)
            return await _database!.UpdateAsync(measurement);
        else
            return await _database!.InsertAsync(measurement);
    }

    // ============================================================================
    // UTILITY OPERATIONS
    // ============================================================================

    public async Task<int> DeleteDatabaseAsync()
    {
        if (_database != null)
        {
            await _database.CloseAsync();
            _database = null;
        }

        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
            return 1;
        }
        return 0;
    }

    public async Task SeedInitialDataAsync()
    {
        await InitAsync();

        // Check if data already exists
        var exerciseCount = await _database!.Table<ExerciseDb>().CountAsync();
        if (exerciseCount > 0)
            return; // Data already seeded

        // Seed some basic exercises (example)
        var exercises = new List<ExerciseDb>
        {
            new ExerciseDb
            {
                NameSl = "Bench Press",
                NameEn = "Bench Press",
                Category = "chest",
                Difficulty = "intermediate",
                DescriptionSl = "Osnovni pritisk na klopi",
                DescriptionEn = "Basic bench press exercise"
            },
            new ExerciseDb
            {
                NameSl = "Počepi",
                NameEn = "Squats",
                Category = "legs",
                Difficulty = "intermediate",
                DescriptionSl = "Osnovni počepi s palico",
                DescriptionEn = "Basic barbell squats"
            },
            new ExerciseDb
            {
                NameSl = "Mrtvi dvig",
                NameEn = "Deadlift",
                Category = "back",
                Difficulty = "advanced",
                DescriptionSl = "Mrtvi dvig s palico",
                DescriptionEn = "Barbell deadlift"
            }
        };

        foreach (var exercise in exercises)
        {
            await _database.InsertAsync(exercise);
        }
    }
}
