
namespace GenerationFakeUserData.Shared.Models
{
    public class UserInfo
    {
        private static JsonParseUser _jsonParseUser = default!;
        private static Random _rnd = default!;

        public int Number { get; set; } = default!;
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public static UserInfo GenerateUser(int seed, int number, double countError, JsonParseUser jsonParseUser)
        {
            _rnd = new Random(seed);
            _jsonParseUser = jsonParseUser;

            var user = new UserInfo()
            {
                Id = seed,
                Number = number,
                Name = GenerateName(),
                Address = GenerateAddress(),
                Phone = GeneratePhone()
            };

            return GenerateErrors(user, countError);
        }

        private static string GenerateName()
        {
            var gender = _rnd.Next(2) == 1 ? _jsonParseUser!.Male : _jsonParseUser!.Female;
            return string.Join(" ", gender.FirstName[_rnd.Next(gender.FirstName.Length)],
                                   gender.MiddleName?[_rnd.Next(gender.MiddleName.Length)],
                                   gender.LastName[_rnd.Next(gender.LastName.Length)]);
        }

        private static string GenerateAddress()
        {
            var villangeOrCity = _jsonParseUser.Village != null && _rnd.Next(5) == 4
               ? string.Format("{0}, _, {1}", _jsonParseUser.Village[_rnd.Next(_jsonParseUser.Village.Count)], _rnd.Next(150))
               : string.Format("{0}, _, {1}, {2}", _jsonParseUser.City[_rnd.Next(_jsonParseUser.City.Count)], _rnd.Next(300), _rnd.Next(1000));
            villangeOrCity = villangeOrCity.Replace("_", _jsonParseUser.Street[_rnd.Next(_jsonParseUser.Street.Count)]);

            return string.Join(", ", _jsonParseUser.Region[_rnd.Next(_jsonParseUser.Region.Count)], villangeOrCity);
        }

        private static string GeneratePhone()
        {
            return string.Format("{0}{1}{2}", _jsonParseUser.Phone.CodeCountry, _jsonParseUser.Phone.RegionCode[_rnd.Next(_jsonParseUser.Phone.RegionCode.Count)], _rnd.Next(9999999));
        }


        private static UserInfo GenerateErrors(UserInfo userInfo, double countError)
        {

            int lenghtPhone = userInfo.Phone.Length;
            int lenghtName = userInfo.Name.Length;
            int lenghtAddress = userInfo.Address.Length;

            int fractionalPart = (int)((countError - Math.Truncate(countError)) * 100);
            if (_rnd.Next(fractionalPart) > fractionalPart / 2)
                countError++;

            for (int i = 0; i < countError; i++)
            {
                switch (_rnd.Next(0, 3))
                {
                    case 0:
                        userInfo.Phone = ErrorHandler(userInfo.Phone, lenghtPhone);
                        break;
                    case 1:
                        userInfo.Name = ErrorHandler(userInfo.Name, lenghtName);
                        break;
                    case 2:
                        userInfo.Address = ErrorHandler(userInfo.Address, lenghtAddress);
                        break;
                }
            }

            return userInfo;
        }

        private static string ErrorHandler(string value, int defaultCount)
        {
            if ((value.Length * 100) / defaultCount > 103)
                return RemoveSymbolInElement(value);
            else if ((value.Length * 100) / defaultCount < 97)
                return InsertSymbolInElement(value);
            else
            {
                return _rnd.Next(0,3) switch
                {
                    0 => RemoveSymbolInElement(value),
                    1 => InsertSymbolInElement(value),
                    2 => SwapSymbolsInElement(value),
                    _ => value
                };
            }
        }

        private static string RemoveSymbolInElement(string value) => value.Remove(_rnd.Next(value.Length), 1);
        private static string InsertSymbolInElement(string value) => value.Insert(_rnd.Next(value.Length), _jsonParseUser.Letter[_rnd.Next(_jsonParseUser.Letter.Length)].ToString());
        private static string SwapSymbolsInElement(string value) => value[0] + value.Substring(1, value.Length - 2) + value[^1];
    }
}
