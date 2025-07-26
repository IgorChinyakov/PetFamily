using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Contracts.DTOs
{
    public enum RequestStatusDto
    {
        Submitted,
        Rejected,
        RevisionRequired,
        Approved,
        OnReview
    }
}
