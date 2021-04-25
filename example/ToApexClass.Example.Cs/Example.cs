using System;
using System.Collections.Generic;
using ToApexClass.Attributes;

namespace ToApexClass.Example.Cs
{
    [ApexName("C# Example")]
    public class Example
    {
        public string Details { get; set; } = "Disappearing text.";
        public bool IsTrueBool { get; set; }
        public DateTime? Timestamp { get; set; }
        public int Count { get; set; }
        
        [ApexExclude]
        public List<string> Todo { get; set; }
    }
}
