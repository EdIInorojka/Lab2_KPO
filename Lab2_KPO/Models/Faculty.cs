﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_KPO.Models
{
    public class Faculty
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
