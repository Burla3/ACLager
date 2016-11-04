using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses {
    public class HomeMenuBlock {
        public HomeMenuBlock(string color, string title, string description) {
            Color = color;
            Title = title;
            Description = description;
        }

        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}