using SQLite;

namespace HoldON.Models;

// ============================================================================
// USER MODELS
// ============================================================================

[Table("users")]
public class User
{
    [PrimaryKey, AutoIncrement, Column("user_id")]
    public int UserId { get; set; }

    [Column("username"), Unique, NotNull]
    public string Username { get; set; } = string.Empty;

    [Column("email"), Unique, NotNull]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash"), NotNull]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("last_login")]
    public DateTime? LastLogin { get; set; }

    [Column("preferred_language")]
    public string PreferredLanguage { get; set; } = "sl"; // "sl" or "en"

    [Column("profile_image_url")]
    public string? ProfileImageUrl { get; set; }
}

[Table("user_profiles")]
public class UserProfileDb
{
    [PrimaryKey, AutoIncrement, Column("profile_id")]
    public int ProfileId { get; set; }

    [Column("user_id"), Unique, NotNull]
    public int UserId { get; set; }

    [Column("age")]
    public int? Age { get; set; }

    [Column("gender")]
    public string? Gender { get; set; } // "M", "F", "Other"

    [Column("weight_kg")]
    public double? WeightKg { get; set; }

    [Column("height_cm")]
    public double? HeightCm { get; set; }

    [Column("fitness_goal")]
    public string? FitnessGoal { get; set; } // "muscle_gain", "weight_loss", etc.

    [Column("experience_level")]
    public string? ExperienceLevel { get; set; } // "beginner", "intermediate", "advanced"

    [Column("available_equipment")]
    public string? AvailableEquipment { get; set; } // JSON array

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

// ============================================================================
// EXERCISE MODELS
// ============================================================================

[Table("exercises")]
public class ExerciseDb
{
    [PrimaryKey, AutoIncrement, Column("exercise_id")]
    public int ExerciseId { get; set; }

    [Column("name_sl"), NotNull]
    public string NameSl { get; set; } = string.Empty;

    [Column("name_en"), NotNull]
    public string NameEn { get; set; } = string.Empty;

    [Column("description_sl")]
    public string? DescriptionSl { get; set; }

    [Column("description_en")]
    public string? DescriptionEn { get; set; }

    [Column("category")]
    public string? Category { get; set; } // "chest", "back", "legs", etc.

    [Column("difficulty")]
    public string? Difficulty { get; set; } // "beginner", "intermediate", "advanced"

    [Column("equipment_required")]
    public string? EquipmentRequired { get; set; } // JSON array

    [Column("primary_muscles")]
    public string? PrimaryMuscles { get; set; } // JSON array

    [Column("secondary_muscles")]
    public string? SecondaryMuscles { get; set; } // JSON array

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("exercise_media")]
public class ExerciseMedia
{
    [PrimaryKey, AutoIncrement, Column("media_id")]
    public int MediaId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("media_type")]
    public string MediaType { get; set; } = "image"; // "image" or "video"

    [Column("media_url"), NotNull]
    public string MediaUrl { get; set; } = string.Empty;

    [Column("media_order")]
    public int MediaOrder { get; set; }
}

[Table("exercise_instructions")]
public class ExerciseInstruction
{
    [PrimaryKey, AutoIncrement, Column("instruction_id")]
    public int InstructionId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("language")]
    public string Language { get; set; } = "sl"; // "sl" or "en"

    [Column("step_number"), NotNull]
    public int StepNumber { get; set; }

    [Column("instruction_text"), NotNull]
    public string InstructionText { get; set; } = string.Empty;
}

// ============================================================================
// WORKOUT PROGRAM MODELS
// ============================================================================

[Table("workout_programs")]
public class WorkoutProgramDb
{
    [PrimaryKey, AutoIncrement, Column("program_id")]
    public int ProgramId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("program_name_sl")]
    public string? ProgramNameSl { get; set; }

    [Column("program_name_en")]
    public string? ProgramNameEn { get; set; }

    [Column("description_sl")]
    public string? DescriptionSl { get; set; }

    [Column("description_en")]
    public string? DescriptionEn { get; set; }

    [Column("goal")]
    public string? Goal { get; set; } // "muscle_gain", "weight_loss", etc.

    [Column("duration_weeks")]
    public int? DurationWeeks { get; set; }

    [Column("workouts_per_week")]
    public int? WorkoutsPerWeek { get; set; }

    [Column("is_template")]
    public bool IsTemplate { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("program_workouts")]
public class ProgramWorkout
{
    [PrimaryKey, AutoIncrement, Column("program_workout_id")]
    public int ProgramWorkoutId { get; set; }

    [Column("program_id"), NotNull]
    public int ProgramId { get; set; }

    [Column("day_of_week")]
    public int? DayOfWeek { get; set; } // 1-7

    [Column("week_number")]
    public int? WeekNumber { get; set; }

    [Column("workout_name_sl")]
    public string? WorkoutNameSl { get; set; }

    [Column("workout_name_en")]
    public string? WorkoutNameEn { get; set; }

    [Column("workout_order")]
    public int? WorkoutOrder { get; set; }
}

[Table("program_exercises")]
public class ProgramExercise
{
    [PrimaryKey, AutoIncrement, Column("program_exercise_id")]
    public int ProgramExerciseId { get; set; }

    [Column("program_workout_id"), NotNull]
    public int ProgramWorkoutId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("exercise_order")]
    public int? ExerciseOrder { get; set; }

    [Column("sets")]
    public int? Sets { get; set; }

    [Column("reps_min")]
    public int? RepsMin { get; set; }

    [Column("reps_max")]
    public int? RepsMax { get; set; }

    [Column("rest_seconds")]
    public int? RestSeconds { get; set; }

    [Column("notes_sl")]
    public string? NotesSl { get; set; }

    [Column("notes_en")]
    public string? NotesEn { get; set; }
}

// ============================================================================
// WORKOUT SESSION MODELS
// ============================================================================

[Table("workout_sessions")]
public class WorkoutSession
{
    [PrimaryKey, AutoIncrement, Column("session_id")]
    public int SessionId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("program_workout_id")]
    public int? ProgramWorkoutId { get; set; }

    [Column("session_date"), NotNull]
    public DateTime SessionDate { get; set; }

    [Column("start_time")]
    public DateTime? StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("duration_minutes")]
    public int? DurationMinutes { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("feeling_rating")]
    public int? FeelingRating { get; set; } // 1-5

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("workout_sets")]
public class WorkoutSetDb
{
    [PrimaryKey, AutoIncrement, Column("set_id")]
    public int SetId { get; set; }

    [Column("session_id"), NotNull]
    public int SessionId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("set_number"), NotNull]
    public int SetNumber { get; set; }

    [Column("reps")]
    public int? Reps { get; set; }

    [Column("weight_kg")]
    public double? WeightKg { get; set; }

    [Column("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [Column("distance_meters")]
    public double? DistanceMeters { get; set; }

    [Column("is_warmup")]
    public bool IsWarmup { get; set; } = false;

    [Column("is_dropset")]
    public bool IsDropset { get; set; } = false;

    [Column("is_failure")]
    public bool IsFailure { get; set; } = false;

    [Column("rest_seconds")]
    public int? RestSeconds { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ============================================================================
// PERSONAL RECORDS
// ============================================================================

[Table("personal_records")]
public class PersonalRecord
{
    [PrimaryKey, AutoIncrement, Column("record_id")]
    public int RecordId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("record_type")]
    public string RecordType { get; set; } = "1RM"; // "1RM", "max_reps", "max_volume", etc.

    [Column("value"), NotNull]
    public double Value { get; set; }

    [Column("unit")]
    public string Unit { get; set; } = "kg"; // "kg", "reps", "meters", "seconds"

    [Column("achieved_date"), NotNull]
    public DateTime AchievedDate { get; set; }

    [Column("session_id")]
    public int? SessionId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }
}

[Table("strength_standards")]
public class StrengthStandard
{
    [PrimaryKey, AutoIncrement, Column("standard_id")]
    public int StandardId { get; set; }

    [Column("exercise_id"), NotNull]
    public int ExerciseId { get; set; }

    [Column("gender")]
    public string? Gender { get; set; } // "M" or "F"

    [Column("bodyweight_kg_min")]
    public double? BodyweightKgMin { get; set; }

    [Column("bodyweight_kg_max")]
    public double? BodyweightKgMax { get; set; }

    [Column("beginner_kg")]
    public double? BeginnerKg { get; set; }

    [Column("novice_kg")]
    public double? NoviceKg { get; set; }

    [Column("intermediate_kg")]
    public double? IntermediateKg { get; set; }

    [Column("advanced_kg")]
    public double? AdvancedKg { get; set; }

    [Column("elite_kg")]
    public double? EliteKg { get; set; }
}

// ============================================================================
// BADGES & ACHIEVEMENTS
// ============================================================================

[Table("badges")]
public class Badge
{
    [PrimaryKey, AutoIncrement, Column("badge_id")]
    public int BadgeId { get; set; }

    [Column("badge_name_sl"), NotNull]
    public string BadgeNameSl { get; set; } = string.Empty;

    [Column("badge_name_en"), NotNull]
    public string BadgeNameEn { get; set; } = string.Empty;

    [Column("description_sl")]
    public string? DescriptionSl { get; set; }

    [Column("description_en")]
    public string? DescriptionEn { get; set; }

    [Column("badge_type")]
    public string? BadgeType { get; set; } // "record", "consistency", "milestone", etc.

    [Column("icon_url")]
    public string? IconUrl { get; set; }

    [Column("criteria_json")]
    public string? CriteriaJson { get; set; } // JSON object

    [Column("rarity")]
    public string? Rarity { get; set; } // "common", "rare", "epic", "legendary"
}

[Table("user_badges")]
public class UserBadge
{
    [PrimaryKey, AutoIncrement, Column("user_badge_id")]
    public int UserBadgeId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("badge_id"), NotNull]
    public int BadgeId { get; set; }

    [Column("earned_date")]
    public DateTime EarnedDate { get; set; } = DateTime.Now;

    [Column("progress_value")]
    public double? ProgressValue { get; set; }

    [Column("is_unlocked")]
    public bool IsUnlocked { get; set; } = false;
}

// ============================================================================
// CHALLENGES
// ============================================================================

[Table("challenges")]
public class Challenge
{
    [PrimaryKey, AutoIncrement, Column("challenge_id")]
    public int ChallengeId { get; set; }

    [Column("challenge_name_sl"), NotNull]
    public string ChallengeNameSl { get; set; } = string.Empty;

    [Column("challenge_name_en"), NotNull]
    public string ChallengeNameEn { get; set; } = string.Empty;

    [Column("description_sl")]
    public string? DescriptionSl { get; set; }

    [Column("description_en")]
    public string? DescriptionEn { get; set; }

    [Column("challenge_type")]
    public string? ChallengeType { get; set; } // "total_weight", "total_reps", etc.

    [Column("target_value")]
    public double? TargetValue { get; set; }

    [Column("target_unit")]
    public string? TargetUnit { get; set; }

    [Column("icon_url")]
    public string? IconUrl { get; set; }

    [Column("is_global")]
    public bool IsGlobal { get; set; } = false;
}

[Table("user_challenges")]
public class UserChallenge
{
    [PrimaryKey, AutoIncrement, Column("user_challenge_id")]
    public int UserChallengeId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("challenge_id"), NotNull]
    public int ChallengeId { get; set; }

    [Column("start_date"), NotNull]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("current_progress")]
    public double CurrentProgress { get; set; } = 0;

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = false;

    [Column("completed_date")]
    public DateTime? CompletedDate { get; set; }
}

// ============================================================================
// SOCIAL FEATURES
// ============================================================================

[Table("friendships")]
public class Friendship
{
    [PrimaryKey, AutoIncrement, Column("friendship_id")]
    public int FriendshipId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("friend_id"), NotNull]
    public int FriendId { get; set; }

    [Column("status")]
    public string Status { get; set; } = "pending"; // "pending", "accepted", "blocked"

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("accepted_at")]
    public DateTime? AcceptedAt { get; set; }
}

[Table("leaderboards")]
public class Leaderboard
{
    [PrimaryKey, AutoIncrement, Column("leaderboard_id")]
    public int LeaderboardId { get; set; }

    [Column("leaderboard_name_sl"), NotNull]
    public string LeaderboardNameSl { get; set; } = string.Empty;

    [Column("leaderboard_name_en"), NotNull]
    public string LeaderboardNameEn { get; set; } = string.Empty;

    [Column("category")]
    public string? Category { get; set; } // "total_volume", "consistency", etc.

    [Column("time_period")]
    public string? TimePeriod { get; set; } // "daily", "weekly", "monthly", "all_time"

    [Column("exercise_id")]
    public int? ExerciseId { get; set; }
}

[Table("leaderboard_entries")]
public class LeaderboardEntry
{
    [PrimaryKey, AutoIncrement, Column("entry_id")]
    public int EntryId { get; set; }

    [Column("leaderboard_id"), NotNull]
    public int LeaderboardId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("score"), NotNull]
    public double Score { get; set; }

    [Column("rank")]
    public int? Rank { get; set; }

    [Column("period_start")]
    public DateTime? PeriodStart { get; set; }

    [Column("period_end")]
    public DateTime? PeriodEnd { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

[Table("activity_feed")]
public class ActivityFeed
{
    [PrimaryKey, AutoIncrement, Column("activity_id")]
    public int ActivityId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("activity_type")]
    public string ActivityType { get; set; } = "workout"; // "workout", "record", "badge", etc.

    [Column("activity_data")]
    public string? ActivityData { get; set; } // JSON object

    [Column("is_public")]
    public bool IsPublic { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("activity_likes")]
public class ActivityLike
{
    [PrimaryKey, AutoIncrement, Column("like_id")]
    public int LikeId { get; set; }

    [Column("activity_id"), NotNull]
    public int ActivityId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("activity_comments")]
public class ActivityComment
{
    [PrimaryKey, AutoIncrement, Column("comment_id")]
    public int CommentId { get; set; }

    [Column("activity_id"), NotNull]
    public int ActivityId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("comment_text"), NotNull]
    public string CommentText { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ============================================================================
// REST & RECOVERY
// ============================================================================

[Table("rest_timers")]
public class RestTimer
{
    [PrimaryKey, AutoIncrement, Column("timer_id")]
    public int TimerId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("session_id")]
    public int? SessionId { get; set; }

    [Column("exercise_id")]
    public int? ExerciseId { get; set; }

    [Column("rest_duration_seconds"), NotNull]
    public int RestDurationSeconds { get; set; }

    [Column("started_at")]
    public DateTime StartedAt { get; set; } = DateTime.Now;

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }
}

[Table("sleep_logs")]
public class SleepLog
{
    [PrimaryKey, AutoIncrement, Column("sleep_id")]
    public int SleepId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("sleep_date"), NotNull]
    public DateTime SleepDate { get; set; }

    [Column("sleep_duration_hours")]
    public double? SleepDurationHours { get; set; }

    [Column("sleep_quality")]
    public int? SleepQuality { get; set; } // 1-5

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("recovery_logs")]
public class RecoveryLog
{
    [PrimaryKey, AutoIncrement, Column("recovery_id")]
    public int RecoveryId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("log_date"), NotNull]
    public DateTime LogDate { get; set; }

    [Column("muscle_soreness_level")]
    public int? MuscleSorenessLevel { get; set; } // 1-5

    [Column("energy_level")]
    public int? EnergyLevel { get; set; } // 1-5

    [Column("stress_level")]
    public int? StressLevel { get; set; } // 1-5

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ============================================================================
// NUTRITION
// ============================================================================

[Table("food_database")]
public class FoodDatabase
{
    [PrimaryKey, AutoIncrement, Column("food_id")]
    public int FoodId { get; set; }

    [Column("food_name_sl"), NotNull]
    public string FoodNameSl { get; set; } = string.Empty;

    [Column("food_name_en"), NotNull]
    public string FoodNameEn { get; set; } = string.Empty;

    [Column("serving_size_g")]
    public double? ServingSizeG { get; set; }

    [Column("calories")]
    public double? Calories { get; set; }

    [Column("protein_g")]
    public double? ProteinG { get; set; }

    [Column("carbs_g")]
    public double? CarbsG { get; set; }

    [Column("fat_g")]
    public double? FatG { get; set; }

    [Column("fiber_g")]
    public double? FiberG { get; set; }

    [Column("sugar_g")]
    public double? SugarG { get; set; }

    [Column("is_custom")]
    public bool IsCustom { get; set; } = false;

    [Column("user_id")]
    public int? UserId { get; set; }
}

[Table("nutrition_logs")]
public class NutritionLog
{
    [PrimaryKey, AutoIncrement, Column("nutrition_id")]
    public int NutritionId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("log_date"), NotNull]
    public DateTime LogDate { get; set; }

    [Column("meal_type")]
    public string? MealType { get; set; } // "breakfast", "lunch", "dinner", "snack"

    [Column("food_id"), NotNull]
    public int FoodId { get; set; }

    [Column("servings"), NotNull]
    public double Servings { get; set; } = 1;

    [Column("total_calories")]
    public double? TotalCalories { get; set; }

    [Column("total_protein_g")]
    public double? TotalProteinG { get; set; }

    [Column("total_carbs_g")]
    public double? TotalCarbsG { get; set; }

    [Column("total_fat_g")]
    public double? TotalFatG { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("daily_nutrition_goals")]
public class DailyNutritionGoal
{
    [PrimaryKey, AutoIncrement, Column("goal_id")]
    public int GoalId { get; set; }

    [Column("user_id"), Unique, NotNull]
    public int UserId { get; set; }

    [Column("daily_calories")]
    public int? DailyCalories { get; set; }

    [Column("daily_protein_g")]
    public double? DailyProteinG { get; set; }

    [Column("daily_carbs_g")]
    public double? DailyCarbsG { get; set; }

    [Column("daily_fat_g")]
    public double? DailyFatG { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

// ============================================================================
// GOALS & REMINDERS
// ============================================================================

[Table("user_goals")]
public class UserGoal
{
    [PrimaryKey, AutoIncrement, Column("goal_id")]
    public int GoalId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("goal_type")]
    public string GoalType { get; set; } = "custom"; // "weight", "strength", "workout_frequency", etc.

    [Column("goal_name_sl"), NotNull]
    public string GoalNameSl { get; set; } = string.Empty;

    [Column("goal_name_en"), NotNull]
    public string GoalNameEn { get; set; } = string.Empty;

    [Column("target_value")]
    public double? TargetValue { get; set; }

    [Column("current_value")]
    public double CurrentValue { get; set; } = 0;

    [Column("target_date")]
    public DateTime? TargetDate { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; } = false;

    [Column("completed_date")]
    public DateTime? CompletedDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("reminders")]
public class Reminder
{
    [PrimaryKey, AutoIncrement, Column("reminder_id")]
    public int ReminderId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("reminder_type")]
    public string ReminderType { get; set; } = "custom"; // "workout", "nutrition", "water", etc.

    [Column("title_sl"), NotNull]
    public string TitleSl { get; set; } = string.Empty;

    [Column("title_en"), NotNull]
    public string TitleEn { get; set; } = string.Empty;

    [Column("message_sl")]
    public string? MessageSl { get; set; }

    [Column("message_en")]
    public string? MessageEn { get; set; }

    [Column("reminder_time")]
    public TimeSpan? ReminderTime { get; set; }

    [Column("reminder_days")]
    public string? ReminderDays { get; set; } // JSON array: [1,2,3,4,5]

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

[Table("motivational_messages")]
public class MotivationalMessage
{
    [PrimaryKey, AutoIncrement, Column("message_id")]
    public int MessageId { get; set; }

    [Column("message_sl"), NotNull]
    public string MessageSl { get; set; } = string.Empty;

    [Column("message_en"), NotNull]
    public string MessageEn { get; set; } = string.Empty;

    [Column("category")]
    public string? Category { get; set; } // "workout", "nutrition", "recovery", etc.

    [Column("trigger_condition")]
    public string? TriggerCondition { get; set; } // JSON
}

// ============================================================================
// BODY MEASUREMENTS
// ============================================================================

[Table("body_measurements")]
public class BodyMeasurement
{
    [PrimaryKey, AutoIncrement, Column("measurement_id")]
    public int MeasurementId { get; set; }

    [Column("user_id"), NotNull]
    public int UserId { get; set; }

    [Column("measurement_date"), NotNull]
    public DateTime MeasurementDate { get; set; }

    [Column("weight_kg")]
    public double? WeightKg { get; set; }

    [Column("body_fat_percentage")]
    public double? BodyFatPercentage { get; set; }

    [Column("chest_cm")]
    public double? ChestCm { get; set; }

    [Column("waist_cm")]
    public double? WaistCm { get; set; }

    [Column("hips_cm")]
    public double? HipsCm { get; set; }

    [Column("thigh_cm")]
    public double? ThighCm { get; set; }

    [Column("bicep_cm")]
    public double? BicepCm { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
