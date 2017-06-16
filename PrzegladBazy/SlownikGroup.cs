using System;
using System.Collections.Generic;

namespace PrzegladBazy
{
    [Serializable]
    public class SlownikGroup
    {
        public List<string> _slowniki = new List<string>();
        public string _title = null;
        

        public SlownikGroup()
        {
            _title = "noname";
        }

        public override string ToString()
        {
            return _title;
        }
    }
}