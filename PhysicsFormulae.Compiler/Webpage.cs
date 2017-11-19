﻿using System;

namespace PhysicsFormulae.Compiler
{
    public class Webpage : Reference
    {
        public string Title { get; set; }
        public string WebsiteTitle { get; set; }
        public string URL { get; set; }
        public DateTime DateAccessed { get; set; }

        public Webpage()
        {
            Type = ReferenceType.Webpage;
        }
    }
}
