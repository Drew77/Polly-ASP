using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polly.Models
{
    public class CreateVM
    {
        public string Title { get; set; }

        public string Question { get; set; }

        public String[] Answers { get; set; }
    }
}