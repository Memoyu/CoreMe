﻿using System;

namespace CoreMe.Core.AOP.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LoggerAttribute : Attribute
    {
        public string Template { get; }

        public LoggerAttribute(string template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }
    }
}
