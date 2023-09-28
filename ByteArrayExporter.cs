using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XlsxTransverter
{
    internal class ByteArrayExporter : Exporter
    {

        public ByteArrayExporter( XlsxLoader loader, string dataFolder, bool raw )
        {
            dataFolder = App.GetRealPath( dataFolder );
            this.loader = loader;
            var dt = dataTable = loader.Sheets[ 0 ];
            using ( FileStream fs = new FileStream( Path.Combine( dataFolder, $"DR{loader.dataTableName}.bytes" ), FileMode.Create, FileAccess.Write ) )
            {
                using ( BinaryWriter bw = new BinaryWriter( fs, Encoding.UTF8 ) )
                {
                    if ( raw )
                    {
                        WriteRaw( dt, bw );
                    }
                    else
                    {
                        WriteDataTable( dt, bw );
                    }
                    bw.Flush();
                    bw.Close();
                }
                fs.Close();
            }
        }

        public void WriteRaw( DataTable dt, BinaryWriter bw )
        {

            //获取Header
            int ignoreRows = 3;
            header = GetHeaders( dt );

            //写入行数
            bw.Write( ( ushort ) ( dt.Rows.Count - ignoreRows ) );

            //Raw标志位
            bw.Write( true );

            //写入列数
            bw.Write( ( ushort ) ( header.Item1.Count ) );

            //写入列类型
            for ( int i = 0; i < header.Item2.Count; i++ )
            {
                bw.Write( ( ushort ) header.Item2[ i ] );
            }

            //写入Var
            for ( int i = 0; i < header.Item3.Count; i++ )
            {
                bw.Write( header.Item3[ i ] );
            }

            //写入数据
            WriteData( ignoreRows, dt, bw );
        }

        public void WriteDataTable( DataTable dt, BinaryWriter bw )
        {
            int ignoreRows = 3;
            //写入行数
            bw.Write( ( ushort ) ( dt.Rows.Count - ignoreRows ) );
            //Raw标志位
            bw.Write( false );
            header = GetHeaders( dt );
            WriteData( ignoreRows, dt, bw );
        }

        public void WriteData( int ignoreRows, DataTable dt, BinaryWriter bw )
        {
            for ( int i = ignoreRows; i < dt.Rows.Count; i++ )
            {
                for ( int j = 0; j < header.Item1.Count; j++ )
                {
                    int col = header.Item1[ j ];
                    object value = dt.Rows[ i ][ col ];
                    switch ( header.Item2[ j ] )
                    {
                        case DataType.int_:
                            {
                                if ( int.TryParse( value.ToString(), out int result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( int ) 0 );
                                }
                            }
                            break;
                        case DataType.uint_:
                            {
                                if ( uint.TryParse( value.ToString(), out uint result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( uint ) 0 );
                                }
                            }
                            break;
                        case DataType.long_:
                            {
                                if ( long.TryParse( value.ToString(), out long result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( long ) 0 );
                                }
                            }
                            break;
                        case DataType.ulong_:
                            {
                                if ( ulong.TryParse( value.ToString(), out ulong result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( ulong ) 0 );
                                }
                            }
                            break;
                        case DataType.string_:
                            {
                                bw.Write( value.ToString() );
                            }
                            break;
                        case DataType.float_:
                            {
                                if ( float.TryParse( value.ToString(), out float result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( 0f );
                                }
                            }
                            break;
                        case DataType.double_:
                            {
                                if ( double.TryParse( value.ToString(), out double result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( double ) 0 );
                                }
                            }
                            break;
                        case DataType.decimal_:
                            {
                                if ( decimal.TryParse( value.ToString(), out decimal result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( ( decimal ) 0 );
                                }
                            }
                            break;
                        case DataType.bool_:
                            {
                                if ( bool.TryParse( value.ToString(), out bool result ) )
                                {
                                    bw.Write( result );
                                }
                                else
                                {
                                    bw.Write( default( bool ) );
                                }
                            }
                            break;
                    }
                }

            }

        }


        public (List<int>, List<DataType>, List<string>, List<string>) GetHeaders( DataTable dataTable )
        {
            DataRow varRow = dataTable.Rows[ 0 ];
            DataRow typeRow = dataTable.Rows[ 1 ];
            DataRow commentsRow = dataTable.Rows[ 2 ];
            List<int> validColumn = new List<int>();
            List<DataType> types = new List<DataType>();
            List<string> vars = new List<string>();
            List<string> comments = new List<string>();
            for ( int i = 0; i < varRow.ItemArray.Length; i++ )
            {
                var var_ = varRow[ i ];
                if ( var_ == null )
                {
                    continue;
                }

                var varName = var_.ToString();
                if ( string.IsNullOrEmpty( varName ) || string.IsNullOrWhiteSpace( varName ) || varName.StartsWith( "#" ) )
                {
                    continue;
                }

                if ( !vars.Contains( varName ) )
                {
                    vars.Add( varName );
                }
                else
                {
                    throw new Exception( $"Recurring fields: {varName}" );
                }
                validColumn.Add( i );

                string comment = commentsRow[ i ].ToString();
                comments.Add( comment );

                string typeName = typeRow[ i ].ToString();
                typeName = typeName.ToLower();
                switch ( typeName )
                {
                    case "int":
                        types.Add( DataType.int_ );
                        break;
                    case "uint":
                        types.Add( DataType.uint_ );
                        break;
                    case "long":
                        types.Add( DataType.long_ );
                        break;
                    case "ulong":
                        types.Add( DataType.long_ );
                        break;
                    case "string":
                        types.Add( DataType.string_ );
                        break;
                    case "float":
                        types.Add( DataType.float_ );
                        break;
                    case "double":
                        types.Add( DataType.double_ );
                        break;
                    case "decimal":
                        types.Add( DataType.decimal_ );
                        break;
                    case "bool":
                        types.Add( DataType.bool_ );
                        break;
                    default:
                        validColumn.Remove( i );
                        vars.Remove( varName );
                        comments.Remove( comment );
                        Console.WriteLine( $"Error Type <{loader.dataTableName}>: {typeName} row({i})" );
                        break;
                }

            }
            return (validColumn, types, vars, comments);
        }
    }


}
