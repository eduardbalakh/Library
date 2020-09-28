using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Data
{
    class WordEntity
    {
        public int ID { get; set; }
        public string word { get; set; }
        public int frequency { get; set; }

        public override string ToString()
        {
            return word;
        }
    }
}
