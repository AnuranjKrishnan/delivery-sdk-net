﻿using Newtonsoft.Json.Linq;
using System;

namespace KenticoCloud.Delivery
{
    /// <summary>
    /// Represents an element in content type.
    /// </summary>
    public class TypeElement : ITypeElement
    {
        /// <summary>
        /// Element's type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Element's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Element's codename
        /// </summary>
        public string Codename { get; set; }

        /// <summary>
        /// Initializes type element information.
        /// </summary>
        /// <param name="element">JSON with element data.</param>
        public TypeElement(JToken element, string codename = "")
        {
            Type = element["type"].ToString();
            Name = element["name"].ToString();
            Codename = String.IsNullOrEmpty(codename) ? element["codename"].ToString() : codename;
        }
    }
}