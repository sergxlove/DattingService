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
        }

        public bool AddInerest(Interest interest)
        {
            if (SelectInterests.Count > 11) return false;
            SelectInterests.Add(interest);
            return true;
        }

        public bool RemoveInterest(Interest interest)
        {
            if(SelectInterests.Count > 11) return false;
            var result = SelectInterests.Remove(GetSkinString(interest));
            return result;
        }

        public static string GetSkinString<T>(T value)
        {
            FieldInfo field = value!.GetType().GetField(value.ToString()!)!;
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>()!;
            return attribute?.Description ?? value.ToString()!;
        }
    }
}
