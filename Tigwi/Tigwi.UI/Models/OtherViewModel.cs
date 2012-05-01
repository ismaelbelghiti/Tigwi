using System;
using System.Collections;
using System.Collections.Generic;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OtherViewModel
    {
        public OtherViewModel(string user)
        {
            accountName = user;
            posts = new List<PostViewModel>();
            for (int i=0;i<50;i++)
            {
                posts.Add(new PostViewModel());
            }
        }
        public List<PostViewModel>posts { get; private set; }
        public string accountName { get; private set; }

    }
}