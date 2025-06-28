using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.ValueObjects.Discussion
{
    public class DiscussionStatus
    {
        public Status Value { get; }

        //ef core
        private DiscussionStatus()
        {
        }

        private DiscussionStatus(Status status)
        {
            Value = status;
        }

        public static DiscussionStatus Create(Status status) => new DiscussionStatus(status);

        public enum Status
        {
            Open,
            Closed,
        }
    }
}
