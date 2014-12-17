using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissingLinkPro.Helpers
{
    /**
     * Methods that help with debugging via console.
     **/
    public static class DebugHelper
    {
        /**
        * Quick shortcut method for printing to the diagnostic console, sans new line.
        * @para string s:  the string to be printed
        **/
        public static void display(string s)
        {
            System.Diagnostics.Debug.Write(s);
        } // display

        /**
         * Quick shortcut method for printing to the diagnostic console, with new line.
         * @para string s:  the string to be printed
         **/
        public static void displayln(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        } // displayln
    }
}