using System.Text;
namespace XlsxTransverter
{
    internal class Gen_CS_Define_Class : IOutScriptHandler
    {

        public Gen_CS_Define_Class( Exporter exporter, string outDir )
        {
            outDir = App.GetRealPath( outDir );
            if ( !Directory.Exists( outDir ) )
            {
                Console.WriteLine( $"Error: {outDir} is not exist" );
                return;
            }

            var header = exporter.header;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "// Encoding: {0}\n", Encoding.UTF8.ToString() );
            sb.AppendFormat( "// Xlsx Name: {0}.xlsx\n", exporter.loader.dataTableName );
            sb.AppendLine( "// Bug feedback: isysprey@foxmail.com" );
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat( "public class DR{0} : IDRTable\r\n{{", exporter.loader.dataTableName );
            sb.AppendLine();

            for ( int i = 0; i < header.Item2.Count; i++ )
            {
                var typeName = header.Item2[ i ].ToString();
                var systemTypeName = typeName.Remove( typeName.Length - 1 );

                sb.AppendLine( "\t/// <summary>" );
                sb.AppendLine( "\t/// " + header.Item4[ i ] );
                sb.AppendLine( "\t/// </summary>" );
                sb.AppendFormat( "\tpublic {0} {1};", systemTypeName, header.Item3[ i ] );
                sb.AppendLine();
            }

            string IdName = header.Item3[ 0 ];
            var typeName1 = header.Item2[ 0 ].ToString();
            var systemTypeName1 = typeName1.Remove( typeName1.Length - 1 );
            sb.AppendLine( $"\tpublic {systemTypeName1} Get_ID( )\n\t{{" );
            sb.AppendLine( $"\t\treturn {IdName};" );
            sb.AppendLine( "\t}" );
            sb.AppendLine( "\tpublic void Decode( System.IO.BinaryReader br )\n\t{" );
            for ( int i = 0; i < header.Item2.Count; i++ )
            {
                var item_type = header.Item2[ i ];
                var item_var = header.Item3[ i ];
                switch ( item_type )
                {
                    case DataType.int_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadInt32();" );
                        }
                        break;
                    case DataType.uint_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadUInt32();" );
                        }
                        break;
                    case DataType.long_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadInt64();" );
                        }
                        break;
                    case DataType.ulong_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadUInt64();" );
                        }
                        break;
                    case DataType.string_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadString();" );
                        }
                        break;
                    case DataType.float_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadSingle();" );
                        }
                        break;
                    case DataType.double_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadDouble();" );
                        }
                        break;
                    case DataType.decimal_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadDecimal();" );
                        }
                        break;
                    case DataType.bool_:
                        {
                            sb.AppendLine( $"\t\t{item_var} = br.ReadBoolean();" );
                        }
                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine( "\t}" );

            sb.Append( '}' );
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();


            mFile = Path.Combine( outDir, $"DR{exporter.loader.dataTableName}.cs" );
            mCode = sb.ToString();
        }

        private string mFile = string.Empty;
        private string mCode = string.Empty;
        public void Apply( )
        {
            using ( FileStream file = new FileStream( mFile, FileMode.Create, FileAccess.Write ) )
            {
                using ( TextWriter writer = new StreamWriter( file, Encoding.UTF8 ) )
                    writer.Write( mCode );
            }
        }

    }
}
