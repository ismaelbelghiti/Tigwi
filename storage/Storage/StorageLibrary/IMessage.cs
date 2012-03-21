using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IMessage
    {
        int Post_id {get; set;}
        int Poster_id {get; set;}
        DateTime Post_date {get; set;}
        string Post_content { get; set; }
    }
}
