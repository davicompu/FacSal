﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facsal.Models.Files
{
    public abstract class Report
    {
        public byte[] BinaryData { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
    }
}