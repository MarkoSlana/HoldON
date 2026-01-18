namespace HoldON.Services;

public class ExerciseVideoService
{
    private readonly Dictionary<string, string> _exerciseVideos = new()
    {
        // HIIT & Cardio
        { "Burpees", "https://www.youtube.com/watch?v=auBLPXO8Fww" },
        { "Mountain climbers", "https://www.youtube.com/watch?v=nmwgirgXLYM" },
        { "Kettlebell swing", "https://www.youtube.com/watch?v=YSxHifyI6s8" },
        { "Box jumps", "https://www.youtube.com/watch?v=hxldG9FX4j4" },
        { "Battle ropes", "https://www.youtube.com/watch?v=5gMHcKImTLg" },

        // Prsi & Ramena
        { "Bench press", "https://www.youtube.com/watch?v=rT7DgCr-3pg" },
        { "Poševni bench press", "https://www.youtube.com/watch?v=8iPEnn-ltC8" },
        { "Poševni bench press z giricami", "https://www.youtube.com/watch?v=8iPEnn-ltC8" },
        { "Vojaški pritisk", "https://www.youtube.com/watch?v=2yjwXTZQDDI" },
        { "Dviganje sveč bočno", "https://www.youtube.com/watch?v=3VcKaXpzqRo" },
        { "Dips", "https://www.youtube.com/watch?v=2z8JmcrW-As" },

        // Triceps & Biceps
        { "Iztegi za triceps", "https://www.youtube.com/watch?v=d_KZxkY_0cM" },
        { "Triceps work", "https://www.youtube.com/watch?v=d_KZxkY_0cM" },
        { "Dvigovanje uteži za biceps", "https://www.youtube.com/watch?v=ykJmrZ5v0Oo" },
        { "Hammer curls", "https://www.youtube.com/watch?v=zC3nLlEvin4" },
        { "Dvigovanje uteži", "https://www.youtube.com/watch?v=ykJmrZ5v0Oo" },

        // Hrbet
        { "Mrtvi dvig", "https://www.youtube.com/watch?v=op9kVnSso6Q" },
        { "Rumunski mrtvi dvig", "https://www.youtube.com/watch?v=2SHsk9AzdjA" },
        { "Veslanje s palico", "https://www.youtube.com/watch?v=9efgcAjQe7E" },
        { "Veslanje", "https://www.youtube.com/watch?v=9efgcAjQe7E" },
        { "Lat pulldown", "https://www.youtube.com/watch?v=CAwf7n6Luuc" },
        { "Pull-ups", "https://www.youtube.com/watch?v=eGo4IYlbE5g" },

        // Noge
        { "Počep s palico", "https://www.youtube.com/watch?v=ultWZbUMPL8" },
        { "Front squat", "https://www.youtube.com/watch?v=uYumuL_G_V0" },
        { "Leg press", "https://www.youtube.com/watch?v=IZxyjW7MPJQ" },
        { "Leg curl", "https://www.youtube.com/watch?v=ELOCsoDSmrg" },
        { "Dvigovanje na prste", "https://www.youtube.com/watch?v=gwLzBJYoWlI" },
        { "Bulgarian split squat", "https://www.youtube.com/watch?v=2C-uNgKwPLE" },

        // Core & Other
        { "Planking", "https://www.youtube.com/watch?v=ASdvN_XEl_c" },
        { "Potisk", "https://www.youtube.com/watch?v=IODxDxX7oi4" },

        // Cardio Equipment
        { "Tek/kolesarjenje", "https://www.youtube.com/watch?v=brFHyOtTwH4" },
        { "Kardio (tek/kolesarjenje)", "https://www.youtube.com/watch?v=brFHyOtTwH4" },
        { "Rowing machine", "https://www.youtube.com/watch?v=zQ82RYIFLN8" },
        { "Skakanje z vrvico", "https://www.youtube.com/watch?v=FJmRQ5iTXKE" },
        { "Sprint intervali", "https://www.youtube.com/watch?v=b5VOBF6so1I" },

        // General
        { "Ogrevanje", "https://www.youtube.com/watch?v=8lDC4Ri9zAQ" },
        { "Ohlajanje", "https://www.youtube.com/watch?v=9TaWJLD2Z1Q" }
    };

    public string GetVideoUrl(string exerciseName)
    {
        // Try exact match first
        if (_exerciseVideos.TryGetValue(exerciseName, out var url))
        {
            return url;
        }

        // Try partial match
        var matchingKey = _exerciseVideos.Keys.FirstOrDefault(key =>
            exerciseName.Contains(key, StringComparison.OrdinalIgnoreCase) ||
            key.Contains(exerciseName, StringComparison.OrdinalIgnoreCase));

        return matchingKey != null ? _exerciseVideos[matchingKey] : string.Empty;
    }
}
