using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;

namespace DattingService.Core.Models
{
    public class Interests
    {
        public Guid Id { get; private set; }

        public int[] SelectInterests { get; private set; }
        public enum Interest
        {
            [Description("Computer games")]
            ComputerGames = 1,
            [Description("Fashion")]
            Fashion = 2,
            [Description("Cars")]
            Cars = 3,
            [Description("IT and technology")]
            ITAndTechnology = 4,
            [Description("Psychology")]
            Psychology = 5,
            [Description("Astrology")]
            Astrology = 6,
            [Description("Meditation")]
            Meditation = 7,
            [Description("Comics")]
            Comics = 8,
            [Description("Manga")]
            Manga = 9,
            [Description("Fandoms")]
            Fandoms = 10,
            [Description("Collecting")]
            Collecting = 11,
            [Description("Language learning")]
            LanguageLearning = 12,
            [Description("Streaming")]
            Streaming = 13,
            [Description("Games on a stick")]
            GamesOnAStick = 14,


            [Description("Cinemas")]
            Cinemas = 15,
            [Description("Concerts and shows")]
            ConcertsAndShows = 16,
            [Description("Museums and galleries")]
            MuseumsAndGalleries = 17,
            [Description("Theaters")]
            Theaters = 18,
            [Description("Outdoor activities")]
            OutdoorActivities = 19,
            [Description("Festivals")]
            Festivals = 20,
            [Description("Parties and clubs")]
            PartiesAndClubs = 21,
            [Description("Restaurants")]
            Restaurants = 22,
            [Description("Traveling")]
            Traveling = 23,
            [Description("Shopping")]
            Shopping = 24,
            [Description("MeetingFriends")]
            MeetingFriends = 25,
            [Description("Art")]
            Art = 26,
            [Description("Active leisure")]
            ActiveLeisure = 27,
            [Description("Master classes")]
            MasterClasses = 28,
            [Description("Karaoke")]
            Karaoke = 29,
            [Description("Quizzes")]
            Quizzes = 30,

            [Description("Running")]
            Running = 31,
            [Description("Fitness")]
            Fitness = 32,
            [Description("Cycling")]
            Cycling = 33,
            [Description("Hiking")]
            Hiking = 34,
            [Description("Yoga")]
            Yoga = 35,
            [Description("Pilates")]
            Pilates = 36,
            [Description("Snowboarding")]
            Snowboarding = 37,
            [Description("Rollerblading")]
            Rollerblading = 38,
            [Description("Skateboarding")]
            Skateboarding = 39,
            [Description("Skating")]
            Skating = 40,
            [Description("Walking")]
            Walking = 41,
            [Description("Alpinism")]
            Alpinism = 42,

            [Description("Pizza")] 
            Pizza = 43, 
            [Description("Sushi")] 
            Sushi = 44, 
            [Description("Burger")] 
            Burger = 45, 
            [Description("Healthy eating")] 
            HealthyEating = 46, 
            [Description("Veganism")]
            Veganism = 47, 
            [Description("Vegetarianism")] 
            Vegetarianism = 48,
            [Description("Coffee")] 
            Coffee = 49,
            [Description("Tea")] 
            Tea = 50, 
            [Description("Desserts")] 
            Desserts = 51, 
            [Description("Sweets")]
            Sweets = 52,
            [Description("Home cooking")]
            HomeCooking = 53, 
            [Description("Babies")] 
            Babies = 54, 
            [Description("Pasta")]
            Pasta = 55, 
            [Description("Shawarma")] 
            Shawarma = 56, 
            [Description("Spicy food")] 
            SpicyFood = 57,

            [Description("Football")] 
            Football = 58, 
            [Description("Swimming")]
            Swimming = 59, 
            [Description("Volleyball")]
            Volleyball = 60, 
            [Description("Basketball")] 
            Basketball = 61, 
            [Description("Hockey")]
            Hockey = 62, 
            [Description("Weightlifting")]
            Weightlifting = 63,
            [Description("Boxing")]
            Boxing = 64, 
            [Description("Cybersport")]
            Cybersport = 65, 
            [Description("Athletics")]
            Athletics = 66, 
            [Description("Tennis")] 
            Tennis = 67,

            [Description("Cooking")]
            Cooking = 68,
            [Description("Board games")]
            BoardGames = 69, 
            [Description("Movies and TV shows")] 
            MoviesAndTVShows = 70,
            [Description("Gardening")]
            Gardening = 71, 
            [Description("Books")]
            Books = 72, 
            [Description("DIY")] 
            DIY = 73, 
            [Description("Online learning")] 
            OnlineLearning = 74, 
            [Description("Watching shows")]
            WatchingShows = 75, 
            [Description("Podcasts")]
            Podcasts = 76,

            [Description("Countryside trips")] 
            CountrysideTrips = 77, 
            [Description("Excursions")] 
            Excursions = 78,
            [Description("Beach holidays")] 
            BeachHolidays = 79, 
            [Description("Fishing and hunting")]
            FishingAndHunting = 80, 
            [Description("Cruises")] 
            Cruises = 81,
            [Description("Mountains")]
            Mountains = 82,
            [Description("Events and concerts")] 
            EventsAndConcerts = 83, 
            [Description("Traveling abroad")] 
            TravelingAbroad = 84,
            [Description("Extreme")]
            Extreme = 85, 
            [Description("Traveling in Russia")] 
            TravelingInRussia = 86,

            [Description("Cats")] 
            Cats = 87, 
            [Description("Dogs")] 
            Dogs = 88, 
            [Description("Birds")] 
            Birds = 89,
            [Description("Fish")] 
            Fish = 90, 
            [Description("Rabbits")]
            Rabbits = 91, 
            [Description("Turtles")] 
            Turtles = 92, 
            [Description("Snakes")] 
            Snakes = 93, 
            [Description("Lizards")] 
            Lizards = 94, 
            [Description("Hamsters")] 
            Hamsters = 95,

            [Description("Comedies")] 
            Comedies = 96,
            [Description("Cartoons")]
            Cartoons = 97, 
            [Description("Historical films")] 
            HistoricalFilms = 98, 
            [Description("Detectives")]
            Detectives = 99, 
            [Description("Thrillers")]
            Thrillers = 100,
            [Description("Horror")] 
            Horror = 101, 
            [Description("Dramas")] 
            Dramas = 102, 
            [Description("Melodramas")]
            Melodramas = 103, 
            [Description("War films")] 
            WarFilms = 104, 
            [Description("Anime")] 
            Anime = 105, 
            [Description("Documentaries")]
            Documentaries = 106, 
            [Description("Game shows")] 
            GameShows = 107,
            [Description("Stand-up")] 
            StandUp = 108, 
            [Description("Fantasy")] 
            Fantasy = 109,

            [Description("Pop music")] 
            PopMusic = 110, 
            [Description("Hip-hop")]
            HipHop = 111, 
            [Description("Electronic music")] 
            ElectronicMusic = 112, 
            [Description("Rock")] 
            Rock = 113,
            [Description("Rap")] 
            Rap = 114, 
            [Description("Classical music")] 
            ClassicalMusic = 115, 
            [Description("Metal")]
            Metal = 116, 
            [Description("Techno")] 
            Techno = 117, 
            [Description("Blues")]
            Blues = 118, 
            [Description("Melancholic music")]
            MelancholicMusic = 119, 
            [Description("K-pop")] 
            KPop = 120
        }

        public Interests(Guid id, int[] interests)
        {
            Id = id;
            SelectInterests = interests;
        }

        public bool UpdateInterest(List<int> interests)
        {
            SelectInterests = interests.ToArray();
            return true;
        }

        public static string GetString<T>(T value)
        {
            FieldInfo field = value!.GetType().GetField(value.ToString()!)!;
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>()!;
            return attribute?.Description ?? value.ToString()!;
        }

        public static List<string> GetAll()
        {
            return Enum.GetValues(typeof(Interest))
                .Cast<Interest>()
                .Select(x => GetString(x))
                .ToList();
        }

    }
}
