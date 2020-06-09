using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeOrNot.Model
{
    public class Number
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool Prime { get; set; }

        public string Factors { get; set; }

        public bool? Correct { get; set; }

    }
}
