using System;
using System.Collections.Generic;
using System.Text;

namespace ShamirSecretSharing.FiniteFieldIntegers
{
    public class FiniteFieldCompatibilityException : Exception
    {
        public FiniteFieldInteger First { get; }
        public FiniteFieldInteger Second { get; }
        public FiniteFieldCompatibilityException(FiniteFieldInteger first, FiniteFieldInteger second) 
            : base($"\"{first}\" and \"{second}\" are not compatible. They have different modulos.")
        {
            First = first;
            Second = second;
        }
    }
}
