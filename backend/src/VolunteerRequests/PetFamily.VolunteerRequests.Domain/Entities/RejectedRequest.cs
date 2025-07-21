using CSharpFunctionalExtensions;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Domain.Entities
{
    public class RejectedRequest : Entity<RequestId>
    {
        //ef core
        private RejectedRequest()
        {
        }

        public VolunteerRequest? VolunteerRequest { get; set; }

        public RejectionDate RejectionDate { get; set; }

        public RejectedRequest(
            RequestId id,
            RejectionDate rejectionDate) : base(id)
        {
            RejectionDate = rejectionDate;
        } 
    }
}
