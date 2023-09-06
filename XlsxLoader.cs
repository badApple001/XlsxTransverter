using System;
using System.IO;
using System.Data;
using ExcelDataReader;
using System.Text;

namespace XlsxTransverter
{
    internal class XlsxLoader
    {
        private DataSet mData;
        public string dataTableName { private set; get; }
        public XlsxLoader( string filePath )
        {
            dataTableName = Path.GetFileNameWithoutExtension( filePath );
            filePath = App.GetRealPath( filePath );

            Encoding.RegisterProvider( CodePagesEncodingProvider.Instance );

            using ( var stream = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
            {
                using ( var reader = ExcelReaderFactory.CreateReader( stream ) )
                {
                    var result = reader.AsDataSet( createDataSetReadConfig() );
                    mData = result;
                }
            }

            if ( Sheets.Count < 1 )
            {
                throw new Exception( "Excel file is empty: " + filePath );
            }

        }

        public DataTableCollection Sheets
        {
            get
            {
                return this.mData.Tables;
            }
        }

        private ExcelDataSetConfiguration createDataSetReadConfig()
        {
            var tableConfig = new ExcelDataTableConfiguration()
            {
                // Gets or sets a value indicating whether to use a row from the 
                // data as column names.
                UseHeaderRow = false,
            };

            return new ExcelDataSetConfiguration()
            {
                // Gets or sets a value indicating whether to set the DataColumn.DataType
                // property in a second pass.
                UseColumnDataType = false,

                // Gets or sets a callback to obtain configuration options for a DataTable. 
                ConfigureDataTable = ( tableReader ) => { return tableConfig; },
            };
        }

    }
}
