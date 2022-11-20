using GenerationFakeUserData.Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace GenerationFakeUserData.Server.Services
{
    public class GenerationService
    {
        public List<UserInfo> ReceiveFakeUserData(ConfigureGenerationRequest configure)
        {

            List<UserInfo> users = new List<UserInfo>();
            var jsonParseUser = JsonConvert.DeserializeObject<JsonParseUser>(File.ReadAllText("wwwroot/" + configure.Region + ".json"))!;

            for (int i = 0; i < 10; i++)
            {
                users.Add(UserInfo.GenerateUser(((configure.Seed + configure.Page) * 10) + i + 1, (configure.Page * 10) + i + 1, configure.CountError, jsonParseUser));
            }

            return users;
        }
    }
}
