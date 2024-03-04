using Newtonsoft.Json;

namespace ProductAPI.Helpers
{
    public class CommonHelper
    {
        public static T GetMockModel<T>()
        {
            StreamReader reader = new StreamReader($"{AppContext.BaseDirectory}/MockData/products.json");
            var jsonFileData = reader.ReadToEnd();

            T desObj = JsonConvert.DeserializeObject<T>(jsonFileData);
            return desObj;
        }
    }
}
