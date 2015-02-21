using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PlayerQuest
    {
        public Quest Details { get; set; }
        public bool IsCompleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PlayerQuest(Quest details)
        {
            Details = details;
            IsCompleted = false;
            Name = details.Name;
            Description = details.Description;
        }
    }
}