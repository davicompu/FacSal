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
    public class MeritSummaryReport : Report
    {
        const int NUM_COLUMNS = 6;
        const int SUMMARY_DATA_COLUMNS = 0;
        private string[] labels;
        int Row { get; set; }

        public MeritSummaryReport(Department department, IEnumerable<Salary> salaries)
            : base()
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(department.Name);
            this.labels = new string[6] { "Highest increase", "Highest percentage", "Median increase", "Mean increase", "Lowest increase", "Lowest percentage" };

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
            FileName = "FacSal_MeritSummaryReport_" +
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    System.DateTime.Now, TimeZoneInfo.Local.Id, "Eastern Standard Time").ToString()
                + ".xlsx";

        }

        public MeritSummaryReport(IEnumerable<Department> departments, IEnumerable<Salary> salaries, bool singleSheet)
            : base()
        {
            ExcelPackage package = new ExcelPackage();
            this.labels = new string[6] { "Highest increase", "Highest percentage", "Median increase", "Mean increase", "Lowest increase", "Lowest percentage" };
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
            FileName = "FacSal_MeritSummaryReport_" +
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    System.DateTime.Now, TimeZoneInfo.Local.Id, "Eastern Standard Time").ToString()
                + ".xlsx";
        }

        private Dictionary<string,double> getMeritSummary(IEnumerable<Salary> salaries_data)
        {
            List<Salary> salariesSorted = salaries_data.OrderBy(x => x.MeritIncrease).ToList();

            var maxIncrease = salariesSorted[(salariesSorted.Count) - 1];
            var minIncrease = salariesSorted[0];
            double median = 0, mean = 0;
            int total = 0;
            foreach (var salary in salariesSorted)
            {
                total += salary.MeritIncrease;
            }

            mean = (double)total / salariesSorted.Count; //TODO: Ask about cast

            int half = (int)Math.Floor((double)salariesSorted.Count / 2);

            if ((salariesSorted.Count % 2) != 0)
            {
                median = (double)salariesSorted.ElementAt(half).MeritIncrease;
            }
            else
            {
                median = (double)(salariesSorted.ElementAt(half - 1).MeritIncrease + salariesSorted.ElementAt(half).MeritIncrease) / 2;
            }

            Dictionary<string, double> dictionary =new Dictionary<string, double>();

            dictionary.Add("max_increase", maxIncrease.MeritIncrease);
            dictionary.Add("max_increase_percentage", getMeritPercentageIncrease(maxIncrease));
            dictionary.Add("min_increase", minIncrease.MeritIncrease);
            dictionary.Add("min_increase_percentage", getMeritPercentageIncrease(minIncrease));
            dictionary.Add("median", median);
            dictionary.Add("mean", mean);

            return dictionary;
        }

        private double getMeritPercentageIncrease(Salary salary)
        {
            double totalAmount = salary.BaseAmount + salary.AdminAmount + salary.EminentAmount + salary.PromotionAmount;
            double percentage = (double)salary.MeritIncrease / (totalAmount);
            return percentage;
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
                var summary = getMeritSummary(salaries);
                Row++;
                column = 0;
                sheet.Cells[Row, ++column].Value = summary["max_increase"];
                sheet.Cells[Row, ++column].Value = summary["max_increase_percentage"];
                sheet.Cells[Row, ++column].Value = summary["median"];
                sheet.Cells[Row, ++column].Value = summary["mean"];
                sheet.Cells[Row, ++column].Value = summary["min_increase"];
                sheet.Cells[Row, ++column].Value = summary["min_increase_percentage"];

            }
            #endregion

            return sheet;
        }

        private ExcelWorksheet PerformFinalFormatting(ExcelWorksheet sheet)
        {
            //Header
            sheet.HeaderFooter.FirstHeader.LeftAlignedText = "VIRGINIA TECH\n"
                + "MERIT SUMMARY REPORT\n" +
                "FY " + ConfigurationManager.AppSettings["CycleYear"];

            //Footer
            sheet.HeaderFooter.FirstFooter.CenteredText = System.DateTime.Now.ToShortDateString() +
                " Merit Summary Report FY " +
                ConfigurationManager.AppSettings["CycleYear"];

            //Printing
            sheet.PrinterSettings.Orientation = eOrientation.Landscape;
            sheet.PrinterSettings.FitToPage = true;
            sheet.PrinterSettings.FitToWidth = 1;
            sheet.PrinterSettings.FitToHeight = 0;

            var currencyFormat = "_($* #,##0_);_($* (#,##0);_($* \"-\"_);_(@_)";
            var percentageFormat = "0.0%";

            ExcelRange range_highestMeritIncrease = sheet.Cells[1, 1, Row, 1];
            ExcelRange range_lowestMeritIncrease = sheet.Cells[1, 5, Row, 5];

            ExcelRange range_meritIncreaseMeanMedian = sheet.Cells[1, 3, Row, 4];

            ExcelRange range_highestMeritPercentage = sheet.Cells[1, 2, Row, 2];
            ExcelRange range_lowestMeritPercentage = sheet.Cells[1, 6, Row, 6];

            

            //Cell styling

            range_highestMeritIncrease.Style.Numberformat.Format = currencyFormat;
            range_lowestMeritIncrease.Style.Numberformat.Format = currencyFormat;
            range_meritIncreaseMeanMedian.Style.Numberformat.Format = currencyFormat;

            range_highestMeritPercentage.Style.Numberformat.Format = percentageFormat;
            range_lowestMeritPercentage.Style.Numberformat.Format = percentageFormat;
            
            sheet.Cells.AutoFitColumns();

            return sheet;
        }
    }
}