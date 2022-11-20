using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationFakeUserData.Shared.Models
{
    public class ConfigureGenerationRequest
    {
        public string Region { get; set; } = default!;
        public double CountError { get; set; } = default!;
        public int Seed { get; set; } = default!;
        public int Page { get; set; } = default!;
    }
}
