using ChrisJSherm.Extensions;
using OfficeOpenXml;
using SalaryEntities.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Facsal.Models.Files
{
    public class MeetingAlternativeReport : Report
    {
        const int NUM_COLUMNS = 16;
        const int SUMMARY_DATA_COLUMNS = 0;

        int Row { get; set; }

        public MeetingAlternativeReport(Department department, IEnumerable<Salary> salaries) : base()
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(department.Name);

            #region Table Labels
            Row++;
            sheet = WriteTableLabels(sheet);
            #endregion

            #region Department Data
            sheet = WriteDepartmentData(sheet, department, salaries);
            #endregion

            sheet = PerformFinalFormatting(sheet);

            BinaryData = package.GetAsByteArray();
            FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            FileName = "FacSal_MeetingReport_" + 
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    System.DateTime.Now, TimeZoneInfo.Local.Id, "Eastern Standard Time").ToString()
                + ".xlsx";
            
        }

        public MeetingAlternativeReport(IEnumerable<Department> departments, IEnumerable<Salary> salaries, bool singleSheet) : base()
        {
            ExcelPackage package = new ExcelPackage();

            if (singleSheet)
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Unit");

                #region Table Labels
                Row++;
                sheet = WriteTableLabels(sheet);
                #endregion
                
                foreach (var department in departments)
                {
                    #region Department Data
                    sheet = WriteDepartmentData(sheet, department, salaries
                        .Where(s => s.Person.Employments.Any(e => e.DepartmentId == department.Id)));
                    #endregion
                }

                sheet = PerformFinalFormatting(sheet);
            }
            else
            {
                foreach (var department in departments)
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add(department.Name);

                    #region Table Labels
                    Row++;
                    sheet = WriteTableLabels(sheet);
                    #endregion

                    #region Department Data
                    sheet = WriteDepartmentData(sheet, department, salaries
                        .Where(s => s.Person.Employments.Any(e => e.DepartmentId == department.Id)));
                    #endregion

                    sheet = PerformFinalFormatting(sheet);

                    Row = 0;
                }
            }

            BinaryData = package.GetAsByteArray();
            FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            FileName = "FacSal_MeetingReport_" +
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    System.DateTime.Now, TimeZoneInfo.Local.Id, "Eastern Standard Time").ToString()
                + ".xlsx";
        }

        private ExcelWorksheet WriteTableLabels(ExcelWorksheet sheet)
        {
            int column = 0;

            ExcelRange range_labels = sheet.Cells[Row, 1, Row, NUM_COLUMNS];
            range_labels.Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Bold));
            range_labels.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            range_labels.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range_labels.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(200, 200, 200));
            range_labels.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.Person.FullName);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.RankType.Name);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.AppointmentType.Name);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.FullTimeEquivalent);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.BaseAmount);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.AdminAmount);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.EminentAmount);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.PromotionAmount);
            sheet.Cells[Row, ++column].Value = DataAnnotationsHelper.GetPropertyName<Salary>(s => s.TotalAmount);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.MeritIncrease);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.SpecialIncrease);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.EminentIncrease);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.NewEminentAmount);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.NewTotalAmount);
            sheet.Cells[Row, ++column].Value =
                DataAnnotationsHelper.GetPropertyName<Salary>(s => s.TotalChange);
            sheet.Cells[Row, ++column].Value =
               DataAnnotationsHelper.GetPropertyName<Salary>(s => s.Comments);
            
            return sheet;
        }

        private ExcelWorksheet WriteDepartmentData(ExcelWorksheet sheet, Department department, IEnumerable<Salary> salaries)
        {
            int column = 0;

            #region Department Name
            Row++;
            ExcelRange range_departmentName = sheet.Cells[Row, 1, Row, NUM_COLUMNS];
            range_departmentName.Merge = true;
            range_departmentName.Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Italic));
            range_departmentName.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            range_departmentName.Value = department.Name;
            #endregion

            #region Department salaries
            if (salaries.Count() == 0)
            {
                Row++;
                ExcelRange range_departmentData = sheet.Cells[Row, 1, Row, NUM_COLUMNS];
                range_departmentData.Merge = true;
                range_departmentData.Style.Font.SetFromFont(new Font("Calibri", 11, FontStyle.Regular));
                range_departmentData.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                range_departmentData.Value = "No records";
            }
            else
            {
                foreach (var salary in salaries)
                {
                    Row++;
                    column = 0;
                    sheet.Cells[Row, ++column].Value = salary.Person.FullName;
                    sheet.Cells[Row, ++column].Value = salary.RankType.Name;
                    sheet.Cells[Row, ++column].Value = salary.AppointmentType.Name;
                    sheet.Cells[Row, ++column].Value = salary.FullTimeEquivalent;
                    sheet.Cells[Row, ++column].Value = salary.BaseAmount;
                    sheet.Cells[Row, ++column].Value = salary.AdminAmount;
                    sheet.Cells[Row, ++column].Value = salary.EminentAmount;
                    sheet.Cells[Row, ++column].Value = salary.PromotionAmount;
                    sheet.Cells[Row, ++column].FormulaR1C1 = "SUM(RC[-4]:RC[-1])";
                    sheet.Cells[Row, ++column].Value = salary.MeritIncrease;
                    sheet.Cells[Row, ++column].Value = salary.SpecialIncrease;
                    sheet.Cells[Row, ++column].Value = salary.EminentIncrease;                 
                    sheet.Cells[Row, ++column].FormulaR1C1 = 
                        "RC[-6]+(RC[-6]/RC[-4])*(RC[-3]+RC[-2])+RC[-1]";
                    sheet.Cells[Row, ++column].FormulaR1C1 = 
                        "RC[-9]+RC[-8]+RC[-6]+RC[-4]+RC[-3]+RC[-2]+RC[-1]";
                    sheet.Cells[Row, ++column].FormulaR1C1 =
                        "RC[-1]/RC[-6]-1";
                    sheet.Cells[Row, ++column].Value = salary.Comments;
                }
            }
            #endregion

            return sheet;
        }

        private ExcelWorksheet PerformFinalFormatting(ExcelWorksheet sheet)
        {
            //Header
            sheet.HeaderFooter.FirstHeader.LeftAlignedText = "VIRGINIA TECH\n"
                + "MEETING REPORT\n" +
                "FY " + ConfigurationManager.AppSettings["CycleYear"];

            //Footer
            sheet.HeaderFooter.FirstFooter.CenteredText = System.DateTime.Now.ToShortDateString() +
                " Meeting Report FY " +
                ConfigurationManager.AppSettings["CycleYear"];

            //Printing
            sheet.PrinterSettings.Orientation = eOrientation.Landscape;
            sheet.PrinterSettings.FitToPage = true;
            sheet.PrinterSettings.FitToWidth = 1;
            sheet.PrinterSettings.FitToHeight = 0;
            ExcelRange range_numberFormatting =
                sheet.Cells[1, 5, Row, NUM_COLUMNS - 1];
            ExcelRange range_percentFormatting =
                sheet.Cells[1, NUM_COLUMNS, Row, NUM_COLUMNS];

            //Cell styling
            range_numberFormatting.Style.Numberformat.Format = "_($* #,##0_);_($* (#,##0);_($* \"-\"_);_(@_)";
            range_percentFormatting.Style.Numberformat.Format = "0.0%";

            sheet.Cells.AutoFitColumns();

            return sheet;
        }
    }
}