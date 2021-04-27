using System;

namespace ToApexClass.Attributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field |
                    AttributeTargets.Method |
                    AttributeTargets.Class |
                    AttributeTargets.Interface)]
    public class ApexExclude : Attribute { }

    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field |
                    AttributeTargets.Method |
                    AttributeTargets.Class |
                    AttributeTargets.Interface)]
    public class ApexName : Attribute
    {
        private string val;

        public ApexName(string val) { this.val = val; }
    }

    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field)]
    public class ApexDefaultValue : Attribute
    {
        private string val;

        public ApexDefaultValue(string val) { this.val = val; }
    }

    /// Match and replace a placeholder identifier in Apex code.
    [AttributeUsage(AttributeTargets.Class)]
    public class ApexPlaceholder : Attribute
    {
        private string val;

        public ApexPlaceholder(string val) { this.val = val; }
    }
    
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field |
                    AttributeTargets.Method |
                    AttributeTargets.Class |
                    AttributeTargets.Interface)]
    public class ApexComment : Attribute
    {
        private string val;

        public ApexComment(string val) { this.val = val; }
    }
}
