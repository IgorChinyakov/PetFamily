using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Options
{
    public class VolunteerRequestSettings
    {
        public const string Path = "VolunteerRequestSettings";

        public int DaysAfterRejectionToMakeAnotherRequest { get; set; }
    }
}
