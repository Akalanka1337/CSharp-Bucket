 
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
 
 
 
 
 
 
 SystemReport _objSystemReport = new SystemReport();
                        _objSystemReport.ReportID = Convert.ToInt32(_objSystemSetting.ExtraField1);
                        _objSystemReport.GetByID();
                        string PathFile = Server.MapPath("~/Content/DisplayReport" + _objEmployee.EmployeeNo.ToString() + ".repx");
                        FileInfo file = new System.IO.FileInfo(PathFile);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        using (FileStream fs = File.Create(PathFile))
                        {
                           
                            fs.Flush();
                            fs.Dispose();
                        }

                        CommonFunctions.WriteFiletoDisk(_objSystemReport.ReportBitstreem, PathFile);

                        XtraReport report = new XtraReport();
                        report.DataSource = dspayslipsource;
                        report.DataMember = "Table";
                        report.LoadLayout(PathFile);

                        PdfExportOptions optns = new PdfExportOptions();
                        optns.PasswordSecurityOptions.OpenPassword = "NINE";
     

                        MemoryStream mem = new MemoryStream();

                        report.ExportToPdf(mem, optns);
                        WriteDocumentToResponse(mem.ToArray(), "pdf", false, "PaySlip" + ddSalaryYear.SelectedItem.Text + "-" + ddSalaryMonth.SelectedItem.Text + ".pdf");                       
                        file.Delete();
