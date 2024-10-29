using System.Data; 

namespace XlsxTransverter
{

    public enum DataType
    {

        int_,
        uint_,

        long_,
        ulong_,

        string_,

        float_,
        double_,

        decimal_,
        bool_,

    }

    abstract class Exporter
    {
        public DataTable? dataTable;

        /// <summary>
        /// Item1  int[] Row下标数组
        /// Item2  DataType[] 类型数组
        /// Item3  string[] 变量名数组
        /// Item4  string[] 注释数组
        /// </summary>
        public (List<int>, List<DataType>, List<string>, List<string>) header { protected set; get; }

        public XlsxLoader? loader { protected set; get; }
    }


    interface IOutScriptHandler
    {
        void Apply();
    }



    internal class App
    {
        public static string GetRealPath( string path )
        {
            if ( path.StartsWith( "./" ) )
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                path = dir + path.Remove( 0, 2 );
            }

            return path;
        }
    }

}


