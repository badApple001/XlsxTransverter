using XlsxTransverter;


string url = App.GetRealPath( args[ 0 ] );
string csParent = App.GetRealPath( args[ 1 ] );
string binParent = App.GetRealPath( args[ 2 ] );
bool.TryParse( args[ 3 ], out bool raw );

if ( url.ToLower().EndsWith( ".xlsx" ) )
{

    if ( File.Exists( url ) )
    {
        XlsxLoader loader = new( url );

        ByteArrayExporter exporter = new( loader, binParent, raw );

        //导出CS声明文件
        if ( !raw )
        {
            IOutScriptHandler outScriptHandler = new Gen_CS_Define_Class( exporter, csParent );
            outScriptHandler.Apply();
        }

        //导出Ts声明文件.... 其它类型

        //导出完成
        Console.WriteLine( $"{loader.dataTableName} export finish." );
    }
    else
    {
        Console.WriteLine( $"Wrong path: {url}" );
    }
}
else if ( Directory.Exists( url ) )
{
    string[] xlsxs = Directory.GetFiles( url, "*.xlsx" );
    List<Task> tasks = new List<Task>();

    void Convert( string xlsx, bool raw )
    {
        XlsxLoader loader = new( xlsx );
        ByteArrayExporter exporter = new( loader, binParent, raw );
        //导出CS声明文件
        if ( !raw )
        {
            IOutScriptHandler outScriptHandler = new Gen_CS_Define_Class( exporter, csParent );
            outScriptHandler.Apply();
        }
        //导出Ts声明文件.... 其它类型
        Console.WriteLine( $"{loader.dataTableName} export finish." );
    }

    //忽略列表
    List<string> rawfiles = new List<string>();
    for ( int i = 3; i < args.Length; i++ )
    {
        rawfiles.Add( args[ i ] );
    }

    foreach ( var xlsx in xlsxs )
    {
        if ( xlsx.Contains( "~$" ) )
        {
            continue;
        }
        var xlsxname = Path.GetFileNameWithoutExtension( xlsx );
        tasks.Add( new Task( ( ) => Convert( xlsx, rawfiles.Contains( xlsxname ) ) ) );
    }

    foreach ( var task in tasks )
    {
        task.Start();
    }

    Task.WaitAll( tasks.ToArray() );

    Console.WriteLine( $"total xlsx Convert finish. {tasks.Count}" );
}
else
{
    Console.WriteLine( $"Wrong path: {url}\nThere are three parameters: the first is the directory or file for xlsx, and the second is the directory for cs script output. The third directory is the output byte stream data file directory." );
}
