using CsvHelper;
using GenerationFakeUserData.Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace GenerationFakeUserData.Server.Services
{
    public class GenerationService
    {
        public List<UserInfo> ReceiveFakeUserData(ConfigureGenerationRequest configure)
        {

            List<UserInfo> users = new List<UserInfo>();
            var jsonParseUser = JsonConvert.DeserializeObject<JsonParseUser>(File.ReadAllText("wwwrowot/" + configure.Region + ".json"))!;

            for (int i = 0; i < 10; i++)
            {
                users.Add(UserInfo.GenerateUser(((configure.Seed + configure.Page) * 10) + i + 1, (configure.Page * 10) + i + 1, configure.CountError, jsonParseUser));
            }

            return users;
        }

        public byte[] SaveGenerationUserData(List<UserInfo> users)
        {

            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(users);
                }
                var stringFile = writer.ToString();
                return Encoding.UTF8.GetBytes(stringFile);
            }
        }
    }
}
