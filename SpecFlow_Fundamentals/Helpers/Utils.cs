using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Fundamentals.Helpers
{
    public static class Utils
    {
        //
        // Summary:
        //     Convenient method to get the string value of a variable or alternative if the
        //     target is null or empty
        //
        // Parameters:
        //   target:
        //     the string that is to be expected for null or empty
        //
        //   alternative:
        //     the alternative string that is to be return if "target" is null or empty. Note!
        //     no check for null or empty are applied to the alternative:
        //
        // Returns:
        //     the original value of the "target" parameter if it is not null or empty, otherwise
        //     returns the original value of the "alternative" parameter.
        public static string GetNonEmptyValueOrAlternative(string target, string alternative)
        {
            if (!string.IsNullOrEmpty(target))
            {
                return target;
            }

            return alternative;
        }
    }
}
