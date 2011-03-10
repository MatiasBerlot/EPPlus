﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using System.IO;

namespace ExcelPackageTest
{
    [TestClass]
    public class ReadTemplate
    {
        [TestMethod]
        public void ReadDrawing()
        {
            using (ExcelPackage pck = new ExcelPackage(new FileInfo(@"Test\Drawing.xlsx"))) 
            {
                var ws = pck.Workbook.Worksheets["Pyramid"];
                Assert.AreEqual(ws.Cells["V24"].Value, 104D);
            }

        }
        [TestMethod]
        public void ReadWorkSheet()
        {
            FileStream instream = new FileStream(@"Test\Worksheet.xlsx", FileMode.Open, FileAccess.ReadWrite);
            using (ExcelPackage pck = new ExcelPackage(instream))
            {
                var ws = pck.Workbook.Worksheets["Perf"];
                Assert.AreEqual(ws.Cells["H6"].Formula, "B5+B6");
            }
            instream.Close();
        }
        [TestMethod]
        public void ReadStreamWithTemplateWorkSheet()
        {
            FileStream instream = new FileStream(@"Test\Worksheet.xlsx", FileMode.Open, FileAccess.Read);
            MemoryStream stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage(stream, instream))
            {
                var ws = pck.Workbook.Worksheets["Perf"];
                Assert.AreEqual(ws.Cells["H6"].Formula, "B5+B6");
            
               var wsHF=pck.Workbook.Worksheets["Header footer"];
               Assert.AreEqual(wsHF.HeaderFooter.firstFooter.LeftAlignedText, "First Left");
               Assert.AreEqual(wsHF.HeaderFooter.firstFooter.RightAlignedText, "First Right");

                Assert.AreEqual(wsHF.HeaderFooter.evenFooter.CenteredText, "even Centered");

               Assert.AreEqual(wsHF.HeaderFooter.oddFooter.LeftAlignedText, "odd Left");
               Assert.AreEqual(wsHF.HeaderFooter.oddFooter.CenteredText,"odd Centered");
               Assert.AreEqual(wsHF.HeaderFooter.oddFooter.RightAlignedText, "odd Right");

               foreach (var cell in pck.Workbook.Names["Data"])
               {
                   Assert.IsNotNull(cell);
               }
                
                pck.SaveAs(new FileInfo(@"Test\Worksheet2.xlsx"));
            
            }

            instream.Close();
        }
        [TestMethod]
        public void ReadStreamSaveAsStream()
        {
            FileStream instream = new FileStream(@"Test\Worksheet.xlsx", FileMode.Open, FileAccess.ReadWrite);
            MemoryStream stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage(instream))
            {
                var ws = pck.Workbook.Worksheets["Perf"];
                pck.SaveAs(stream);
            }
            instream.Close();
        }
    }
}
