using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Options
{
    public class CleanUpSettings()
    {
        public const string CLEAN_UP_SETTINGS = "CleanUpSettings";

        public int DaysBeforeDeletion { get; set; }
    }
}
