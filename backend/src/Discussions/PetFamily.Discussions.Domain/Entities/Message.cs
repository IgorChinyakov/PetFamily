﻿using PetFamily.Discussions.Domain.ValueObjects.Message;
using PetFamily.Discussions.Domain.ValueObjects.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Discussions.Domain.Entities
{
    public class Message
    {
        //ef core
        private Message()
        {
        }

        public MessageId Id {  get; set; }

        public UserId UserId {  get; set; }

        public Text Text { get; set; }

        public CreationDate CreationDate { get; set; }

        public IsEdited IsEdited { get; set; }

        public Message(MessageId messageId, 
            UserId userId, 
            Text text, 
            CreationDate creationDate, 
            IsEdited isEdited)
        {
            Id = messageId;
            UserId = userId;
            Text = text;
            CreationDate = creationDate;
            IsEdited = isEdited;
        }

        public void EditText(Text text) => Text = text;

        public void ChangeIsEdit(IsEdited isEdited) => IsEdited = isEdited;
    }
}
