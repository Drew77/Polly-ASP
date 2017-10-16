using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polly.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Option { get; set; }

        public int Votes { get; set; }

        public int PollId { get; set; }
    }
}