using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses {
    public class HomeMenuBlock {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="isAdminOnly"></param>
        public HomeMenuBlock(string color, string title, string description, string controller, string action, bool isAdminOnly) {
            Color = color;
            Title = title;
            Description = description;
            Controller = controller;
            Action = action;
            IsAdminOnly = isAdminOnly;
        }

        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAdminOnly { get; set; }
    }
}