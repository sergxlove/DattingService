using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;

namespace DattingService.Core.Models
{
    public class Interests
    {
        public Guid Id { get; private set; }

        public JArray SelectInterests { get; private set; } = new JArray();
        public enum Interest
        {
            [Description("Sport")]
            Sport = 1,
            [Description("Travel")]
            Travel = 2
        }

        public Interests(Guid id, JArray interests)
        {
            Id = id;
            SelectInterests = interests;
        }

        public bool UpdateInterest(List<Interest> interests)
        {
            SelectInterests.Clear();
            foreach (Interest item in interests)
            {
                SelectInterests.Add(item);
            }
            return true;
        }

        public static string GetSkinString<T>(T value)
        {
            FieldInfo field = value!.GetType().GetField(value.ToString()!)!;
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>()!;
            return attribute?.Description ?? value.ToString()!;
        }

        public static List<string> GetAll()
        {
            return Enum.GetValues(typeof(Interest))
                .Cast<Interest>()
                .Select(x => GetSkinString(x))
                .ToList();
        }

    }
}
