using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace MSCRateClient
{
    public class ExcelHelper
    {

        public static DataTable ImportExcel(Stream stream, int iRow, int iColumn)
        {
            try
            {
                Workbook doc = new Workbook();
                doc.Shared = true;
                doc.Open(stream);
                Worksheet oSheet = doc.Worksheets[0];

                DataTable dt_excel = new DataTable("Table");
                DataColumn dc;

                for (int i = 0; i < oSheet.Cells.Columns.Count; i++)
                {
                    if (!string.IsNullOrEmpty(oSheet.Cells[iRow, i].Value as string))
                    {
                        dc = new DataColumn(oSheet.Cells[iRow, i].Value as string);
                        dt_excel.Columns.Add(dc);
                    }
                }

                for (int i = iRow + 1; i < oSheet.Cells.Rows.Count; i++)
                {
                    DataRow dr = dt_excel.NewRow();
                    for (int j = 0; j < oSheet.Cells.Columns.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(oSheet.Cells[iRow, j].Value as string))
                            dr[oSheet.Cells[iRow, j].Value.ToString()] = oSheet.Cells[i, j].Value;
                    }
                    dt_excel.Rows.Add(dr);
                }
                return dt_excel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ImportExcel(string sFileName, int iRow, int iColumn)
        {
            try
            {
                Workbook doc = new Workbook();
                doc.Shared = true;
                doc.Open(sFileName);
                Worksheet oSheet = doc.Worksheets[0];

                DataTable dt_excel = new DataTable(sFileName);
                DataColumn dc;

                for (int i = 0; i < oSheet.Cells.Columns.Count; i++)
                {
                    if (!string.IsNullOrEmpty(oSheet.Cells[iRow, i].Value as string))
                    {
                        dc = new DataColumn(oSheet.Cells[iRow, i].Value as string);
                        dt_excel.Columns.Add(dc);
                    }
                }

                for (int i = iRow + 1; i < oSheet.Cells.Rows.Count; i++)
                {
                    DataRow dr = dt_excel.NewRow();
                    for (int j = 0; j < oSheet.Cells.Columns.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(oSheet.Cells[iRow, j].Value as string))
                            dr[oSheet.Cells[iRow, j].Value.ToString()] = oSheet.Cells[i, j].Value;
                    }
                    dt_excel.Rows.Add(dr);
                }
                return dt_excel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static MemoryStream ExportExcel(DataTable dt_excel, string sMapPath)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(sMapPath);
                if (!info.Exists)
                {
                    info.Create();
                }

                Workbook doc = new Workbook();
                doc.Shared = true;
                Worksheet oSheet = doc.Worksheets[0];


                Style style = doc.Styles[doc.Styles.Add()];
                style.Font.Name = "Arial";
                style.Font.Size = 8;
                style.IsTextWrapped = true;
                Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                oSheet.Cells.ApplyStyle(style, styleFlag);

                for (int i = dt_excel.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt_excel.Columns[i].ColumnName.Contains("Hidden"))
                    {
                        dt_excel.Columns.RemoveAt(i);
                    }
                }

                for (int i = 0; i < dt_excel.Columns.Count; i++)
                {
                    oSheet.Cells[0, i].PutValue(dt_excel.Columns[i].ColumnName);
                    oSheet.Cells[0, i].Style.Font.IsBold = true;
                }

                Style style1 = doc.Styles[doc.Styles.Add()];//新增样式 
                style1.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style1.Font.Name = "Arial";//文字字体 
                style1.Font.Size = 8;//文字大小 
                style1.IsLocked = false;//单元格解锁 
                                        //style1.Font.IsBold = true;//粗体 
                style1.ForegroundColor = System.Drawing.Color.FromArgb(0x99, 0xcc, 0xff);//设置背景色 
                style1.Pattern = BackgroundType.Solid; //设置背景样式 
                style1.IsTextWrapped = true;//单元格内容自动换行 

                for (int i = 0; i < dt_excel.Rows.Count; i++)
                {
                    for (int j = 0; j < dt_excel.Columns.Count; j++)
                    {

                        if (dt_excel.Columns[j].ColumnName == "BookingDate" || dt_excel.Columns[j].ColumnName == "StatusDate")
                            oSheet.Cells[i + 1, j].PutValue(Convert.ToDateTime(dt_excel.Rows[i][j]).ToString("dd-MM-yyyy HH:mm:ss"));

                        else if (dt_excel.Columns[j].ColumnName == "WarningAlert" || dt_excel.Columns[j].ColumnName == "ConfirmationComment")
                        {
                            oSheet.Cells[i + 1, j].PutValue(dt_excel.Rows[i][j]);
                            oSheet.Cells[i + 1, j].SetStyle(style1);
                        }
                        //else if (dt_excel.Columns[j].ColumnName == "BookingNumber")
                        //{
                        //	oSheet.Cells[i + 1, j].PutValue(dt_excel.Rows[i][j]);
                        //	oSheet.Cells[i + 1, j].SetStyle(style1);
                        //}
                        else
                            oSheet.Cells[i + 1, j].PutValue(dt_excel.Rows[i][j]);

                    }
                }
                for (int i = 0; i < oSheet.Cells.MaxDataColumn; i++)
                {
                    if (i != 8 && i != 9)
                        oSheet.AutoFitColumn(i);
                    else
                        oSheet.Cells.SetColumnWidth(i, 50);
                }

                //oSheet.column.SetColumnWidth(9, 40);
                //oSheet.Cells.SetColumnWidth(10, 40);
                return doc.SaveToStream();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ExportExcel(DataTable dt_excel, string sMapPath, string sFileName, Func<object, string> funConvertDateTime = null)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(sMapPath);
                if (!info.Exists)
                {
                    info.Create();
                }

                string sFullPath = sMapPath + sFileName;

                Workbook doc = new Workbook();
                doc.Shared = true;
                Worksheet oSheet = doc.Worksheets[0];

                oSheet.Cells.SetColumnWidth(0, 30);

                Style style = doc.Styles[doc.Styles.Add()];
                style.Font.Name = "Arial";
                style.Font.Size = 8;
                style.IsTextWrapped = true;
                Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                oSheet.Cells.ApplyStyle(style, styleFlag);

                for (int i = dt_excel.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt_excel.Columns[i].ColumnName.Contains("Hidden"))
                    {
                        dt_excel.Columns.RemoveAt(i);
                    }
                }

                for (int i = 0; i < dt_excel.Columns.Count; i++)
                {
                    oSheet.Cells[0, i].PutValue(dt_excel.Columns[i].ColumnName);
                    oSheet.Cells[0, i].Style.Font.IsBold = true;
                }

                for (int i = 0; i < dt_excel.Rows.Count; i++)
                {
                    for (int j = 0; j < dt_excel.Columns.Count; j++)
                    {
                        oSheet.Cells.SetColumnWidth(j, 40);
                        if (dt_excel.Columns[j].DataType == typeof(DateTime) && funConvertDateTime != null)
                            oSheet.Cells[i + 1, j].PutValue(funConvertDateTime(dt_excel.Rows[i][j]));
                        else
                            oSheet.Cells[i + 1, j].PutValue(dt_excel.Rows[i][j]);
                    }
                }
                oSheet.AutoFitColumns();
                doc.Save(sFullPath);
                return sFullPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}