﻿using System.ComponentModel.DataAnnotations;

namespace Task_Management_Api.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public List<TaskComment>? TaskComments { get; set; }
    }
}
