using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Polly.Models
{
    public class Poll
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public List<Tag> UsersVoted { get; set; }

        public string Question { get; set; }
        [Display(Name = "Possible Answers")]
        public List<Answer> PollAnswers { get; set; }

        public Poll()
        {
            PollAnswers = new List<Answer>();
            UsersVoted = new List<Tag>();
        }

        public void Create(CreateVM ViewModel)
        {
            this.Title = ViewModel.Title;
            this.Question = ViewModel.Question;
            foreach (var Answer in ViewModel.Answers)
            {
                if (Answer == "")
                {
                    break;
                }
                
                var NewAnswer = new Answer();
                NewAnswer.Option = Answer;
                this.PollAnswers.Add(NewAnswer);
            }
        }

    }
    public class Tag
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string User { get; set; }
    }


}