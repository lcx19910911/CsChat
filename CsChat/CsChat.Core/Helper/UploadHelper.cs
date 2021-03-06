﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CsChat.Core
{
    public class UploadHelper
    {
        public static string Save(HttpPostedFileBase file,string mark)
        {
            var root = @"Upload/" + mark;
            string phicyPath = Path.Combine(HttpRuntime.AppDomainAppPath, root);   
            var fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string path= Path.Combine(phicyPath, fileName);
            if (!Directory.Exists(phicyPath))
                Directory.CreateDirectory(phicyPath);
            file.SaveAs(path);
            return string.Format("/{0}/{1}",root,fileName);
        }

        public static string ExportInto(HttpPostedFileBase file, string mark)
        {
            var root = @"ExportInto/" + mark;
            string phicyPath = Path.Combine(HttpRuntime.AppDomainAppPath, root);
            var fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string path = Path.Combine(phicyPath, fileName);
            if (!Directory.Exists(phicyPath))
                Directory.CreateDirectory(phicyPath);
            file.SaveAs(path);
            return string.Format("/{0}/{1}", root, fileName);
        }
    }
}
