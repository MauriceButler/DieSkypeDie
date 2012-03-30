using System;
using System.Linq;

namespace DieSkypeDie
{
    public class PItem
    {
        public string ProcessName { get; set; }
        public string Title { get; set; }

        public PItem(string processname, string title)
        {
            this.ProcessName = processname;
            this.Title = title;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.Title))
            {
                return string.Format("{0} ({1})", this.ProcessName, this.Title);
            }
            else
            {
                return string.Format("{0}", this.ProcessName);
            }
        }
    }
}