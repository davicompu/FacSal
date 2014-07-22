using ChrisJSherm.Extensions;
using FacsalData;
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
    public class SalariesByFacultyTypeReport : Report
    {
        const int NUM_COLUMNS = 5;
        const int SUMMARY_DATA_COLUMNS = 0;
        private Department department;
        private string[] labels;

        int Row { get; set; }

        public SalariesByFacultyTypeReport(Department department, IQueryable<Salary> salaries)
            : base()
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(department.Name);
            this.labels = new string[5] { "Type", "Starting salaries", "Merit Increases", "Special Increases", "New Salaries" };

            #region Table Labels
            Row++;
            sheet = WriteTableLabels(sheet);
            #endregion
            #region Department Data
            sheet = WriteDepartmentData(sheet, department, salaries.GroupBy(s => s.FacultyTypeId)
                        .Select(sg => new
                        {
                            FacultyType = sg.Select(x => x.FacultyType.Name),
                            StartingSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                                x.EminentAmount + x.PromotionAmount),
                            MeritIncreases = sg.Sum(x => x.MeritIncrease),
                            SpecialIncreases = sg.Sum(x => x.SpecialIncrease),
                            EminentIncreases = sg.Sum(x => x.EminentIncrease),
                            NewSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                                x.EminentAmount + x.PromotionAmount + x.MeritIncrease +
                                x.SpecialIncrease + x.EminentIncrease)
                        })
                    .ToList()
                        );
            #endregion
            //System.Diagnostics.Debug.WriteLine(salaries);
            sheet = PerformFinalFormatting(sheet);

            BinaryData = package.GetAsByteArray();
            FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            FileName = "FacSal_SalariesByFacultyTypeReport_" +
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    System.DateTime.Now, TimeZoneInfo.Local.Id, "Eastern Standard Time").ToString()
                + ".xlsx";

        }

        public SalariesByFacultyTypeReport(IEnumerable<Department> departments, IQueryable<Salary> salaries, bool singleSheet)
            : base()
        {
            ExcelPackage package = new ExcelPackage();
            this.labels = new string[5] { "Type", "Starting salaries", "Merit Increases", "Special Increases", "New Salaries" };
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
                        .Where(s => s.Person.Employments.Any(e => e.DepartmentId == department.Id))
                        .GroupBy(s => s.FacultyTypeId)
                        .Select(sg => new
                        {
                            FacultyType = sg.Select(x => x.FacultyType.Name),
                            StartingSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                                x.EminentAmount + x.PromotionAmount),
                            MeritIncreases = sg.Sum(x => x.MeritIncrease),
                            SpecialIncreases = sg.Sum(x => x.SpecialIncrease),
                            EminentIncreases = sg.Sum(x => x.EminentIncrease),
                            NewSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                                x.EminentAmount + x.PromotionAmount + x.MeritIncrease +
                                x.SpecialIncrease + x.EminentIncrease)
                        })
                    .ToList()
                        );
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
                        .Where(s => s.Person.Employments.Any(e => e.DepartmentId == department.Id))
                        .GroupBy(s => s.FacultyTypeId)
                        .Select(sg => new
                        {
                        FacultyType = sg.Select(x => x.FacultyType.Name),
                        StartingSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                            x.EminentAmount + x.PromotionAmount),
                        MeritIncreases = sg.Sum(x => x.MeritIncrease),
                        SpecialIncreases = sg.Sum(x => x.SpecialIncrease),
                        EminentIncreases = sg.Sum(x => x.EminentIncrease),
                        NewSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                            x.EminentAmount + x.PromotionAmount + x.MeritIncrease +
                            x.SpecialIncrease + x.EminentIncrease)
                    })
                    .ToList()
                        );
                    #endregion

                    sheet = PerformFinalFormatting(sheet);

                    Row = 0;
                }
            }

            BinaryData = package.GetAsByteArray();
            FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            FileName = "FacSal_SalariesByFacultyTypeReport_" +
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

            foreach (string label in this.labels)
            {
                sheet.Cells[Row, ++column].Value = label;
            }

            return sheet;
        }

        private ExcelWorksheet WriteDepartmentData(ExcelWorksheet sheet, Department department, IEnumerable<dynamic> salaries)
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
                    sheet.Cells[Row, ++column].Value = salary.FacultyType[0];
                    sheet.Cells[Row, ++column].Value = salary.StartingSalaries;
                    sheet.Cells[Row, ++column].Value = salary.MeritIncreases;
                    sheet.Cells[Row, ++column].Value = salary.SpecialIncreases;
                    sheet.Cells[Row, ++column].Value = salary.NewSalaries;
                }

            }


            #endregion

            return sheet;
        }

        private ExcelWorksheet PerformFinalFormatting(ExcelWorksheet sheet)
        {
            //Header
            sheet.HeaderFooter.FirstHeader.LeftAlignedText = "VIRGINIA TECH\n"
                + "SALARY BY FACULTY TYPE REPORT\n" +
                "FY " + ConfigurationManager.AppSettings["CycleYear"];

            //Footer
            sheet.HeaderFooter.FirstFooter.CenteredText = System.DateTime.Now.ToShortDateString() +
                " Salary By Faculty Report FY " +
                ConfigurationManager.AppSettings["CycleYear"];

            //Printing
            sheet.PrinterSettings.Orientation = eOrientation.Landscape;
            sheet.PrinterSettings.FitToPage = true;
            sheet.PrinterSettings.FitToWidth = 1;
            sheet.PrinterSettings.FitToHeight = 0;
            ExcelRange range_numberFormatting =
                sheet.Cells[1, 2, Row, NUM_COLUMNS];
            //Cell styling
            range_numberFormatting.Style.Numberformat.Format = "_($* #,##0_);_($* (#,##0);_($* \"-\"_);_(@_)";


            sheet.Cells.AutoFitColumns();

            return sheet;
        }
    }
}