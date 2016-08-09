using System;
using System.Collections.Generic;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using System.Reflection;
using NPOI.HSSF.Extractor;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using System.Web;
using System.Text;

namespace CommonUtil
{
    /// <summary>
    /// NPOI操作excel辅助类
    /// </summary>
    public static class ExcelUtil
    {
        #region 定义与初始化
        public static HSSFWorkbook workbook;
        [Flags]
        public enum LinkType
        {
            网址,
            档案,
            邮件,
            内容
        };

        /// <summary>
        /// 初始化文档属性
        /// </summary>
        private static void InitializeWorkbook()
        {
            if (workbook == null)
                workbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI";
            workbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "文件作者信息"; //填加xls文件作者信息
            si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
            si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
            si.Comments = "作者信息"; //填加xls文件作者信息
            si.Title = "标题信息"; //填加xls文件标题信息
            si.Subject = "主题信息";//填加文件主题信息
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;
        }
        #endregion

        #region 资料形态转换
        public static void WriteSteamToFile(MemoryStream ms, string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }
        public static void WriteSteamToFile(byte[] data, string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            data = null;
            fs = null;
        }
        public static Stream WorkBookToStream(HSSFWorkbook InputWorkBook)
        {
            MemoryStream ms = new MemoryStream();
            InputWorkBook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
        public static HSSFWorkbook StreamToWorkBook(Stream InputStream)
        {
            HSSFWorkbook WorkBook = new HSSFWorkbook(InputStream);
            return WorkBook;
        }
        public static HSSFWorkbook MemoryStreamToWorkBook(MemoryStream InputStream)
        {
            HSSFWorkbook WorkBook = new HSSFWorkbook(InputStream as Stream);
            return WorkBook;
        }
        public static MemoryStream WorkBookToMemoryStream(HSSFWorkbook InputStream)
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            InputStream.Write(file);
            return file;
        }
        public static Stream FileToStream(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists == true)
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                return fs;
            }
            else return null;
        }
        public static Stream MemoryStreamToStream(MemoryStream ms)
        {
            return ms as Stream;
        }
        #endregion

        #region DataTable与Excel资料格式转换
        /// <summary>
        /// 将DataTable转成Stream输出.
        /// </summary>
        /// <param name="SourceTable">The source table.</param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            InitializeWorkbook();

            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // 处理标题
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // 处理内容
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        /// <summary>
        /// 将DataTable转成Workbook(自定资料型态)输出.
        /// </summary>
        /// <param name="SourceTable">The source table.</param>
        /// <returns></returns>
        public static HSSFWorkbook RenderDataTableToWorkBook(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }
            return workbook;
        }

        /// <summary>
        /// 将DataTable资料输出成档案。
        /// </summary>
        /// <param name="SourceTable">The source table.</param>
        /// <param name="FileName">文件保存路径</param>
        public static void RenderDataTableToExcel(DataTable SourceTable, string FileName)
        {
            MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }

        /// <summary>
        /// 从流读取资料到DataTable.
        /// </summary>
        /// <param name="ExcelFileStream">The excel file stream.</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <param name="HeaderRowIndex">Index of the header row.</param>
        /// <param name="HaveHeader">if set to <c>true</c> [have header].</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, bool HaveHeader)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();
            ISheet sheet = workbook.GetSheet(SheetName);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                string ColumnName = (HaveHeader == true) ? headerRow.GetCell(i).StringCellValue : "f" + i.ToString();
                DataColumn column = new DataColumn(ColumnName);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            int RowStart = (HaveHeader == true) ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
            for (int i = RowStart; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }


        /// <summary>
        /// 從位元流讀取資料到DataTable.
        /// </summary>
        /// <param name="ExcelFileStream">The excel file stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="HeaderRowIndex">Index of the header row.</param>
        /// <param name="HaveHeader">if set to <c>true</c> [have header].</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, bool HaveHeader)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();
            ISheet sheet = workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                string ColumnName = (HaveHeader == true) ? headerRow.GetCell(i).StringCellValue : "f" + i.ToString();
                DataColumn column = new DataColumn(ColumnName);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            int RowStart = (HaveHeader == true) ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
            for (int i = RowStart; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
        #endregion

        #region List<T>与Excel资料格式转换
        public static Stream RenderListToExcel<T>(List<T> SourceList)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();

            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            PropertyInfo[] properties = typeof(T).GetProperties();

            int columIndex = 0;
            foreach (PropertyInfo column in properties)
            {
                headerRow.CreateCell(columIndex).SetCellValue(column.Name);
                columIndex++;
            }

            int rowIndex = 1;
            foreach (T item in SourceList)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                columIndex = 0;

                foreach (PropertyInfo column in properties)
                {
                    dataRow.CreateCell(columIndex).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    columIndex++;
                }
                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static Stream RenderListToExcel<T>(List<T> SourceList, Dictionary<string, string> head)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();

            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            PropertyInfo[] properties = typeof(T).GetProperties();

            int columIndex = 0;
            foreach (PropertyInfo column in properties)
            {
                headerRow.CreateCell(columIndex).SetCellValue(head[column.Name] == null ? column.Name : head[column.Name].ToString());
                columIndex++;
            }

            int rowIndex = 1;
            foreach (T item in SourceList)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                columIndex = 0;

                foreach (PropertyInfo column in properties)
                {
                    dataRow.CreateCell(columIndex).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    columIndex++;
                }
                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static HSSFWorkbook RenderListToWorkbook<T>(List<T> SourceList)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();

            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            PropertyInfo[] properties = typeof(T).GetProperties();

            int columIndex = 0;
            foreach (PropertyInfo column in properties)
            {
                headerRow.CreateCell(columIndex).SetCellValue(column.Name);
                columIndex++;
            }

            int rowIndex = 1;
            foreach (T item in SourceList)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                columIndex = 0;

                foreach (PropertyInfo column in properties)
                {
                    dataRow.CreateCell(columIndex).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    columIndex++;
                }
                rowIndex++;
            }

            return workbook;
        }

        public static HSSFWorkbook RenderListToWorkbook<T>(List<T> SourceList, Dictionary<string, string> head)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();

            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            PropertyInfo[] properties = typeof(T).GetProperties();

            int columIndex = 0;
            foreach (PropertyInfo column in properties)
            {
                headerRow.CreateCell(columIndex).SetCellValue(head[column.Name] == null ? column.Name : head[column.Name].ToString());
                columIndex++;
            }

            int rowIndex = 1;
            foreach (T item in SourceList)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                columIndex = 0;

                foreach (PropertyInfo column in properties)
                {
                    dataRow.CreateCell(columIndex).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                    columIndex++;
                }
                rowIndex++;
            }

            return workbook;
        }

        public static void RenderListToExcel<T>(List<T> SourceList, string FileName)
        {
            MemoryStream ms = RenderListToExcel(SourceList) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }

        public static void RenderListToExcel<T>(List<T> SourceList, Dictionary<string, string> head, string FileName)
        {
            MemoryStream ms = RenderListToExcel(SourceList, head) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }

        public static List<T> RenderListFromExcel<T>(Stream ExcelFileStream, string SheetName)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();

            ISheet sheet = workbook.GetSheet(SheetName);
            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            List<T> list = new List<T>();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo column in properties)
                {
                    int j = headerRow.Cells.FindIndex(delegate (ICell c)
                    {
                        return c.StringCellValue == column.Name;
                    });

                    if (j >= 0 && row.GetCell(j) != null)
                    {
                        object value = ToType(column.PropertyType, row.GetCell(j).ToString());
                        column.SetValue(t, value, null);
                    }
                }

                list.Add(t);
            }
            return list;
        }

        public static List<T> RenderListFromExcel<T>(Stream ExcelFileStream, int SheetIndex)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();

            ISheet sheet = workbook.GetSheetAt(SheetIndex);
            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            List<T> list = new List<T>();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo column in properties)
                {
                    int j = headerRow.Cells.FindIndex(delegate (ICell c)
                    {
                        return c.StringCellValue == column.Name;
                    });

                    if (j >= 0 && row.GetCell(j) != null)
                    {
                        object value = ToType(column.PropertyType, row.GetCell(j).ToString());
                        column.SetValue(t, value, null);
                    }
                }

                list.Add(t);
            }
            return list;
        }

        public static List<T> RenderListFromExcel<T>(Stream ExcelFileStream, string SheetName, Dictionary<string, string> head)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();

            ISheet sheet = workbook.GetSheet(SheetName);
            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            List<T> list = new List<T>();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo column in properties)
                {
                    int j = headerRow.Cells.FindIndex(delegate (ICell c)
                    {
                        return c.StringCellValue == (head[column.Name] == null ? column.Name : head[column.Name].ToString());
                    });

                    if (j >= 0 && row.GetCell(j) != null)
                    {
                        object value = ToType(column.PropertyType, row.GetCell(j).ToString());
                        column.SetValue(t, value, null);
                    }
                }

                list.Add(t);
            }
            return list;
        }

        public static List<T> RenderListFromExcel<T>(Stream ExcelFileStream, int SheetIndex, Dictionary<string, string> head)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();

            ISheet sheet = workbook.GetSheetAt(SheetIndex);
            IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
            int cellCount = headerRow.LastCellNum;

            List<T> list = new List<T>();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                T t = Activator.CreateInstance<T>();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo column in properties)
                {
                    int j = headerRow.Cells.FindIndex(delegate (ICell c)
                    {
                        return c.StringCellValue == (head[column.Name] == null ? column.Name : head[column.Name].ToString());
                    });

                    if (j >= 0 && row.GetCell(j) != null)
                    {
                        object value = ToType(column.PropertyType, row.GetCell(j).ToString());
                        column.SetValue(t, value, null);
                    }
                }

                list.Add(t);
            }
            return list;
        }

        public static object ToType(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }

            MethodInfo parseMethod = null;

            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Static
                | BindingFlags.Public))
            {
                if (mi.Name == "Parse" && mi.GetParameters().Length == 1)
                {
                    parseMethod = mi;
                    break;
                }
            }

            if (parseMethod == null)
            {
                throw new ArgumentException(string.Format(
                    "Type: {0} has not Parse static method!", type));
            }

            return parseMethod.Invoke(null, new object[] { value });
        }
        #endregion

        #region 字符串阵列与Excel资料格式转换
        /// <summary>
        /// 建立datatable
        /// </summary>
        /// <param name="ColumnName">欄位名用逗號分隔</param>
        /// <param name="value">data陣列 , rowmajor</param>
        /// <returns>DataTable</returns>
        public static DataTable CreateDataTable(string ColumnName, string[,] value)
        {
            /*  輸入範例
            string cname = " name , sex ";
            string[,] aaz = new string[4, 2];
            for (int q = 0; q < 4; q++)
                for (int r = 0; r < 2; r++)
                    aaz[q, r] = "1";
            dataGridView1.DataSource = NewMediaTest1.Model.Utility.DataSetUtil.CreateDataTable(cname, aaz);
            */
            int i, j;
            DataTable ResultTable = new DataTable();
            string[] sep = new string[] { "," };

            string[] TempColName = ColumnName.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            DataColumn[] CName = new DataColumn[TempColName.Length];
            for (i = 0; i < TempColName.Length; i++)
            {
                DataColumn c1 = new DataColumn(TempColName[i].ToString().Trim(), typeof(object));
                ResultTable.Columns.Add(c1);
            }
            if (value != null)
            {
                for (i = 0; i < value.GetLength(0); i++)
                {
                    DataRow newrow = ResultTable.NewRow();
                    for (j = 0; j < TempColName.Length; j++)
                    {
                        newrow[j] = string.Copy(value[i, j].ToString());

                    }
                    ResultTable.Rows.Add(newrow);
                }
            }
            return ResultTable;
        }
        /// <summary>
        /// Creates the string array.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static string[,] CreateStringArray(DataTable dt)
        {
            int ColumnNum = dt.Columns.Count;
            int RowNum = dt.Rows.Count;
            string[,] result = new string[RowNum, ColumnNum];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    result[i, j] = string.Copy(dt.Rows[i][j].ToString());
                }
            }
            return result;
        }
        /// <summary>
        /// 將陣列輸出成位元流.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="SourceTable">The source table.</param>
        /// <returns></returns>
        public static Stream RenderArrayToExcel(string ColumnName, string[,] SourceTable)
        {
            DataTable dt = CreateDataTable(ColumnName, SourceTable);
            return RenderDataTableToExcel(dt);
        }
        /// <summary>
        /// 將陣列輸出成檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="SourceTable">The source table.</param>
        public static void RenderArrayToExcel(string FileName, string ColumnName, string[,] SourceTable)
        {
            DataTable dt = CreateDataTable(ColumnName, SourceTable);
            RenderDataTableToExcel(dt, FileName);
        }
        /// <summary>
        /// 將陣列輸出成WorkBook(自訂資料型態).
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="SourceTable">The source table.</param>
        /// <returns></returns>
        public static HSSFWorkbook RenderArrayToWorkBook(string ColumnName, string[,] SourceTable)
        {
            DataTable dt = CreateDataTable(ColumnName, SourceTable);
            return RenderDataTableToWorkBook(dt);
        }

        /// <summary>
        /// 將位元流資料輸出成陣列.
        /// </summary>
        /// <param name="ExcelFileStream">The excel file stream.</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <param name="HeaderRowIndex">Index of the header row.</param>
        /// <param name="HaveHeader">if set to <c>true</c> [have header].</param>
        /// <returns></returns>
        public static string[,] RenderArrayFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex, bool HaveHeader)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();
            ISheet sheet = workbook.GetSheet(SheetName);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            int RowStart = (HaveHeader == true) ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
            for (int i = RowStart; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return CreateStringArray(table);
        }

        /// <summary>
        /// 將位元流資料輸出成陣列.
        /// </summary>
        /// <param name="ExcelFileStream">The excel file stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="HeaderRowIndex">Index of the header row.</param>
        /// <param name="HaveHeader">if set to <c>true</c> [have header].</param>
        /// <returns></returns>
        public static string[,] RenderArrayFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex, bool HaveHeader)
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
            InitializeWorkbook();
            ISheet sheet = workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;
            int RowStart = (HaveHeader == true) ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;
            for (int i = RowStart; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return CreateStringArray(table);
        }
        #endregion

        #region 超链接
        /// <summary>
        /// 在位元流儲存格中建立超連結.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetNameOrIndex">Index of the sheet name or.</param>
        /// <param name="LinkName">Name of the link.</param>
        /// <param name="LinkValueOrIndex">Index of the link value or.</param>
        /// <param name="s1">The s1.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="CellIndex">Index of the cell.</param>
        /// <returns></returns>
        public static Stream MakeLink(Stream InputStream, string SheetNameOrIndex, string LinkName, string LinkValueOrIndex, LinkType s1, int RowIndex, int CellIndex)
        {
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ICellStyle hlink_style = workbook.CreateCellStyle();
            IFont hlink_font = workbook.CreateFont();
            hlink_font.Underline = FontUnderlineType.Single;
            hlink_font.Color = IndexedColors.Blue.Index;
            hlink_style.SetFont(hlink_font);
            string ResultLinkValue = string.Empty;
            int ResultSheet;
            ISheet sheet;
            if (int.TryParse(SheetNameOrIndex, out ResultSheet) == true)
                sheet = workbook.GetSheetAt(ResultSheet);
            else
                sheet = workbook.GetSheet(SheetNameOrIndex);
            ICell cell = sheet.CreateRow(RowIndex).CreateCell(CellIndex);
            cell.SetCellValue(LinkName);
            HSSFHyperlink link;
            switch (s1.ToString())
            {
                case "网址":
                    link = new HSSFHyperlink(HyperlinkType.Url);
                    ResultLinkValue = string.Copy(LinkValueOrIndex);
                    break;
                case "档案":
                    link = new HSSFHyperlink(HyperlinkType.File);
                    ResultLinkValue = string.Copy(LinkValueOrIndex);
                    break;
                case "邮件":
                    link = new HSSFHyperlink(HyperlinkType.Email);
                    // ResultLinkValue = string.Copy(LinkValue);   
                    ResultLinkValue = "mailto:" + LinkValueOrIndex;
                    break;
                case "内容":
                    int result;
                    link = new HSSFHyperlink(HyperlinkType.Document);
                    if (int.TryParse(LinkValueOrIndex, out result) == true)
                        ResultLinkValue = "'" + workbook.GetSheetName(result) + "'!A1";
                    else
                        ResultLinkValue = "'" + LinkValueOrIndex + "'!A1";
                    break;
                default:
                    link = new HSSFHyperlink(HyperlinkType.Url);
                    break;
            }
            link.Address = (ResultLinkValue);
            cell.Hyperlink = (link);
            cell.CellStyle = (hlink_style);
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 在檔案儲存格中建立超連結.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetNameOrIndex">Index of the sheet name or.</param>
        /// <param name="LinkName">Name of the link.</param>
        /// <param name="LinkValueOrIndex">Index of the link value or.</param>
        /// <param name="s1">The s1.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="CellIndex">Index of the cell.</param>
        public static void MakeLink(string FileName, Stream InputStream, string SheetNameOrIndex, string LinkName, string LinkValueOrIndex, LinkType s1, int RowIndex, int CellIndex)
        {
            MemoryStream ms = MakeLink(InputStream, SheetNameOrIndex, LinkName, LinkValueOrIndex, s1, RowIndex, CellIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 建立新位元流並在儲存格中建立超連結.
        /// </summary>
        /// <param name="SheetNameOrIndex">Index of the sheet name or.</param>
        /// <param name="LinkName">Name of the link.</param>
        /// <param name="LinkValueOrIndex">Index of the link value or.</param>
        /// <param name="s1">The s1.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="CellIndex">Index of the cell.</param>
        /// <returns></returns>
        public static Stream MakeLinkFromEmpty(string SheetNameOrIndex, string LinkName, string LinkValueOrIndex, LinkType s1, int RowIndex, int CellIndex)
        {

            workbook = new HSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet();
            //ISheet sheet = hssfworkbook.CreateSheet("Hyperlinks");
            ////cell style for hyperlinks
            ////by default hyperlinks are blue and underlined        
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ICellStyle hlink_style = workbook.CreateCellStyle();
            IFont hlink_font = workbook.CreateFont();
            hlink_font.Underline = FontUnderlineType.Single;
            hlink_font.Color = IndexedColors.Blue.Index;
            hlink_style.SetFont(hlink_font);
            string ResultLinkValue = string.Empty;
            int ResultSheet;
            ISheet sheet;
            if (int.TryParse(SheetNameOrIndex, out ResultSheet) == true)
                sheet = workbook.GetSheetAt(ResultSheet);
            else
                sheet = workbook.GetSheet(SheetNameOrIndex);
            ICell cell = sheet.CreateRow(RowIndex).CreateCell(CellIndex);
            cell.SetCellValue(LinkName);
            HSSFHyperlink link;
            switch (s1.ToString())
            {
                case "网址":
                    link = new HSSFHyperlink(HyperlinkType.Url);
                    ResultLinkValue = string.Copy(LinkValueOrIndex);
                    break;
                case "档案":
                    link = new HSSFHyperlink(HyperlinkType.File);
                    ResultLinkValue = string.Copy(LinkValueOrIndex);
                    break;
                case "邮件":
                    link = new HSSFHyperlink(HyperlinkType.Email);
                    // ResultLinkValue = string.Copy(LinkValue);   
                    ResultLinkValue = "mailto:" + LinkValueOrIndex;
                    break;
                case "内容":
                    int result;
                    link = new HSSFHyperlink(HyperlinkType.Document);
                    if (int.TryParse(LinkValueOrIndex, out result) == true)
                        ResultLinkValue = "'" + workbook.GetSheetName(result) + "'!A1";
                    else
                        ResultLinkValue = "'" + LinkValueOrIndex + "'!A1";
                    break;
                default:
                    link = new HSSFHyperlink(HyperlinkType.Url);
                    break;
            }
            link.Address = (ResultLinkValue);
            cell.Hyperlink = (link);
            cell.CellStyle = (hlink_style);
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 建立新檔案並在儲存格中建立超連結.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="SheetNameOrIndex">Index of the sheet name or.</param>
        /// <param name="LinkName">Name of the link.</param>
        /// <param name="LinkValueOrIndex">Index of the link value or.</param>
        /// <param name="s1">The s1.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="CellIndex">Index of the cell.</param>
        public static void MakeLinkFromEmpty(string FileName, string SheetNameOrIndex, string LinkName, string LinkValueOrIndex, LinkType s1, int RowIndex, int CellIndex)
        {
            MemoryStream ms = MakeLinkFromEmpty(SheetNameOrIndex, LinkName, LinkValueOrIndex, s1, RowIndex, CellIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 设定字体字形
        public static ICellStyle SetCellStyle(IFont InputFont)
        {
            InitializeWorkbook();
            ICellStyle style1 = workbook.CreateCellStyle();
            style1.SetFont(InputFont);
            return style1;
        }
        /// <summary>
        /// 設定字體顏色大小到位元流.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="FontName">Name of the font.</param>
        /// <param name="FontSize">Size of the font.</param>
        /// <param name="IsAllSheet">if set to <c>true</c> [is all sheet].</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <returns></returns>
        public static Stream ApplyStyleToFile(Stream InputStream, string FontName, short FontSize, bool IsAllSheet, params string[] SheetName)
        {
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            IFont font = workbook.CreateFont();
            ICellStyle Style = workbook.CreateCellStyle();
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            Style.SetFont(font);
            MemoryStream ms = new MemoryStream();
            int i;
            if (IsAllSheet == true)
            {
                for (i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet Sheets = workbook.GetSheetAt(0);
                    for (int k = Sheets.FirstRowNum; k <= Sheets.LastRowNum; k++)
                    {
                        IRow row = Sheets.GetRow(k);
                        for (int l = row.FirstCellNum; l < row.LastCellNum; l++)
                        {
                            ICell Cell = row.GetCell(l);
                            Cell.CellStyle = Style;
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < SheetName.Length; i++)
                {
                    ISheet Sheets = workbook.GetSheet(SheetName[i]);
                    for (int k = Sheets.FirstRowNum; k <= Sheets.LastRowNum; k++)
                    {
                        IRow row = Sheets.GetRow(k);
                        for (int l = row.FirstCellNum; l < row.LastCellNum; l++)
                        {
                            ICell Cell = row.GetCell(l);
                            Cell.CellStyle = Style;
                        }
                    }
                }
            }
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 設定字體顏色大小到位元流.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="FontName">Name of the font.</param>
        /// <param name="FontSize">Size of the font.</param>
        /// <param name="IsAllSheet">if set to <c>true</c> [is all sheet].</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <returns></returns>
        public static Stream ApplyStyleToFile(Stream InputStream, string FontName, short FontSize, bool IsAllSheet, params int[] SheetIndex)
        {
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            IFont font = workbook.CreateFont();
            ICellStyle Style = workbook.CreateCellStyle();
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            Style.SetFont(font);
            int i;
            if (IsAllSheet == true)
            {
                for (i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet Sheets = workbook.GetSheetAt(0);
                    for (int k = Sheets.FirstRowNum; k <= Sheets.LastRowNum; k++)
                    {
                        IRow row = Sheets.GetRow(k);
                        for (int l = row.FirstCellNum; l < row.LastCellNum; l++)
                        {
                            ICell Cell = row.GetCell(l);
                            Cell.CellStyle = Style;
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < SheetIndex.Length; i++)
                {
                    ISheet Sheets = workbook.GetSheetAt(SheetIndex[i]);
                    for (int k = Sheets.FirstRowNum; k <= Sheets.LastRowNum; k++)
                    {
                        IRow row = Sheets.GetRow(k);
                        for (int l = row.FirstCellNum; l < row.LastCellNum; l++)
                        {
                            ICell Cell = row.GetCell(l);
                            Cell.CellStyle = Style;
                        }
                    }
                }
            }
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 設定字體顏色大小到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="FontName">Name of the font.</param>
        /// <param name="FontSize">Size of the font.</param>
        /// <param name="IsAllSheet">if set to <c>true</c> [is all sheet].</param>
        /// <param name="SheetName">Name of the sheet.</param>
        public static void ApplyStyleToFile(string FileName, Stream InputStream, string FontName, short FontSize, bool IsAllSheet, params string[] SheetName)
        {
            MemoryStream ms = ApplyStyleToFile(InputStream, FontName, FontSize, IsAllSheet, SheetName) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 設定字體顏色大小到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="FontName">Name of the font.</param>
        /// <param name="FontSize">Size of the font.</param>
        /// <param name="IsAllSheet">if set to <c>true</c> [is all sheet].</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        public static void ApplyStyleToFile(string FileName, Stream InputStream, string FontName, short FontSize, bool IsAllSheet, params int[] SheetIndex)
        {
            MemoryStream ms = ApplyStyleToFile(InputStream, FontName, FontSize, IsAllSheet, SheetIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 建立空白excel檔
        /// <summary>
        /// 建立空白excel檔到位元流.
        /// </summary>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <returns></returns>
        public static Stream CreateEmptyFile(params string[] SheetName)
        {
            MemoryStream ms = new MemoryStream();
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            if (SheetName == null)
            {
                workbook.CreateSheet();
            }
            else
            {
                foreach (string temp in SheetName)
                    workbook.CreateSheet(temp);
            }
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 建立空白excel檔到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="SheetName">Name of the sheet.</param>
        public static void CreateEmptyFile(string FileName, params string[] SheetName)
        {
            MemoryStream ms = CreateEmptyFile(SheetName) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 設定格線
        /// <summary>
        /// 設定格線到位元流.
        /// </summary>
        /// <param name="InputSteam">The input steam.</param>
        /// <param name="haveGridLine">if set to <c>true</c> [have grid line].</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <returns></returns>
        public static Stream SetGridLine(Stream InputSteam, bool haveGridLine, params string[] SheetName)
        {
            workbook = new HSSFWorkbook(InputSteam);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            if (SheetName == null)
            {
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet s1 = workbook.GetSheetAt(i);
                    s1.DisplayGridlines = haveGridLine;
                }
            }
            else
            {
                foreach (string TempSheet in SheetName)
                {
                    ISheet s1 = workbook.GetSheet(TempSheet);
                    s1.DisplayGridlines = haveGridLine;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 設定格線到位元流.
        /// </summary>
        /// <param name="InputSteam">The input steam.</param>
        /// <param name="haveGridLine">if set to <c>true</c> [have grid line].</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <returns></returns>
        public static Stream SetGridLine(Stream InputSteam, bool haveGridLine, params int[] SheetIndex)
        {
            workbook = new HSSFWorkbook(InputSteam);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            if (SheetIndex == null)
            {
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet s1 = workbook.GetSheetAt(i);
                    s1.DisplayGridlines = haveGridLine;
                }
            }
            else
            {
                foreach (int TempSheet in SheetIndex)
                {
                    ISheet s1 = workbook.GetSheetAt(TempSheet);
                    s1.DisplayGridlines = haveGridLine;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 設定格線到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputSteam">The input steam.</param>
        /// <param name="haveGridLine">if set to <c>true</c> [have grid line].</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        public static void SetGridLine(string FileName, Stream InputSteam, bool haveGridLine, params int[] SheetIndex)
        {
            MemoryStream ms = SetGridLine(InputSteam, haveGridLine, SheetIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 設定格線到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputSteam">The input steam.</param>
        /// <param name="haveGridLine">if set to <c>true</c> [have grid line].</param>
        /// <param name="SheetName">Name of the sheet.</param>
        public static void SetGridLine(string FileName, Stream InputSteam, bool haveGridLine, params string[] SheetName)
        {
            MemoryStream ms = SetGridLine(InputSteam, haveGridLine, SheetName) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 擷取字串從excel檔案
        /// <summary>
        /// 從位元流將資料轉成字串輸出
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <returns></returns>
        public static string ExtractStringFromFileStream(Stream InputStream)
        {
            HSSFWorkbook HBook = new HSSFWorkbook(InputStream);
            ExcelExtractor extractor = new ExcelExtractor(HBook);
            return extractor.Text;
        }
        /// <summary>
        /// 從檔案將資料轉成字串輸出
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public static string ExtractStringFromFileStream(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            if (fi.Exists == true)
            {
                using (FileStream fs = fi.Open(FileMode.Open))
                {
                    HSSFWorkbook HBook = new HSSFWorkbook(fs);
                    ExcelExtractor extractor = new ExcelExtractor(HBook);
                    return extractor.Text;
                }
            }
            else return null;
        }
        #endregion

        #region 設定群組

        /// <summary>
        /// 設定群組到位元流.
        /// </summary>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <param name="IsRow">if set to <c>true</c> [is row].</param>
        /// <param name="From">From.</param>
        /// <param name="End">The end.</param>
        /// <returns></returns>
        public static Stream CreateGroup(string SheetName, bool IsRow, int From, int End)
        {
            MemoryStream ms = new MemoryStream();
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            ISheet sh = workbook.CreateSheet(SheetName);
            for (int i = 0; i <= End; i++)
            {
                sh.CreateRow(i);
            }
            if (IsRow == true)
                sh.GroupRow(From, End);
            else
                sh.GroupColumn((short)From, (short)End);

            workbook.Write(ms);
            ms.Flush();
            return ms;

        }
        /// <summary>
        /// 建立群組到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="SheetName">Name of the sheet.</param>
        /// <param name="IsRow">if set to <c>true</c> [is row].</param>
        /// <param name="From">From.</param>
        /// <param name="End">The end.</param>
        public static void CreateGroup(string FileName, string SheetName, bool IsRow, int From, int End)
        {
            MemoryStream ms = CreateGroup(SheetName, IsRow, From, End) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 從樣板建立檔案

        /// <summary>
        /// 從樣板建立位元流.
        /// </summary>
        /// <param name="TemplateFileName">Name of the template file.</param>
        /// <returns></returns>
        public static Stream CreateFileStreamFromTemplate(string TemplateFileName)
        {
            FileInfo fi = new FileInfo(TemplateFileName);
            if (fi.Exists == true)
            {
                MemoryStream ms = new MemoryStream();
                FileStream file = new FileStream(TemplateFileName, FileMode.Open, FileAccess.Read);
                workbook = new HSSFWorkbook(file);
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "測試公司";
                workbook.DocumentSummaryInformation = dsi;
                ////create a entry of SummaryInformation
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = "測試公司Excel檔案";
                si.Title = "測試公司Excel檔案";
                si.Author = "killysss";
                si.Comments = "謝謝您的使用！";
                workbook.SummaryInformation = si;
                workbook.Write(ms);
                ms.Flush();
                return ms;
            }
            else return null;
        }

        /// <summary>
        /// 從樣板建立檔案.
        /// </summary>
        /// <param name="TemplateFileName">Name of the template file.</param>
        /// <param name="OutputFileName">Name of the output file.</param>
        public static void CreateFileFromTemplate(string TemplateFileName, string OutputFileName)
        {
            FileInfo fi = new FileInfo(TemplateFileName);
            if (fi.Exists == true)
            {
                MemoryStream ms = CreateFileStreamFromTemplate(TemplateFileName) as MemoryStream;
                WriteSteamToFile(ms, OutputFileName);
            }
            else
            {

            }
        }
        #endregion

        #region 嵌入圖片
        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="wb">The wb.</param>
        /// <returns></returns>
        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }
        /// <summary>
        /// 嵌入圖片到位元流.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="PicFileName">Name of the pic file.</param>
        /// <param name="IsOriginalSize">if set to <c>true</c> [is original size].</param>
        /// <param name="RowPosition">The row position.</param>
        /// <returns></returns>
        public static Stream EmbedImage(Stream InputStream, int SheetIndex, string PicFileName, bool IsOriginalSize, int[] RowPosition)
        {
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet1 = workbook.GetSheetAt(SheetIndex);

            EmbedImage(sheet1, PicFileName, IsOriginalSize, RowPosition);

            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 嵌入圖片到檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="PicFileName">Name of the pic file.</param>
        /// <param name="IsOriginalSize">if set to <c>true</c> [is original size].</param>
        /// <param name="RowPosition">The row position.</param>
        public static void EmbedImage(string FileName, int SheetIndex, Stream InputStream, string PicFileName, bool IsOriginalSize, int[] RowPosition)
        {
            MemoryStream ms = EmbedImage(InputStream, SheetIndex, PicFileName, IsOriginalSize, RowPosition) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 建立新位元流並嵌入圖片.
        /// </summary>
        /// <param name="PicFileName">Name of the pic file.</param>
        /// <param name="IsOriginalSize">if set to <c>true</c> [is original size].</param>
        /// <param name="RowPosition">The row position.</param>
        /// <returns></returns>
        public static Stream EmbedImage(string PicFileName, bool IsOriginalSize, int[] RowPosition)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet1 = workbook.CreateSheet();
            IDrawing patriarch = sheet1.CreateDrawingPatriarch();
            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0,
                RowPosition[0], RowPosition[1], RowPosition[2], RowPosition[3]);
            anchor.AnchorType = AnchorType.MoveDontResize;
            //load the picture and get the picture index in the workbook
            IPicture picture = patriarch.CreatePicture(anchor, LoadImage(PicFileName, workbook));
            //Reset the image to the original size.
            if (IsOriginalSize == true)
                picture.Resize();
            //Line Style
            //picture.LineStyle = LineStyle.None;
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 建立新檔案並嵌入圖片.
        /// </summary>
        /// <param name="ExcelFileName">Name of the excel file.</param>
        /// <param name="PicFileName">Name of the pic file.</param>
        /// <param name="IsOriginalSize">if set to <c>true</c> [is original size].</param>
        /// <param name="RowPosition">The row position.</param>
        public static void EmbedImage(string ExcelFileName, string PicFileName, bool IsOriginalSize, int[] RowPosition)
        {
            MemoryStream ms = EmbedImage(PicFileName, IsOriginalSize, RowPosition) as MemoryStream;
            WriteSteamToFile(ms, ExcelFileName);
        }

        /// <summary>
        /// 插入图片到指定位置
        /// </summary>
        /// <param name="sheet">标签</param>
        /// <param name="PicFileName">图片地址</param>
        /// <param name="IsOriginalSize">保持原尺寸</param>
        /// <param name="RowPosition">单元格位置</param>
        private static void EmbedImage(ISheet sheet, string PicFileName, bool IsOriginalSize, int[] RowPosition)
        {
            IDrawing patriarch = sheet.CreateDrawingPatriarch();
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0,
                RowPosition[0], RowPosition[1], RowPosition[2], RowPosition[3]);
            anchor.AnchorType = AnchorType.MoveDontResize;
            //load the picture and get the picture index in the workbook
            IPicture picture = patriarch.CreatePicture(anchor, LoadImage(PicFileName, workbook));
            //Reset the image to the original size.
            if (IsOriginalSize == true)
                picture.Resize();
            //Line Style
            //picture.LineStyle = LineStyle.None;
        }
        #endregion

        #region 合并单元格
        /// <summary>
        /// 合併儲存格於位元流.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="RowFrom">The row from.</param>
        /// <param name="ColumnFrom">The column from.</param>
        /// <param name="RowTo">The row to.</param>
        /// <param name="ColumnTo">The column to.</param>
        /// <returns></returns>
        public static Stream MergeCell(Stream InputStream, int SheetIndex, int RowFrom, int ColumnFrom, int RowTo, int ColumnTo)
        {
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            InitializeWorkbook();
            ISheet sheet1 = workbook.GetSheetAt(SheetIndex);
            sheet1.AddMergedRegion(new CellRangeAddress(RowFrom, ColumnFrom, RowTo, ColumnTo));
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 合併儲存格於檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="RowFrom">The row from.</param>
        /// <param name="ColumnFrom">The column from.</param>
        /// <param name="RowTo">The row to.</param>
        /// <param name="ColumnTo">The column to.</param>
        public static void MergeCell(string FileName, Stream InputStream, int SheetIndex, int RowFrom, int ColumnFrom, int RowTo, int ColumnTo)
        {
            MemoryStream ms = MergeCell(InputStream, SheetIndex, RowFrom, ColumnFrom, RowTo, ColumnTo) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 建立新位元流並合併儲存格.
        /// </summary>
        /// <param name="RowFrom">The row from.</param>
        /// <param name="ColumnFrom">The column from.</param>
        /// <param name="RowTo">The row to.</param>
        /// <param name="ColumnTo">The column to.</param>
        /// <returns></returns>
        public static Stream MergeCell(int RowFrom, int ColumnFrom, int RowTo, int ColumnTo)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            InitializeWorkbook();
            ISheet sheet1 = workbook.CreateSheet();
            sheet1.AddMergedRegion(new CellRangeAddress(RowFrom, ColumnFrom, RowTo, ColumnTo));
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 建立新檔案並合併儲存格.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="RowFrom">The row from.</param>
        /// <param name="ColumnFrom">The column from.</param>
        /// <param name="RowTo">The row to.</param>
        /// <param name="ColumnTo">The column to.</param>
        public static void MergeCell(string FileName, int RowFrom, int ColumnFrom, int RowTo, int ColumnTo)
        {
            MemoryStream ms = MergeCell(RowFrom, ColumnFrom, RowTo, ColumnTo) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 設定儲存格公式
        /// <summary>
        /// 設定儲存格公式於位元流.
        /// </summary>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="Formula">The formula.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        /// <returns></returns>
        public static Stream SetFormula(Stream InputStream, int SheetIndex, string Formula, int RowIndex, int ColumnIndex)
        {
            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            workbook = new HSSFWorkbook(InputStream);
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet1 = workbook.GetSheetAt(SheetIndex);
            sheet1.CreateRow(RowIndex).CreateCell(ColumnIndex).SetCellFormula(Formula);
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 設定儲存格公式於檔案.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="InputStream">The input stream.</param>
        /// <param name="SheetIndex">Index of the sheet.</param>
        /// <param name="Formula">The formula.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        public static void SetFormula(string FileName, Stream InputStream, int SheetIndex, string Formula, int RowIndex, int ColumnIndex)
        {
            MemoryStream ms = SetFormula(InputStream, SheetIndex, Formula, RowIndex, ColumnIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        /// <summary>
        /// 建立新位元流並設定儲存格公式.
        /// </summary>
        /// <param name="Formula">The formula.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        /// <returns></returns>
        public static Stream SetFormula(string Formula, int RowIndex, int ColumnIndex)
        {
            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet1 = workbook.CreateSheet();
            sheet1.CreateRow(RowIndex).CreateCell(ColumnIndex).SetCellFormula(Formula);
            workbook.Write(ms);
            ms.Flush();
            return ms;
        }
        /// <summary>
        /// 建立新檔案並設定儲存格公式.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="Formula">The formula.</param>
        /// <param name="RowIndex">Index of the row.</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        public static void SetFormula(string FileName, string Formula, int RowIndex, int ColumnIndex)
        {
            MemoryStream ms = SetFormula(Formula, RowIndex, ColumnIndex) as MemoryStream;
            WriteSteamToFile(ms, FileName);
        }
        #endregion

        #region 导入导出
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            //标签
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    //超出65535条新建标签
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    SetTableHeader(sheet, dtSource.Columns.Count, strHeaderText);

                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(1);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                //单元格格式
                ICellStyle dataStyle = workbook.CreateCellStyle();
                IFont cellfont = workbook.CreateFont();
                cellfont.FontName = "隶书";
                dataStyle.SetFont(cellfont);
                foreach (DataColumn column in dtSource.Columns)
                {
                    SetCellValue(row, dataRow, column, dataStyle);
                }
                #endregion

                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                workbook.Close();
                return ms;
            }
        }

        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        /// 读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 单元格赋值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dataRow"></param>
        /// <param name="column"></param>
        /// <param name="cellStyle"></param>
        private static void SetCellValue(DataRow row, HSSFRow dataRow, DataColumn column, ICellStyle cellStyle)
        {
            HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
            newCell.CellStyle = cellStyle;

            string drValue = row[column].ToString();

            switch (column.DataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    DateTime dateV;
                    DateTime.TryParse(drValue, out dateV);
                    newCell.SetCellValue(dateV.ToShortDateString());
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }

        /// <summary>
        /// 设置表头及样式
        /// </summary>
        /// <param name="sheet">标签页</param>
        /// <param name="colmunsCount">列数</param>
        /// <param name="strHeaderText">表头</param>
        private static void SetTableHeader(HSSFSheet sheet, int colmunsCount, string strHeaderText)
        {
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            headerRow.HeightInPoints = 25;
            headerRow.CreateCell(0).SetCellValue(strHeaderText);

            HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            font.Color = NPOI.HSSF.Util.HSSFColor.Blue.Index;//设置为蓝色
            headStyle.SetFont(font);

            headerRow.GetCell(0).CellStyle = headStyle;
            //合并为一个单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, colmunsCount - 1));
        }
        #endregion
    }
}
