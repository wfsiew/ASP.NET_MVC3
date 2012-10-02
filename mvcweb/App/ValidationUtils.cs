using System;
using System.ComponentModel.DataAnnotations;

namespace mvcweb.App
{
    internal class IntAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int val = (int)value;
            return val > 0;
        }
    }

    internal class DoubleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            double val = (double)value;
            return val > 0;
        }
    }
}